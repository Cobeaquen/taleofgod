using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProtoBuf;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class Collider
    {
        [ProtoMember(1)]
        public int width;
        [ProtoMember(2)]
        public int height;

        [ProtoMember(3)]
        public Vector2 position;

        public Texture2D debugSprite;
        public Vector2 debugOrigin;

        public Collider(Vector2 position, int width, int height)
        {
            this.position = position;
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
        public void DrawDebug(SpriteBatch batch)
        {
            if (debugSprite != null)
            {
                batch.Draw(debugSprite, position, null, Color.White, 0f, debugOrigin, 1f, SpriteEffects.None, 0f); // fiiix mee
            }
        }
    }
}
