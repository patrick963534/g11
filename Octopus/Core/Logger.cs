using System;
using System.Collections.Generic;
using System.Text;
using Octopus.Commands;

namespace Octopus.Core
{
    public class Logger
    {
        private static Logger s_singleton = new Logger();
        private static object s_lockobject = new object();

        private static List<string> s_messages = new List<string>();
        private static int pkg_counter = 0;
        private static int dead_pkg_counter = 0;

        private static Dictionary<NetCommandType, int> m_cmdCounter_recv = new Dictionary<NetCommandType, int>();
        private static Dictionary<NetCommandType, int> m_cmdCounter_send = new Dictionary<NetCommandType, int>();

        public static void Counter_Pkg_Recv()
        {
            lock (s_lockobject)
            {
                pkg_counter++;
            }
        }

        public static void Counter_Dead_Pkg()
        {
            lock (s_lockobject)
            {
                dead_pkg_counter++;
            }
        }

        public static void CounterCommand_Recv(NetCommandType cmd)
        {
            lock (s_lockobject)
            {
                if (!m_cmdCounter_recv.ContainsKey(cmd))
                {
                    m_cmdCounter_recv.Add(cmd, 1);
                }
                else
                {
                    m_cmdCounter_recv[cmd]++;
                }
            }
        }

        public static void CounterCommand_Send(NetCommandType cmd)
        {
            lock (s_lockobject)
            {
                if (!m_cmdCounter_send.ContainsKey(cmd))
                {
                    m_cmdCounter_send.Add(cmd, 1);
                }
                else
                {
                    m_cmdCounter_send[cmd]++;
                }
            }
        }

        public static string Get_Dead_Pkg_Counter()
        {
            lock (s_lockobject)
            {
                return string.Format("Dead part packages : {0} \r\n", dead_pkg_counter);
            }
        }

        public static string Get_Recv_Part_Packages()
        {
            lock (s_lockobject)
            {
                return string.Format("Recv part packages : {0} \r\n", pkg_counter);
            }
        }

        public static string Get_Recv_CounterCommandMsg()
        {
            lock (s_lockobject)
            {
                string msg = string.Empty;

                foreach (KeyValuePair<NetCommandType, int> pair in m_cmdCounter_recv)
                {
                    msg += string.Format("{0} : {1} \r\n", pair.Key, pair.Value);
                }

                return msg;
            }
        }

        public static string Get_Send_CounterCommandMsg()
        {
            lock (s_lockobject)
            {
                string msg = string.Empty;

                foreach (KeyValuePair<NetCommandType, int> pair in m_cmdCounter_send)
                {
                    msg += string.Format("{0} : {1} \r\n", pair.Key, pair.Value);
                }

                return msg;
            }
        }

        public static void WriteLine(string msg)
        {
            lock (s_lockobject)
            {
                s_messages.Add(string.Format("[{0:D6}][{1}]{2}\r\n",
                    s_messages.Count + 1, DateTime.Now.ToShortTimeString(), msg));
            }            
        }

        public static List<string> GetMessageByIdx(ref int index)
        {
            lock (s_lockobject)
            {
                if (index < s_messages.Count - 1)
                {
                    int count = Math.Min(3000, s_messages.Count - 1 - index);

                    if (count != 0)
                    {
                        index = s_messages.Count - 1;
                        return s_messages.GetRange(s_messages.Count - 1 - count, count);
                    }
                }

                return null;
            }          
        }
    }
}
