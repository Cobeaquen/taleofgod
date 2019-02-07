using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TheTaleOfGod.enemies;

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

        private float timeToFire = 0f;
        private MouseState prevMouseState;

        private object[] avoid;

        public Gun(float damage, float fireRate, bool autoFire, Vector2 position, Bullet bullet, params string[] avoid)
        {
            this.damage = damage;
            this.fireRate = fireRate;
            this.autoFire = autoFire;
            this.position = position;
            this.bullet = bullet;
            this.avoid = avoid;
            
            //sprite = Game1.content.Load<Texture2D>("textures\\laser_pistol");
            sprite = DebugTextures.GenerateRectangle(5, 10, Color.DarkGreen);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            bullets = new List<Bullet>();

            prevMouseState = Mouse.GetState();
        }

        public virtual void Update(GameTime gameTime, Vector2 position, float rotation, Vector2 lookDirection)
        {
            this.position = position + lookDirection * positionOffset;
            this.rotation = rotation;

            /*if (timeToFire > 0)
            {
                timeToFire -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }*/

            #region bullet movement and collisions

            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Update(gameTime);

                    Vector2 start = bullets[i].previousPosition - bullets[i].forwardDirection * bullets[i].sprite.Height;
                    Vector2 end = bullets[i].position + bullets[i].forwardDirection * bullets[i].sprite.Height;

                    if (bullets[i].destroyTime < 0)
                    {
                        bullets.RemoveAt(i);
                        return;
                    }
                    ray = new Raycast(start, end);
                    // this collision rectangle does not change according to its rotation - NEED RAYCASTS (I'M TAKING TIME TO DEVELOP RETARDED METHODS JUST TO THEN REMOVE THEM AND IMPLEMENT ANOTHER METHOD) pls help
                    if (ray.Intersecting(out Collider[] colInfo))
                    {
                        Hit(colInfo, i);
                        return;
                    }
                }
            }
            if (timeToFire > 0)
            {
                timeToFire -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            #endregion
        }
        public bool CanFire()
        {
            if (0 >= timeToFire)
            {
                return true;
            }
            return false;
        }

        public void Hit(Collider[] hitInfo, int bulletIndex)
        {
            if (hitInfo != null)
            {
                foreach (var info in hitInfo)
                {
                    if (avoid.Contains(info.tag))
                    {
                        return;
                    }
                    
                    if (info.owner is Enemy)
                    {
                        Enemy enemy = (Enemy)info.owner;
                        enemy.Damage(damage);
                    }
                    else if (info.owner is Character)
                    {
                        Character character = (Character)info.owner;
                        character.Damage(damage);
                    }
                }
            }

            bullets.RemoveAt(bulletIndex);
        }

        public virtual void Fire(Vector2 lookDirection)
        {
            timeToFire = 1f / fireRate;
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
