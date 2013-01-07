using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Octopus.Commands;
using System.IO;
using Octopus.Core;

namespace Octopus.Net
{
    public class IncomingPackagePool
    {
        private static IncomingPackagePool s_singleton = new IncomingPackagePool();
        private static byte[] buffer = new byte[102400];

        private Dictionary<string, Sender> m_senders = new Dictionary<string, Sender>();

        static IncomingPackagePool()
        { }

        public static bool Receive(Socket socket)
        {
            if (socket.Available <= 0)
                return false;

            try
            {
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, 0);
                EndPoint ep = iep;
                bool received = false;

                int len = socket.ReceiveFrom(buffer, ref ep);
                while (len != 0)
                {
                    received = true;
                    Logger.Counter_Pkg_Recv();
                    s_singleton.AddPackage(NetPackage.Parse(buffer, len, (IPEndPoint)ep));

                    if (socket.Available <= 0)
                        break;

                    len = socket.ReceiveFrom(buffer, ref ep);
                }

                return received;
            }
            catch (Exception e)
            {
                Logger.WriteLine(e.Message);
            }
            
            return false;
        }

        private void AddPackage(NetPackage package)
        {
            if (package == null)
                return;

            if (package.RemoteEP.Port != NetService.SocketSendPort)
                return;

            package.RemoteEP.Port = NetService.SocketReadPort;

            if (!package.IsRemoveProcessedPackageType)
                OutgoingPackagePool.AddFirst(NetPackageGenerater.TellReceived(package.ID, package.RemoteEP));

            string key = UserInfo.ToUserToken(package.RemoteEP);
            Sender sender = null;
            if (m_senders.ContainsKey(key))
            {
                sender = m_senders[key];
            }
            else
            {
                sender = new Sender(package.RemoteEP);
                m_senders.Add(key, sender);
            }

            sender.AddPackage(package);
        }

        private class Sender
        {
            public IPEndPoint RemoteEP;
            private Dictionary<int, Content> m_contents = new Dictionary<int, Content>();
            private Dictionary<int, int> m_pkg_ids = new Dictionary<int, int>();

            public Sender(IPEndPoint remoteEP)
            {
                RemoteEP = remoteEP;
            }

            public void AddPackage(NetPackage package)
            {
                if (package == null)
                    return;

                if (m_pkg_ids.ContainsKey(package.ID))
                    return;

                m_pkg_ids.Add(package.ID, package.ID);

                Content content = null;

                if (m_contents.ContainsKey(package.ContentID))
                {
                    content = m_contents[package.ContentID];
                }
                else
                {
                    content = new Content(package.ContentID);
                    m_contents.Add(package.ContentID, content);
                }

                content.Add(package);

                if (content.IsFull())
                {
                    Cmd cmd = null;
                    byte[] data = content.CombineData();

                    Logger.CounterCommand_Recv(content.CommandType);
                    if (content.CommandType != NetCommandType.RemoveProcessedPackage)
                        Logger.WriteLine("Recv Command: " + content.CommandType.ToString());

                    switch (content.CommandType)
                    {
                        case NetCommandType.AppendTextMessage:
                            cmd = new NP_AppendTextMessageCmd(data, RemoteEP); 
                            break;
                        case NetCommandType.RemoveProcessedPackage: 
                            cmd = new NP_RemoveProcessedPackageCmd(data, RemoteEP); 
                            break;
                        case NetCommandType.BroadcastFindUser:
                            cmd = new NP_BroadcastFindUserCmd(data, RemoteEP);
                            break;
                        case NetCommandType.AddUser:
                            cmd = new NP_AddUserCmd(data, RemoteEP);
                            break;
                        case NetCommandType.FindGroupUser:
                            cmd = new NP_FindGroupUserCmd(data, RemoteEP);
                            break;
                        case NetCommandType.AddGroupUser:
                            cmd = new NP_AddGroupUserCmd(data, RemoteEP);
                            break;
                        case NetCommandType.AppendGroupTextMessage:
                            cmd = new NP_AppendGroupTextMessageCmd(data, RemoteEP);
                            break;
                        case NetCommandType.CreateNewGroup:
                            cmd = new NP_CreateNewGroupCmd(data, RemoteEP);
                            break;
                        case NetCommandType.AppendImageMessage:
                            cmd = new NP_AppendImageMessageCmd(data, RemoteEP);
                            break;
                        case NetCommandType.AppendGroupImageMessage:
                            cmd = new NP_AppendGroupImageMessageCmd(data, RemoteEP);
                            break;
                        case NetCommandType.UserOffline:
                            cmd = new NP_UserOfflineCmd(data, RemoteEP);
                            break;
                        case NetCommandType.VersionUpdate:
                            cmd = new NP_VersionUpdateCmd(data, RemoteEP);
                            break;
                        
                        default: break;
                    }

                    CommandPool.AddCommand(cmd);
                    m_contents.Remove(content.ID);
                }
            }
        }

        private class Content
        {
            private int m_ID;
            private int m_count;
            private NetCommandType type;
            private Dictionary<int, NetPackage> m_packages = new Dictionary<int, NetPackage>();

            public Content(int ID)
            {
                m_ID = ID;
            }

            public int ID
            {
                get { return m_ID; }
            }

            public NetCommandType CommandType
            {
                get { return type; }
            }

            public void Add(NetPackage package)
            {
                if (m_packages.ContainsKey(package.OrderID))
                    return;
                else
                    m_packages.Add(package.OrderID, package);
                
                if (package.OrderID == 1)
                {
                    type = (NetCommandType)package.CommandID;
                    m_count = package.PackageCount;
                }
            }

            public bool IsFull()
            {
                return m_packages.Count == m_count;
            }

            public byte[] CombineData()
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    List<NetPackage> nps = new List<NetPackage>(m_packages.Values);
                    nps.Sort(new PackageComparer());

                    foreach (NetPackage np in nps)
                    {
                        ms.Write(np.Buffer, np.Buffer.Length - np.Size, np.Size);
                    }

                    return ms.ToArray();
                }
            }

            private class PackageComparer : IComparer<NetPackage>
            {
                #region IComparer<NetPackage> Members

                public int Compare(NetPackage x, NetPackage y)
                {
                    if (x.OrderID < y.OrderID)
                        return -1;
                    else if (x.OrderID == y.OrderID)
                        return 0;
                    else
                        return 1;
                }

                #endregion
            }
        }
    }
}
