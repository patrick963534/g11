using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Octopus.Base;

namespace Octopus.Core
{
    public class TouchVerifyCore
    {
        private static TouchVerifyCore s_singleton;

        private Info info = new Info();
        private Thread thread;

        static TouchVerifyCore()
        {
            s_singleton = new TouchVerifyCore();
        }

        public static void Start()
        {
            s_singleton.thread = new Thread(new ThreadStart(s_singleton.run));
            s_singleton.thread.Name = "Touch_Verify_Core_Thread";
            s_singleton.thread.IsBackground = true;
            s_singleton.thread.Start();
        }

        public static void Stop()
        {
            s_singleton.thread.Abort();
        }

        private void run()
        {
            Thread.Sleep(3 * 1000);

            while (true)
            {
                if (s_singleton.info.day != DateTime.Now.Day)
                {
                    s_singleton.info.day = DateTime.Now.Day;
                    s_singleton.info.morning = false;
                    s_singleton.info.afternoon = false;
                }

                if (DateTime.Now.Hour <= 9 && !s_singleton.info.morning)
                {
                    s_singleton.info.morning = TouchVerify.Tell();
                }
                else if (DateTime.Now.Hour >= 18 && !s_singleton.info.afternoon)
                {
                    s_singleton.info.afternoon = TouchVerify.Tell();
                }

                Thread.Sleep(4 * 1024);
            }
        }

        private class Info
        {
            public int day;
            public bool morning;
            public bool afternoon;
        }
    }
}
