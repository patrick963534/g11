using System;
using System.Collections.Generic;
using System.Text;

namespace Octopus.Core
{
    public class MessageStore
    {
        private List<string> Messages = new List<string>();
        private List<string> MessageUsers = new List<string>();

        public int MsgCount
        {
            get { return Messages.Count; }
        }

        public string GetFormmattedMsg(int idx)
        {
            if (idx < MessageUsers.Count)
            {
                string html_usr = MsgInputConfig.FormatUsername(MessageUsers[idx]);
                string html_msg = Messages[idx];
                string all = string.Format("{0}</br>{1}</br>", html_usr, html_msg);

                return all;
            }

            return null;
        }

        public void AppendMessage(string msg, string name)
        {
            Messages.Add(msg);
            MessageUsers.Add(name);
        }
    }
}
