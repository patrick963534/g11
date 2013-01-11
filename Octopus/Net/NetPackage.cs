using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Octopus.Commands;
using Octopus.Core;

namespace Octopus.Net
{
    public class NetPackage
    {
        public static int MagicNumber = 0x13572468;
        public static int CounterPackageID;
        public static int CounterContentID;
        private static object m_lockobject = new object();

        public byte[] Buffer;
        public int ID;
        public NetCommandType CommandID;
        public int ContentID;
        public int OrderID;
        public int PackageCount;
        public int Size;
        public IPEndPoint RemoteEP;

        public static int GeneratePackageID()
        {
            lock (m_lockobject)
            {
                if (CounterPackageID == 0)
                {
                    CounterPackageID = (int)(DateTime.Now.Ticks % int.MaxValue);
                }

                return CounterPackageID++;
            }
        }

        public static int GenerateContentID()
        {
            lock (m_lockobject)
            {
                if (CounterContentID == 0)
                {
                    CounterContentID = (int)(DateTime.Now.Ticks % int.MaxValue);
                }

                return CounterContentID++;
            }
        }

        public static NetPackage Parse(byte[] buffer, int sz, IPEndPoint ep)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                int magic = br.ReadInt32();
                if (magic != MagicNumber)
                    return null;

                NetPackage package = new NetPackage();
                package.RemoteEP = new IPEndPoint(ep.Address, ep.Port);
                package.ID = br.ReadInt32();
                package.ContentID = br.ReadInt32();
                package.OrderID = br.ReadInt32();

                if (package.OrderID == 1)
                {
                    package.CommandID = (NetCommandType)br.ReadInt32();
                    package.PackageCount = br.ReadInt32();
                }

                package.Size = sz - (int)br.BaseStream.Position;
                package.Buffer = new MemoryStream(buffer, 0, sz).ToArray();

