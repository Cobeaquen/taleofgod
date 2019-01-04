﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheTaleOfGod
{
    public class Text
    {
        public int fieldWidth, fieldHeight;
        public string text;

        public SpriteFont font;

        public Vector2 position;
        Vector2 origin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="fieldWidth"></param>
        /// <param name="fieldHeight"></param>
        /// <param name="font"></param>
        public Text(Vector2 position, string text, int fieldWidth, int fieldHeight, SpriteFont font)
        {
            this.position = position;
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.text = text;
            this.font = font;
            this.text = ClampText();
            origin = font.MeasureString(this.text) / 2f;
            //origin = new Vector2(fieldWidth/2f, fieldHeight/2f) / Game1.instance.resolutionScale;
        }
        public string ClampText()
        {
            string[] words = text.Split(' ');

            string result = "";

            foreach (var word in words)
            {
                if (font.MeasureString(result + " " + word).X > fieldWidth) // the text is too wide
                {
                    result += "\n";
                }
                if (font.MeasureString(result + word).Y > fieldHeight)
                {
                    break;
                }
                result += word + " ";
            }
            return result;
        }
        public void Draw(SpriteBatch batch)
        {
            batch.DrawString(font, text, position, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public static Text[] StringsToText(string[] values, Vector2 position, int fieldWidth, int fieldHeight, SpriteFont font)
        {
            Text[] texts = new Text[values.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = new Text(position, values[i], fieldWidth, fieldHeight, font);
            }
            return texts;
        }
    }
}