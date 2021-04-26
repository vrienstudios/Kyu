using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resx.Classes;
using System.Drawing;
using KyuBase.UIElements;

namespace WinTest.Scenes
{
    class mainMenu : KyuBase.Engine.SceneBase
    {
        public override FObject[] LoadScene()
        {
            //Resource res = Resources.GetResource("beachmoon-1.jpg");
            //res.resize(800, 800);
            //Bitmap b = res.bmp;
            Bitmap b = (Bitmap)Bitmap.FromFile(@"F:\Work\Programming\K\Kyu\WinTest\bin\Debug\net5.0-windows\resx\ArtResources\bgs\beachmoon-1.jpg");
            b = Resources.ResizeBmp(800, 800, b);
            FObject background = new FObject(0, 0, b);
            background.onMouseHover += Background_onMouseHover;
            ChoiceList menus = new ChoiceList(0, 0, 10, new string[] { "Play", "Credits" }, Resources.ResizeBmp(200,150, Resources.GetResource("building-1.jpg").asBmp()));
            

            return new FObject[] { menus.choices[0], menus.choices[1], background };
        }

        private void Background_onMouseHover(FObject fObject, int x, int y)
        {
            Console.WriteLine("Hovering");
        }

        public override KyuBase.Engine.SceneBase NextScene()
        {
            throw new NotImplementedException();
        }
    }
}
