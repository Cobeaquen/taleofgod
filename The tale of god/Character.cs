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

        public float speed = 10f;
        public float maxInteractionDistance = 125f;

        public Vector2 position;
        public Texture2D sprite; // customize this with clothes
        public Vector2 origin;

        public bool isInteracting;

        public Vector2 move;
        public Vector2 previousMove;

        KeyboardState prevState;

        public Character()
        {
            isInteracting = false;
            position = new Vector2(100, 100);
        }

        public void LoadCharacter()
        {
            sprite = Game1.content.Load<Texture2D>("textures\\chr1\\chr_head");

            A = Vector2.Zero; // top left
            B = new Vector2(sprite.Width, 0); // top right
            C = new Vector2(0, sprite.Width); // bottom left
            D = new Vector2(sprite.Width, sprite.Height); // bottom right

            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);

            prevState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            move = Vector2.Zero;

            #region input

            #region move

            if (state.IsKeyDown(Keys.D))
            {
                move.X += speed;
            }
            else if (state.IsKeyDown(Keys.A))
            {
                move.X -= speed;
            }
            if (state.IsKeyDown(Keys.W))
            {
                move.Y -= speed;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                move.Y += speed;
            }

            #endregion

            if (state.IsKeyDown(Keys.Enter) & !prevState.IsKeyDown(Keys.Enter))
            {
                if (!Game1.npc.interacting)
                {
                    float dist = Vector2.Distance(Game1.npc.position, position);
                    if (dist < maxInteractionDistance)
                        Interact(Game1.npc);
                    else
                        Console.WriteLine("too far away to interact");
                }
            }

            prevState = state;

            #endregion

            if (move.Length() > speed)
            {
                move = new Vector2(move.X / 2f, move.Y / 2f);
            }

            Rectangle[] colliders = Collision.Colliding_Rectangle(new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height));

            if (colliders.Length > 0) // calculate from which side we are colliding
            {
                var col = colliders[0];
                Console.WriteLine("the player is colliding with an object in the scene");
                //move = Vector2.Zero;
            }

            if (!isInteracting)
            {
                previousMove = move;
                Move(position + move);
            }
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

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 3f, SpriteEffects.None, 0f);
        }
    }
}
