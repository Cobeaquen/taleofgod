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
        public string tag;

        public object owner;

        [ProtoMember(4)]
        public Vector2 position;

        public int Right;
        public int Left;
        public int Top;
        public int Bottom;

        public Texture2D debugSprite;
        public Vector2 debugOrigin;

        public Collider(Vector2 position, int width, int height, string tag = null, object owner = null)
        {
            this.position = new Vector2(position.X, position.Y);
            this.width = width;
            this.height = height;
            this.tag = tag;
            this.owner = owner;

            Right = (int)position.X + width / 2;
            Left = (int)position.X - width / 2;
            Top = (int)position.Y - height / 2;
            Bottom = (int)position.Y + height / 2;
        }
        public Collider()
        {

        }
        public void Update(GameTime gameTime)
        {

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
