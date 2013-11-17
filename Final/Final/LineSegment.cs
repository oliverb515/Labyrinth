using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final
{
    struct LineSegment
    {
        public Vector2 a;
        public Vector2 b;

        public LineSegment(Vector2 p1, Vector2 p2)
        {
            a = p1;
            b = p2;
        }

        public LineSegment(int a, int b, int c, int d)
        {
            this.a = new Vector2(a, b);
            this.b = new Vector2(c, d);
        }
    }
}
