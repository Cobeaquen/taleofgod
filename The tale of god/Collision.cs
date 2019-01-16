using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheTaleOfGod
{
    public class Collision
    {
        // latest collision test
        public static Texture2D debugTexture;
        public static Vector2 colPosition;
        public static Vector2 colOrigin;

        /// <summary>
        /// is an object colliding with any of the scene objects?
        /// </summary>
        /// <param name="position">origin</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>

        public static Rectangle[] CollidingRectangle(Vector2 position, Cell[] cellCheck, int width, int height) // needs to return multiple tags if colliding with multiple objects (return collider instead of rectangle)
        {
            List<Rectangle> colliders = null;

            Rectangle rect1 = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

            foreach (var cell in cellCheck)
            {
                if (cell.colliders.Count > 0)
                {
                    foreach (var co in cell.colliders)
                    {
                        Rectangle rect2 = new Rectangle((int)co.position.X - co.width / 2, (int)co.position.Y - co.height / 2, co.width, co.height);

                        if (rect1 == rect2) // remember that this may be causing issues for different objects with the same size
                            continue;

                        if (Game1.instance.debugDrawing)
                        {
                            debugTexture = DebugTextures.GenerateHollowRectangele(rect1.Width, rect1.Height, 1, Color.White);
                            colPosition = rect1.Center.ToVector2();
                            colOrigin = new Vector2(debugTexture.Width / 2f, debugTexture.Height / 2f);
                        }

                        Rectangle col = Rectangle.Intersect(rect1, rect2);
                        if (!col.IsEmpty)
                        {
                            if (colliders == null)
                            {
                                colliders = new List<Rectangle>();
                            }
                            colliders.Add(col);
                            if (Game1.instance.debugDrawing)
                            {
                                debugTexture = DebugTextures.GenerateHollowRectangele(col.Width, col.Height, 1, Color.White);
                                colPosition = col.Center.ToVector2();
                                colOrigin = new Vector2(debugTexture.Width / 2f, debugTexture.Height / 2f);
                            }
                        }
                    }
                }
            }

            /*foreach (var co in Game1.instance.map.colliders) // loop through every possible collidable object (has to be more efficient in the fututre)
            {
                

                #region debug

                if (Game1.instance.debugDrawing)
                {
                    //debugTexture = DebugTextures.GenerateHollowRectangele(rect1.Width, rect1.Height, 2, Color.White);
                    //colPosition = rect1.Location.ToVector2();
                    //colOrigin = new Vector2(debugTexture.Width / 2f, debugTexture.Height / 2f);
                }

                #endregion
            }*/

            if (colliders == null)
            {
                return null;
            }
            return colliders.ToArray();
        }

        public static Rectangle[] CollidingRectangle(Vector2 position, Cell[] cellCheck, int width, int height, out object[] colInfo) // need multiple collider objects!!
        {
            List<Rectangle> colliders = null;
            List<object> colInfos = null;

            Rectangle rect1 = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

            foreach (var cell in cellCheck)
            {
                if (cell.colliders.Count > 0)
                {
                    foreach (var co in cell.colliders)
                    {
                        Rectangle rect2 = new Rectangle((int)co.position.X - co.width / 2, (int)co.position.Y - co.height / 2, co.width, co.height);

                        if (rect1 == rect2)
                            continue;

                        if (Game1.instance.debugDrawing)
                        {
                            debugTexture = DebugTextures.GenerateHollowRectangele(rect1.Width, rect1.Height, 1, Color.White);
                            colPosition = rect1.Center.ToVector2();
                            colOrigin = new Vector2(debugTexture.Width / 2f, debugTexture.Height / 2f);
                        }

                        Rectangle col = Rectangle.Intersect(rect1, rect2);
                        if (!col.IsEmpty)
                        {
                            if (colliders == null)
                            {
                                colliders = new List<Rectangle>();
                            }
                            if (co.owner != null)
                            {
                                if (colInfos == null)
                                {
                                    colInfos = new List<object>();
                                }
                                colInfos.Add(co.owner);
                            }

                            colliders.Add(col);
                            if (Game1.instance.debugDrawing)
                            {
                                debugTexture = DebugTextures.GenerateHollowRectangele(col.Width, col.Height, 1, Color.White);
                                colPosition = col.Center.ToVector2();
                                colOrigin = new Vector2(debugTexture.Width / 2f, debugTexture.Height / 2f);
                            }
                        }
                    }
                }
            }

            /*foreach (var co in Game1.instance.map.colliders) // loop through every possible collidable object (has to be more efficient in the fututre)
            {
                

                #region debug

                if (Game1.instance.debugDrawing)
                {
                    //debugTexture = DebugTextures.GenerateHollowRectangele(rect1.Width, rect1.Height, 2, Color.White);
                    //colPosition = rect1.Location.ToVector2();
                    //colOrigin = new Vector2(debugTexture.Width / 2f, debugTexture.Height / 2f);
                }

                #endregion
            }*/

            if (colliders == null)
            {
                colInfo = null;
                return null;
            }
            if (colInfos != null)
            {
                colInfo = colInfos.ToArray();
            }
            else
            {
                colInfo = null;
            }

            return colliders.ToArray();
        }
    }
    public enum CollisionDirection
    {
        None, Top, Bottom, Right, Left
    }
}
