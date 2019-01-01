using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheTaleOfGod
{
    public class Camera
    {
        public Vector2 position;
        public Matrix view;

        private Vector2 previousPos = Vector2.Zero;
        private Vector2 Target = Vector2.Zero;

        public float minPositionX;
        public float minPositionY;
        public float maxPositionX;
        public float maxPositionY;

        public float freeMovespeed = 4f;

        public Camera(Vector2 startPosition, float xMin, float xMax, float yMin, float yMax)
        {
            position = startPosition;
            Target = position;

            minPositionX = xMin;
            maxPositionX = xMax;
            minPositionY = yMin;
            maxPositionY = yMax;
        }

        public void MoveTowards(Vector2 target, GameTime gameTime)
        {
            if (position.X < minPositionX)
            {
                position.X = minPositionX;
            }
            else
            {
                position = Vector2.Lerp(position, target, 1f/gameTime.ElapsedGameTime.Ticks * 5);
            }
            HitMapWall();
            SetDisplay(position);
        }
        public void HitMapWall()
        {
            bool hit = false;
            if (position.X < minPositionX)
            {
                hit = true;
                position.X = minPositionX;
            }
            else if (position.X > maxPositionX)
            {
                hit = true;
                position.X = maxPositionX;
            }
            if (position.Y < minPositionY)
            {
                hit = true;
                position.Y = minPositionY;
            }
            else if (position.Y > maxPositionY)
            {
                hit = true;
                position.Y = maxPositionY;
            }
            /*if (hit && PlatformerGame.settings.EditorMode)
            {
                Target = position;
            }*/

        }
        public Vector2 WindowToWorldSpace(Vector2 windowPosition)
        {
            // First, must go from window -> camera space
            Vector2 cameraSpace = WindowToCameraSpace(windowPosition);

            // And then, camera -> world space
            return Vector2.Transform(cameraSpace, Matrix.Invert(view));
        }

        public Vector2 WindowToCameraSpace(Vector2 windowPosition)
        {
            // Scale for camera bounds that vary from window
            // Also, must adjust for translation if camera isn't at 0, 0 in screen space (such as a mini-map)
            return (windowPosition / Game1.instance.resolutionScale);
        }

        public bool HasMoved(float distance)
        {
            if (Math.Abs(previousPos.X - position.X) > distance || Math.Abs(previousPos.Y - position.Y) > distance)
            {
                previousPos = position;
                return true;
            }
            return false;
        }
        public void MoveAroundFreely(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S))
            {
                Vector2 move = Vector2.Zero;
                if (state.IsKeyDown(Keys.D))
                {
                    move.X += freeMovespeed;
                }
                else if (state.IsKeyDown(Keys.A))
                {
                    move.X -= freeMovespeed;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    move.Y -= freeMovespeed;
                }
                else if (state.IsKeyDown(Keys.S))
                {
                    move.Y += freeMovespeed;
                }

                move *= gameTime.ElapsedGameTime.Ticks / 100000f;
                position += move;

                HitMapWall();

                SetDisplay(position);
            }
        }
        public Vector2 WindowToWorldCoords(Vector2 position)
        {
            return position - new Vector2(view.Translation.X, view.Translation.Y);
        }
        public void SetDisplay(Vector2 position)
        {
            view = Matrix.CreateTranslation(new Vector3(Game1.screenCenter - position / Game1.instance.resolutionScale2, 0f)); // try putting in loadContent method for optimization
        }
    }
}