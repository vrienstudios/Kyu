using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace KyuBase.Objects
{
    public class SpriteArray
    {
        private Sprite[][] _tiles;

        public Sprite[][] tiles
        {
            get
            {
                return _tiles;
            }
            set
            {
                _tiles = value;
            }
        }

        public void setTile(Sprite a, int x, int y)
        {
            tiles[x][y] = a;
            if (OnChange != null)
                OnChange(a, x, y);
        }

        public Sprite[][] this[Sprite a, int x, int y]
        {
            get { return tiles; }
            set
            {
                tiles[x][y] = a;
                //OnChange(x, y);
            }
        }

        public EventHandler change;
        public delegate void changeH(Sprite a, int x, int y);
        public event changeH OnChange;
    }
}
