using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTaleOfGod
{
    public class Collider
    {
        public int width, height;
        public Vector2 position;

        public Collider(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public Collider()
        {

        }
        public void Update(GameTime gameTime)
        {
            if (Collision.CollidingRectangle(new Rectangle(position.ToPoint(), new Point(width, height))) != null) // colliding
            {

            }
        }
    }
}
