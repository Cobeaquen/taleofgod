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
    public class Bar
    {
        public Texture2D progress;
        public Texture2D border;

        public Vector2 origin;

        public Vector2 position;

        public float Value { get; set; }
        public float maxValue;

        public float width;
        public float height;

        public Color color1;
        public Color color2;

        private Color color;

        public Bar (int width, int height, Color color1, Color color2, int borderThickness = 0, float startValue = 0f)
        {
            Value = startValue;

            this.color1 = color1;
            this.color2 = color2;

            this.width = width;
            this.height = height;

            progress = DebugTextures.GenerateRectangle(width, height, Color.White);

            if (borderThickness > 0)
            {
                border = DebugTextures.GenerateHollowRectangele(width, height, borderThickness, Color.Black);
                origin = new Vector2(border.Width / 2f, border.Height / 2f);
            }

            if (color2 == null)
            {
                color2 = color1;
            }
        }

        public void ChangeValue(float newValue)
        {
            Value = newValue;
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 origin1 = new Vector2(progress.Width, progress.Height / 2);

            color = Color.Lerp(color2, color1, Value);

            batch.Draw(progress, position + new Vector2(progress.Width / 2f, 0f), null, color, 0f, origin1, new Vector2(Value, 1f), SpriteEffects.None, 1f);

            if (border != null)
            {
                batch.Draw(border, position, null, color, 0f, origin, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}