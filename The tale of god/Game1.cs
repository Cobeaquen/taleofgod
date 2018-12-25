using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace TheTaleOfGod
{
    public class Game1 : Game
    {
        #region singleton

        public static Game1 instance;

        #endregion

        public static GraphicsDevice graphicsDevice;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<SceneObject> sceneObjects = new List<SceneObject>();

        public Character character = new Character();

        public static NPC npc;

        public static ContentManager content { get; set; }

        public static Vector2 screenCenter;

        public Wall testWall = new Wall(Vector2.One * 300, 100, 200);

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
            graphicsDevice = GraphicsDevice;

            character.LoadCharacter();

            npc = new NPC();
            npc.Load(DebugTextures.GenerateRectangle(100, 100, Color.White));

            foreach (var so in sceneObjects)
            {
                so.Load(); // is this function only being called on the subclass?
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            character.Draw(spriteBatch);

            npc.Draw(spriteBatch, gameTime);

            foreach (var so in sceneObjects)
            {
                so.Draw(spriteBatch); // is this function only being called on the subclass?
            }

            if (debugDrawing)
            {
                if (Collision.debugTexture != null)
                {
                    spriteBatch.Draw(Collision.debugTexture, Collision.colPosition, null, Color.Red, 0f, Collision.colOrigin, 1f, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetApplicationSettings()
        {
            // resolution
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
        }
    }
}