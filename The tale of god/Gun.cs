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
        public float positionOffset = 10f;

        public Bullet bullet;
        public float bulletDestroyTime;

        public Vector2 position;
        public float rotation;

        public List<Bullet> bullets;

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
            //sprite = Game1.content.Load<Texture2D>("textures\\laser_pistol");
            sprite = DebugTextures.GenerateRectangle(5, 10, Color.DarkGreen);
            origin = new Vector2(sprite.Width/2f, sprite.Height/2f);

            bullets = new List<Bullet>();


        }
        public virtual void Update(GameTime gameTime, Vector2 position, float rotation, Vector2 lookDirection)
        {
            this.position = position + (lookDirection * positionOffset);
            this.rotation = rotation;

            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Update(gameTime);
                    if (bullets[i].destroyTime < 0)
                    {
                        bullets.RemoveAt(i);
                    }
                    else if (Collision.Colliding_Rectangle(new Rectangle(bullets[i].position.ToPoint(), new Point(bullets[i].sprite.Width, bullets[i].sprite.Height))) != null)
                    {
                        bullets.RemoveAt(i);
                    }
                }
            }
        }
        public virtual void Fire(Vector2 lookDirection)
        {
            Console.WriteLine("FIRED");
            bullets.Add(Bullet.SpawnBullet(bullet, position, rotation, lookDirection));
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
            foreach (var b in bullets)
            {
                b.Draw(batch);
            }
        }

        public static void DestroyBullet(Bullet b)
        {
            
        }
    }
}
