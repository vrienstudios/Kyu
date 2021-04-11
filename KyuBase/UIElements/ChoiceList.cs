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
    }
}
