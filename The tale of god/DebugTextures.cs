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
        public static GraphicsDevice graphicsDevice;

        public static void LoadTextures(GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
        }

        public static Texture2D GenerateRectangle(int width, int height, Color color)
        {
            Texture2D tx = new Texture2D(graphicsDevice, width, height);

            Color[] clrs = new Color[width * height];

            for (int i = 0; i < width * height; i++)
            {
                clrs[i] = color;
            }
            tx.SetData(clrs);
            return tx;
        }
        public static Texture2D GenerateHollowRectangele(int width, int height, int edgeSize, Color color)
        {
            Texture2D tx = new Texture2D(graphicsDevice, width, height);

            Color[] clrs = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x >= edgeSize && x < width - edgeSize && y >= edgeSize && y < height - edgeSize)
                    {
                        clrs[y * width + x] = Color.Transparent;
                    }
                    else
                    {
                        clrs[y * width + x] = color;
                    }
                }
            }
            tx.SetData(clrs);
            return tx;
        }
    }
}
