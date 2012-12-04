using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace OctopusServer.Core
{
    public class NetServer
    {
        private static NetServer s_singleton;

        private Thread thread;
        private Socket socket;

        static NetServer()
        {
            s_singleton = new NetServer();
        }

        public static void Start()
        {
            s_singleton.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s_singleton.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            s_singleton.socket.Bind(new IPEndPoint(IPAddress.Any, 31937));

            s_singleton.thread = new Thread(new ThreadStart(s_singleton.run));
            s_singleton.thread.Name = "NetServer_Thread";
            s_singleton.thread.IsBackground = true;
            s_singleton.thread.Start();

            Workbench.Log("Start Updating Service...");
        }

        private void run()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                EndPoint from = new IPEndPoint(IPAddress.Any, 0);
                int length = socket.ReceiveFrom(bytes, ref from);
                if (length != 0)
                {
                    ClientInfo info = ClientInfo.Create(bytes);
                    if (info == null)
                        continue;

                    if (info.Command.Contains("Octopus_Update"))
                    {
                        Workbench.Log(string.Format("Data Require from: {0}", from.ToString()));
                        Workbench.Log("Begin Sending Data...");
                        Workbench.Log("Remove original path: " + info.Path);
                        Workbench.Log("Version: " + info.Version);

                        if (info.Version.Contains(DataManager.Version))
                            continue;

                        ((IPEndPoint)from).Port = info.Port;

                        byte[] data_bytes = File.ReadAllBytes(DataManager.UpdateFilePath);
                        socket.SendTo(Helper.IntToBytes(data_bytes.Length), from);

                        int sz = data_bytes.Length;
                        int offset = 0;
                        while (offset < sz)
                        {
                            Thread.Sleep(50);

                            int batch = Math.Min(10 * 1024, sz - offset);
                            socket.SendTo(data_bytes, offset, batch, SocketFlags.None, from);
                            offset += batch;
                        }

                        Workbench.Log("Finish Sending Data.");
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private class ClientInfo
        {
            public string Command;
            public int Port;
            public string Version;
            public string Path;

            public static ClientInfo Create(byte[] bytes)
            {
                ClientInfo info = new ClientInfo();
                string[] subs = Helper.BytesToString(bytes).Split(';');
                if (subs.Length != 4)
                    return null;

                info.Command = subs[0];
                info.Port = int.Parse(subs[1]);
                info.Path = subs[2];
                info.Version = subs[3];

                return info;
            }
        }
    }
}
