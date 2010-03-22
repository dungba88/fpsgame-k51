using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FPSGame.Engine;
using FPSGame.Engine.GameState;
using FPSGame.Object;
using FPSGame.Factory;
using FPSGame.Core;

namespace FPSGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FPSGame : Microsoft.Xna.Framework.Game
    {
        public const int WIDTH = 800;
        public const int HEIGHT = 600;
        public const bool FULLSCREEN_ENABLED = false;

        public GraphicsDeviceManager graphics { get; protected set; }
        SpriteBatch spriteBatch;
        Brick brick, brick1;

        private static FPSGame instance = new FPSGame();

        private IGameState currentState = null;
        private FirstPersonCamera fpsCamera = null;
        private Player player;
        private bool isUpdating;
        private bool shouldEnd;

        public static FPSGame GetInstance()
        {
            return instance;
        }

        private FPSGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.IsFullScreen = FULLSCREEN_ENABLED;
            Content.RootDirectory = "Content";
            fpsCamera = new FirstPersonCamera(this, new Vector3(5, 2, 5), new Vector3(10, 2, 5), Vector3.Up);
            Components.Add(fpsCamera);
            player = new Player();

            isUpdating = false;
            shouldEnd = false;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public FirstPersonCamera GetFPSCamera()
        {
            return fpsCamera;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ResourceManager.LoadTexture2D(Content, "back", ResourceManager.BACK_BUTTON);
            ResourceManager.LoadTexture2D(Content, "back_off", ResourceManager.BACK_BUTTON);
            ResourceManager.LoadTexture2D(Content, "credits", ResourceManager.ABOUT_BUTTON);
            ResourceManager.LoadTexture2D(Content, "credits_off", ResourceManager.ABOUT_BUTTON_OFF);
            ResourceManager.LoadTexture2D(Content, "exitgame", ResourceManager.EXIT_BUTTON);
            ResourceManager.LoadTexture2D(Content, "exitgame_off", ResourceManager.EXIT_BUTTON_OFF);
            ResourceManager.LoadTexture2D(Content, "oneplayer", ResourceManager.NEW_GAME_BUTTON);
            ResourceManager.LoadTexture2D(Content, "oneplayer_off", ResourceManager.NEW_GAME_BUTTON_OFF);
            ResourceManager.LoadTexture2D(Content, "option", ResourceManager.OPTION_BUTTON);
            ResourceManager.LoadTexture2D(Content, "option_off", ResourceManager.OPTION_BUTTON_OFF);
            ResourceManager.LoadTexture2D(Content, "floor111", ResourceManager.FLOOR_TEXTURE);
            ResourceManager.LoadTexture2D(Content, "wall222", ResourceManager.WALL_TEXTURE);
            ResourceManager.LoadTexture2D(Content, "ceiling222", ResourceManager.CEILING_TEXTURE);
            ResourceManager.LoadTexture2D(Content, "playergun", ResourceManager.PLAYER_DEFAULT_GUN);
            ResourceManager.RegisterResource(ResourceManager.FONT, Content.Load<SpriteFont>("Times New Roman"));

            //brick = new Brick(Vector3.Zero, Vector3.Forward, Vector3.Up, 0.125f, ResourceManager.GetResource<Texture2D>(ResourceManager.FLOOR_TEXTURE));
            //brick.Begin();
            //brick1 = new Brick(new Vector3(0.25f, 0, 0.125f), Vector3.Forward, Vector3.Up, 0.125f, ResourceManager.GetResource<Texture2D>(ResourceManager.FLOOR_TEXTURE));
            //brick1.Begin();

            //MapLoader.GetInstance().BuildMap("Maps/map1.xml");

            SetGameState(new MainMenuState());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (shouldEnd)
                Exit();
            isUpdating = true;

            // TODO: Add your update logic here
            if (currentState != null)
            {
                currentState.Update(gameTime);
                //ObjectManager.GetInstance().Update(gameTime);
            }
            base.Update(gameTime);
            isUpdating = false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (currentState != null)
            {
                currentState.Draw(gameTime);

                //reset render state for 3D drawing
                FPSGame.GetInstance().GraphicsDevice.RenderState.DepthBufferEnable = true;
                FPSGame.GetInstance().GraphicsDevice.RenderState.AlphaBlendEnable = false;
                FPSGame.GetInstance().GraphicsDevice.RenderState.AlphaTestEnable = false;

                currentState.Draw3D(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetGameState(IGameState state)
        {
            if (currentState!=null&&!currentState.IsDead())
                currentState.End();
            currentState = state;
            currentState.StartOver();
        }

        public void DrawSprite(Texture2D texture, Vector2 pos, Color col)
        {
            spriteBatch.Draw(texture, pos, col);
        }

        public void DrawSprite(Texture2D texture, Vector2 pos)
        {
            DrawSprite(texture, pos, Color.White);
        }

        public void DrawString(String text, Vector2 pos, Color color)
        {
            SpriteFont font = ResourceManager.GetResource<SpriteFont>(ResourceManager.FONT);
            spriteBatch.DrawString(font, text, pos, color);
        }

        public void QuitGame()
        {
            if (isUpdating)
            {
                shouldEnd = true;
            }
        }

        public void ShowMouse()
        {
            this.IsMouseVisible = true;
        }

        public void HideMouse()
        {
            this.IsMouseVisible = false;
        }

        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }
    }
}
