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
            bar = new Bar(400, 120, Color.MediumSpringGreen, Color.Red, Color.DarkSlateGray, 3, 1f)
            {
                position = new Vector2(240, 1000), maxValue = maxHealth
            };
            HPText = new Text(bar.position + new Vector2(-bar.width/2f + Text.guiFont.MeasureString("HP").X, -100), "HP", 200, 100, 1, Text.guiFont, Color.White);
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
