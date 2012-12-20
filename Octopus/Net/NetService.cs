using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Octopus.Commands;
using Octopus.Core;

namespace Octopus.Net
{
    public class NetService
    {
        public static int ListenPort = 51378;
        private static NetService s_singleton = new NetService();
        private static List<NetPackage> s_tempPackages = new List<NetPackage>();

        private Thread m_thread;
        private Socket m_socket;
        private bool m_isRunning;
        private int log_timer;
        private int user_refresh_timer;

        private NetService()
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());           

            m_thread = new Thread(new ThreadStart(run));
            m_thread.Name = "Octopus_NetService";

            try
            {
                m_isRunning = true;

                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_socket.EnableBroadcast = true;
                m_socket.Blocking = false;
                m_socket.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
                Logger.WriteLine(UserInfo.ParseIPEndpoint((IPEndPoint)m_socket.LocalEndPoint));
            }
            catch (System.Exception ex)
            {
                Logger.WriteLine("Fail to create service socket. please restart it.");
                Logger.WriteLine(ex.Message);

                m_isRunning = false;
            }            
        }

        public static void Start()
        {
            s_singleton.m_thread.Start();
        }

        public static void Stop()
        {
            s_singleton.m_thread.IsBackground = true;
            s_singleton.m_isRunning = false;
        }

        private void run()
        {
            int prev = DateTime.Now.Millisecond;

            while (m_isRunning)
            {
                int now = DateTime.Now.Millisecond;
                int ellapse = Math.Max(0, now - prev);
                prev = now;

                IncomingPackagePool.Receive(m_socket);
                thread_command();
                thread_outgoing(ellapse);
                thread_notify_log_message(ellapse);
                thread_refresh_user_list(ellapse);

                Thread.Sleep(60);
            }
        }

        private void thread_command()
        {
            Cmd cmd = CommandPool.EndQueue();
            while (cmd != null)
            {
                cmd.Execute();
                cmd = CommandPool.EndQueue();
            }
        }

        private void thread_outgoing(int ellapse)
        {
            s_tempPackages.Clear();
            OutgoingPackagePool.GrabProcessPackages(ellapse, s_tempPackages, 5);
            foreach (NetPackage p in s_tempPackages)
            {
                if (p.OrderID == 1)
                {
                    Logger.CounterCommand_Send((NetCommandType)p.CommandID);
                    Logger.WriteLine("Send Command: " + ((NetCommandType)p.CommandID).ToString());
                }

                m_socket.SendTo(p.Buffer, p.Buffer.Length, SocketFlags.None, p.RemoteEP);
            }

            s_tempPackages.Clear();
            OutgoingPackagePool.DequeueUnprocess(s_tempPackages, 10);
            foreach (NetPackage p in s_tempPackages)
            {
                if (p.OrderID == 1)
                {
                    Logger.CounterCommand_Send((NetCommandType)p.CommandID);
                    Logger.WriteLine("Send Command: " + ((NetCommandType)p.CommandID).ToString());
                }

                m_socket.SendTo(p.Buffer, p.Buffer.Length, SocketFlags.None, p.RemoteEP);
            }

            OutgoingPackagePool.RemoveDeadProcessed();
        }

        private void thread_notify_log_message(int ellapse)
        {
            log_timer += ellapse;
            if (log_timer > 300)
            {
                log_timer = 0;
                Workbench.NotifyUpdateLog();
            }
        }

        private void thread_refresh_user_list(int ellapse)
        {
            user_refresh_timer += ellapse;
            if (user_refresh_timer > 60 * 1000)
            {
                user_refresh_timer = 0;
                OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());
            }

        }
    }
}
