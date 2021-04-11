using KyuBase.Objects;
using System;
using System.Drawing;
using System.IO;

namespace WinTest
{
    /// <summary>
    /// This is a test program, and this program is not meant to be used as an example to write your own.
    /// </summary>
    class Program
    { 
        public static KyuWIN.WinComponents.FHandle handle;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Kyu by Vrien Studios...");
            Console.WriteLine("Loading Art Resources");
            Resx.Classes.Resources.GetAllDirs(Directory.GetCurrentDirectory() + "\\resx", new string[] { });

            foreach (string str in Resx.Classes.Resources.dirs)
            {
                Console.WriteLine(str);
            }

            Resx.Classes.Resources.LoadImages();
            handle = new KyuWIN.WinComponents.FHandle(800, 800, new Point(0,0), "VTest", false);
            Bitmap bg = new Bitmap(handle.xOffset, handle.yOffset);
            Graphics b = Graphics.FromImage(bg);
            b.Clear(Color.Black);
            FObject fobj = new FObject(0, 0, bg);
            handle.AddFObject(fobj);
        }
    }
}
