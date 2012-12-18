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
    internal class IncomingPackagePool
    {
        private static IncomingPackagePool s_singleton = new IncomingPackagePool();
        private static byte[] buffer = new byte[102400];

        private Dictionary<string, Sender> m_senders = new Dictionary<string, Sender>();

        static IncomingPackagePool()
        { }

        internal static bool Receive(Socket socket)
        {
            if (socket.Available <= 0)
                return false;

            try
            {
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, 0);
                EndPoint ep = iep;

                int len = socket.ReceiveFrom(buffer, ref ep);
                if (len != 0)
                {
                    s_singleton.AddPackage(NetPackage.Parse(buffer, len, (IPEndPoint)ep));
                    return true;
                }
            }
            catch (Exception e)
            {
                Workbench.Log(e.Message);
            }
            
            return false;
        }

        private void AddPackage(NetPackage package)
        {
            if (package == null)
                return;

            string key = UserInfo.ParseIPEndpoint(package.RemoteEP);
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

            public Sender(IPEndPoint remoteEP)
            {
                RemoteEP = remoteEP;
            }

            public void AddPackage(NetPackage package)
            {
                if (package == null)
                    return;

                if (package.CommandID != (int)NetCommandType.RemoveProcessedPackage)
                    OutgoingPackagePool.AddFirst(NetPackageGenerater.TellReceived(package.ID, package.RemoteEP));

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
                    UserInfo user = UserInfoManager.FindUser(RemoteEP);

                    switch (content.CommandType)
                    {
                        case NetCommandType.AppendTextMessage: 
                            cmd = new NP_AppendTextMessageCmd(data, user); 
                            break;
                        case NetCommandType.RemoveProcessedPackage: 
                            cmd = new NP_RemoveProcessedPackageCmd(package.ID); 
                            break;
                        case NetCommandType.BroadcastFindUser:
                            cmd = new NP_BroadcastFindUserCmd(data, RemoteEP);
                            break;
                        case NetCommandType.AddUser:
                            cmd = new NP_AddUserCmd(data, RemoteEP);
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
                        return 1;
                    else if (x.OrderID == y.OrderID)
                        return 0;
                    else
                        return -1;
                }

                #endregion
            }
        }
    }
}
