using System;
using System.Collections.Generic;
using System.Text;
using Octopus.Controls;

namespace Octopus.Core
{
    public class GroupInfo
    {
        public string Key;
        public string Name;
        public GroupChatterForm Chatter;
        public bool IsReceiveNewMessage;

        public MessageStore MessageStore = new MessageStore();
        private Dictionary<string, UserInfo> m_users = new Dictionary<string, UserInfo>();

        private GroupInfo()
        { }

        public GroupInfo(string key, string name)
        {
            Key = key;
            Name = name;
        }

        public static GroupInfo Create(string name)
        {
            GroupInfo group = new GroupInfo(Guid.NewGuid().ToString(), name);
            return group;
        }

        public int GetUserCount()
        {
            return m_users.Count;
        }

        public void QueryGroupUsers()
        {
            if (Chatter == null)
                return;

            Chatter.QueryGroupUsers();
        }

        public bool ContainsUser(UserInfo user)
        {
            return m_users.ContainsKey(user.GetToken());
        }

        public void DeleteUser(UserInfo user)
        {
            string token = user.GetToken();
            if (m_users.ContainsKey(token))
            {
                m_users.Remove(token);

                if (Chatter != null)
                    Chatter.DeleteUser(user);
            }
        }

        public void AddUser(UserInfo user)
        {
            if (m_users.ContainsKey(user.GetToken()))
                return;

            m_users.Add(user.GetToken(), user);

            if (Chatter != null)
                Chatter.AddUser(user);
        }

        public UserInfo[] GetUserArray()
        {
            return new List<UserInfo>(m_users.Values).ToArray();
        }

        public void ShowChatter()
        {
            if (Chatter == null)
                Chatter = new GroupChatterForm(this);

            Chatter.Show();
            Chatter.Activate();

            IsReceiveNewMessage = false;
        }

        public void AppendMessage(string msg, string user)
        {
            MessageStore.AppendMessage(msg, user);

            if (Chatter != null)
            {
                Chatter.ShowMessage();
                IsReceiveNewMessage = false;
            }
            else
            {
                IsReceiveNewMessage = true;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class GroupInfoManager
    {
        private static object LockObject = new object();
        private static Dictionary<string, GroupInfo> m_groups = new Dictionary<string, GroupInfo>();

        public static GroupInfo FindGroupWhichHaveNewMessage()
        {
            lock (LockObject)
            {
                foreach (GroupInfo grp in m_groups.Values)
                {
                    if (grp.IsReceiveNewMessage)
                        return grp;
                }

                return null;
            }
        }

        public static void DeleteGroup(GroupInfo group)
        {
            lock (LockObject)
            {
                if (m_groups.ContainsKey(group.Key))
                    m_groups.Remove(group.Key);
            }
        }

        public static void AddGroup(GroupInfo group)
        {
            lock (LockObject)
            {
                if (!m_groups.ContainsKey(group.Key))
                    m_groups.Add(group.Key, group);
            }
        }

        public static GroupInfo[] GetGroupArray()
        {
            lock (LockObject)
            {
                List<GroupInfo> users = new List<GroupInfo>(m_groups.Values);
                return users.ToArray();
            }
        }

        public static GroupInfo FindGroup(string groupKey)
        {
            lock (LockObject)
            {
                if (m_groups.ContainsKey(groupKey))
                    return m_groups[groupKey];

                return null;
            }
        }
    }
}
