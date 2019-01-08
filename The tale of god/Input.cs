using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TheTaleOfGod
{
    public class Input
    {
        public static MouseState mousePreviousState;
        public static MouseState mouseState;

        public static Vector2 mousePosition;

        public static bool LeftMouseButtonUp(bool continuous)
        {
            return continuous ? mouseState.LeftButton == ButtonState.Released : mouseState.LeftButton == ButtonState.Released && mousePreviousState.LeftButton == ButtonState.Pressed;
        }
        public static bool LeftMouseButtonDown(bool continuous)
        {
            return continuous ? mouseState.LeftButton == ButtonState.Pressed : mouseState.LeftButton == ButtonState.Pressed && mousePreviousState.LeftButton == ButtonState.Released;
        }
        public static void BeginCheckInput()
        {
            mouseState = Mouse.GetState();
            mousePosition = mouseState.Position.ToVector2();
        }
        public static void EndCheckInput()
        {
            mousePreviousState = mouseState;
        }
    }
}
