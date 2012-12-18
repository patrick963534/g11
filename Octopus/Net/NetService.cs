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
    internal class NetService
    {
        public static int ListenPort = 51378;
        private static NetService s_singleton;

        private Thread m_thread;
        private Socket m_socket;
        private bool m_isRunning;

        static NetService()
        {
            s_singleton = new NetService();
        }

        private NetService()
        {
            m_thread = new Thread(new ThreadStart(run));
            m_thread.IsBackground = true;

            try
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_socket.EnableBroadcast = true;
                m_socket.Blocking = false;
                m_socket.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
                Workbench.Log(UserInfo.ParseIPEndpoint((IPEndPoint)m_socket.LocalEndPoint));
            }
            catch (System.Exception ex)
            {
                Workbench.Log("Fail to create service socket. please restart it.");
                Workbench.Log(ex.Message);
            }            
        }

        internal static void Start()
        {
            s_singleton.m_isRunning = true;
            s_singleton.m_thread.Start();
        }

        internal static void Stop()
        {
            s_singleton.m_isRunning = false;
        }

        private void run()
        {
            while (m_isRunning)
            {
                IncomingPackagePool.Receive(m_socket);
                CommandPool.ExecuteAll();

                NetPackage np = OutgoingPackagePool.PeekFirst();
                if (np != null)
                    m_socket.SendTo(np.Buffer, np.Buffer.Length, SocketFlags.None, np.RemoteEP);

                Thread.Sleep(100);
            }
        }
    }
}
