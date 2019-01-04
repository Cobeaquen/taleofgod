using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ProtoBuf;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class SceneObject
    {
        [ProtoMember(1)]
        public Vector2 position;
        public Vector2 origin;
        [ProtoMember(2)]
        public int width;
        [ProtoMember(3)]
        public int height;

        protected Texture2D sprite;

        public SceneObject(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public SceneObject()
        {

        }

        public virtual void Load()
        {
            sprite = DebugTextures.GenerateRectangle(width, height, Color.PowderBlue);
            origin = new Vector2(width / 2f, height / 2f);
        }

        public virtual void Update() // unnecessary?? yes probably
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