                return package;
            }
        }

        public static NetPackage Create(int contentID, int orderID, NetCommandType commandID, int packageCount, byte[] data, IPEndPoint iep)
        {
            using (BinaryWriter bw = new BinaryWriter(new MemoryStream()))
            {
                int packageID = GeneratePackageID();

                bw.Write(MagicNumber);
                bw.Write(packageID);
                bw.Write(contentID);
                bw.Write(orderID);

                if (orderID == 1)
                {
                    bw.Write((int)commandID);
                    bw.Write(packageCount);
                }

                bw.Write(data, 0, data.Length);
                bw.Flush();
                bw.BaseStream.Seek(0, SeekOrigin.Begin);
                
                NetPackage package = new NetPackage();

                package.RemoteEP = new IPEndPoint(iep.Address, iep.Port);
                package.ID = packageID;
                package.ContentID = contentID;
                package.OrderID = orderID;
                package.CommandID = commandID;
                package.PackageCount = packageCount;
                package.Buffer = ((MemoryStream)bw.BaseStream).ToArray();
                package.Size = data.Length;

                return package;
            }
        }

        public static NetPackage[] ContentCreate(NetCommandType commandID, byte[] data, IPEndPoint iep)
        {
            int contentID = NetPackage.GenerateContentID();

            int max_pkg_sz = 384;
            int offset = 0;
            int order = 1;
            int count = (data.Length + max_pkg_sz - 1) / max_pkg_sz;
            NetPackage[] packages = new NetPackage[count];

            int length = data.Length;
            while (length != 0)
            {
                int sz = Math.Min(384, length);
                byte[] buf = new MemoryStream(data, offset, sz).ToArray();
                packages[order - 1] = (NetPackage.Create(contentID, order, commandID, count, buf, iep));

                order++;
                offset += sz;
                length -= sz;
            }

            return packages;
        }
    }

    public class NetPackageGenerater
    {
        public static NetPackage[] BroadcastFindUser()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, NetService.SocketReadPort);
            return NetPackage.ContentCreate(NetCommandType.BroadcastFindUser, Helper.GetBytes(DataManager.WhoAmI), iep);
        }

        public static NetPackage[] AddUser(IPEndPoint iep)
        {
            return NetPackage.ContentCreate(NetCommandType.AddUser, Helper.GetBytes(DataManager.WhoAmI), iep);
        }

        public static NetPackage[] AppendTextMessage(string msg, IPEndPoint iep)
        {
            return NetPackage.ContentCreate(NetCommandType.AppendTextMessage, Helper.GetBytes(msg), iep);
        }

        public static NetPackage[] TellReceived(int packageID, IPEndPoint iep)
        {
            return NetPackage.ContentCreate(NetCommandType.RemoveProcessedPackage, BitConverter.GetBytes(packageID), iep);
        }

        public static NetPackage[] FindGroupUser(string groupKey, IPEndPoint iep)
        {
            return NetPackage.ContentCreate(NetCommandType.FindGroupUser, Helper.GetBytes(groupKey + ";" + DataManager.WhoAmI), iep);
        }

        public static NetPackage[] AddGroupUser(string groupKey, IPEndPoint iep)
        {
            return NetPackage.ContentCreate(NetCommandType.AddGroupUser, Helper.GetBytes(groupKey), iep);
        }

        public static NetPackage[] AppendGroupTextMessage(string groupKey, string msg, IPEndPoint iep)
        {
            string val = string.Format("{0};{1}", groupKey, msg);
            return NetPackage.ContentCreate(NetCommandType.AppendGroupTextMessage, Helper.GetBytes(val), iep);
        }

        public static NetPackage[] CreateNewGroup(string groupKey, string name, IPEndPoint iep)
        {
            string val = groupKey + ";" + name;
            return NetPackage.ContentCreate(NetCommandType.CreateNewGroup, Helper.GetBytes(val), iep);
        }

        public static NetPackage[] AppendImageMessage(string imagePath, IPEndPoint iep)
        {
            MemoryStream stream = new MemoryStream();
            byte[] bytes = null;

            bytes = Helper.GetBytes(Path.GetFileName(imagePath));
            stream.Write(Helper.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);

            bytes = File.ReadAllBytes(imagePath);
            stream.Write(bytes, 0, bytes.Length);

            bytes = stream.ToArray();
            stream.Dispose();

            return NetPackage.ContentCreate(NetCommandType.AppendImageMessage, bytes, iep);
        }

        public static NetPackage[] AppendGroupImageMessage(string groupKey, string imagePath, byte[] imageData, IPEndPoint iep)
        {
            MemoryStream stream = new MemoryStream();
            byte[] bytes = null;

            bytes = Helper.GetBytes(Path.GetFileName(groupKey));
            stream.Write(Helper.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);

            bytes = Helper.GetBytes(Path.GetFileName(imagePath));
            stream.Write(Helper.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);

            stream.Write(imageData, 0, imageData.Length);

            bytes = stream.ToArray();
            stream.Dispose();

            return NetPackage.ContentCreate(NetCommandType.AppendGroupImageMessage, bytes, iep);
        }

        public static NetPackage[] UserOffline(IPEndPoint iep)
        {
            return NetPackage.ContentCreate(NetCommandType.UserOffline, Helper.GetBytes(DataManager.WhoAmI), iep);
        }

        public static NetPackage[] VersionUpdate(byte[] updateFileData, IPEndPoint iep)
        {
            byte[] bytes = Helper.GetBytes(DataManager.Version);
            MemoryStream ms = new MemoryStream();

            ms.Write(Helper.GetBytes(bytes.Length), 0, 4);
            ms.Write(bytes, 0, bytes.Length);

            ms.Write(Helper.GetBytes(updateFileData.Length), 0, 4);
            ms.Write(updateFileData, 0, updateFileData.Length);

            return NetPackage.ContentCreate(NetCommandType.VersionUpdate, ms.ToArray(), iep);
        }
    }
}
