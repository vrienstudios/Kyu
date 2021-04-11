using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using KyuBase.Objects;

namespace KyuE.Engine
{
    public class ImageResources
    {
        public static List<Sprite> artImageResources = new List<Sprite>();

        //Dimension header
        public static int width, height;

        // All bitmaps pre-sized to correct dimensions.
        public static Bitmap text;

        public Sprite GetResource(string resourceName) => artImageResources.First(o => o.name == resourceName);

        public static Bitmap ResizeBmp(int x, int y, Bitmap bmp)
        {
            Bitmap n = new Bitmap(x, y);
            Graphics g = Graphics.FromImage(n);
            g.DrawImage(bmp, 0, 0, x, y);
            g.Dispose();
            return n;
        }

        public static void LoadImages()
        {
            foreach (string str in sDirs)
            {
                string[] aski = Directory.GetFiles(str);
                foreach (string no in aski)
                {
                    artImageResources.Add(new Sprite((Bitmap)Bitmap.FromFile(no)) { name = no.Split('\\').Last() });
                    Console.WriteLine("Loaded: {0}", no.Split('\\').Last());
                }
            }
        }

        public static SList sDirs = new SList();

        /// <summary>
        /// Load all directories found into directory listings.
        /// </summary>
        /// <param name="startingDir"></param>
        /// <param name="dirsFound"></param>
        /// <returns></returns>
        public static string[] GetAllDirs(String startingDir, string[] dirsFound)
        {
            return startingDir != null ? Directory.GetFiles(startingDir).Length > 0 && sDirs.Count() == 0 ? sDirs + startingDir == true ? eDir(Directory.GetDirectories(startingDir)) : null : Directory.GetDirectories(startingDir).Length > 0 ? GetAllDirs(null, dirsFound.Concat(Directory.GetDirectories(startingDir)).ToArray()) : null : eDir(dirsFound);
        }

        private static string[] eDir(string[] dirs)
        {
            string[] found = { };
            sDirs.AddRange(dirs);
            for (int idx = 0; idx < dirs.Length; idx++)
            {
                GetAllDirs(dirs[idx], new string[] { });
            }

            return null;
        }
    }
}
