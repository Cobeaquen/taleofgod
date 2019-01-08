﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using ProtoBuf;

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

        public static GraphicsDevice graphicsDevice;

        RenderTarget2D scene;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<SceneObject> sceneObjects = new List<SceneObject>();

        public Character character = new Character();

        public static NPC npc1;
        public static NPC npc2;

        public static ContentManager content { get; set; }

        public static Vector2 screenCenter;

        public Wall testWall = new Wall(Vector2.One * 40, 50, 100);

        public bool debugDrawing = false;

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

            resolutionScale = GraphicsDevice.Viewport.Width / gameWidth;

            graphicsDevice = GraphicsDevice;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DebugTextures.LoadTextures(GraphicsDevice);
            Dialogue.InitializeDialogueSystem(graphics.GraphicsDevice.Viewport);

            screenCenter = new Vector2(gameWidth / 2f, gameHeight / 2f);

            character.LoadCharacter();

            npc1 = new NPC(new Vector2(100, -100), DebugTextures.GenerateRectangle(16, 32, Color.Yellow), "Hello world!!", "Great to see you decided to play this game!!");

            npc2 = new NPC(new Vector2(150, 10), DebugTextures.GenerateRectangle(16, 32, Color.Yellow), "Hello, my name is tommy! I used to live in peace", "watering me plants in me garden everyday, until the unpredictable struck");

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

            if (!character.isInteracting)
            {
                character.Update(gameTime);
            }

            foreach (var npc in NPC.npcs)
            {
                npc.Update(gameTime);
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

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, character.camera.view);

            character.Draw(spriteBatch);

            foreach (var npc in NPC.npcs)
            {
                npc.Draw(spriteBatch, gameTime);
            }

            foreach (var so in sceneObjects)
            {
                so.Draw(spriteBatch); // is this function only being called on the base class?
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

            foreach (var npc in NPC.npcs)
            {
                npc1.DrawDialogue(spriteBatch, gameTime);
                npc2.DrawDialogue(spriteBatch, gameTime);
            }

            //spriteBatch.DrawString(npc.font, "testing out this new rendertartget!! NICE JOB TEAM", new Vector2( Mouse.GetState().Position.X, Mouse.GetState().Position.Y), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
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