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

namespace TheTaleOfGod.enemies
{
    public class Enemy
    {
        public float speed;
        public Vector2 position;
        public Cell cell = new Cell(0, 0);

        public Cell[] NearbyCells { get; set; }

        public float attackRange;
        public float targetRange;

        public Texture2D sprite;
        public Vector2 origin;

        public float maxHealth;
        public float health;

        public float rotation;
        public Vector2 lookDirection;

        public Character target;
        public Vector2 targetPosition;

        private Vector2 move;
        private Collider collider;

        Raycast ray;

        private HealthBar healthBar;
        static Vector2 healthBarOffset = new Vector2(0, 15);

        public Enemy(float speed, float maxHealth, float attackRange, float targetRange, Vector2 position, Texture2D sprite, Character target)
        {
            this.speed = speed;
            this.maxHealth = maxHealth;
            this.attackRange = attackRange;
            this.targetRange = targetRange;
            this.position = position;
            this.sprite = sprite;
            this.target = target;

            healthBar = new HealthBar();

            collider = new Collider(position, sprite.Width, sprite.Height, "enemy", this);
            Game1.instance.map.colliders.Add(collider);

            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            health = maxHealth;
        }

        public virtual void Update(GameTime gameTime)
        {
            // follow the target
            Vector2 dir = target.position - position;
            float magnitude = dir.Length();
            if (magnitude < attackRange)
            {
                dir.Normalize();
                if (magnitude > targetRange)
                {
                    Vector2 direction = dir;

                    if (direction != Vector2.Zero)
                    {
                        move = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    Cell newCell = Cell.GetCell(position);
                    if (cell != newCell)
                    {
                        cell.enemies.Remove(this);
                        cell.colliders.Remove(collider);
                        cell = newCell;
                        cell.enemies.Add(this);
                        cell.colliders.Add(collider);
                    }
                }
                else
                {
                    move = Vector2.Zero;
                }
                float newrot = Game1.VectorToAngle(dir);

                newrot = Math.Sign(newrot) == -1 ? newrot + MathHelper.TwoPi : newrot;

                Console.WriteLine(rotation);

                float rot = newrot - rotation;

                if (Math.Abs(rotation - newrot) > MathHelper.Pi)
                {
                    newrot += MathHelper.TwoPi;
                }
                rotation += rot * 0.001f;

                /*if (Math.Abs(newrot - rotation) > MathHelper.Pi)
                {
                    rotation = newrot + (MathHelper.TwoPi - newrot - rotation) * 0.002f;
                }
                else
                {
                    rotation = rotation + (newrot - rotation) * 0.002f;
                }*/

                //rotation = Game1.LerpRotation(a, newrot, 0.09f);

                lookDirection = dir;
            }
            else
            {
                move = Vector2.Zero;
            }

            ray = new Raycast(position, target.position);

            if (ray.Intersecting(out object[] colinfo, out Vector2 point))
            {
                foreach (var info in colinfo)
                {
                    if (info is SceneObject col)
                    {
                        float shortestDistance = float.MaxValue;
                        Node closestNode = null;

                        foreach (var node in col.collider.nodes)
                        {
                            float distance = Vector2.Distance(point, node.position);
                            if (distance < shortestDistance)
                            {
                                shortestDistance = distance;
                                closestNode = node;
                            }
                        }

                        targetPosition = closestNode.position;
                        break;
                    }
                    else
                    {
                        targetPosition = target.position;
                    }
                }
            }
            else
            {
                targetPosition = target.position;
            }

            NearbyCells = Cell.GetAreaOfCells(Cell.GetCell(position), 4, 4);

            Rectangle enemyRect = new Rectangle((int)position.X - sprite.Width / 2, (int)position.Y - sprite.Height / 2, sprite.Width, sprite.Height);
            Rectangle[] colliders = Collision.CollidingRectangle(position, NearbyCells, sprite.Width, sprite.Height, out object[] colInfo);

            if (colliders != null)
            {
                foreach (var col in colliders)
                {
                    Collision.RestrictPosition(enemyRect, col, ref move);
                }
                foreach (var info in colInfo)
                {
                    OnCollisionEnter(info);
                }
            }
            position += move;

            healthBar.position = position + healthBarOffset;
            collider.position = position;
        }

        public virtual void Damage(float damage)
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

        public virtual void OnCollisionEnter(object colinfo)
        {

        }
        public virtual void InflictDamage(Character target)
        {
            //target.Damage()
        }

        public virtual void Die()
        {
            Game1.instance.map.enemies.Remove(this);
            cell.colliders.Remove(collider);
            Game1.instance.map.colliders.Remove(collider);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
            //ray.Draw(batch);
            healthBar.Draw(batch);
        }
    }
}
