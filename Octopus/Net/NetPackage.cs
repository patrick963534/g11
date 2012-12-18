using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Octopus.Commands;

namespace Octopus.Net
{
    internal class NetPackage
    {
        public static int MagicNumber = 0x13572468;
        public static int CounterPackageID = 1;
        public static int CounterContentID = 1;

        public byte[] Buffer;
        public int ID;
        public int CommandID;
        public int ContentID;
        public int OrderID;
        public int PackageCount;
        public int Size;
        public IPEndPoint RemoteEP;

        public static int GeneratePackageID()
        {
            return CounterPackageID++;
        }

        public static int GenerateContentID()
        {
            return CounterContentID++;
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
                    package.CommandID = br.ReadInt32();
                    package.PackageCount = br.ReadInt32();
                }

                package.Size = sz - (int)br.BaseStream.Position;
                package.Buffer = new MemoryStream(buffer, 0, sz).ToArray();

                return package;
            }
        }

        public static NetPackage Create(int contentID, int orderID, int commandID, int packageCount, byte[] data, IPEndPoint iep)
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
                    bw.Write(commandID);
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
    }

    internal class NetPackageGenerater
    {
        public static NetPackage BroadcastFindUser()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, NetService.ListenPort);
            byte[] data = Encoding.UTF8.GetBytes(Dns.GetHostName());
            int contentID = NetPackage.GenerateContentID();
            return NetPackage.Create(contentID, 1, (int)NetCommandType.BroadcastFindUser, 1, data, iep);
        }

        public static NetPackage AddUser(IPEndPoint iep)
        {
            byte[] data = Encoding.UTF8.GetBytes(Dns.GetHostName());
            int contentID = NetPackage.GenerateContentID();
            return NetPackage.Create(contentID, 1, (int)NetCommandType.AddUser, 1, data, iep);
        }

        public static NetPackage AppendTextMessage(string msg, IPEndPoint iep)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            int contentID = NetPackage.GenerateContentID();
            return NetPackage.Create(contentID, 1, (int)NetCommandType.AppendTextMessage, 1, data, iep);
        }

        public static NetPackage TellReceived(int packageID, IPEndPoint iep)
        {
            byte[] data = BitConverter.GetBytes(packageID);
            int contentID = NetPackage.GenerateContentID();
            return NetPackage.Create(contentID, 1, (int)NetCommandType.RemoveProcessedPackage, 1, data, iep);
        }
    }
}
