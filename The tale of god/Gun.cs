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

        public Raycast ray;

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

                    Vector2 deltaPos = bullets[i].position - bullets[i].previousPosition;

                    Vector2 start = bullets[i].previousPosition - bullets[i].forwardDirection * bullets[i].sprite.Height;
                    Vector2 end = bullets[i].position + bullets[i].forwardDirection * bullets[i].sprite.Height;

                    if (bullets[i].destroyTime < 0)
                    {
                        bullets.RemoveAt(i);
                        return;
                    }
                    ray = new Raycast(start, end);//, bullets[i].position + deltaPos);
                    // this collision rectangle does not change according to its rotation - NEED RAYCASTS (I'M TAKING TIME TO DEVELOP RETARDED METHODS JUST TO THEN REMOVE THEM AND IMPLEMENT ANOTHER METHOD) pls help
                    if (ray.Intersecting(out object[] colInfo))
                    {
                        Hit(colInfo, i);
                        return;
                    }

                    /*if (Collision.CollidingRectangle(bullets[i].position, bullets[i].NearbyCells, bullets[i].sprite.Width, bullets[i].sprite.Height, out object[] colInfo) != null)//new Rectangle((int)bullets[i].position.X - bullets[i].sprite.Width/2, (int)bullets[i].position.Y - bullets[i].sprite.Height/2, bullets[i].sprite.Width, bullets[i].sprite.Height), out tag) != null)
                    {
                        Hit(colInfo, i);
                    }*/
                    //else
                    //{
                    //}
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
