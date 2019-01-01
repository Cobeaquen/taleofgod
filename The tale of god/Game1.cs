using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace TheTaleOfGod
{
    public class Game1 : Game
    {
        #region singleton

        public static Game1 instance;

        #endregion

        #region game dimensions

        public static readonly int gameWidth = 640;
        public static readonly int gameHeight = 360;

        public float resolutionScale;
        public float resolutionScale2;

        #endregion

        #region fps
        double totalSeconds;
        int frames;
        float fps;
        #endregion

        public static GraphicsDevice graphicsDevice;

        RenderTarget2D scene;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<SceneObject> sceneObjects = new List<SceneObject>();

        public Character character = new Character();

        public static NPC npc;

        public static ContentManager content { get; set; }

        public static Vector2 screenCenter;

        public Wall testWall = new Wall(Vector2.One * 40, 40, 70);

        public bool debugDrawing = true;

        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;

            sceneObjects.Add(testWall);

            SetApplicationSettings();
        }

        protected override void Initialize()
        {
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;

            scene = new RenderTarget2D(graphics.GraphicsDevice, gameWidth, gameHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenCenter = new Vector2(gameWidth / 2f, gameHeight / 2f);
            graphicsDevice = GraphicsDevice;

            resolutionScale = GraphicsDevice.Viewport.Width / gameWidth;
            resolutionScale2 = gameWidth / gameHeight;

            character.LoadCharacter();

            npc = new NPC();
            npc.Load(DebugTextures.GenerateRectangle(16, 32, Color.White));

            foreach (var so in sceneObjects)
            {
                so.Load(); // is this function only being called on the base class?
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            character.Update(gameTime);
            npc.Update(gameTime);

            #region fps

            totalSeconds += gameTime.ElapsedGameTime.Ticks / 10000000f;
            frames++;
            fps = (1f / (float)totalSeconds) * frames;

            if ((int)totalSeconds > 1)
            {
                frames = 0;
                totalSeconds = 0;
                Console.WriteLine(fps);
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.SetRenderTarget(scene);

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, character.camera.view);

            spriteBatch.Draw(scene, Vector2.Zero, Color.White);

            character.Draw(spriteBatch);

            npc.Draw(spriteBatch, gameTime);

            foreach (var so in sceneObjects)
            {
                so.Draw(spriteBatch); // is this function only being called on the base class?
            }

            if (debugDrawing)
            {
                if (Collision.debugTexture != null)
                {
                    spriteBatch.Draw(Collision.debugTexture, Collision.colPosition, null, Color.DarkRed, 0f, Collision.colOrigin, 1f, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(scene, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, resolutionScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            spriteBatch.Begin(); // drawing ui

            //spriteBatch.DrawString(npc.font, "testing out this new rendertartget!! NICE JOB TEAM", new Vector2( Mouse.GetState().Position.X, Mouse.GetState().Position.Y), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetApplicationSettings()
        {
            // screen resolution
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphics.IsFullScreen = false;
        }
    }
}