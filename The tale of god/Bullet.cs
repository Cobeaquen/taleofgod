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
    public class Bullet
    {
        public Texture2D sprite;
        public Vector2 origin;

        public BulletType bulletType;

        public float velocity;

        public Bullet(float velocity, BulletType bulletType)
        {
            this.velocity = velocity;
            this.bulletType = bulletType;
        }
        public virtual void Load ()
        {
            sprite = DebugTextures.GenerateRectangle(5, 2, Color.Black);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }
    public enum BulletType
    {
        Normal
    }
}
