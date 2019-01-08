using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheTaleOfGod;

namespace TheTaleOfGod
{
    public class GUI
    {
        public static SpriteFont defaultFont;

        public static List<Button> buttons;
        public static List<Text> texts;

        public GUI()
        {
            buttons = new List<Button>();
            texts = new List<Text>();
        }
        public static void LoadGUI(SpriteFont defaultFont)
        {
            GUI.defaultFont = defaultFont;
        }
        public void Update(GameTime gameTime)
        {
            foreach (var b in buttons)
            {
                b.Update(gameTime);
            }
        }

        public void DrawGUI(SpriteBatch batch)
        {
            foreach (var b in buttons)
            {
                b.Draw(batch);
            }
            foreach (var t in texts)
            {
                t.Draw(batch);
            }
        }
    }
}
