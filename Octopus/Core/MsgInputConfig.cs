using System;
using System.Collections.Generic;
using System.Text;

namespace Octopus.Core
{
    public class MsgInputConfig
    {
        public static string FontName = "微软雅黑";
        public static string FontSize = "12";
        public static string FontColor = "#111111";

        public static string FormatMessage(string msg)
        {
            StringBuilder builder = new StringBuilder();
            string[] lines = msg.Split(new char[] { '\n' });
            foreach (string line in lines)
            {
                builder.Append(string.Format("<label style='font-family:{0};font-size:{1};color:{2}'>{3}</label>",
                                FontName, FontSize, FontColor, line.Replace(" ", "&nbsp")));
                builder.Append("<br/>");
            }

            return builder.ToString();
        }

        public static string FormatImageMessage(string imgFilename)
        {
            return string.Format("<img src='{0}'></img><br/>", imgFilename);
        }

        public static string FormatUsername(string user)
        {
            return string.Format("<label style='font-family:微软雅黑;font-size:10pt;color:#1746AF'>{0}&nbsp&nbsp{1}</lable>", 
                user, DateTime.Now.ToShortTimeString());
        }
    }
}
