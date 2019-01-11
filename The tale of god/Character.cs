using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheTaleOfGod
{
    public class Character
    {
        #region domain references

        public Vector2 A, B, C, D; // sprite edge points. A is top-left, rest goes right -> down

        #endregion

        public float speed = 2.5f;
        public float maxInteractionDistance = 40f;

        public Vector2 position;
        public float rotation;
        public Texture2D sprite; // customize this with clothes
        public Vector2 origin;

        public float maxHealth = 100f;
        public float health;

        public bool isInteracting;

        public Gun gun;

        public Vector2 move;
        public Vector2 previousMove;

        public Camera camera;

        public HealthBar healthBar;
        public Vector2 healthBarOffset = new Vector2(0, 22);

        KeyboardState prevKeyState;
        MouseState prevMouseState;

        private float timeToFire;

        CollisionDirection colDir;

        public Character()
        {
            isInteracting = false;
            position = new Vector2(100, 100);

            gun = new Gun(25f, 5f, true, position, new Bullet(10f, BulletType.Normal));

            sprite = DebugTextures.GenerateRectangle(16, 32, Color.PaleVioletRed);

            A = Vector2.Zero; // top left
            B = new Vector2(sprite.Width, 0); // top right
            C = new Vector2(0, sprite.Width); // bottom left
            D = new Vector2(sprite.Width, sprite.Height); // bottom right

            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            camera = new Camera(new Vector2(0, 0), -500, 500, -500, 500);

            health = maxHealth;
            healthBar = new HealthBar();

            prevKeyState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        Rectangle playerRect;
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            move = Vector2.Zero;

            #region input

            #region movement and collision

            playerRect = new Rectangle((int)position.X - sprite.Width / 2, (int)position.Y - sprite.Height / 2, sprite.Width, sprite.Height);

            Rectangle[] colliders = Collision.CollidingRectangle(position, sprite.Width, sprite.Height, out object[] colInfo);

            if (keyState.IsKeyDown(Keys.D) && colDir != CollisionDirection.Left)
            {
                move.X += speed;
            }
            else if (keyState.IsKeyDown(Keys.A) && colDir != CollisionDirection.Right)
            {
                move.X -= speed;
            }
            if (keyState.IsKeyDown(Keys.W) && colDir != CollisionDirection.Bottom)
            {
                move.Y -= speed;
            }
            else if (keyState.IsKeyDown(Keys.S) && colDir != CollisionDirection.Top)
            {
                move.Y += speed;
            }

            if (move.Length() > speed)
            {
                move = new Vector2(move.X / 2f, move.Y / 2f);
            }
            move *= (float)gameTime.ElapsedGameTime.Ticks/100000;

            if (colliders != null) // calculate from which side we are colliding
            {
                foreach (var col in colliders)
                {
                    if (playerRect.Right > col.Right && (col.Height >= col.Width)) // colliding from the right
                    {
                        if (!(move.X > 0) && col.Width != col.Height)
                        {
                            move.X = col.Width - 1f;
                        }
                        colDir = CollisionDirection.Right;
                    }
                    else if (playerRect.Left < col.Left && col.Height >= col.Width) // colliding from the left
                    {
                        if (!(move.X < 0) && col.Width != col.Height)
                        {
                            move.X = 1f - col.Width;
                        }
                        colDir = CollisionDirection.Left;
                    }
                    else if (playerRect.Top < col.Top) // colliding from the top
                    {
                        if (!(move.Y < 0) && col.Width != col.Height)
                        {
                            move.Y = 1f - col.Height;
                        }
                        colDir = CollisionDirection.Top;
                    }
                    else if (playerRect.Bottom > col.Bottom) // colliding from the bottom
                    {
                        if (!(move.Y > 0) && col.Width != col.Height)
                        {
                            move.Y = col.Height - 1f;
                        }
                        colDir = CollisionDirection.Bottom;
                    }
                }
                if (colInfo != null)
                {
                    foreach (var info in colInfo)
                    {
                        Enemy enemy = (Enemy)info;
                        if (enemy != null) // colliding with the enemy
                        {
                            Damage(enemy.meleeDamage);
                            Vector2 dir = position - enemy.position;
                            dir.Normalize();
                            Knock(dir, enemy.meleeDamage * 10);
                        }
                    }
                }
            }
            else
            {
                colDir = CollisionDirection.None;
            }

            previousMove = move;
            Move(position + move);

            #endregion

            camera.MoveTowards(position, gameTime);

            #region rotation

            Vector2 mouseDirection = camera.WindowToWorldSpace(mouseState.Position.ToVector2()) - position;
            mouseDirection.Normalize();

            rotation = (float)Math.Atan2(mouseDirection.Y, mouseDirection.X) - MathHelper.PiOver2;

            #endregion

            #region interacting

            if (keyState.IsKeyDown(Keys.Enter) & !prevKeyState.IsKeyDown(Keys.Enter))
            {
                foreach (var npc in Game1.instance.map.npcs)
                {
                    if (!npc.interacting)
                    {
                        float dist = Vector2.Distance(npc.position, position);
                        if (dist < maxInteractionDistance)
                        {
                            Interact(npc);
                            break;
                        }
                        else
                            Console.WriteLine("too far away to interact");
                    }
                }
            }

            #endregion

            #region shooting

            gun.Update(gameTime, position, rotation, mouseDirection);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (0 >= timeToFire)
                {
                    if (gun.autoFire || prevMouseState.LeftButton == ButtonState.Released) // fire here
                    {
                        timeToFire = 1f / gun.fireRate;
                        gun.Fire(mouseDirection);
                    }
                }
            }
            if (timeToFire > 0)
            {
                timeToFire -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            #endregion

            prevKeyState = keyState;
            prevMouseState = mouseState;

            #endregion

            healthBar.position = position + healthBarOffset;
        }

        public void Move(Vector2 newPos)
        {
            position = newPos;
        }

        public void Interact(NPC npc)
        {
            isInteracting = true;
            npc.Interact();
        }

        public void Damage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                health = 0;
                Die();
            }
            else
            {
                healthBar.ChangeValue(health/maxHealth);
                Console.WriteLine("Character at {0} health", health);
            }
        }
        public void Knock(Vector2 direction, float velocity)
        {
            position += direction * velocity;
        }

        public void Die()
        {
            Console.WriteLine("Character is dead :(");
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            gun.Draw(batch);
            healthBar.Draw(batch);
        }
    }
}
