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
using XNAnimation;

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
        private String info = "";
        private String infoapp = "";
        SimpleCharacter enemy;

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
            fpsCamera = new FirstPersonCamera(this, new Vector3(3, Camera.HEIGHT, 40), new Vector3(3, Camera.HEIGHT, 10), Vector3.Up);
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
            ResourceManager.LoadTexture2D(Content, "jailbars", ResourceManager.JAIL_BARS);
            ResourceManager.LoadTexture2D(Content, "gunfire1", ResourceManager.GUNFIRE);
            ResourceManager.RegisterResource(ResourceManager.FONT, Content.Load<SpriteFont>("Times New Roman"));
            ResourceManager.RegisterResource(ResourceManager.PLAYER_GUN_SND, Content.Load<SoundEffect>(@"Sounds/Gun 5"));
            ResourceManager.RegisterResource(ResourceManager.OPERA_THEME_SONG, Content.Load<SoundEffect>(@"Sounds/opera"));
            ResourceManager.RegisterResource(ResourceManager.TERRORIST, Content.Load<SkinnedModel>(@"Models/PlayerMarine"));
            ResourceManager.RegisterResource(ResourceManager.TERRORIST_WEAPON, Content.Load<Model>(@"Models/colt-xm177"));

            //enemy = new SimpleCharacter(ResourceManager.GetResource<SkinnedModel>(ResourceManager.TERRORIST), 0.1f, new Vector3(0, -2, 0), null, 10);
            //enemy.AttachWeapon(ResourceManager.GetResource<Model>(ResourceManager.TERRORIST_WEAPON));
            //enemy.SetPosition(fpsCamera.GetPosition());
            //Vector3 fixPos = new Vector3(3, -2.8f, 60);
            //enemy.scale = 1;
            //enemy.fixPos = fixPos;
            //enemy.fixRotX = -3.24f;
            //enemy.fixRotY = 0;
            //enemy.fixRotZ = MathHelper.PiOver4;
            //enemy.Begin();

            SetGameState(new MainMenuState());
        }

        public void DrawInformation(String info)
        {
            DrawString(info, new Vector2(10, 10), Color.Aqua);
        }

        public T LoadModel<T>(String model, String id)
        {
            if (!ResourceManager.IsResourceRegistered(id))
            {
                ResourceManager.RegisterResource(id, Content.Load<T>(@"Models/" + model));
            }
            return ResourceManager.GetResource<T>(id);
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
                DrawInformation(info + infoapp);

                //reset render state for 3D drawing
                FPSGame.GetInstance().GraphicsDevice.RenderState.DepthBufferEnable = true;
                FPSGame.GetInstance().GraphicsDevice.RenderState.AlphaBlendEnable = false;
                FPSGame.GetInstance().GraphicsDevice.RenderState.AlphaTestEnable = false;

                currentState.Draw3D(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawLine3D(Vector3 src, Vector3 dst)
        { 
            Vector3 srcprj = GraphicsDevice.Viewport.Project(src, fpsCamera.GetProjection(), fpsCamera.GetView(), Matrix.Identity);
            Vector3 dstprj = GraphicsDevice.Viewport.Project(dst, fpsCamera.GetProjection(), fpsCamera.GetView(), Matrix.Identity);
            Line line = new Line(new Color[] { Color.Aqua });
            //DrawString("srcprj: " + srcprj.X + "/" + srcprj.Y + " dstprj: " + dstprj.X + "/" + dstprj.Y, new Vector2(10, 25), Color.Red);
            line.Render(new Vector2(srcprj.X, srcprj.Y), new Vector2(dstprj.X, dstprj.Y), Color.Red, 0);
            //line.Render(new Vector2(0, 0), new Vector2(400, 400), Color.Red, 0);
        }

        public void SetInfo(String s)
        {
            info = s;
        }

        public void AppendInfo(String s)
        {
            infoapp = s;
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

        public void DrawSprite(Texture2D texture, Vector2 pos, int depth)
        {
            spriteBatch.Draw(texture, pos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
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
