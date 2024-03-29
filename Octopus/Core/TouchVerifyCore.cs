﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Octopus.Base;
using System.Windows.Forms;

namespace Octopus.Core
{
    public class TouchVerifyCore
    {
        private static TouchVerifyCore s_singleton = new TouchVerifyCore();
        private static string PressTouchMessage = "快去奉献你的指纹吧, 否则后果自负~~!!";
        private static string LunchMessage = "是时间进行肠胃建设了... :D";
        private static string AfternoonRestMessage = "出去走动走动吧~~";

        private Info info = new Info();
        private bool m_isRunning;
        private Thread thread;

        public static void Start()
        {
            s_singleton.m_isRunning = true;

            s_singleton.thread = new Thread(new ThreadStart(s_singleton.run));
            s_singleton.thread.Name = "Touch_Verify_Core_Thread";
            s_singleton.thread.Start();
        }

        public static void Stop()
        {
            s_singleton.m_isRunning = false;
            s_singleton.thread.IsBackground = true;
        }

        private void run()
        {
            Thread.Sleep(3 * 1000);

            try
            {
                thread_run();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.WriteLine(ex.Message);
            }
        }

        private void thread_run()
        {
            while (m_isRunning)
            {
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;

                if (s_singleton.info.day != DateTime.Now.Day)
                {
                    s_singleton.info.day = DateTime.Now.Day;
                    s_singleton.info.morning = false;
                    s_singleton.info.afternoon = false;
                }

                if (!s_singleton.info.morning && hour == 8)
                {
                    s_singleton.info.morning = TouchVerify.Tell(PressTouchMessage);
                }
                else if (!s_singleton.info.afternoon && hour >= 18)
                {
                    s_singleton.info.afternoon = TouchVerify.Tell(PressTouchMessage);
                }
                else if (!s_singleton.info.lunch && ((hour == 11 && minute >= 55) || (hour == 12)))
                {
                    s_singleton.info.lunch = TouchVerify.Tell(LunchMessage);
                }
                else if (!s_singleton.info.afternoon_rest && (hour == 15 && minute >= 45))
                {
                    s_singleton.info.afternoon_rest = TouchVerify.Tell(AfternoonRestMessage);
                }

                Thread.Sleep(4 * 1024);
            }
        }

        private class Info
        {
            public int day;
            public bool morning;
            public bool afternoon;
            public bool lunch;
            public bool afternoon_rest;
        }
    }
}
