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
            
            //sprite = Game1.content.Load<Texture2D>("textures\\laser_pistol");
            sprite = DebugTextures.GenerateRectangle(5, 10, Color.DarkGreen);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            bullets = new List<Bullet>();
        }

        public virtual void Update(GameTime gameTime, Vector2 position, float rotation, Vector2 lookDirection)
        {
            this.position = position + lookDirection * positionOffset;
            this.rotation = rotation;

            // remember to upgrade this to use raycasts later, this is an unreliable method - it will cause issues on lower framerates (plus raycasts are needed for laser guns)

            #region bullet movement and collisions

            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Update(gameTime);

                    if (bullets[i].destroyTime < 0)
                    {
                        bullets.RemoveAt(i);
                    }
                    // this collision rectangle does not change according to its rotation - NEED RAYCASTS (I'M TAKING TIME TO DEVELOP RETARDED METHODS JUST TO THEN REMOVE THEM AND IMPLEMENT ANOTHER METHOD) pls help
                    else if (Collision.CollidingRectangle(bullets[i].position, bullets[i].NearbyCells, bullets[i].sprite.Width, bullets[i].sprite.Height, out object[] colInfo) != null)//new Rectangle((int)bullets[i].position.X - bullets[i].sprite.Width/2, (int)bullets[i].position.Y - bullets[i].sprite.Height/2, bullets[i].sprite.Width, bullets[i].sprite.Height), out tag) != null)
                    {
                        Hit(colInfo, i);
                    }
                    else
                    {
                        Vector2 direction = bullets[i].position - bullets[i].previousPosition;
                        Vector2 bulletMove = bullets[i].forwardDirection * direction.Length();
                        if ((int)Math.Abs(direction.X) < 1f)
                        {
                            direction.X = 1;
                        }
                        if ((int)Math.Abs(direction.Y) < 1f)
                        {
                            direction.Y = 1;
                        }

                        Vector2 pos = bullets[i].position - bulletMove;

                        if (direction.Y < 0 & direction.X > 0)
                        {
                            pos.Y -= Math.Abs(direction.Y);
                        }
                        else if (direction.Y > 0 & direction.X < 0)
                        {
                            pos.X -= Math.Abs(direction.X);
                        }
                        else if (direction.Y < 0 & direction.X < 0)
                        {
                            pos.X -= Math.Abs(direction.X);
                            pos.Y -= Math.Abs(direction.Y);
                        }
                        if (Collision.CollidingRectangle(pos, bullets[i].NearbyCells, (int)Math.Abs(direction.X), (int)Math.Abs(direction.Y), out object[] colInfo2) != null)//new Rectangle(pos.ToPoint(), new Point((int)Math.Abs(direction.X), (int)Math.Abs(direction.Y)))) != null)
                        {
                            Hit(colInfo2, i);
                            Console.WriteLine("bullet avoided target on a frame");
                        }
                    }
                }
            }
            #endregion
        }

        public void Hit(object[] hitInfo, int bulletIndex)
        {
            if (hitInfo != null)
            {
                foreach (var info in hitInfo)
                {
                    Enemy enemy = (Enemy)info;
                    if (enemy != null) // collided with an enemy
                    {
                        enemy.Damage(damage);
                    }
                }
            }

            bullets.RemoveAt(bulletIndex);
        }

        public virtual void Fire(Vector2 lookDirection)
        {
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
