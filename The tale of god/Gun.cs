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
    public class Gun
    {
        public Texture2D sprite;
        public Vector2 origin;

        public float damage = 10f;
        public float fireRate = 1f;
        public bool autoFire = false;

        public Bullet bullet;

        public Vector2 position;

        public Gun(float damage, float fireRate, bool autoFire, Vector2 position, Bullet bullet)
        {
            this.damage = damage;
            this.fireRate = fireRate;
            this.autoFire = autoFire;
            this.position = position;
            this.bullet = bullet;
        }

        public virtual void Load()
        {
            //sprite = Game1.content.Load<Texture2D>("gun");
            sprite = DebugTextures.GenerateRectangle(2, 2, Color.DarkGreen);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            bullet.Load();
        }
        public virtual void Update(GameTime gameTime, Vector2 position)
        {
            this.position = position;
        }
        public virtual void Fire()
        {
            Console.WriteLine("FIRED");
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
