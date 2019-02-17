using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheTaleOfGod.enemies;

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

        public Cell cell;

        Cell[] NearbyCells { get; set; }

        public float maxHealth = 100f;
        public float health;

        public bool isInteracting;

        public Gun gun;

        public Vector2 move;
        public Vector2 previousMove;

        public Camera camera;

        public Collider collider;

        public HealthBar healthBar;
        public Vector2 healthBarOffset = new Vector2(0, 22);

        KeyboardState prevKeyState;
        MouseState prevMouseState;

        CollisionDirection colDir;

        public Character()
        {
            isInteracting = false;
            position = new Vector2(200, 200);

            gun = new Gun(5f, 5f, true, position, new Bullet(7.5f, BulletType.Normal), "character");

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

            collider = new Collider(position, sprite.Width, sprite.Height, "character", this);
            Game1.instance.map.colliders.Add(collider);

            cell = Cell.GetCell(position);
            cell.colliders.Add(collider);
        }

        Rectangle playerRect;
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            move = Vector2.Zero;

            NearbyCells = Cell.GetAreaOfCells(Cell.GetCell(position), 5, 5);

            #region input/movement

            #region movement & collision

            playerRect = new Rectangle((int)position.X - sprite.Width / 2, (int)position.Y - sprite.Height / 2, sprite.Width, sprite.Height);

            Rectangle[] colliders = Collision.CollidingRectangle(position, NearbyCells, sprite.Width, sprite.Height, out Collider[] colInfo);

            if (keyState.IsKeyDown(Keys.D) && colDir != CollisionDirection.Left)
            {
                move.X += 1;
            }
            else if (keyState.IsKeyDown(Keys.A) && colDir != CollisionDirection.Right)
            {
                move.X -= 1;
            }
            if (keyState.IsKeyDown(Keys.W) && colDir != CollisionDirection.Bottom)
            {
                move.Y -= 1;
            }
            else if (keyState.IsKeyDown(Keys.S) && colDir != CollisionDirection.Top)
            {
                move.Y += 1;
            }

            if (move.X != 0 && move.Y != 0)
            {
                move = new Vector2(Math.Sign(move.X) * Game1.oneOverSqrt2, Math.Sign(move.Y) * Game1.oneOverSqrt2);
            }

            move *= speed * gameTime.ElapsedGameTime.Ticks/100000f;

            if (colliders != null)
            {
                foreach (var col in colliders)
                {
                    Collision.RestrictPosition(playerRect, col, ref move);
                }
                if (colInfo != null)
                {
                    foreach (var info in colInfo)
                    {
                        if (info.owner is Enemy enemy) // colliding with the enemy
                        {
                            Damage(6);
                            Vector2 dir = position - enemy.position;
                            dir.Normalize();
                            Knock(dir, 6 * 10);
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
                if (gun.CanFire())
                {
                    gun.Fire(mouseDirection);
                }
            }



            #endregion

            prevKeyState = keyState;
            prevMouseState = mouseState;

            #endregion

            Cell newCell = Cell.GetCell(position);
            if (cell != newCell)
            {
                cell.colliders.Remove(collider);
                cell = newCell;
                cell.colliders.Add(collider);
            }
            collider.position = position;
            collider.Update(gameTime);

            cell = Cell.GetCell(position);

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
            if (Game1.instance.drawGrid)
            {
                foreach (var cell in NearbyCells)
                {
                    cell.Draw(batch, Color.Green);
                }
                batch.Draw(Cell.CellSprite, cell.ToVector2(), null, Color.LightGoldenrodYellow, 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 1f);
            }
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            gun.Draw(batch);
            healthBar.Draw(batch);
        }
    }
}
