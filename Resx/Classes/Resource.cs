using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Resx.Classes
{
    public enum ResxType
    {
        Image,
        Text,
        MiscFile,
    }

    public class Resource
    {
        public string name;
        public string path;
        private FHandle fh;
        public ResxType resourceType;
        public Bitmap bmp;
        public Sprite sprite;
        public FObject fobj;
        public Graphics gr;

        /// <summary>
        /// Make a resource from a path
        /// </summary>
        /// <param name="path">Path of the resource</param>
        public Resource(string path)
        {
            name = path.Split('\\').Last();
            this.path = path;
        }

        /// <summary>
        /// Make a resource from a path that will be associated with a FHandle
        /// </summary>
        /// <param name="path">Path of the resource</param>
        /// <param name="fh">FHandle</param>
        public Resource(string path, FHandle fh)
        {
            name = path.Split('\\').Last();
            this.path = path;
            this.fh = fh;
        }

        /// <summary>
        /// Make a bitmap from the resource path that can be later accessed using bmp
        /// </summary>
        /// <returns>Bitmap created</returns>
        public Bitmap asBmp()
        {
            bmp = new Bitmap(path);
            return bmp;
        }

        public Resource resize(int w, int h)
        {
            if (bmp == null)
            {
                asBmp();
            }
            bmp = Resources.ResizeBmp(w, h, bmp);
            return this;
        }

        /// <summary>
        /// Make a graphics from the resource bmp that can be later accessed using gr
        /// </summary>
        /// <returns>Graphics created</returns>
        public Graphics asGraphics()
        {
            if (bmp == null)
            {
                asBmp();
            }
            gr = Graphics.FromImage(bmp);
            return gr;
        }

        /// <summary>
        /// Make a FObject from the resource bmp that can be later accessed using fh
        /// </summary>
        /// <param name="x">X of the FObject</param>
        /// <param name="y">Y of the FObject</param>
        /// <param name="addToFHandle">Whether when the FObject is initialized, add it to the FHandle list of FObjects</param>
        /// <returns></returns>
        public FObject asFObject(int x, int y, bool addToFHandle = false)
        {
            if (bmp == null)
            {
                asBmp();
            }
            fobj = new FObject(x, y, bmp);
            if (addToFHandle || fh != null)
            {
                fh.FObjects.Add(fobj);
            }
            return fobj;
        }
    }
}
