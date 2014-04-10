using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PaperCraft.onGame;
using PaperCraft.scene;
using Microsoft.Xna.Framework.Audio;

namespace PaperCraft
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SceneManager smgr;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth =  1366;
            graphics.PreferredBackBufferHeight = 768;
            //graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = true;

            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            smgr = new SceneManager(graphics);
            smgr.AddScene("GameScene", new GameScene());
            smgr.AddScene("SplashScene", new SplashScene());
            smgr.AddScene("MenuScene", new MenuScene());
            smgr.AddScene("StageSelectScene", new StageSelectScene());

            smgr.ChangeScene("MenuScene");
        }

        protected override void Initialize()
        {
            smgr.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SoundManager.addSound("pass", Content.Load<SoundEffect>("pass"));
            SoundManager.addSound("collide", Content.Load<SoundEffect>("collide"));

            smgr.Create(Content);
            BoundingSphereRenderer.InitializeGraphics(graphics.GraphicsDevice, 50);
        }

        protected override void UnloadContent()
        {
            //smgr.Stop();
        }

        protected override void Update(GameTime gameTime)
        {
            float timeDelta = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            smgr.Update(timeDelta);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DepthStencilState d = new DepthStencilState();
            d.DepthBufferEnable = true;
            graphics.GraphicsDevice.DepthStencilState = d;

            smgr.Draw();
            base.Draw(gameTime);
        }
    }
}
