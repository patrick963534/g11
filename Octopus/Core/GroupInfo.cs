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

        public GroupInfo(string key, string name)
        {

        }

        public static GroupInfo Create(string name)
        {


            return null;
        }
    }

    public static class GroupInfoManager
    {
        private static object LockObject = new object();
        private static Dictionary<string, GroupInfo> m_groups = new Dictionary<string, GroupInfo>();

        public static void AddGroup(GroupInfo group)
        {
            lock (LockObject)
            {
                m_groups.Add(group.Key, group);
            }
        }
    }
}
