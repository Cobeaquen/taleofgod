using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using ProtoBuf;
using TheTaleOfGod.enemies;

namespace TheTaleOfGod
{
    public class Game1 : Game
    {
        #region singleton

        public static Game1 instance;

        #endregion

        #region game dimensions

        public static readonly int gameWidth = 480;
        public static readonly int gameHeight = 270;

        public static float resolutionScale;

        #endregion

        #region fps
        double totalSeconds;
        int frames;
        float fps;
        #endregion

        public Map map;

        public static GraphicsDevice graphicsDevice;

        RenderTarget2D scene;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Character character;

        public static NPC npc1;
        public static NPC npc2;

        public Enemy enemy;

        public static ContentManager content { get; set; }

        public static Vector2 screenCenter;

        public bool debugDrawing = false;
        public bool drawGrid = false;

        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;

            SetApplicationSettings();
        }

        protected override void Initialize()
        {
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(graphics.GraphicsDevice, gameWidth, gameHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            resolutionScale = GraphicsDevice.Viewport.Width / gameWidth;

            graphicsDevice = GraphicsDevice;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DebugTextures.LoadTextures(GraphicsDevice);

            Cell.CreateGrid(new Point(-3200, -3200), 500, 500);

            Dialogue.InitializeDialogueSystem(graphics.GraphicsDevice.Viewport);

            map = new Map();

            screenCenter = new Vector2(gameWidth / 2f, gameHeight / 2f);

            character = new Character();

            map.npcs.Add(new NPC(new Vector2(100, -100), DebugTextures.GenerateRectangle(16, 32, Color.Yellow), "Hello world!!", "Great to see you decided to play this game!!"));
            map.npcs.Add(new NPC(new Vector2(150, 10), DebugTextures.GenerateRectangle(16, 32, Color.Yellow), "Hello, my name is tommy! I used to live in peace", "watering me plants in me garden everyday, until the unpredictable struck"));
            map.enemies.Add(new PeasantSoldier(50f, 0.005f, 200f, 300f, 50, screenCenter, DebugTextures.GenerateRectangle(16, 16, Color.DarkGray), character));
            map.enemies.Add(new PeasantSoldier(86, 0.004f, 200f, 300f, 50, new Vector2(53, 3466), DebugTextures.GenerateRectangle(16, 16, Color.GreenYellow), character));
            map.enemies.Add(new PeasantSoldier(70f, 0.004f, 200f, 300f, 50, new Vector2(62, 23), DebugTextures.GenerateRectangle(16, 16, Color.DarkRed), character));
            map.enemies.Add(new PeasantSoldier(69f, 0.004f, 200f, 300f, 50, new Vector2(97, 2), DebugTextures.GenerateRectangle(16, 16, Color.Red), character));
            map.enemies.Add(new PeasantSoldier(82f, 0.004f, 200f, 300f, 50, new Vector2(64, 0), DebugTextures.GenerateRectangle(16, 16, Color.Yellow), character));
            map.enemies.Add(new PeasantSoldier(56f, 0.004f, 200f, 300f, 50, new Vector2(2, 49), DebugTextures.GenerateRectangle(16, 16, Color.Blue), character));

            Vector2 pos = Cell.SnapToGrid(Vector2.One * 40);

            map.objects.Add(new Wall(pos, 64, 64, true));

            Raycast.RayCastTest();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Cell.UpdateCellsOnScreen(character.camera.position, gameWidth, gameHeight);

            if (!character.isInteracting)
            {
                character.Update(gameTime);
            }

            foreach (var npc in map.npcs)
            {
                npc.Update(gameTime);
            }

            foreach (var col in map.colliders)
            {
                col.Update(gameTime);
            }

            foreach (var enemy in map.enemies)
            {
                enemy.Update(gameTime);
            }

            foreach (var so in map.objects)
            {
                so.Update();
            }

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

            graphics.GraphicsDevice.Clear(Color.DimGray);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, character.camera.view);

            if (drawGrid)
            {
                foreach (var col in map.colliders)
                {
                    foreach (var ray in col.edges)
                    {
                        ray.Draw(spriteBatch);
                    }
                }
            }

            character.Draw(spriteBatch);

            if (drawGrid)
            {
                Cell.DrawGrid(spriteBatch);
                if (character.gun.ray != null)
                {
                    character.gun.ray.Draw(spriteBatch);
                }
            }

            foreach (var npc in map.npcs)
            {
                npc.Draw(spriteBatch, gameTime);
            }

            foreach (var so in map.objects)
            {
                so.Draw(spriteBatch); // is this function only being called on the base class?
            }

            foreach (var enemy in map.enemies)
            {
                enemy.Draw(spriteBatch);
            }

            if (debugDrawing)
            {
                if (Collision.debugTexture != null)
                {
                    spriteBatch.Draw(Collision.debugTexture, Collision.colPosition, null, Color.DarkRed, 0f, Collision.colOrigin, 0f, SpriteEffects.None, 1f);
                }
            }

            spriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(scene, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, resolutionScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            spriteBatch.Begin(); // drawing ui

            foreach (var npc in map.npcs)
            {
                npc.DrawDialogue(spriteBatch, gameTime);
            }

            //spriteBatch.DrawString(npc.font, "testing out this new rendertartget!! NICE JOB TEAM", new Vector2( Mouse.GetState().Position.X, Mouse.GetState().Position.Y), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region General functions

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float LerpRotation(float start, float end, float amount)
        {
            return MathHelper.Lerp(start, end, amount);
        }

        public static float Sigmoid(float x, float steepness)
        {
            return (steepness * x) / (float)Math.Sqrt(1f + (float)Math.Pow(steepness * x, 2f));
        }

        #endregion


        private void SetApplicationSettings()
        {
            // screen resolution
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 810;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphics.IsFullScreen = false;
        }
    }
}