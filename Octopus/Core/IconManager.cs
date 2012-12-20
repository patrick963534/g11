using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Octopus.Properties;

namespace Octopus.Core
{
    public class IconManager
    {
        public static Icon Tray_Normal;
        public static Icon Tray_Blank;

        static IconManager()
        {
            Tray_Normal = Resources.astah;
            Tray_Blank = Resources.tray_blank;
        }
    }
}
