using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Octopus.Controls;

namespace Octopus.Core
{
    internal class UserInfo
    {
        public IPEndPoint RemoteIP;
        public string Username;
        public string Messages = string.Empty;
        public ChatForm Chatter;

        public UserInfo(IPEndPoint ipe, string name)
        {
            RemoteIP = ipe;
            Username = name;
        }

        public override string ToString()
        {
            return Username;
        }

        public void AppendMessage(string msg)
        {
            if (!string.IsNullOrEmpty(Messages))
                Messages += "\r\n";

            Messages += msg;

            if (Chatter != null)
                Chatter.ShowMessage();
        }

        public static string ParseIPEndpoint(IPEndPoint ipe)
        {
            return ipe.Address.ToString() + ":" + ipe.Port;
        }
    }

    internal class UserInfoManager
    {
        private static UserInfoManager s_singleton;

        private Dictionary<string, UserInfo> m_users = new Dictionary<string, UserInfo>();

        static UserInfoManager()
        {
            s_singleton = new UserInfoManager();
        }

        public static void AddUser(UserInfo user)
        {
            s_singleton.m_users.Add(UserInfo.ParseIPEndpoint(user.RemoteIP), user);
        }

        public static UserInfo FindUser(IPEndPoint ipe)
        {
            string key = UserInfo.ParseIPEndpoint(ipe);
            if (s_singleton.m_users.ContainsKey(key))
                return s_singleton.m_users[key];

            return null;
        }
    }
}
