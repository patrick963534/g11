using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Octopus.Core
{
    internal static class Helper
    {
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

        internal static int BytesToInt(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            BinaryReader r = new BinaryReader(ms);

            int v = r.ReadInt32();

            r.Close();
            return v;
        }
    }
}
