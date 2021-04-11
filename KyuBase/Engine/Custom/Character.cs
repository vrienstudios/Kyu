using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace KyuBase.Engine.Custom
{
    class Character : Player
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public Character(Bitmap current) : base(new Sprite(current))
        {
            
        }
    }
}
