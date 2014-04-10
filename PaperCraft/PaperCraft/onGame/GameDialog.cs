using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PaperCraft.scene;

namespace PaperCraft.onGame
{
    class GameDialog
    {
        private KinectInterface kInterface;
        private bool onFocus = false;

        int mode = 0; // 0 - complete // 1 - fail // 2- pause

        float width = 0;
        float height = 0;

        Texture2D complete;
        Texture2D fail;
        Texture2D pause;

        Texture2D mouse;
        Texture2D px;

        MouseState mState = Mouse.GetState();
        Vector2 ms = new Vector2(0, 0);

        private float exitTime = 0;
        private float retryTime = 0;

        public void Initialize(float width,float height) {

            mode = 0;
            exitTime = 0;
            retryTime = 0;
            onFocus = false;

            this.height = height;
            this.width = width;
        }

        public void Create(SceneManager smgr, Texture2D px,Texture2D complete, Texture2D fail, Texture2D pause, Texture2D mouse){
            this.kInterface = smgr.getKinect();
            this.px = px;
            this.complete = complete;
            this.fail = fail;
            this.pause = pause;
            this.mouse = mouse;
        }

        public void Draw(SpriteBatch spr) {

            if (onFocus == false) return;

            spr.Draw(px, Vector2.Zero, null,new Color(1,1,1,0.75f), 0, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0);

            if (mode == 0)
            {
                spr.Draw(complete, new Vector2(this.width / 2 - complete.Width / 2, this.height / 2 - complete.Height / 2), Color.White);
            }
            else if (mode == 1)
            {
                spr.Draw(fail, new Vector2(this.width / 2 - fail.Width / 2, this.height / 2 - fail.Height / 2), Color.White);
            }
            else if (mode == 2)
            {
                spr.Draw(pause, new Vector2(this.width / 2 - pause.Width / 2, this.height / 2 - pause.Height / 2), Color.White);
            }

            spr.Draw(mouse, new Vector2(ms.X, ms.Y), Color.White);
        }

        public void Update(float timeDelta,SceneManager smgr){

            if (kInterface.getIsAvail())
            {
                Vector3 LHPos = 1000 * kInterface.getLeftHandPosition();
                ms = new Vector2(smgr.getGDM().GraphicsDevice.Viewport.Width / 2.0f - 25 + LHPos.X, smgr.getGDM().GraphicsDevice.Viewport.Height / 2.0f - 25 - LHPos.Y);//kInterface.getLeftHandPosition();
            }
            else
            {
                mState = Mouse.GetState();
                ms.X = mState.X;
                ms.Y = mState.Y;
            }

            if (mode == 0)
            {
                Vector2 pos = new Vector2(this.width / 2 - complete.Width / 2, this.height / 2 - complete.Height / 2);

                bool onExit = false;
                if (pos.X + 139 < ms.X && ms.X < pos.X + 139 +131)
                {
                    if (pos.Y + 276 < ms.Y && ms.Y < pos.Y + 276 + 141)
                    {
                        onExit = true;
                    }
                }

                if (onExit)
                {
                    exitTime += timeDelta;
                }
                else
                {
                    exitTime = 0;
                }

                if (exitTime > 1)
                {
                    MediaPlayer.Stop();
                    smgr.ChangeScene("StageSelectScene");
                }
                /////////////////////////////////////////////////
                bool onRetry = false;
                if (pos.X + 276 < ms.X && ms.X < pos.X + 276 + 131)
                {
                    if (pos.Y + 276 < ms.Y && ms.Y < pos.Y + 276 + 141)
                    {
                        onRetry = true;
                    }
                }

                if (onRetry)
                {
                    retryTime += timeDelta;
                }
                else
                {
                    retryTime = 0;
                }

                if (retryTime > 1)
                {
                    MediaPlayer.Stop();
                    smgr.ChangeScene("GameScene");
                }
            }
            else if (mode == 1)
            {
                Vector2 pos = new Vector2(this.width / 2 - fail.Width / 2, this.height / 2 - fail.Height / 2);

                bool onExit = false;
                if (pos.X + 0 < ms.X && ms.X < pos.X + 0 + 131)
                {
                    if (pos.Y + 276 < ms.Y && ms.Y < pos.Y + 276 + 141)
                    {
                        onExit = true;
                    }
                }

                if (onExit)
                {
                    exitTime += timeDelta;
                }
                else
                {
                    exitTime = 0;
                }

                if (exitTime > 1)
                {
                    MediaPlayer.Stop();
                    smgr.ChangeScene("StageSelectScene");
                }
                /////////////////////////////////////////////////
                bool onRetry = false;
                if (pos.X + 138 < ms.X && ms.X < pos.X + 138 + 131)
                {
                    if (pos.Y + 276 < ms.Y && ms.Y < pos.Y + 276 + 141)
                    {
                        onRetry = true;
                    }
                }

                if (onRetry)
                {
                    retryTime += timeDelta;
                }
                else
                {
                    retryTime = 0;
                }

                if (retryTime > 1)
                {
                    MediaPlayer.Stop();
                    smgr.ChangeScene("GameScene");
                }
            }
            else if (mode == 2) {
                if (Keyboard.GetState().IsKeyUp(Keys.Escape)) {
                    onFocus = false;
                }
            }
        }

        public bool getOnFocus() {
            return this.onFocus;
        }

        public void setFocusOnMode(int mode) {
            this.onFocus = true;
            this.mode = mode;
        }
    }
}
