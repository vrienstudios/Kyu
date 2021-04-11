using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace KyuBase.UIElements
{
    public class DropDownMenu
    {
        private Bitmap theme;
        private Bitmap dArrow;

        public String[] Items;
        public int selectedIndex;

        public FObject baseT;
        public FObject _base;
        public Bitmap _open;
        private FObject nmText;
        public FObject[] itemsText;

        private Font font;

        public bool isOpen = false;
        Graphics a;
        private int x, y;
        public DropDownMenu(Bitmap theme, Bitmap dArrow, String[] items, int x, int y, int spacing)
        {
            this.theme = theme;
            this.dArrow = dArrow;
            this.Items = items;
            itemsText = new FObject[items.Length];

            int row = y + spacing;
            this.x = x;
            this.y = y;

            Bitmap bm = new Bitmap(theme.Width, theme.Height);
            for (uint idx = 0; idx < items.Length; idx++)
            {
                FObject t = new FObject(0, row, theme);
                row = row + spacing;
                t.onMouseHover += T_onMouseHover;
                t.DrawStringCenter(items[idx]);
                itemsText[idx] = t;
            }

            font = new Font(FontFamily.GenericSansSerif, 12);

            a = Graphics.FromImage(bm);
            a.DrawImage(theme, 0, 0);
            _base = new FObject(x, y, bm);
            _base.DrawStringCenter(items[0]);
            _base.onClick += _base_onClick;

            baseT = new FObject(x, y, theme);
            baseT.DrawStringCenter(items[0]);

            Bitmap bp = new Bitmap(theme.Width, row);
            a = Graphics.FromImage(bp);
            a.DrawImage(_base.window, 0, 0);
            foreach (FObject fobj in itemsText)
                a.DrawImage(fobj.window, fobj.x, fobj.y);
            _open = bp;
        }

        public void selectItem(int i)
        {
            _base.ClearText();
            _base.DrawStringCenter(Items[i]);
            baseT.ClearText();
            baseT.DrawStringCenter(Items[i]);

            Bitmap bp = new Bitmap(_open.Width, _open.Height);
            a = Graphics.FromImage(bp);
            a.DrawImage(baseT.window, 0, 0);
            foreach (FObject fobj in itemsText)
                a.DrawImage(fobj.window, fobj.x, fobj.y);
            _open = bp;

            if (onItemSelect != null)
                onItemSelect(Items[i]);
        }

        public delegate void itemSelected(String selectedText);
        public event itemSelected onItemSelect;

        private void _base_onClick(FObject fObject, int x, int y)
        {

        }

        private void T_onMouseHover(FObject fObject, int x, int y)
        {
            Console.WriteLine("OO GOLLY");
        }
    }
}
