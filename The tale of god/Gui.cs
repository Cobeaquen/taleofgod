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
    public class Gui
    {
        public Bar bar;
        public Text HPText;

        public Gui(float maxHealth)
        {
            bar = new Bar(200, 60, Color.MediumSpringGreen, Color.Red, 5)
            {
                position = new Vector2(150, 700), maxValue = maxHealth
            };
            HPText = new Text(bar.position + new Vector2(-bar.width/2f + Text.defaultFont.MeasureString("HP").X, -50), "HP", 200, 100, 1, Text.defaultFont, Color.White);
        }

        public void Update()
        {

        }
        public void UpdateHealth(float health)
        {
            bar.ChangeValue(health / bar.maxValue);
        }

        public void Draw(SpriteBatch batch)
        {
            bar.Draw(batch);
            HPText.Draw(batch);
        }
    }
}
