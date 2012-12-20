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

        private static string s_msg = string.Empty;
        private static int msg_counter = 0;

        private static Dictionary<NetCommandType, int> m_cmdCounter_recv = new Dictionary<NetCommandType, int>();
        private static Dictionary<NetCommandType, int> m_cmdCounter_send = new Dictionary<NetCommandType, int>();

        public static string Msg
        {
            get { return s_msg; }
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
                msg_counter++;
                s_msg += string.Format("[{0:D6}][{1}]{2}\r\n",
                    msg_counter, DateTime.Now.ToShortTimeString(), msg);
            }            
        }
    }
}
