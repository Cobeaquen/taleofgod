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
        static Vector2 latestButtonPosition;

        public Vector2 position;
        public Vector2 origin;

        public Texture2D image;
        public Text text;
        public Color color;

        public int timesPressed;
        public float timePressed;

        public bool autoRelease;

        public delegate void onPressed();
        onPressed pressed;

        MouseState previousState;

        public Button(Vector2 position, string text, bool autoRelease, Texture2D image, SpriteFont font, onPressed onPressed)
        {
            this.position = position;
            this.image = image;
            this.autoRelease = autoRelease;
            this.text = new Text(position, text, image.Width, image.Height, font, Color.White);
            origin = new Vector2(image.Width / 2f, image.Height / 2f);
            pressed = onPressed;

            previousState = Mouse.GetState();
            color = Color.White;

            latestButtonPosition = position;
        }

        public static Button Debug(string text, int width, int height, Color color, onPressed onPressed)
        {
            return new Button(latestButtonPosition + new Vector2(width * 1.5f, 0), text, false, DebugTextures.GenerateRectangle(width, height, color), GUI.defaultFont, onPressed);
        }

        public void Update(GameTime gameTime)
        {
            if (MouseOver(Input.mousePosition.ToPoint()))
            {
                color.R = 150;
                color.G = 150;
                color.B = 150;
                text.color = color;
                if (Input.LeftMouseButtonUp(false))
                {
                    pressed();
                    timesPressed++;
                }
                else if (Input.LeftMouseButtonDown(false) && !autoRelease)
                {
                    pressed();
                    timePressed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                color = Color.White;
                text.color = color;
            }
        }

        public bool MouseOver(Point mousePosition)
        {
            return mousePosition.Y < position.Y + image.Height / 2f && mousePosition.Y > position.Y - image.Height / 2f && mousePosition.X < position.X + image.Width / 2f && mousePosition.X > position.X - image.Width / 2f;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
            text.Draw(batch);
        }
    }
}
