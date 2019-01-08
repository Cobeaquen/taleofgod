using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheTaleOfGod;
using System;
using System.IO;

namespace MapEditor
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RenderTarget2D scene;

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

        public static float resolutionScale;

        #endregion

        #region collider
        private Collider col;
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
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(graphics.GraphicsDevice, TheTaleOfGod.Game1.gameWidth, TheTaleOfGod.Game1.gameHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            resolutionScale = GraphicsDevice.Viewport.Width / TheTaleOfGod.Game1.gameWidth;
            TheTaleOfGod.Game1.resolutionScale = resolutionScale;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GUI.LoadGUI(Content.Load<SpriteFont>("gui\\fonts\\font"));
            DebugTextures.LoadTextures(GraphicsDevice);

            hud = new GUI();
            if (File.Exists("maps\\map1.bin"))
            {
                map = Map.Load("maps\\map1.bin");
                foreach (var c in map.colliders)
                {
                    c.debugSprite = DebugTextures.GenerateHollowRectangele(c.width, c.height, 2, Color.DarkRed);
                    c.debugOrigin = new Vector2(c.width / 2f, c.height / 2f);
                }
            }
            else
            {
                map = new Map();
            }

            screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
            screenBottomLeft = new Vector2(0, GraphicsDevice.Viewport.Height);
            screenBottomRight = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            screenTopLeft = new Vector2(0, 0);
            screenTopRight = new Vector2(GraphicsDevice.Viewport.Width, 0);

            GUI.buttons.Add(new Button(screenTopLeft + new Vector2(150, 150), "PLACE COLLIDER", true, DebugTextures.GenerateRectangle(150, 150, Color.Sienna), GUI.defaultFont, PlaceCollider));
            GUI.buttons.Add(Button.Debug("PLACE TILE", 150, 150, Color.DarkSeaGreen, PlaceTiles));

            previousState = Mouse.GetState();
        }

        protected override void UnloadContent()
        {
            Console.WriteLine("Saving map...");
            map.Save("maps\\map1.bin");
            Console.WriteLine("Map saved");
        }

        protected override void Update(GameTime gameTime)
        {
            Input.BeginCheckInput();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            hud.Update(gameTime);

            switch (currentAction)
            {
                case Action.None:
                    break;
                case Action.PlacingCollider:
                    PlaceCollider();
                    break;
                default:
                    break;
            }

            Input.EndCheckInput();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.SetRenderTarget(scene);

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

            #region collider
            foreach (var col in map.colliders)
            {
                col.DrawDebug(spriteBatch);
            }
            if (col != null)
            {
                col.DrawDebug(spriteBatch);
            }
            #endregion

            spriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(scene, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, resolutionScale, SpriteEffects.None, 0f);

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

        #region collider
        public void PlaceCollider()
        {
            currentAction = Action.PlacingCollider;

            if (Input.LeftMouseButtonDown(false) && !colClicked) // clicked
            {
                colPosition1 = Input.mousePosition / resolutionScale;
                colClicked = true;
            }
            else if (colClicked)
            {
                colPosition2 = Input.mousePosition / resolutionScale;
                if ((int)Math.Abs(colPosition1.X - colPosition2.X) > 0 && (int)Math.Abs(colPosition1.Y - colPosition2.Y) > 0)
                {
                    int width = (int)Math.Abs(colPosition1.X - colPosition2.X);
                    int height = (int)Math.Abs(colPosition1.Y - colPosition2.Y);

                    col = new Collider((colPosition1 + colPosition2) / 2, width, height);
                    col.debugSprite = DebugTextures.GenerateHollowRectangele(width, height, 2, Color.DarkRed);
                    col.debugOrigin = new Vector2(width / 2, height / 2);

                    if (Input.LeftMouseButtonDown(false)) // clicked
                    {
                        currentAction = Action.None;
                        colClicked = false;
                        map.colliders.Add(col);
                    }
                }
            }
        }
        #endregion

        #region tiles
        public void PlaceTiles()
        {

        }
        #endregion

        public enum Action
        {
            None, PlacingCollider
        }
    }
}
