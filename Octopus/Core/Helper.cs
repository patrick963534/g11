using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Octopus.Core
{
    public static class Helper
    {
        public static byte[] GetBytes(int val)
        {
            return BitConverter.GetBytes(val);
        }

        public static byte[] GetBytes(string val)
        {
            return Encoding.UTF8.GetBytes(val);
        }

        public static string GetString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static int GetInt(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        public static string FormatMessage(string user, string msg)
        {
            return string.Format("[{0}][{1}:{2}:{3}]\r\n{4}\r\n",
                user, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, msg);
        }
    }
}
