using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OctopusServer.Core
{
    internal class Helper
    {
        internal static string BytesToString(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            StreamReader r = new StreamReader(ms);
            string s = r.ReadToEnd();
            r.Close();
            return s;
        }

        internal static byte[] StringToBytes(string val)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter w = new StreamWriter(ms);
            w.Write(val);
            w.Flush();

            byte[] bytes = ms.ToArray();

            w.Close();
            return bytes;
        }

        internal static byte[] IntToBytes(int val)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter w = new BinaryWriter(ms);
            w.Write(val);

            byte[] bytes = ms.ToArray();

            w.Close();
            return bytes;
        }
    }
}
