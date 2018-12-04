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

        public static bool Colliding_Rectangle(Vector2 position, float width, float height)
        {
            float topEdge = position.Y - height / 2f;
            float rightEdge = position.X + width / 2f;
            float leftEdge = position.X - width / 2f;
            float bottomEdge = position.Y + height / 2f;

            foreach (var so in Game1.instance.sceneObjects) // loop through every possible collidable object (has to be more efficent in the fututre)
            {
                float so_topEdge = so.position.Y - so.height / 2f;
                float so_bottomEdge = so.position.Y + so.height/ 2f;
                float so_leftEdge = so.position.X - so.width / 2f;
                float so_rightEdge = so.position.X + so.width / 2f;

                if (leftEdge < so_rightEdge && rightEdge > so_leftEdge && bottomEdge > so_topEdge && topEdge < so_bottomEdge)
                {
                    Console.WriteLine("colliding on the x-axis");

                    if (topEdge < so_topEdge) // colliding from above
                    {
                        Game1.instance.character.position.Y = so_topEdge - height / 2f;
                    }
                    else if (bottomEdge > so_bottomEdge) // colliding from below
                    {
                        Game1.instance.character.position.Y = so_bottomEdge + height / 2f;
                    }

                    if (leftEdge < so_leftEdge) // colliding from the left
                    {
                        Game1.instance.character.position.X = so_leftEdge - width/2f;
                    }
                    else if (rightEdge > so_rightEdge) // colliding from the right
                    {
                        Game1.instance.character.position.X = so_rightEdge + width/2f;
                    }
                }
            }

            return false;
        }
    }
}
