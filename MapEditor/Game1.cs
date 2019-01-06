using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheTaleOfGod;
using System;

namespace MapEditor
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GUI hud;

        Map map;

        Action currentAction;

        MouseState previousState;

        #region domain references

        public static Vector2 screenCenter;
        public static Vector2 screenTopRight;
        public static Vector2 screenTopLeft;
        public static Vector2 screenBottomRight;
        public static Vector2 screenBottomLeft;

        #endregion

        #region collider
        private Texture2D colSprite;
        private Vector2 colOrigin;
        private Vector2 colPosition1;
        private Vector2 colPosition2;
        private bool colClicked = false;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SetApplicationSettings();
            currentAction = Action.None;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GUI.LoadGUI(Content.Load<SpriteFont>("gui\\fonts\\font"));
            DebugTextures.LoadTextures(GraphicsDevice);

            hud = new GUI();
            map = new Map();

            screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
            screenBottomLeft = new Vector2(0, GraphicsDevice.Viewport.Height);
            screenBottomRight = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            screenTopLeft = new Vector2(0, 0);
            screenTopRight = new Vector2(GraphicsDevice.Viewport.Width, 0);

            GUI.buttons.Add(new Button(screenTopLeft + new Vector2(100, 100), "PLACE COLLIDER", true, DebugTextures.GenerateRectangle(150, 150, Color.Sienna), GUI.defaultFont, PlaceCollider));

            previousState = Mouse.GetState();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState state = Mouse.GetState();

            hud.Update();

            switch (currentAction)
            {
                case Action.None:
                    break;
                case Action.PlacingCollider:
                    PlaceCollider(state);
                    break;
                default:
                    break;
            }

            previousState = state;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

            switch (currentAction)
            {
                case Action.None:
                    break;
                case Action.PlacingCollider:
                    DrawCollider();
                    break;
                default:
                    break;
            }

            hud.DrawGUI(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void SetApplicationSettings()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        public void PlaceCollider(MouseState state)
        {
            Console.WriteLine("PLACING COLLIDER");
            currentAction = Action.PlacingCollider;
            
            if (state.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released && !colClicked) // clicked
            {
                colPosition1 = state.Position.ToVector2();
                colClicked = true;
            }
            if (colClicked)
            {
                colPosition2 = state.Position.ToVector2();
                if (state.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released) // clicked
                {
                    colPosition2 = state.Position.ToVector2();
                }
                if ((int)Math.Abs(colPosition1.X - colPosition2.X) > 0 && (int)Math.Abs(colPosition1.Y - colPosition2.Y) > 0)
                {
                    colSprite = DebugTextures.GenerateRectangle((int)Math.Abs(colPosition1.X - colPosition2.X), (int)Math.Abs(colPosition1.Y - colPosition2.Y), Color.DarkRed);
                    colOrigin = new Vector2(colSprite.Width / 2, colSprite.Height / 2);
                }
            }
        }
        public void DrawCollider()
        {
            if (colSprite != null)
            {
                spriteBatch.Draw(colSprite, colPosition1 - colPosition2, null, Color.White, 0f, colPosition1, 1f, SpriteEffects.None, 0f); // fiiix mee
            }
        }

        public enum Action
        {
            None, PlacingCollider
        }
    }
}
