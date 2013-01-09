using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Octopus.Commands;
using Octopus.Core;
using System.Windows.Forms;

namespace Octopus.Net
{
    public class NetService
    {
        public static int SocketReadPort = 51668;
        public static int SocketSendPort = 52868;
        public static int SocketBufferSize = 81920;
        private static NetService s_singleton = new NetService();
        private static List<NetPackage> s_tempPackages = new List<NetPackage>();

        private Thread m_thread;
        private Socket m_read_socket;
        private Socket m_send_socket;
        private bool m_isRunning;
        private int user_refresh_timer;

        private NetService()
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());
            OutgoingPackagePool.AddFirst(NetPackageGenerater.BroadcastFindUser());
            OutgoingPackagePool.AddProcessedPackage(NetPackageGenerater.BroadcastFindUser());
            OutgoingPackagePool.AddProcessedPackage(NetPackageGenerater.BroadcastFindUser());

            m_thread = new Thread(new ThreadStart(run));
            m_thread.Name = "Octopus_NetService";

            try
            {
                m_isRunning = true;

                m_read_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_read_socket.EnableBroadcast = true;
                m_read_socket.Blocking = false;
                m_read_socket.SendBufferSize = SocketBufferSize;
                m_read_socket.ReceiveBufferSize = SocketBufferSize;
                m_read_socket.Bind(new IPEndPoint(IPAddress.Any, SocketReadPort));
                Logger.WriteLine(UserInfo.ToUserToken((IPEndPoint)m_read_socket.LocalEndPoint));

                m_send_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_send_socket.EnableBroadcast = true;
                m_send_socket.Blocking = true;
                m_send_socket.SendBufferSize = SocketBufferSize;
                m_send_socket.ReceiveBufferSize = SocketBufferSize;
                m_send_socket.Bind(new IPEndPoint(IPAddress.Any, SocketSendPort));
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
                try
                {
                    while (m_isRunning)
                    {
                        int now = DateTime.Now.Millisecond;
                        int ellapse = Math.Max(0, now - prev);
                        prev = now;

                        thread_refresh_user_list(ellapse);

                        bool work = false;
                        work |= IncomingPackagePool.Receive(m_read_socket);
                        thread_command();
                        work |= thread_outgoing(ellapse);

                        if (work)
                            Thread.Sleep(1);
                        else
                            Thread.Sleep(60);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                    Logger.WriteLine(ex.Message);
                    Logger.WriteLine("The NetService is stopped, please restart this tool.");
                }
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

        private bool thread_outgoing(int ellapse)
        {
            s_tempPackages.Clear();

            OutgoingPackagePool.GrabProcessPackages(ellapse, s_tempPackages, 5);
            OutgoingPackagePool.DequeueUnprocess(s_tempPackages);

            foreach (NetPackage p in s_tempPackages)
            {
                if (p.OrderID == 1)
                {
                    Logger.CounterCommand_Send((NetCommandType)p.CommandID);

                    if (p.CommandID != NetCommandType.RemoveProcessedPackage &&
                        p.CommandID != NetCommandType.AddUser)
                    {
                        UserInfo userinfo = UserInfoManager.FindUser(p.RemoteEP);
                        string user = (userinfo == null) ? "no user" : userinfo.Username;

                        Logger.WriteLine(string.Format("Send Command: {0}. Package ID: {1}. Target User: {2}", p.CommandID, p.ID, user));
                    }
                }

                int length = m_send_socket.SendTo(p.Buffer, p.Buffer.Length, SocketFlags.None, p.RemoteEP);
            }

            OutgoingPackagePool.RemoveDeadProcessed();

            return s_tempPackages.Count != 0;
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
