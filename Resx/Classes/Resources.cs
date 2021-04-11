using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Resx.Classes
{
    public class Resources
    {
        public static List<Resource> resources = new List<Resource>();
        public static List<Resources> resxObj = new List<Resources>();
        private static FHandle fh;

        /// <summary>
        /// Create a Resources class that will look for files in a specific path.
        /// </summary>
        /// <param name="path">Path in which the resources are located.</param>
        public Resources(string path)
        {
            GetDirs(path);
        }

        /// <summary>
        /// Create a Resources class that will look for files in a specific path and will associate the resources with an specific FHandle.
        /// </summary>
        /// <param name="path">Path in which the resources are located.</param>
        public Resources(string path, FHandle fhandle)
        {
            fh = fhandle;
            GetDirs(path);
        }

        public static void Load(string pth) => GetDirs(pth);

        public static void Load(string pth, FHandle fha)
        {
            fh = fha;
            GetDirs(pth);
        }

        public static Bitmap ResizeBmp(int width, int height, Image image)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Resource GetResource(string resourceName)
        {
            try
            {
                return resources.First(res => res.name == resourceName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get files and directories in a certain path and adds them to the resources list.
        /// </summary>
        /// <param name="path">Path in which the files are located.</param>
        /// <returns>The resources lists</returns>
        public static List<Resource> GetDirs(string path)
        {
            // Path starts with / or \ ? Then search in the current directory appended by the path, else search in the path
            string directory = path.StartsWith("/") || path.StartsWith(@"\") ? Directory.GetCurrentDirectory() + path : path;
            foreach (string dir in Directory.GetFiles(directory))
            {
                resources.Add(new Resource(dir, fh));
            }
            return resources;
        }
    }
}
