using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Octopus.Core
{
    public class CustomFaceItem
    {
        public int ID;
        public Image Icon;
        public string Filename;
    }

    public class CustomFaceManager
    {
        private static string cfg_file = "CustomFace.cfg";
        private static int MagicNumber = 0x192742;

        private static List<CustomFaceItem> m_items = new List<CustomFaceItem>();
        private static Dictionary<string, string> m_item_names = new Dictionary<string, string>();
        private static bool m_init;

        public const int IconSize = 32;

        private static void Init()
        {
            if (m_init)
                return;

            m_init = true;

            string path = GetCustomFaceCfgPath();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                int length = (int)fs.Length;
                int byteCount = 0;

                while (byteCount < length)
                {
                    CustomFaceItem item = new CustomFaceItem();

                    byte[] bytes = new byte[4];
                    fs.Read(bytes, 0, 4);
                    int magic = Helper.GetInt(bytes);
                    if (magic != MagicNumber)
                        return;

                    fs.Read(bytes, 0, 4);
                    int strlen = Helper.GetInt(bytes);
                    byte[] fileBytes = new byte[strlen];
                    fs.Read(fileBytes, 0, fileBytes.Length);
                    item.Filename = Helper.GetString(fileBytes);

                    fs.Read(bytes, 0, 4);
                    int imglen = Helper.GetInt(bytes);
                    byte[] imgBytes = new byte[imglen];
                    fs.Read(imgBytes, 0, imgBytes.Length);
                    item.Icon = new Bitmap(new MemoryStream(imgBytes));

                    item.ID = m_items.Count;

                    if (!m_item_names.ContainsKey(item.Filename))
                    {
                        m_item_names.Add(item.Filename, item.Filename);
                        m_items.Add(item);
                    }
                }
            }
        }

        private static string GetCustomFaceCfgPath()
        {
            return Path.Combine(DataManager.GetCfgFolderPath(), cfg_file);
        }

        private static Bitmap GenerateIconImageBytes(string imageFilenPath)
        {
            string name = Path.GetFileName(imageFilenPath);
            string path = Path.Combine(DataManager.GetCustomFaceFolderPath(), name);

            if (imageFilenPath != path)
            {
                File.Copy(imageFilenPath, path, true);
            }

            Bitmap img = new Bitmap(path);

            int w = IconSize;
            int h = IconSize;

            if (img.Width > img.Height)
                h = IconSize * img.Height / img.Width;
            else
                w = IconSize * img.Width / img.Height;

            Rectangle dst = new Rectangle((IconSize - w) / 2, (IconSize - h) / 2, w, h);
            Rectangle src = new Rectangle(0, 0, img.Width, img.Height);

            Bitmap icon = new Bitmap(IconSize, IconSize);
            Graphics g = Graphics.FromImage(icon);
            g.DrawImage(img, dst, src, GraphicsUnit.Pixel);
            g.Dispose();
            img.Dispose();

            return icon;
        }

        public static void AddCustomFace(string imageFilePath)
        {
            Init();

            string name = Path.GetFileName(imageFilePath);
            if (m_item_names.ContainsKey(name))
                return;

            m_item_names.Add(name, name);

            Bitmap icon = GenerateIconImageBytes(imageFilePath);
            MemoryStream ms = new MemoryStream();
            icon.Save(ms, ImageFormat.Png);

            using (FileStream fs = new FileStream(GetCustomFaceCfgPath(), FileMode.Append, FileAccess.Write))
            {
                byte[] bytes = null;
                byte[] length = null;

                bytes = Helper.GetBytes(MagicNumber);
                fs.Write(bytes, 0, 4);

                bytes = Helper.GetBytes(name);
                length = Helper.GetBytes(bytes.Length);

                fs.Write(length, 0, 4);
                fs.Write(bytes, 0, bytes.Length);

                bytes = ms.ToArray();
                length = Helper.GetBytes(bytes.Length);

                fs.Write(length, 0, 4);
                fs.Write(bytes, 0, bytes.Length);

                fs.Flush();
            }

            CustomFaceItem item = new CustomFaceItem();
            item.Filename = name;
            item.Icon = icon;
            item.ID = m_items.Count;
            m_items.Add(item);
        }

        public static int GetItemCount()
        {
            Init();

            return m_items.Count;
        }

        public static CustomFaceItem GetItem(int id)
        {
            Init();

            if (id < m_items.Count)
                return m_items[id];

            return null;
        }

        public static void Dispose()
        {
            foreach (CustomFaceItem item in m_items)
            {
                item.Icon.Dispose();
            }

            m_items.Clear();
            m_item_names.Clear();
            m_init = false;
        }
    }
}
