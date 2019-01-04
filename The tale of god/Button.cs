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
    public class Button
    {
        public Vector2 position;
        public Vector2 origin;

        public Texture2D image;
        public Text text;

        public bool autoRelease;

        public delegate void onPressed();
        onPressed pressed;

        MouseState previousState;

        public Button(Vector2 position, string text, bool autoRelease, Texture2D image, SpriteFont font, onPressed onPressed)
        {
            this.position = position;
            this.image = image;
            this.autoRelease = autoRelease;
            this.text = new Text(position, text, image.Width, image.Height, font);
            origin = new Vector2(image.Width / 2f, image.Height / 2f);
            pressed = onPressed;

            previousState = Mouse.GetState();
        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed && (!autoRelease || previousState.LeftButton == ButtonState.Released))
            {
                if (MouseOver(state.Position))
                {
                    pressed();
                }
            }
            previousState = state;
        }

        public bool MouseOver(Point mousePosition)
        {
            return mousePosition.Y < position.Y + image.Height / 2f && mousePosition.Y > position.Y - image.Height / 2f && mousePosition.X < position.X + image.Width / 2f && mousePosition.X > position.X - image.Width / 2f;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            text.Draw(batch);
        }
    }
}
