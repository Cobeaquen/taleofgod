using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheTaleOfGod
{
    public class Dialogue
    {
        public static Vector2 DialoguePosition { get; set; }
        public static Vector2 DialogueBoxPosition { get; set; }

        public float talkSpeed;
        public float textSize;

        public static Texture2D dialogueBox;

        public Text[] lines;
        public string line;
        public Text currentLine;
        public Queue<char> characters = new Queue<char>();
        public Queue<string> dialogue = new Queue<string>();

        public static void InitializeDialogueSystem(Viewport viewport)
        {
            //dialogueBox = Game1.content.Load<Texture2D>("textures\\ui\\dialogue_box");
            dialogueBox = DebugTextures.GenerateRectangle(200, 50, new Color(new Vector4(0.15686274509803921568627450980392f, 0.15686274509803921568627450980392f, 0.15686274509803921568627450980392f, 1f)));
            DialogueBoxPosition = new Vector2(-dialogueBox.Width/2, Game1.gameHeight / 4);
            DialoguePosition = DialogueBoxPosition * Game1.instance.resolutionScale + new Vector2(viewport.Width, viewport.Height)/2 + new Vector2(15, 25); //Game1.instance.resolutionScale;
        }

        public Dialogue(string[] lines, float textSize, SpriteFont font)
        {
            this.lines = Text.StringsToText(lines, DialoguePosition, (int)(dialogueBox.Width * Game1.instance.resolutionScale), (int)(dialogueBox.Height * Game1.instance.resolutionScale), font);
            this.textSize = textSize;
            currentLine = new Text(DialoguePosition, "", (int)(dialogueBox.Width * Game1.instance.resolutionScale), (int)(dialogueBox.Height * Game1.instance.resolutionScale), font);
        }

        public void LoadSpeech() // initializes the speech of the npc
        {
            dialogue = new Queue<string>();
            foreach (var txt in lines)
            {
                dialogue.Enqueue(txt.text);
            }
        }

        public bool AdvanceCharacter()
        {
            if (characters.Count > 0)
            {
                char c = characters.Dequeue();
                currentLine.text += c;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AdvanceLine()
        {
            if (dialogue.Count > 0)
            {
                line = dialogue.Dequeue();
                characters = new Queue<char>(line);
                currentLine.text = "";

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (characters.Count > 0)
                AdvanceCharacter();

            //Vector2 txtsize = font.MeasureString(line);
            //Vector2 txtorigin = new Vector2(txtsize.X / 2f, txtsize.Y / 2f);
            Vector2 txtpos = new Vector2(Game1.screenCenter.X, 3 * Game1.screenCenter.Y / 2f);

            currentLine.Draw(batch);
            //batch.DrawString(lines[0].font, currentLine, DialoguePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f); // remember the font here is always set to the first lines font
        }
        public void DrawDialogueBox(SpriteBatch batch)
        {
            Vector2 windowPosition = Game1.instance.character.camera.position + DialogueBoxPosition;// - new Vector2(600, 950);

            batch.Draw(dialogueBox, windowPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }
    }
}
