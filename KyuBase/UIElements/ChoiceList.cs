using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace KyuBase.UIElements
{
    public class ChoiceList
    {
        public FObject[] choices;

        public ChoiceList(int x, int y, int spacing, string[] choices, Bitmap choiceBackground, bool animated = false, int timing = 10)
        {
            List<FObject> fobL = new List<FObject>();
            foreach (string choice in choices)
            {
                FObject fobj = new FObject(x, y, choiceBackground);
                fobj.DrawStringCenter(choice);
                fobj.ChangeOpacity(0.5f);

                if (animated == true)
                    fobj.AnimatedOpacityChange(0.95);
                fobL.Add(fobj);
                y += spacing;
            }
            this.choices = fobL.ToArray();
        }

        public void Shift(int x, int y)
        {
            foreach (FObject fobj in choices)
            {
                fobj.x = fobj.x + x;
                fobj.y = fobj.y + y;
            }
        }

        public void Center(int x, int y)
        {
            foreach(FObject fobj in choices)
            {
                //(this.window.Width / 2) - (graphics.MeasureString(str, font).Width / 2)
                fobj.x = (x / 2) - fobj.window.Width / 2;
                fobj.y = ((y / 2) - fobj.window.Height / 2) + fobj.y;
            }
        }
    }
}
