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
        public float talkSpeed;

        public string[] lines;
        public string line;
        public string currentLine;
        public Queue<char> characters = new Queue<char>();
        public Queue<string> dialogue = new Queue<string>();

        public Dialogue(params string[] lines)
        {
            this.lines = lines;
        }

        public void LoadLines() // initializes the speech of the npc
        {
            dialogue = new Queue<string>(lines);
        }

        public void AdvanceCharacter()
        {
            char c = characters.Dequeue();
            currentLine += c;
        }

        public bool AdvanceLine()
        {
            if (dialogue.Count > 0)
            {
                line = dialogue.Dequeue();

                characters = new Queue<char>(line);

                currentLine = "";

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
