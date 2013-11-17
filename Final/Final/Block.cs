using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Final
{
    class Block : Entity
    {

        public int value;
        public int x;
        public int y;
        
        public Block(int val) {
            value = val;
        }

        public Block(int val, int x, int y) {
            value = val;
            this.x = x;
            this.y = y;
        }

        public override Rectangle getRect()
        {
            return new Rectangle(x*32, y*32, 32, 32);
        }

    }
}
