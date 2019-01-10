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
        public float value;

        public Vector2 position;
        public Texture2D sprite;
        public Texture2D progress;

        private Vector2 origin;

        public HealthBar()
        {
            value = 1f;
            sprite = DebugTextures.GenerateHollowRectangele(10, 5, 1, Color.Black);
            progress = DebugTextures.GenerateRectangle(10, 5, Color.Green);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
        public void ChangeValue(float value)
        {
            this.value = value;
            progress = DebugTextures.GenerateRectangle((int)(value * 10), 5, Color.White);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            Vector2 origin1 = new Vector2(0, sprite.Height / 2f);
            batch.Draw(progress, position, null, Color.White, 0f, origin1, new Vector2(value, 1f), SpriteEffects.None, 0f);
        }
    }
}
