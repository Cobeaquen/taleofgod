using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheTaleOfGod
{
    public class DebugTextures
    {
        public static Texture2D GenerateSquare(GraphicsDevice grph, int width, int height, Color color)
        {
            Texture2D tx = new Texture2D(grph, width, height);

            Color[] clrs = new Color[width * height];

            for (int i = 0; i < width * height; i++)
            {
                clrs[i] = color;
            }
            tx.SetData(clrs);
            return tx;
        }
    }
}
