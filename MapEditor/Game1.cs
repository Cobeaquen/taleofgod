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

        #region domain references

        public static Vector2 screenCenter;
        public static Vector2 screenTopRight;
        public static Vector2 screenTopLeft;
        public static Vector2 screenBottomRight;
        public static Vector2 screenBottomLeft;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SetApplicationSettings();
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

            GUI.buttons.Add(new Button(screenTopLeft + new Vector2(100, 100), "PLACE COLLIDER", true, DebugTextures.GenerateRectangle(150, 150, Color.Black), GUI.defaultFont, PlaceCollider));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            hud.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

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
        }

        public void PlaceCollider()
        {
            Console.WriteLine("PLACING COLLIDER");
        }
    }
}
