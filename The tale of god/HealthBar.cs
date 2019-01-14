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
    public class HealthBar
    {
        public float Value { get; set; }

        public Vector2 position;
        public Texture2D sprite;
        public Texture2D progress;

        private Vector2 origin;

        private Color color;

        static Color color1 = Color.MediumSeaGreen;
        static Color color2 = Color.Red;

        public HealthBar()
        {
            Value = 1f;
            sprite = DebugTextures.GenerateHollowRectangele(10, 5, 1, Color.Black);
            progress = DebugTextures.GenerateRectangle(10, 5, Color.White);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);


        }
        public void ChangeValue(float value)
        {
            Value = value;
            
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            Vector2 origin1 = new Vector2(sprite.Width, progress.Height / 2);

            color = Color.Lerp(color2, color1, Value);

            batch.Draw(progress, position + new Vector2(sprite.Width/2f, 0f), null, color, 0f, origin1, new Vector2(Value, 1f), SpriteEffects.None, 0f);
        }
    }
}