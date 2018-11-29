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
    public class Wall
    {
        public Vector2 position;
        int width = 100;
        int height = 400;

        Texture2D sprite;

        public Wall()
        {

        }

        public void Load(GraphicsDevice grphDev)
        {
            sprite = DebugTextures.GenerateSquare(grphDev, width, height, Color.AliceBlue);
        }

        public void Update()
        {
            
        }
    }
}
