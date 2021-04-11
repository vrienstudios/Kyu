using KyuBase.Objects;
using KyuE.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Resx.Classes.GameState
{
    public static class SaveLoadHandler
    {
        /// <summary>
        /// Basic save on scene. Saves player location scene to scene.
        /// </summary>
        /// <param name="scene">The scene that the player is on</param>
        /// <param name="saveName">The name of the save game</param>
        public static void Save(Type scenes, string saveName = null)
        {
            //Embed data
            Object sc = Activator.CreateInstance(scenes);

            FObject[] fObjects = null;
            foreach (MethodInfo mi in scenes.GetMethods())
            {
                if (mi.Name == "LoadScene")
                {
                    fObjects = (FObject[])mi.Invoke(sc, null);
                    break;
                }
            }

            //TODO: Use ManualResetEvent to watch for trigger to continue.
            Thread.Sleep(1000); // Finish procedural drawing if it exists.

            StringBuilder sb = new StringBuilder();
            String date = DateTime.Now.ToString().Replace('-', '_').Replace(':', ' ');

            sb.AppendLine($"{scenes.Name}-{(saveName == null ? date : saveName)}");

            Bitmap bmp = new Bitmap(ImageResources.width, ImageResources.height);
            Graphics b = Graphics.FromImage(bmp);

            for (int idx = 0; idx < fObjects.Length; idx++)
                b.DrawImage(fObjects[idx].window, fObjects[idx].x, fObjects[idx].y);

            MemoryStream e = new MemoryStream();
            bmp.Save(e, ImageFormat.Png);

            sb.AppendLine(Convert.ToBase64String(e.ToArray()));
            sb.AppendLine("==");
            File.WriteAllText(Directory.GetCurrentDirectory() + $"\\{date}.ksf", sb.ToString());

            //Get Image
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + $"\\{date}.ksf");

            StringBuilder data = new StringBuilder();
            sr.ReadLine(); // Read first line
            while (sr != null && !sr.EndOfStream)
            {
                string ass = sr.ReadLine();
                switch (ass)
                {
                    default:
                        data.AppendLine(ass);
                        break;
                    case "==":
                        sr.Close();
                        sr = null;
                        break;
                }
            }

            Image a;
            //a.Save(Directory.GetCurrentDirectory() + $"\\{date}aa.png");
            return;
        }
    }
}
