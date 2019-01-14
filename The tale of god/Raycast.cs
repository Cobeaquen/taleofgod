using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheTaleOfGod
{
    public class Raycast
    {
        public Vector2 a;
        public Vector2 b;
        public Vector2 r;

        float t;
        float u;

        public Raycast(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
            r = b - a;
        }
        public Raycast(Vector2 a, Vector2 direction, float length)
        {
            this.a = a;
            if (direction.Length() != 1)
                direction.Normalize();

            b = a + direction * length;

            r = direction * length;
        }

        public bool Intersecting(Vector2 c, Vector2 d)
        {
            Vector2 s = d - c;

            t = Cross(c - a, s) / Cross(r, s);

            if (t >= 0 && t <= 1)
            {
                return true;
            }
            return false;
        }

        public float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
