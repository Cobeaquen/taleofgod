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
        public Vector2 offsetPosition = new Vector2(0, -10);

        public Bullet bullet;

        public Vector2 position;
        public float rotation;

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
            sprite = Game1.content.Load<Texture2D>("textures\\laser_pistol");
            //sprite = DebugTextures.GenerateRectangle(5, 10, Color.DarkGreen);
            origin = new Vector2(0, sprite.Height);
            bullet.Load();
        }
        public virtual void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            this.position = position + offsetPosition;
            this.rotation = rotation;
        }
        public virtual void Fire()
        {
            Console.WriteLine("FIRED");
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
