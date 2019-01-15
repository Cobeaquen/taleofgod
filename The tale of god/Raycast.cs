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

        public static Vector2 intersectPos;
        public static Texture2D sprite;
        public static Vector2 origin;

        float t;
        float u;

        public static void RayCastTest()
        {
            sprite = DebugTextures.GenerateRectangle(4, 4, Color.White);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }

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

        public bool Intersecting(out object[] colInfo)//Vector2 c, Vector2 d)
        {
            //Vector2 s = d - c;
            bool collided = false;

            List<object> colinfo = new List<object>();

            foreach (var col in Game1.instance.map.colliders)
            {
                for (int x = 0; x < col.lines.GetLength(0); x++)
                {
                    Vector2 c = col.lines[x, 0];
                    Vector2 s = col.lines[x, 1] - c;

                    t = Cross(c - a, s) / Cross(r, s);

                    if (t >= 0f && t <= 1f)
                    {
                        collided = true;
                        if (col.owner != null)
                        {
                            colinfo.Add(col.owner);
                        }
                        intersectPos = a + r*t;
                    }
                }
            }
            colInfo = colinfo.ToArray();

            return collided;
        }
        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, intersectPos, null, Color.Red, 0f, origin, 1f, SpriteEffects.None, 1f);
        }

        public float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
