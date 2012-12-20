using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Octopus.Controls;

namespace Octopus.Core
{
    public class UserInfo
    {
        public IPEndPoint RemoteIP;
        public string Username;
        public string Messages = string.Empty;
        public ChatForm Chatter;
        public bool IsReceiveNewMessage;

        public UserInfo(IPEndPoint ipe, string name)
        {
            RemoteIP = ipe;
            Username = name;
        }

        public override string ToString()
        {
            return Username;
        }

        public void ShowChatter()
        {
            if (Chatter == null)
                Chatter = new ChatForm(this);

            IsReceiveNewMessage = false;
            Chatter.Show();
            Chatter.Activate();
        }

        public void AppendMessage(string user, string msg)
        {
            if (!string.IsNullOrEmpty(Messages))
                Messages += "\r\n";

            Messages += string.Format("[{0}][{1}:{2}:{3}]\r\n{4}\r\n", 
                user, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, msg);

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

        public static string ParseIPEndpoint(IPEndPoint ipe)
        {
            return ipe.Address.ToString() + ":" + ipe.Port;
        }
    }

    public class UserInfoManager
    {
        private static object LockObject = new object();
        private static Dictionary<string, UserInfo> m_users = new Dictionary<string, UserInfo>();

        public static UserInfo FindUserWhichHaveNewMessage()
        {
            lock (LockObject)
            {
                foreach (UserInfo user in m_users.Values)
                {
                    if (user.IsReceiveNewMessage)
                        return user;
                }

                return null;
            }
        }

        public static void AddUser(UserInfo user)
        {
            lock (LockObject)
            {
                m_users.Add(UserInfo.ParseIPEndpoint(user.RemoteIP), user);
            }
        }

        public static UserInfo FindUser(IPEndPoint ipe)
        {
            lock (LockObject)
            {
                string key = UserInfo.ParseIPEndpoint(ipe);
                if (m_users.ContainsKey(key))
                    return m_users[key];

                return null;
            }            
        }
    }
}
