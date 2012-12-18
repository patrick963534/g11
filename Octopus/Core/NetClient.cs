using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Octopus.Core
{
    internal class NetClient
    {
        private static NetClient s_singleton;

        private Thread broadcast_thread;
        private Thread data_thread;

        private Socket broadcast_socket;
        private Socket data_socket;

        private int data_port;

        static NetClient()
        {
            s_singleton = new NetClient();
        }

        internal static void Start()
        {
            s_singleton.broadcast_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s_singleton.data_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            s_singleton.broadcast_socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            s_singleton.data_socket.Bind(new IPEndPoint(IPAddress.Any, 0));

            s_singleton.data_port = ((IPEndPoint)s_singleton.data_socket.LocalEndPoint).Port;

            s_singleton.broadcast_thread = new Thread(new ThreadStart(s_singleton.broadcast_run));
            s_singleton.broadcast_thread.Name = "Broadcast_Thread";
            s_singleton.broadcast_thread.IsBackground = true;
            s_singleton.broadcast_thread.Start();

            s_singleton.data_thread = new Thread(new ThreadStart(s_singleton.data_run));
            s_singleton.data_thread.Name = "Data_Download_Thread";
            s_singleton.data_thread.IsBackground = true;
            s_singleton.data_thread.Start();
        }

        internal static void Stop()
        {
            try
            {
                s_singleton.broadcast_thread.Abort();
                s_singleton.data_thread.Abort();

                s_singleton.broadcast_socket.Close();
                s_singleton.data_socket.Close();
            }
            catch
            {
            	
            }
        }

        private void data_run()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[102400];
                    Socket socket = s_singleton.data_socket;
                    EndPoint from = new IPEndPoint(IPAddress.Any, 0);

                    int rcv_length = socket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref from);
                    if (rcv_length != 4)
                        continue;

                    int sz = Helper.BytesToInt(buffer);
                    if (sz < 0 && sz >= 10 * 1024 * 1024)
                        continue;

                    MemoryStream stream = new MemoryStream();
                    while (sz != 0)
                    {
                        from = new IPEndPoint(IPAddress.Any, 0);
                        rcv_length = socket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref from);
                        stream.Write(buffer, 0, Math.Min(rcv_length, sz));
                        sz -= rcv_length;
                    }

                    if (File.Exists(DataManager.UpdatingFile))
                        File.Delete(DataManager.UpdatingFile);

                    File.WriteAllBytes(DataManager.UpdatingFile, stream.ToArray());

                    if (!DataManager.InDevelopment)
                    {
                        Process.Start("\"" + DataManager.UpdatingFile + "\"");
                        Workbench.ExitForm();
                    }
                }
                catch
                {
                    Workbench.Log("fail to update... -_-");
                    Thread.Sleep(1000 * 1000);
                }
            }
        }

        private void broadcast_run()
        {
            Thread.Sleep(2 * 1000);

            while (true)
            {
                try
                {
                    Socket socket = s_singleton.broadcast_socket;
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                    Thread.Sleep(5 * 1000);

                    if (socket != null)
                    {
                        IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, 31937);
                        byte[] info_bytes = Helper.StringToBytes("Octopus_Update;" + s_singleton.data_port.ToString() + ";" + DataManager.AppPath + ";" + DataManager.Version);
                        socket.SendTo(info_bytes, groupEP);
                    }

                    Thread.Sleep(60 * 1000);
                }
                catch
                {
                    Workbench.Log("fail to broadcast... -_-");
                    Thread.Sleep(1000 * 1000);
                }
            }
        }
    }
}
