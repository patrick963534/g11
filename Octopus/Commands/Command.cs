using System;
using System.Collections.Generic;
using System.Text;
using Octopus.Net;
using Octopus.Core;
using System.Net;

namespace Octopus.Commands
{
    // Tell the receiver what to do. :D
    public enum NetCommandType
    {
        RemoveProcessedPackage = 0x200,
        AppendTextMessage,
        BroadcastFindUser,
        AddUser,

        FindGroupUser,
        AddGroupUser,
        AppendGroupTextMessage,
        CreateNewGroup,

        CheckUserCount,
        RefreshUsers,
        CheckGroupUserCount,
        RefreshGroupUsers,
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
            m_user.AppendMessage(m_text);
            Logger.WriteLine("Add message:" + m_text);
            if (m_user.Chatter == null)
                Logger.WriteLine("Chatter is null.");
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
        private string m_hostname;

        public NP_AddUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_hostname = Encoding.UTF8.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            Workbench.AddClient(m_hostname, m_remoteIP);
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

        public NP_AppendGroupTextMessageCmd(byte[] data, IPEndPoint remoteIP)
        {
            string val = Helper.GetString(data);
            int pos = val.IndexOf(';');

            m_groupKey = val.Substring(0, pos);
            m_text = val.Substring(pos + 1);
        }

        protected override void ExecuteImpl()
        {
            GroupInfo gi = GroupInfoManager.FindGroup(m_groupKey);
            if (gi != null)
            {
                gi.AppendMessage(m_text);
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
                OutgoingPackagePool.AddFirst(NetPackageGenerater.RefreshUsers(m_remoteIP));
            }
        }
    }

    public class NP_RefreshUsersCmd : Cmd
    {
        public NP_RefreshUsersCmd(byte[] data, IPEndPoint remoteIP)
        { }

        protected override void ExecuteImpl()
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());            
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
                    OutgoingPackagePool.AddFirst(NetPackageGenerater.RefreshGroupUsers(m_groupKey, m_remoteIP));
                }
            }
        }
    }

    public class NP_RefreshGroupUsersCmd : Cmd
    {
        private string m_groupKey;

        public NP_RefreshGroupUsersCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_groupKey = Helper.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            GroupInfo group = GroupInfoManager.FindGroup(m_groupKey);
            if (group != null)
            {
                Workbench.QueryGroupUser(group);
            }
        }
    }
}
