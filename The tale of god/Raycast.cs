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

        public Vector2 intersectPos;
        public static Texture2D sprite;
        public static Vector2 origin;

        float t;
        float u;

        Cell[] closeCells;

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

        public bool Intersecting(out object[] colInfo, out Vector2 point) // use custom class to store more than only object
        {
            bool collided = false;

            List<object> colinfo = new List<object>();

            Cell topLeft = new Cell();

            Cell origin = Cell.GetCell(((a + b) / 2f));

            int width = (int)Math.Ceiling(Math.Abs(b.X - a.X) / Cell.cellWidth) + 2;
            int height = (int)Math.Ceiling(Math.Abs(b.Y - a.Y) / Cell.cellHeight) + 2;

            Vector2 size = Cell.SnapToGrid(r);

            point = Vector2.Zero;

            if (r.X > 0f && r.Y < 0f)
            {
                topLeft = Cell.GetCell((int)a.X - Cell.cellWidth, (int)b.Y - Cell.cellHeight);
            }
            else if (r.X > 0f && r.Y > 0f)
            {
                topLeft = Cell.GetCell((int)a.X - Cell.cellWidth, (int)a.Y - Cell.cellHeight);
            }
            else if (r.X < 0f && r.Y > 0f)
            {
                topLeft = Cell.GetCell((int)b.X - Cell.cellWidth, (int)a.Y - Cell.cellHeight);
            }
            else if (r.X < 0f && r.Y < 0f)
            {
                topLeft = Cell.GetCell((int)b.X - Cell.cellWidth, (int)b.Y - Cell.cellHeight);
            }

            closeCells = Cell.GetAreaOfCellsTopLeft(topLeft, width, height);

            foreach (var cell in closeCells)
            {
                if (cell.colliders.Count > 0)
                {
                    float shortestDistance = float.MaxValue;
                    Vector2 colPoint;
                    foreach (var col in cell.colliders)
                    {
                        for (int x = 0; x < col.edges.Length; x++)
                        {
                            Vector2 c = col.edges[x].a;
                            Vector2 d = col.edges[x].b;
                            Vector2 s = d - c;

                            t = Cross(c - a, s) / Cross(r, s);
                            u = Cross(c - a, r) / Cross(r, s);

                            if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
                            {
                                collided = true;
                                if (col.owner != null)
                                {
                                    colinfo.Add(col.owner);
                                }
                                colPoint = a + r * t;

                                float dist = Vector2.Distance(a, colPoint); // problem where the point returned isn't the closest
                                if (dist < shortestDistance)
                                {
                                    shortestDistance = dist;
                                    point = colPoint;
                                    intersectPos = colPoint;
                                }
                            }
                        }
                    }
                }
            }
            colInfo = colinfo.ToArray();

            return collided;
        }
        public bool Intersecting(out object[] colInfo)
        {
            bool collided = false;

            List<object> colinfo = new List<object>();

            Cell topLeft = new Cell();

            Cell origin = Cell.GetCell(((a + b) / 2f));

            int width = (int)Math.Ceiling(Math.Abs(b.X - a.X) / Cell.cellWidth) + 2;
            int height = (int)Math.Ceiling(Math.Abs(b.Y - a.Y) / Cell.cellHeight) + 2;

            Vector2 size = Cell.SnapToGrid(r);

            if (r.X > 0f && r.Y < 0f)
            {
                topLeft = Cell.GetCell((int)a.X - Cell.cellWidth, (int)b.Y - Cell.cellHeight);
            }
            else if (r.X > 0f && r.Y > 0f)
            {
                topLeft = Cell.GetCell((int)a.X - Cell.cellWidth, (int)a.Y - Cell.cellHeight);
            }
            else if (r.X < 0f && r.Y > 0f)
            {
                topLeft = Cell.GetCell((int)b.X - Cell.cellWidth, (int)a.Y - Cell.cellHeight);
            }
            else if (r.X < 0f && r.Y < 0f)
            {
                topLeft = Cell.GetCell((int)b.X - Cell.cellWidth, (int)b.Y - Cell.cellHeight);
            }

            closeCells = Cell.GetAreaOfCellsTopLeft(topLeft, width, height);

            foreach (var cell in closeCells)
            {
                if (cell.colliders.Count > 0)
                {
                    foreach (var col in cell.colliders)
                    {
                        for (int x = 0; x < col.edges.Length; x++)
                        {
                            Vector2 c = col.edges[x].a;
                            Vector2 d = col.edges[x].b;
                            Vector2 s = d - c;

                            t = Cross(c - a, s) / Cross(r, s);
                            u = Cross(c - a, r) / Cross(r, s);

                            if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
                            {
                                collided = true;
                                if (col.owner != null)
                                {
                                    colinfo.Add(col.owner);
                                }
                                intersectPos = a + r * t;
                            }
                        }
                    }
                }
            }
            colInfo = colinfo.ToArray();

            return collided;
        }
        public bool Intersecting(Raycast ray, out Vector2 point)
        {
            Vector2 c = ray.a;
            Vector2 d = ray.b;
            Vector2 s = d - c;

            t = Cross(c - a, s) / Cross(r, s);
            u = Cross(c - a, r) / Cross(r, s);

            if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
            {
                point = a + r * t;
                return true;
            }
            point = Vector2.Zero;
            return false;
        }

        public void Draw(SpriteBatch batch)
        {
            DebugTextures.DrawDebugLine(batch, a, b, Color.White, 1);
            batch.Draw(sprite, intersectPos, null, Color.Red, 0f, origin, 1f, SpriteEffects.None, 1f);

            if (closeCells != null)
            {
                foreach (var c in closeCells)
                {
                    c.Draw(batch);
                }
            }
        }

        public float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
