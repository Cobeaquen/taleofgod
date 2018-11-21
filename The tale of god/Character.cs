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
        public float speed = 5f;

        public Vector2 position;
        public Texture2D sprite; // customize this with clothes
        public Vector2 origin;

        private Vector2 move;

        public Character()
        {
            position = new Vector2(100, 100);
        }

        public void LoadCharacter()
        {
            sprite = Game1.content.Load<Texture2D>("textures\\chr1\\chr_head");
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState key = Keyboard.GetState();

            move = Vector2.Zero;

            if (key.IsKeyDown(Keys.D))
            {
                move.X += speed;
            }
            else if (key.IsKeyDown(Keys.A))
            {
                move.X -= speed;
            }
            if (key.IsKeyDown(Keys.W))
            {
                move.Y -= speed;
            }
            else if (key.IsKeyDown(Keys.S))
            {
                move.Y += speed;
            }

            position += move;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 4f, SpriteEffects.None, 0f);
        }
    }
}
