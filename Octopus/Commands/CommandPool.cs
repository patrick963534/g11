using System;
using System.Collections.Generic;
using System.Text;

namespace Octopus.Commands
{
    public class CommandPool
    {
        private static CommandPool s_singleton;
        private static object lock_obj = new object();

        private Queue<Cmd> m_commands = new Queue<Cmd>();

        static CommandPool()
        {
            s_singleton = new CommandPool();
        }

        public static void AddCommand(Cmd cmd)
        {
            if (cmd == null)
                return;

            lock (lock_obj)
            {
                s_singleton.m_commands.Enqueue(cmd);
            }            
        }

        public static Cmd EndQueue()
        {
            lock (lock_obj)
            {
                if (s_singleton.m_commands.Count == 0)
                    return null;

                return s_singleton.m_commands.Dequeue();
            }            
        }
    }
}
