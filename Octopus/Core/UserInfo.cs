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

            Chatter.Show();
            Chatter.Activate();

            IsReceiveNewMessage = false;
        }

        public void AppendMessage(string msg)
        {
            if (!string.IsNullOrEmpty(Messages))
                Messages += "\r\n";

            Messages += msg;

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

        public string GetToken()
        {
            return ToUserToken(RemoteIP);
        }

        public static string ToUserToken(IPEndPoint ipe)
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

        public static UserInfo[] GetUserArray()
        {
            lock (LockObject)
            {
                List<UserInfo> users = new List<UserInfo>(m_users.Values);
                return users.ToArray();
            }
        }

        public static void AddUser(UserInfo user)
        {
            lock (LockObject)
            {
                m_users.Add(user.GetToken(), user);
            }
        }

        public static int GetUserCount()
        {
            lock (LockObject)
            {
                return m_users.Count;
            }
        }

        public static UserInfo FindUser(IPEndPoint ipe)
        {
            lock (LockObject)
            {
                string key = UserInfo.ToUserToken(ipe);
                if (m_users.ContainsKey(key))
                    return m_users[key];

                return null;
            }            
        }
    }
}
