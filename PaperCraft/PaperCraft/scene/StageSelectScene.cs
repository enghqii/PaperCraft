using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaperCraft.onGame;

namespace PaperCraft.scene
{
    class StageSelectScene : Scene
    {
        private KinectInterface kInterface;
        SpriteBatch spr;

        Texture2D back = null;
        Texture2D buttons = null;
        Texture2D mouse = null;

        MouseState mState = Mouse.GetState();
        Vector2 ms = new Vector2(0, 0);

        float backTime = 0;

        float st1Time = 0;
        float st2Time = 0;
        float st3Time = 0;
        float st4Time = 0;

        public override void Initialize()
        {
            backTime = 0;

            st1Time = 0;
            st2Time = 0;
            st3Time = 0;
            st4Time = 0;
        }

        public override void Create(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.kInterface = this.smgr.getKinect();
            spr = new SpriteBatch(smgr.getGDM().GraphicsDevice);
            back = content.Load<Texture2D>("bg");
            buttons = content.Load<Texture2D>("select_btns");
            mouse = content.Load<Texture2D>("mouse");
        }

        public override void Draw()
        {
            spr.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spr.Draw(back, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(smgr.getGDM().GraphicsDevice.Viewport.Width / (float)back.Width, smgr.getGDM().GraphicsDevice.Viewport.Height / (float)back.Height), SpriteEffects.None, 0);
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

            bool onBack = false;
            if (591 + 141 < ms.X && ms.X < 591 + 282)
            {
                if (296 + 141 < ms.Y && ms.Y < 296 + 282)
                {
                    onBack = true;
                }
            }

            if (onBack)
            {
                backTime += timeDelta;
            }
            else
            {
                backTime = 0;
            }

            if (backTime > 1)
            {
                smgr.ChangeScene("MenuScene");
            }
            /////////////////////////////////////////////////
            bool onSt1 = false;
            if (296 + 141 < ms.X && ms.X < 296 + 282)
            {
                if (0 + 141 < ms.Y && ms.Y < 0 + 282)
                {
                    onSt1 = true;
                }
            }

            if (onSt1)
            {
                st1Time += timeDelta;
            }
            else
            {
                st1Time = 0;
            }

            if (st1Time > 1)
            {
                ((GameScene)(smgr.getScene("GameScene"))).setStage(0);
                smgr.ChangeScene("GameScene");
            }
            /////////////////////////////////////////////////
            bool onSt2 = false;
            if (296 + 141 < ms.X && ms.X < 296 + 282)
            {
                if (149 + 141 < ms.Y && ms.Y < 149 + 282)
                {
                    onSt2 = true;
                }
            }

            if (onSt2)
            {
                st2Time += timeDelta;
            }
            else
            {
                st2Time = 0;
            }

            if (st2Time > 1)
            {
                ((GameScene)(smgr.getScene("GameScene"))).setStage(1);
                smgr.ChangeScene("GameScene");
            }
            /////////////////////////////////////////////////
            bool onSt3 = false;
            if (445 + 141 < ms.X && ms.X < 445 + 282)
            {
                if (148 + 141 < ms.Y && ms.Y < 148 + 282)
                {
                    onSt3 = true;
                }
            }

            if (onSt3)
            {
                st3Time += timeDelta;
            }
            else
            {
                st3Time = 0;
            }

            if (st3Time > 1)
            {
                ((GameScene)(smgr.getScene("GameScene"))).setStage(2);
                smgr.ChangeScene("GameScene");
            }
            /////////////////////////////////////////////////
            bool onSt4 = false;
            if (444 + 141 < ms.X && ms.X < 444 + 282)
            {
                if (296 + 141 < ms.Y && ms.Y < 296 + 282)
                {
                    onSt4 = true;
                }
            }

            if (onSt4)
            {
                st4Time += timeDelta;
            }
            else {
                st4Time = 0;
            }

            if (st4Time > 1)
            {
                ((GameScene)(smgr.getScene("GameScene"))).setStage(3);
                smgr.ChangeScene("GameScene");
            }
            /////////////////////////////////////////////////

        }
    }
}
