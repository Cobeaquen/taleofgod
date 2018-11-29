using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace TheTaleOfGod
{
    public class NPC
    {
        public Vector2 position;
        public Vector2 origin;
        public Texture2D sprite;
        public float textSize = 1f;

        public bool interacting;

        public SpriteFont font;

        Queue<string> dialogue = new Queue<string>();

        private string currentLine = "";
        string line = "";

        Queue<char> characters = new Queue<char>();

        KeyboardState prevState;

        public NPC()
        {
            interacting = false;
            position = Game1.screenCenter;
        }

        public void LoadLines() // initializes the speech of the npc
        {
            dialogue.Enqueue("are you new here?\ni have not seen you before...");
            dialogue.Enqueue("anyway have a good day");
        }

        public void Load(Texture2D sprite)
        {
            this.sprite = sprite;
            font = Game1.content.Load<SpriteFont>("fonts\\npc1");
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            prevState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (interacting)
            {
                if (state.IsKeyDown(Keys.Enter) & !prevState.IsKeyDown(Keys.Enter))
                {
                    AdvanceLine();
                }
            }

            prevState = state;
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            if (interacting)
            {
                if (characters.Count > 0)
                    AdvanceCharacter();

                Vector2 txtsize = font.MeasureString(line);
                Vector2 txtorigin = new Vector2(txtsize.X / 2f, txtsize.Y / 2f);
                Vector2 txtpos = new Vector2(Game1.screenCenter.X, 3 * Game1.screenCenter.Y / 2f);
                float size = textSize;

                batch.DrawString(font, currentLine, txtpos, Color.Black, 0f, txtorigin, size, SpriteEffects.None, 0f);
            }
        }

        private void AdvanceCharacter()
        {
            char c = characters.Dequeue();
            currentLine += c;
        }

        private void AdvanceLine()
        {
            if (dialogue.Count > 0)
            {
                line = dialogue.Dequeue();

                characters = new Queue<char>(line);

                currentLine = "";
            }
            else
            {
                interacting = false;
                Game1.instance.character.isInteracting = false;
            }
        }

        public void Interact()
        {
            interacting = true;

            LoadLines();
        }
    }
}
