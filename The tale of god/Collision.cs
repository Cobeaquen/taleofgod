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
        /// <summary>
        /// is an object colliding with any of the scene objects?
        /// </summary>
        /// <param name="position">origin</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>

        public static Rectangle[] Colliding_Rectangle(Rectangle rect1)
        {
            /*float topEdge = position.Y - height / 2f;
            float rightEdge = position.X + width / 2f;
            float leftEdge = position.X - width / 2f;
            float bottomEdge = position.Y + height / 2f;
            */

            List<Rectangle> colliders = new List<Rectangle>();

            foreach (var so in Game1.instance.sceneObjects) // loop through every possible collidable object (has to be more efficent in the fututre)
            {
                /*float so_topEdge = so.position.Y - so.height / 2f;
                float so_bottomEdge = so.position.Y + so.height/ 2f;
                float so_leftEdge = so.position.X - so.width / 2f;
                float so_rightEdge = so.position.X + so.width / 2f;
                */

                Rectangle rect2 = new Rectangle(so.position.ToPoint(), new Point(so.width, so.height));

                Rectangle col = Rectangle.Intersect(rect1, rect2);
                if (col != Rectangle.Empty)
                    colliders.Add(col);


                /*if (leftEdge < so_rightEdge && rightEdge > so_leftEdge && bottomEdge > so_topEdge && topEdge < so_bottomEdge)
                {
                    float distFromTop = Math.Abs(Game1.instance.character.position.Y - so_topEdge);
                    float distFromRight = Math.Abs(Game1.instance.character.position.X - so_rightEdge);

                    if (topEdge < so_topEdge) // colliding from above
                    {
                        Game1.instance.character.position.Y -= Game1.instance.character.previousMove.Length();// so_topEdge - height / 2f;
                    }
                    else if (bottomEdge > so_bottomEdge) // colliding from below
                    {
                        Game1.instance.character.position.Y += Game1.instance.character.previousMove.Length(); //so_bottomEdge + height / 2f;
                    }

                    else if (leftEdge < so_leftEdge) // colliding from the left
                    {
                        Game1.instance.character.position.X -= Game1.instance.character.previousMove.Length(); //so_leftEdge - width/2f;
                    }
                    else if (rightEdge > so_rightEdge) // colliding from the right
                    {
                        Game1.instance.character.position.X += Game1.instance.character.previousMove.Length();// so_rightEdge + width/2f;
                    }
                    return true;
                }*/
            }
            return colliders.ToArray();
        }
    }
}
