using Microsoft.Xna.Framework;
using PaperCraft.scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace PaperCraft.onGame
{
    class GameScene : Scene
    {
        private int[,] level = { { 1, 5 , 162, 200, 214}, { 1, 10 , 243, 171, 7}, { 2, 30 , 112, 111, 103}, { 5, 50 ,3, 54, 10} };
        // xna
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        private KinectInterface kInterface;

        Texture2D hand;

        Model scene;
        Vector3 theAmbient = new Vector3(0,0,0);
        //Ring theRing;
        RingManager ringMgr;

        Craft theCraft;
        Camera theCamera;
        SkyBox theSky;
        SkyBox[] skys = null;

        //hp bar
        Texture2D gameUI;
        Texture2D hpbar_back;
        Texture2D hpbar_in;

        GameDialog dialog;

        // songs
        Song[] bgms = new Song[5];

        float gameTime = 0;
        int stage = 0;

        public GameScene() {

            theCraft = new Craft();

            skys = new SkyBox[4];

            //theRing = new Ring(new Random());
            ringMgr = new RingManager();
            dialog = new GameDialog();

            stage = 0;
        }

        public override void Initialize()
        {
            this.graphics = this.smgr.getGDM();
            this.kInterface = this.smgr.getKinect();

            theCamera = new Camera(graphics.GraphicsDevice.Viewport.AspectRatio);
            dialog.Initialize(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            theCraft.Initialize();
            //theRing.Initialize();

            //ringMgr.Initialize(20);
            //gameTime = 0;

            theAmbient.X = level[stage, 2];
            theAmbient.Y = level[stage, 3];
            theAmbient.Z = level[stage, 4];

            ringMgr.Initialize(level[stage,1],theAmbient);
            gameTime = level[stage, 0] * 60;

            if (skys[stage] != null)
            {
                theSky = skys[stage];
            }

            Random rand = new Random();
            int a = rand.Next(0, 4);
            if (bgms[a] != null)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(bgms[a]);
            }
        }

        public override void Create(ContentManager Content)
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            dialog.Create(this.smgr,Content.Load<Texture2D>("px"), Content.Load<Texture2D>("complete"), Content.Load<Texture2D>("fail"), Content.Load<Texture2D>("pause"), Content.Load<Texture2D>("mouse"));

            theCraft.Create(Content.Load<Model>("craft"));
            scene = Content.Load<Model>("hangar");

            Model r = Content.Load<Model>("torus");
            //theRing.Create(r);
            ringMgr.Create(r);

            skys[0] = new SkyBox();
            skys[0].Create(graphics.GraphicsDevice, Content, "miramar");
            skys[1] = new SkyBox();
            skys[1].Create(graphics.GraphicsDevice, Content, "violentdays");
            skys[2] = new SkyBox();
            skys[2].Create(graphics.GraphicsDevice, Content, "grimmnight");
            skys[3] = new SkyBox();
            skys[3].Create(graphics.GraphicsDevice, Content, "interstellar");

            hand = Content.Load<Texture2D>("hand");
            gameUI = Content.Load<Texture2D>("gameUI");
            hpbar_back = Content.Load<Texture2D>("hpbar_back");
            hpbar_in = Content.Load<Texture2D>("hpbar_in");

            //SoundEffectInstance pass = (Content.Load<SoundEffect>("pass")).CreateInstance();
            bgms[0] = Content.Load<Song>("bgm1");
            bgms[1] = Content.Load<Song>("bgm2");
            bgms[2] = Content.Load<Song>("bgm3");
            bgms[3] = Content.Load<Song>("bgm4");
            bgms[4] = Content.Load<Song>("bgm5");


        }

        public override void Draw()
        {
            graphics.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.CornflowerBlue, 1.0f, 0);
            theSky.Draw();

            // scene draw begin

            Matrix[] transforms = new Matrix[scene.Bones.Count];
            scene.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in scene.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.DiffuseColor = new Vector3(1, 1, 1);
                    effect.AmbientLightColor = theAmbient / 1000.0f;

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(Vector3.Zero);
                    effect.Projection = theCamera.getProjMat();
                    effect.View = theCamera.getViewMat();
                }
                mesh.Draw();
            }

            // scene draw end

            //theRing.Draw(theCamera);
            ringMgr.Draw(theCamera);
            theCraft.Draw(theCamera);

            // 2D ui begin
            

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            /*
            spriteBatch.DrawString(spriteFont, "dir x :" + theCraft.getDirection().X + " y : " + theCraft.getDirection().Y + "z : " + theCraft.getDirection().Z, Vector2.Zero, Color.Red);
            theCamera.DrawVectors(spriteBatch, spriteFont, new Vector2(0, 20));

            spriteBatch.DrawString(
                spriteFont,
                "Dist : " + Vector3.Distance(theRing.getPosition(), theCraft.getPosition()) +q
                "     \n" + theRing.getonCollide() +
                "\nd : " + Vector3.Dot(theRing.getPosition() - theCraft.getPosition(), theRing.getDirection()) +
                "     \n" + theRing.getonPass()
                + "\n Cam - Craf dist : " + Vector3.Distance(theCamera.getPositionVector(), theCraft.getPosition())
                , new Vector2(0, 220)
                , Color.Red);
            */

            if (kInterface.getIsAvail())
            {
                Vector3 LHPos = kInterface.getFilteredLHPos();
                Vector3 RHPos = kInterface.getFilteredRHPos();
                Vector3 HDPos = kInterface.getFilteredHDPos();

                /*
                spriteBatch.DrawString(spriteFont, "DEPTH : " + (HDPos.Z - LHPos.Z), new Vector2(0, 500), Color.Red);

                spriteBatch.DrawString(spriteFont, "Roll : " + kInterface.getRollAngle() + " \nPitch : " + kInterface.getPitchAngle() + " \nYaw : " + kInterface.getYawAngle(), new Vector2(0, 560), Color.AliceBlue);
                */
                spriteBatch.Draw(hand, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2.0f - 25 + 300 * LHPos.X, graphics.GraphicsDevice.Viewport.Height / 2.0f - 25 + 300 * -LHPos.Y), Color.White);
                spriteBatch.Draw(hand, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2.0f - 25 + 300 * RHPos.X, graphics.GraphicsDevice.Viewport.Height / 2.0f - 25 + 300 * -RHPos.Y), Color.White);
                spriteBatch.Draw(hand, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2.0f - 25 + 300 * HDPos.X, graphics.GraphicsDevice.Viewport.Height / 2.0f - 25 + 300 * -HDPos.Y), Color.White);
               
            }

            spriteBatch.Draw(this.gameUI, new Vector2(10, 10), Color.White);
            //hp bar
            spriteBatch.Draw(this.hpbar_back, new Vector2(34, 26), Color.White);
            spriteBatch.Draw(this.hpbar_in, new Vector2(34 + 8, 26 + 8), null, Color.White, 0, Vector2.Zero, new Vector2(theCraft.getHP() / 100.0f, 1.0f), SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, "" + ((int)gameTime / 60) + " : " + ((int)gameTime - (int)gameTime / 60 * 60) , new Vector2(35, 90), Color.Black);
            spriteBatch.DrawString(spriteFont, "" + ringMgr.getPassed() + " / " + ringMgr.getCount(), new Vector2(235, 90), Color.Black);
            spriteBatch.DrawString(spriteFont, "" + (int)(theCraft.getDirection().Length() * 100) + " Km/h", new Vector2(35, 120), Color.Black);

            dialog.Draw(spriteBatch);

            spriteBatch.End();
            
            // 2d ui end
        }

        public override void Update(float timeDelta)
        {
            if (dialog.getOnFocus() == true)
            {
                dialog.Update(timeDelta,smgr);
            }
            else
            {
                //dialog.setFocusOnMode(1);

                gameTime -= timeDelta;

                // Kinect
                if(kInterface != null)
                    kInterface.Update(timeDelta);

                // game logic
                //theRing.Update(timeDelta, theCraft);
                ringMgr.Update(timeDelta, theCraft);

                theCraft.Update(timeDelta, kInterface);
                theCamera.setPosition(theCraft);

                // view
                theSky.Update(timeDelta, theCamera);
                theCamera.Update();

                // game over
                if (gameTime < 0 || theCraft.getHP() <= 0)
                {
                    dialog.setFocusOnMode(1);
                }

                // mission complete
                if (ringMgr.getPassed() >= ringMgr.getCount())
                {
                    dialog.setFocusOnMode(0);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                    dialog.setFocusOnMode(2);
                }
            }
        }

        public void setStage(int n) {
            this.stage = n;
        }
    }
}
