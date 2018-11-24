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
        public Texture2D sprite;
        public float textSize = 2f;

        public bool interacting;

        public SpriteFont font;

        Queue<string> dialogue = new Queue<string>();

        private string currentLine = "";
        string line = "";

        Queue<char> characters = new Queue<char>();

        bool isTalking;

        KeyboardState prevState;

        public NPC()
        {
            interacting = false;
            position = Vector2.Zero;

            dialogue.Enqueue("oof, wellp\n" +
            "that stuff happens from time to time");
            dialogue.Enqueue("anyway have a good day");
            dialogue.Enqueue("lol noob");
            dialogue.Enqueue("alluhakbar");
            dialogue.Enqueue("no ty");
        }

        public void Load(Texture2D sprite)
        {
            this.sprite = sprite;
            font = Game1.content.Load<SpriteFont>("fonts\\npc1");
            prevState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (interacting)
            {
                if (state.IsKeyDown(Keys.Space) & !prevState.IsKeyDown(Keys.Space))
                {
                    AdvanceLine();
                    Console.WriteLine("pressed space");
                }
            }

            prevState = state;
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(sprite, new Vector2(100, 100), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            if (interacting)
            {
                if (characters.Count > 0)
                    AdvanceCharacter();

                Vector2 txtsize = font.MeasureString(line);
                Vector2 txtorigin = new Vector2(txtsize.X / 2f, txtsize.Y / 2f);
                Vector2 txtpos = new Vector2(Game1.screenCenter.X, 3 * Game1.screenCenter.Y / 2f);
                float size = textSize;

                batch.DrawString(font, currentLine, txtpos, Color.Red, 0f, txtorigin, size, SpriteEffects.None, 0f);
            }
        }

        public void Talk(SpriteBatch batch, string line, float talkSpeed)
        {
            currentLine = "";

            isTalking = false;
        }

        private void AdvanceCharacter()
        {
            char c = characters.Dequeue();
            currentLine += c;
        }

        private void AdvanceLine()
        {
            if (dialogue.Count > 0 && !isTalking)
            {
                //isTalking = true;

                line = dialogue.Dequeue();

                characters = new Queue<char>(line);

                currentLine = "";
            }
            else
            {
                //isTalking = false;
            }
        }

        public void Interact()
        {
            interacting = true;
        }
    }
}
