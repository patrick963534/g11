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
    public class NetClient
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

        public static void Start()
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

        public static void Stop()
        {
            s_singleton.broadcast_thread.Abort();
            s_singleton.data_thread.Abort();

            s_singleton.broadcast_socket.Close();
            s_singleton.data_socket.Close();
        }

        private void data_run()
        {
            while (true)
            {
                Socket socket = s_singleton.data_socket;

                byte[] size_bytes = new byte[64];
                EndPoint from = new IPEndPoint(IPAddress.Any, 0);
                int rcv_length = socket.ReceiveFrom(size_bytes, 4, SocketFlags.None, ref from);
                if (rcv_length == 4)
                {
                    int sz = Helper.BytesToInt(size_bytes);
                    if (sz > 0 && sz < 100 * 1024 * 1024)
                    {
                        MemoryStream stream = new MemoryStream();
                        byte[] buffer = new byte[100 * 1024];
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

                        Process.Start("\"" + DataManager.UpdatingFile + "\"");
                        Workbench.ExitForm();
                    }
                }
            }
        }

        private void broadcast_run()
        {
            Thread.Sleep(2 * 1000);
            Workbench.Log(DataManager.Version);

            while (true)
            {
                Socket socket = s_singleton.broadcast_socket;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                Thread.Sleep(5 * 1024);

                if (socket != null)
                {
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, 31937);
                    byte[] info_bytes = Helper.StringToBytes("Octopus_Update;" + s_singleton.data_port.ToString() + ";" + DataManager.Original_Path + ";" + DataManager.Version);
                    socket.SendTo(info_bytes, groupEP);
                }

                Thread.Sleep(1000 * 1000);
            }
        }
    }
}
