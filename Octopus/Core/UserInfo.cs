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
        public bool IsReceiveNewMessage;
        public MessageStore MessageStore = new MessageStore();

        private ChatForm Chatter; 
        
        public UserInfo(IPEndPoint ipe, string name)
        {
            RemoteIP = ipe;
            Username = name;
        }

        public override string ToString()
        {
            return Username;
        }

        public void ExitChatter()
        {
            Chatter = null;
        }

        public void ShowChatter()
        {
            if (Chatter == null)
                Chatter = new ChatForm(this);

            Chatter.Show();
            Chatter.Activate();
            Chatter.ShowMessage();

            IsReceiveNewMessage = false;
        }

        public void AppendMessage(string msg, string name)
        {
            MessageStore.AppendMessage(msg, name);
            
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

        public string ToNetString()
        {
            return string.Format("{0},{1}", GetToken(), Username);
        }

        public static UserInfo FromNetString(string netString)
        {
            string[] subs = netString.Split(',');
            if (subs.Length != 2)
                return null;

            string[] addr_strings = subs[0].Split(':');
            if (addr_strings.Length != 2)
                return null;

            IPAddress addr = IPAddress.Parse(addr_strings[0]);
            int port = int.Parse(addr_strings[1]);
            string name = subs[1];
            return new UserInfo(new IPEndPoint(addr, port), name);
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

        public static void DeleteUser(UserInfo user)
        {
            lock (LockObject)
            {
                string token = user.GetToken();
                if (m_users.ContainsKey(token))
                    m_users.Remove(token);
            }
        }

        public static void AddUser(UserInfo user)
        {
            lock (LockObject)
            {
                string token = user.GetToken();
                if (!m_users.ContainsKey(token))
                    m_users.Add(token, user);
            }
        }

        public static int GetUserCount()
        {
            lock (LockObject)
            {
                return m_users.Count;
            }
        }

        public static bool ContainsUser(IPEndPoint ipe)
        {
            lock (LockObject)
            {
                string key = UserInfo.ToUserToken(ipe);
                return m_users.ContainsKey(key);
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
