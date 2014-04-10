using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaperCraft.onGame;

namespace PaperCraft.scene
{
    class MenuScene : Scene
    {
        private KinectInterface kInterface;

        SpriteBatch spr;

        Texture2D back = null;
        Texture2D buttons = null;
        Texture2D mouse = null;

        MouseState mState = Mouse.GetState();
        Vector2 ms = new Vector2(0,0);

        float exitTime = 0;
        float playTime = 0;

        public MenuScene() { 
        }

        public override void Initialize()
        {

            exitTime = 0;
            playTime = 0;
        }

        public override void Create(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.kInterface = this.smgr.getKinect();

            spr = new SpriteBatch(smgr.getGDM().GraphicsDevice);
            back = content.Load<Texture2D>("bg");
            buttons = content.Load<Texture2D>("menu_btns");
            mouse = content.Load<Texture2D>("mouse");
            //Initialize();
        }

        public override void Draw()
        {
            spr.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spr.Draw(back, Vector2.Zero, null, Color.White, 0, Vector2.Zero,new Vector2( smgr.getGDM().GraphicsDevice.Viewport.Width / (float)back.Width ,smgr.getGDM(). GraphicsDevice.Viewport.Height / (float)back.Height), SpriteEffects.None, 0);
            spr.Draw(buttons, new Vector2(141, 141), Color.White);
            spr.Draw(mouse, new Vector2(ms.X, ms.Y), Color.White);
            spr.End();
        }

        public override void Update(float timeDelta)
        {

            if (kInterface.getIsAvail())
            {
                Vector3 LHPos = 1000 * kInterface.getLeftHandPosition();
                ms = new Vector2(this.smgr.getGDM().GraphicsDevice.Viewport.Width / 2.0f - 25 + LHPos.X, this.smgr.getGDM().GraphicsDevice.Viewport.Height / 2.0f - 25 - LHPos.Y);//kInterface.getLeftHandPosition();
            }
            else
            {
                mState = Mouse.GetState();
                ms.X = mState.X;
                ms.Y = mState.Y;
            }

            bool onExit = false;
            if(591 + 141 < ms.X && ms.X < 591 + 282){
                if (446 + 141 < ms.Y && ms.Y < 446 + 282)
                {
                    onExit = true;
                }
            }

            if (onExit)
            {
                exitTime += timeDelta;
            }
            else {
                exitTime = 0;
            }

            if (exitTime > 1) {
                Environment.Exit(0);
            }

            bool onPlay = false;
            if (141 < ms.X && ms.X < 288 + 141)
            {
                if (297 + 141 < ms.Y && ms.Y < 297 + 282)
                {
                    onPlay = true;
                }
            }

            if (onPlay)
            {
                playTime += timeDelta;
            }
            else {
                playTime = 0;
            }

            if (playTime > 1)
            {
                smgr.ChangeScene("StageSelectScene");
            }
        }
    }
}
