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
    public class NPC
    {
        public Vector2 position;
        public Texture2D sprite;
        public float textSize = 2f;

        public bool interacting;

        public SpriteFont font;

        Queue<string> dialogue;

        public NPC()
        {
            interacting = false;
            position = Vector2.Zero;

            dialogue.Enqueue("oof, wellp\n" +
            "that stuff happens from time to time");
            dialogue.Enqueue("anyway have a good day");
        }

        public void Load(Texture2D sprite)
        {
            this.sprite = sprite;
            font = Game1.content.Load<SpriteFont>("fonts\\npc1");
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, new Vector2(100, 100), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            if (interacting)
            {
                Vector2 txtsize = font.MeasureString(line);
                Vector2 txtorigin = new Vector2(txtsize.X / 2f, txtsize.Y / 2f);
                Vector2 txtpos = new Vector2(Game1.screenCenter.X, 3 * Game1.screenCenter.Y / 2f);

                float size = textSize;

                batch.DrawString(font, dialogue, txtpos, Color.Red, 0f, txtorigin, size, SpriteEffects.None, 0f);
            }
        }

        public void Talk(string line)
        {
            
        }

        public void Interact()
        {
            interacting = true;
        }
    }
}
