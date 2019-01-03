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
        public Vector2 position;
        public Vector2 previousPosition;
        public float rotation;

        public float destroyTime = 2f;

        public Texture2D sprite;
        public Vector2 origin;

        public Vector2 forwardDirection;

        public BulletType bulletType;

        public float velocity;

        public Bullet(float velocity, BulletType bulletType)
        {
            this.velocity = velocity;
            this.bulletType = bulletType;
        }

        public Bullet Copy()
        {
            Bullet b = new Bullet(velocity, bulletType);
            return b;
        }

        public void Initialize (Vector2 position, float rotation, Vector2 forwardDirection)
        {
            sprite = DebugTextures.GenerateRectangle(5, 2, Color.Black);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            this.position = position;
            this.rotation = rotation + MathHelper.PiOver2;

            this.forwardDirection = forwardDirection;
        }

        public static Bullet SpawnBullet(Bullet bullet, Vector2 position, float rotation, Vector2 mouseDirection)
        {
            Bullet b = bullet.Copy();
            b.Initialize(position, rotation, mouseDirection);
            return b;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 move = forwardDirection * velocity * gameTime.ElapsedGameTime.Ticks/100000f;
            previousPosition = position;
            position += move;

            destroyTime -= gameTime.ElapsedGameTime.Ticks / 10000000f;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
    public enum BulletType
    {
        Normal
    }
}