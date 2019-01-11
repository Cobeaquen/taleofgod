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
    public class Enemy
    {
        public float speed;
        public Vector2 position;

        public float attackRange;

        public Texture2D sprite;
        public Vector2 origin;

        public float meleeDamage;
        public float maxHealth;
        public float health;

        public float rotation;

        public Character target;

        private Vector2 move;
        private Collider collider;

        private HealthBar healthBar;
        static Vector2 healthBarOffset = new Vector2(0, 15);

        public Enemy(float speed, float maxHealth, float attackRange, float meleeDamage, Vector2 position, Texture2D sprite, Character target)
        {
            this.speed = speed;
            this.maxHealth = maxHealth;
            this.attackRange = attackRange;
            this.meleeDamage = meleeDamage;
            this.position = position;
            this.sprite = sprite;
            this.target = target;

            healthBar = new HealthBar();

            collider = new Collider(position, sprite.Width, sprite.Height, "enemy", this);
            Game1.instance.map.colliders.Add(collider);

            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            health = maxHealth;
        }

        public void Update(GameTime gameTime)
        {
            // follow the target
            float dist = Vector2.Distance(target.position, position);
            if (dist < attackRange)
            {
                Vector2 direction = target.position - position;
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    move = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            
            Rectangle enemyRect = new Rectangle((int)position.X - sprite.Width / 2, (int)position.Y - sprite.Height / 2, sprite.Width, sprite.Height);
            Rectangle[] colliders = Collision.CollidingRectangle(position, sprite.Width, sprite.Height, out object[] colInfo);

            if (colliders != null)
            {
                foreach (var col in colliders)
                {
                    if (enemyRect.Right > col.Right && col.Height >= col.Width) // colliding from the right
                    {
                        if (!(move.X > 0))
                        {
                            move.X = col.Width - 1f;
                        }
                    }
                    else if (enemyRect.Left < col.Left && col.Height >= col.Width) // colliding from the left
                    {
                        if (!(move.X < 0))
                        {
                            move.X = 1f - col.Width;
                        }
                    }
                    else if (enemyRect.Top < col.Top) // colliding from the top
                    {
                        if (!(move.Y < 0))
                        {
                            move.Y = 1f - col.Height;
                        }
                    }
                    else if (enemyRect.Bottom > col.Bottom) // colliding from the bottom
                    {
                        if (!(move.Y > 0))
                        {
                            move.Y = col.Height - 1f;
                        }
                    }
                }
            }
            position += move;
            rotation = Game1.VectorToAngle(move);

            healthBar.position = position + healthBarOffset;
            collider.position = position;
        }

        public void Damage(float damage)
        {
            health -= damage;
            healthBar.ChangeValue(health / maxHealth);

            if (health <= 0)
            {
                health = 0;
                Console.WriteLine("Enemy is dead");
                Die();
            }
            else
            {
                Console.WriteLine("Enemy at {0} health", health);
            }
        }

        public void Die()
        {
            Game1.instance.map.enemies.Remove(this);
            Game1.instance.map.colliders.Remove(collider);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
            healthBar.Draw(batch);
        }
    }
}
