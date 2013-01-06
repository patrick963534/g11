using System;
using System.Collections.Generic;
using System.Text;
using Octopus.Net;
using Octopus.Core;
using System.Net;
using System.IO;

namespace Octopus.Commands
{
    // Tell the receiver what to do. :D
    public enum NetCommandType
    {
        RemoveProcessedPackage = 0x200,

        AppendTextMessage,
        AppendImageMessage,

        BroadcastFindUser,
        AddUser,

        FindGroupUser,
        AddGroupUser,
        AppendGroupTextMessage,
        AppendGroupImageMessage,
        CreateNewGroup,

        CheckUserCount,
        ReturnUserList,
        CheckGroupUserCount,
        ReturnGroupUserList,
    }

    public abstract class Cmd
    {
        public void Execute()
        {
            ExecuteImpl();
        }

        protected abstract void ExecuteImpl();
    }

    public class NP_RemoveProcessedPackageCmd : Cmd
    {
        private int m_packageID;

        public NP_RemoveProcessedPackageCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_packageID = BitConverter.ToInt32(data, 0);
        }

        protected override void ExecuteImpl()
        {
            OutgoingPackagePool.RemoveProcessedPackage(m_packageID);
        }
    }

    public class NP_AppendTextMessageCmd : Cmd
    {
        private string m_text;
        private UserInfo m_user;

        public NP_AppendTextMessageCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_text = Encoding.UTF8.GetString(data);
            m_user = UserInfoManager.FindUser(remoteIP); ;
        }

        protected override void ExecuteImpl()
        {
            m_user.AppendMessage(m_text, m_user.Username);
            Logger.WriteLine("Add message:" + m_text);
        }
    }

    public class NP_BroadcastFindUserCmd : Cmd
    {
        private string m_hostname;
        private IPEndPoint m_remoteIP;

        public NP_BroadcastFindUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_hostname = Encoding.UTF8.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.AddUser(m_remoteIP));
            Workbench.AddClient(m_hostname, m_remoteIP);
        }
    }

    public class NP_AddUserCmd : Cmd
    {
        private IPEndPoint m_remoteIP;
        private string m_name;

        public NP_AddUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_name = Encoding.UTF8.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            if (m_name == DataManager.WhoAmI)
                return;

            Workbench.AddClient(m_name, m_remoteIP);
        }
    }

    public class NP_FindGroupUserCmd : Cmd
    {
        private IPEndPoint m_remoteIP;
        private string m_groupKey;

        public NP_FindGroupUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_groupKey = Helper.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            if (string.IsNullOrEmpty(m_groupKey))
                return;

            if (GroupInfoManager.FindGroup(m_groupKey) != null)
            {
                OutgoingPackagePool.AddFirst(NetPackageGenerater.AddGroupUser(m_groupKey, m_remoteIP));
            }
        }
    }

    public class NP_AddGroupUserCmd : Cmd
    {
        private IPEndPoint m_remoteIP;
        private string m_groupKey;

        public NP_AddGroupUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_groupKey = Helper.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            GroupInfo gi = GroupInfoManager.FindGroup(m_groupKey);
            UserInfo user = UserInfoManager.FindUser(m_remoteIP);

            Workbench.GroupAddUser(gi, user);
        }
    }

    public class NP_AppendGroupTextMessageCmd : Cmd
    {
        private string m_groupKey;
        private string m_text;
        private UserInfo m_user;

        public NP_AppendGroupTextMessageCmd(byte[] data, IPEndPoint remoteIP)
        {
            string val = Helper.GetString(data);
            int pos = val.IndexOf(';');

            m_groupKey = val.Substring(0, pos);
            m_text = val.Substring(pos + 1);

            m_user = UserInfoManager.FindUser(remoteIP);
        }

        protected override void ExecuteImpl()
        {
            GroupInfo gi = GroupInfoManager.FindGroup(m_groupKey);
            if (gi != null && m_user != null)
            {
                gi.AppendMessage(m_text, m_user.Username);
            }
        }
    }

    public class NP_CreateNewGroupCmd : Cmd
    {
        private string m_groupKey;
        private string m_name;

        public NP_CreateNewGroupCmd(byte[] data, IPEndPoint remoteIP)
        {
            string val = Helper.GetString(data);
            int pos = val.IndexOf(';');

            m_groupKey = val.Substring(0, pos);
            m_name = val.Substring(pos + 1);
        }

        protected override void ExecuteImpl()
        {
            Workbench.AddGroup(m_groupKey, m_name);
        }
    }

    public class NP_CheckUserCountCmd : Cmd
    {
        private int m_count;
        private IPEndPoint m_remoteIP;

        public NP_CheckUserCountCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_count = Helper.GetInt(data);
            m_remoteIP = remoteIP;
        }

        protected override void ExecuteImpl()
        {
            int current = UserInfoManager.GetUserCount();

            if (m_count < current)
            {
                OutgoingPackagePool.AddFirst(NetPackageGenerater.ReturnUserList(UserInfoManager.GetUserArray(), m_remoteIP));
            }
        }
    }

    public class NP_ReturnUserListCmd : Cmd
    {
        private string[] m_usrNetStrings;

        public NP_ReturnUserListCmd(byte[] data, IPEndPoint remoteIP)
        {
            string val = Helper.GetString(data);
            m_usrNetStrings = val.Split(new string[] { "$%$" }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected override void ExecuteImpl()
        {
            UserInfo lastUser = null;
            foreach (string ns in m_usrNetStrings)
            {
                lastUser = UserInfo.FromNetString(ns);
                Workbench.AddClient(lastUser.Username, lastUser.RemoteIP);
            }
        }
    }

    public class NP_CheckGroupUserCountCmd : Cmd
    {
        private string m_groupKey;
        private int m_count;
        private IPEndPoint m_remoteIP;

        public NP_CheckGroupUserCountCmd(byte[] data, IPEndPoint remoteIP)
        {
            string val = Helper.GetString(data);
            int pos = val.IndexOf(';');

            m_groupKey = val.Substring(0, pos);
            m_count = int.Parse(val.Substring(pos + 1));
            m_remoteIP = remoteIP;
        }

        protected override void ExecuteImpl()
        {
            GroupInfo group = GroupInfoManager.FindGroup(m_groupKey);
            if (group != null)
            {
                if (group.GetUserCount() > m_count)
                {
                    OutgoingPackagePool.AddFirst(NetPackageGenerater.ReturnGroupUserList(m_groupKey, group.GetUserArray(), m_remoteIP));
                }
            }
        }
    }

    public class NP_ReturnGroupUserListCmd : Cmd
    {
        private string[] m_subs;

        public NP_ReturnGroupUserListCmd(byte[] data, IPEndPoint remoteIP)
        {
            string val = Helper.GetString(data);
            m_subs = val.Split(new string[] { "$%$" }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected override void ExecuteImpl()
        {
            if (m_subs.Length == 0)
                return;

            string groupKey = m_subs[0];
            GroupInfo group = GroupInfoManager.FindGroup(groupKey);
            if (group == null)
                return;

            UserInfo lastUser = null;
            for (int i = 1; i < m_subs.Length; i++)
            {
                lastUser = UserInfo.FromNetString(m_subs[i]);
                Workbench.GroupAddUser(group, lastUser);
            }
        }
    }

    public class NP_AppendImageMessageCmd : Cmd
    {
        private string m_filename;
        private UserInfo m_user;

        public NP_AppendImageMessageCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_user = UserInfoManager.FindUser(remoteIP);
            int length = Helper.GetInt(data);
            m_filename = Helper.GetString(data, 4, length);
            m_filename = Path.Combine(DataManager.GetCustomFaceFolderPath(), m_filename);

            if (!File.Exists(m_filename))
            {
                using (FileStream fs = new FileStream(m_filename, FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(data, length + 4, data.Length - length - 4);
                }
            }
        }

        protected override void ExecuteImpl()
        {
            if (m_user == null)
                return;

            m_user.AppendMessage(MsgInputConfig.FormatImageMessage(m_filename), m_user.Username);
        }
    }

    public class NP_AppendGroupImageMessageCmd : Cmd
    {
        private string m_filename;
        private GroupInfo m_group;
        private UserInfo m_user;

        public NP_AppendGroupImageMessageCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_user = UserInfoManager.FindUser(remoteIP);

            int key_length = Helper.GetInt(data);
            m_group = GroupInfoManager.FindGroup(Helper.GetString(data, 4, key_length));

            int length = Helper.GetInt(data, key_length + 4);
            m_filename = Helper.GetString(data, key_length + 8, length);
            m_filename = Path.Combine(DataManager.GetCustomFaceFolderPath(), m_filename);

            if (!File.Exists(m_filename))
            {
                using (FileStream fs = new FileStream(m_filename, FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(data, 4 + key_length + length + 4, data.Length - 4 - key_length - length - 4);
                }
            }
        }

        protected override void ExecuteImpl()
        {
            if (m_group == null || m_user == null)
                return;

            m_group.AppendMessage(MsgInputConfig.FormatImageMessage(m_filename), m_user.Username);
        }
    }
}
