using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaperCraft.scene
{
    class SplashScene : Scene
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spr;

        SoundEffectInstance song = null;
        Texture2D team = null;
        Texture2D logo = null;
        Texture2D px = null;

        float gameTime =0;

        float width =0;
        float height =0;

        public override void Initialize()
        {
            this.graphics = this.smgr.getGDM();

            width = graphics.GraphicsDevice.Viewport.Width;
            height = graphics.GraphicsDevice.Viewport.Height;

            if (song != null) {
                //song.Play();
            }
        }

        public override void Create(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            spr = new SpriteBatch(graphics.GraphicsDevice);

            song = (content.Load<SoundEffect>("splash")).CreateInstance();
            logo = content.Load<Texture2D>("xnalogo");
            team = content.Load<Texture2D>("t4");
            px = content.Load<Texture2D>("px");
            this.Initialize();
        }

        public override void Draw()
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            spr.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
            spr.Draw(px, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(2000, 2000), 0, 0);
            spr.Draw(team, new Vector2(width / 2.0f - team.Width / 2.0f, height/2.0f - team.Height/2.0f), new Color(1.0f, 1.0f, 1.0f, (float)Math.Sin(gameTime * Math.PI / 2 ) ));
            spr.Draw(logo,new Vector2(width/2.0f - logo.Width/2.0f, height/2.0f - logo.Height/2.0f),new Color(1.0f, 1.0f, 1.0f, -(float)Math.Sin(gameTime * Math.PI / 2 )));

            spr.End();

        }

        public override void Update(float timeDelta)
        {
            gameTime += timeDelta;

            if (song.State == SoundState.Stopped)
            {
                this.smgr.ChangeScene("MenuScene");
            }
        }
    }
}
