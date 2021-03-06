using KyuBase.Objects;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using WinTest.Scenes;

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

            screenClass screenDim = new screenClass();

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
            fobj.onMouseHover += Fobj_onMouseHover;
            handle.AddFObject(fobj);
            fobj.DrawStringCenter("Thank you for playing!\nClick to Continue");
            fobj.DrawString(0, 0, "Beta");

            fobj.onClick += Fobj_onClick;
        }

        private static void Fobj_onClick(FObject fObject, int x, int y)
        {
            mainMenu m = new mainMenu();
            handle.FObjects.Clear();
            handle.FObjects = m.LoadScene().ToList();
        }

        private static void Fobj_onMouseHover(FObject fObject, int x, int y)
        {
            Console.WriteLine("{0}, {1}", x, y);
        }
    }
}
