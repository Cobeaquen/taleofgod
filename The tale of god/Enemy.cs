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
        public float turnSpeed;

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

        public Enemy(float speed, float turnSpeed, float maxHealth, float attackRange, float targetRange, Vector2 position, Texture2D sprite, Character target)
        {
            this.speed = speed;
            this.turnSpeed = turnSpeed;
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

                Vector2 rotationDirection = Game1.AngleToVector(rotation);

                Vector2 newDir = Vector2.Lerp(rotationDirection, dir, turnSpeed);

                float newrot = Game1.VectorToAngle(newDir);

                rotation = newrot;

                lookDirection = newDir;
            }
            else
            {
                move = Vector2.Zero;
            }

            ray = new Raycast(position, target.position);

            if (ray.Intersecting(out Collider[] colinfo, out Vector2 point))
            {
                foreach (var info in colinfo)
                {
                    if (info.owner is SceneObject col)
                    {
                        OnTargetBlocked(info, point);
                        
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
            Rectangle[] colliders = Collision.CollidingRectangle(position, NearbyCells, sprite.Width, sprite.Height, out Collider[] colInfo, "enemy", out Collider[] nearEnemies);

            if (nearEnemies != null)
            {
                foreach (var e in nearEnemies)
                {
                    Enemy enemy = (Enemy)e.owner;
                    if (enemy != this)
                    {
                        Vector2 dif = enemy.position - position;

                        float x = dif.Length() / 50f;
                        if (x <= 1f)
                        {
                            float value = Math.Abs(Game1.Sigmoid(1f - x, 1.3f)) * 0.02f;

                            dif.Normalize();

                            position += dif * -value;
                        }
                    }
                }
            }

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

        public virtual void OnTargetBlocked(Collider col, Vector2 point)
        {
            float shortestDistance = float.MaxValue;
            Node closestNode = null;

            foreach (var node in collider.nodes)
            {
                float distance = Vector2.Distance(point, node.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestNode = node;
                }
            }

            targetPosition = closestNode.position;
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

        public virtual void OnCollisionEnter(Collider colinfo)
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
            /*foreach (var c in NearbyCells)
            {
                c.Draw(batch);
            }*/
            batch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
            //ray.Draw(batch);
            healthBar.Draw(batch);
        }
    }
}
