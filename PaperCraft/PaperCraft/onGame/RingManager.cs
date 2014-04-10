using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PaperCraft.onGame
{
    class RingManager
    {
        private Model ringModel = null;
        private LinkedList<Ring> ringList;
        private Random rand = new Random();

        private Vector3 theAmbient = Vector3.Zero;
        private int count = 0;
        private int passed = 0;

        public RingManager()
        {
            ringList = new LinkedList<Ring>();
        }

        public void Initialize(int count,Vector3 theAmbient)
        {
            this.count = count;
            this.theAmbient = theAmbient;

            if (this.ringModel != null)
            {
                ringList.Clear();

                for (int i = 0; i < count; i++)
                {
                    Ring ring = new Ring(rand,theAmbient);
                    ring.Create(ringModel);
                    ring.Initialize();
                    ringList.AddLast(ring);
                }

            }
        }

        public void Create(Model ringModel){

            this.ringModel = ringModel;

            this.Initialize(this.count,this.theAmbient);
        }

        public void Update(float timeDelta, Craft theCraft)
        {

            this.passed = 0;

            foreach (Ring ring in ringList) {
                ring.Update(timeDelta,theCraft);
                
                if (ring.getisAvail() == false) {
                    this.passed += 1;
                }
            }

        }

        public void Draw(Camera theCamera) {
            foreach (Ring ring in ringList) {
                ring.Draw(theCamera);
            }
        }

        public void Draw(Matrix view, Matrix proj)
        {
            foreach (Ring ring in ringList)
            {
                ring.Draw(view,proj);
            }
        }

        public int getCount() {
            return this.count;
        }

        public int getPassed() {
            return this.passed;
        }
    }
}
