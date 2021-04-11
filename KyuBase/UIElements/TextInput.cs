using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KyuBase.UIElements
{
    public enum TextAlign
    {
        Left = 0,
        Center = 1,
    }

    public class TextInput : FObject
    {

        private int offset;
        private int cLock;
        private string chars;
        private char[] characters;

        private TextAlign _ta;

        public TextInput(Bitmap theme, int x, int y, string dtext = null, TextAlign ta = TextAlign.Left) : base(x, y, theme)
        {
            _ta = ta;
            if (dtext != null)
            {
                switch (ta)
                {
                    case TextAlign.Center:
                        this.DrawStringCenter(dtext);
                        break;
                    case TextAlign.Left:
                        this.DrawString(0, 0, dtext);
                        break;
                }
                chars = dtext;
                characters = dtext.ToCharArray();
            }

            this.onClick += TextInput_onClick;
            this.onMouseHover += TextInput_onMouseHover;
            this.hierachy = 5;
        }

        private void ShiftCharacters(char c, int idx)
        {
            if (c == '\b')
            {
                if (idx == 0)
                    return;
                char[] t = new char[characters.Length - 1];
                int id = idx - 1;
                for (int i = 0; i < characters.Length; i++)
                    switch (i == id)
                    {
                        case true:
                            switch (characters.Length == 0 || id == (characters.Length - 1))
                            {
                                case true:
                                    break;
                                case false:
                                    t[i] = characters[i + 1];
                                    break;
                            }
                            i++;
                            break;
                        case false:
                            {
                                t[i] = characters[i];
                                break;
                            }
                    }
                chars = new string(t);
                characters = t;
                cLock--;
                return;
            }
            else
            {

                if (idx == chars.Length)
                {
                    chars += c;
                    characters = characters.ToArray();
                    cLock++;
                    return;
                }
                else
                {
                    char a = chars[idx];

                    char[] t = new char[chars.Length + 1];
                    for (int i = 0; i < idx; i++)
                        t[i] = characters[i];

                    for (int i = idx; i < chars.Length; i++)
                        t[i + 1] = characters[i];
                    t[idx] = c;

                    this.chars = new string(t);
                    this.characters = t;
                    cLock++;
                    return;
                }
            }
        }

        public void AddCharacter(char c)
        {
            ShiftCharacters(c, cLock);
            this.characters = chars.ToArray();
            this.ClearText();
            switch (_ta)
            {
                case TextAlign.Center:
                    this.DrawStringCenter(chars);
                    break;
                case TextAlign.Left:
                    this.DrawString(0, 0, chars);
                    break;
            }
            GC.Collect();
        }

        private void TextInput_onMouseHover(FObject fObject, int x, int y)
        {

        }

        private void TextInput_onClick(FObject fObject, int x, int y)
        {

            Graphics g = Graphics.FromImage(fObject.window);

            if (_ta == TextAlign.Center)
                x = (x - (this.window.Width / 2) + (int)(g.MeasureString(chars, font).Width / 2));

            float i = g.MeasureString(chars, this.font).Width;
            Console.WriteLine((int)Math.Round((characters.Length / (i / x))));
            Console.WriteLine((characters.Length / (i / x)));
            cLock = (int)Math.Round((characters.Length / (i / x)));

            if (cLock > chars.Length) // TODO: Modify to support multiple rows.
                cLock = chars.Length;
            else if (cLock < 0)
                cLock = 0;
            return;
        }
    }

    public class Label : FObject
    {

        private int offset;

        public Label(Bitmap theme, int x, int y, string dtext = null, TextAlign ta = TextAlign.Left) : base(x, y, theme)
        {
            if (dtext != null)
                switch (ta)
                {
                    case TextAlign.Center:
                        this.DrawStringCenter(dtext);
                        break;
                    case TextAlign.Left:
                        this.DrawString(0, 0, dtext);
                        break;
                }
        }
    }
}
