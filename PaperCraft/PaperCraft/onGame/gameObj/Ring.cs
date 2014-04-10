using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using PaperCraft.onGame;
using Microsoft.Xna.Framework.Audio;

namespace PaperCraft
{
    class Ring : GameObject
    {
        private Model ringModel = null;
        private Random r = null;
        private Ring beforeRing = null;

        private Vector3 position;
        private Vector3 direction;
        private float radius = 1;

        private Matrix[] localMatrices = null;
        private Matrix world;

        private Vector3 diffuse;
        private Vector3 ambient;
        private Vector3 theAmbient;

        private bool isAvail = false;

        private bool onCollide = false;
        private bool onPass = false;

        public Ring(Random r,Vector3 theAmbient) { 
            this.r = r;
            this.beforeRing = null;
            this.theAmbient = theAmbient;
        }

        public Ring(Random r, Ring beforeRing) {
            this.r = r;
            this.beforeRing = beforeRing;
        }

        public void Initialize()
        {
            world = Matrix.CreateTranslation(position) * Matrix.CreateScale(1 / 2);

            if (beforeRing == null)
            {
                position = new Vector3(r.Next(-700, 700), r.Next(0, 700), r.Next(-700, 700));

                direction = new Vector3(r.Next(-1000, 1000), r.Next(-1000, 1000), r.Next(-1000, 1000));
                direction.Normalize();

                radius = r.Next(5000, 20000) / 10000.0f; //1.0f / 5.0f;
            }
            else {

                Vector3 beforePos = beforeRing.getPosition();
                Vector3 beforeDir = beforeRing.getDirection();
                float beforeRad = beforeRing.getRadius();

                position = 0.3f * beforePos + 0.7f * (new Vector3(r.Next(-800, 800), r.Next(0, 800), r.Next(-800, 800)));

                direction = 0.9f * beforeDir + 0.1f * (new Vector3(r.Next(-1000, 1000), r.Next(-1000, 1000), r.Next(-1000, 1000)));
                direction.Normalize();

                radius = 0.5f * beforeRad + 0.5f * (r.Next(2000, 10000) / 10000.0f); //1.0f / 5.0f;

            }
            //randomize diffuse
            diffuse = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
            ambient = (theAmbient / 255.0f);//new Vector3(57 / 255.0f, 158 / 255.0f, 90 / 255.0f);//((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
            diffuse = 0.5f * diffuse + (1 - 0.5f) * ambient;

            isAvail = true;

            world = Matrix.CreateScale(radius) * Matrix.CreateWorld(position, direction, Vector3.Up);

        }

        public void Create(Model _ringModel)
        {
            
            this.ringModel = _ringModel;

            this.localMatrices = new Matrix[this.ringModel.Bones.Count];
            this.ringModel.CopyAbsoluteBoneTransformsTo(this.localMatrices);

        }

        public float getRadius() {
            return this.radius;
        }

        public Vector3 getPosition() {
            return this.position;
        }

        public Vector3 getDirection() {
            return this.direction;
        }

        public bool getonCollide() {
            return this.onCollide;
        }
        public bool getonPass()
        {
            if(isAvail)
                return this.onPass;
            return false;
        }

        public void Update(float timeDelta, Craft theCraft) {

            world = Matrix.CreateScale(radius) * Matrix.CreateWorld(position, direction, Vector3.Up);

            onCollide = onPass = false;

            // 1st bounding sphere collision
             if (Vector3.Distance(this.position, theCraft.getPosition()) <= this.radius * 75.0f)
            {

                // onCollide
                BoundingSphere sp1, sp2;
                Model craftModel = theCraft.getCraftModel();
                Vector3 craftPosition = theCraft.getPosition();

                for (int i = 0; i < this.ringModel.Meshes.Count; i++)
                {

                    sp1 = this.ringModel.Meshes[i].BoundingSphere;
                    sp1.Radius = 7.5f;
                    sp1 = sp1.Transform(this.localMatrices[i] * world); 
                    

                    for (int j = 0; j < craftModel.Meshes.Count; j++)
                    {

                        sp2 = craftModel.Meshes[j].BoundingSphere;
                        sp2 = sp2.Transform(theCraft.getMeshWorldMat(j));
                        sp2.Radius /= 2.0f;

                        if (sp1.Intersects(sp2))
                        {
                            onCollide = true;
                            theCraft.onCollide();
                            SoundManager.playSound("collide");
                            return;
                        }
                    }
                }

                if (isAvail == true)
                {
                    // onPass
                    float d = Vector3.Dot(this.position - craftPosition, this.direction);

                    if (Math.Abs(d) <= 4)
                    {
                        onPass = true;
                        isAvail = false;
                        diffuse = ambient = Vector3.Zero;
                        theCraft.giveForce(2.0f);
                        SoundManager.playSound("pass");
                    }
                }

            }

        }

        public void Draw(Camera theCamera) {

            //if (isAvail == false) { return; }

            foreach (ModelMesh mesh in ringModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.AmbientLightColor = ambient;
                    effect.DiffuseColor = diffuse;

                    effect.World = this.localMatrices[mesh.ParentBone.Index] * world;
                    effect.Projection = theCamera.getProjMat();
                    effect.View = theCamera.getViewMat();
                }
                mesh.Draw();

                /*
                BoundingSphere sp1 = mesh.BoundingSphere;
                sp1.Radius = 7.5f;
                sp1 = sp1.Transform(this.localMatrices[mesh.ParentBone.Index] * world);
                BoundingSphereRenderer.Render(sp1, theCamera.getViewMat(), theCamera.getProjMat(), Color.Blue);
                */
            }

        }

        // not in gameScene
        public void Draw(Matrix view,Matrix proj)
        {

            //if (isAvail == false) { return; }

            foreach (ModelMesh mesh in ringModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.AmbientLightColor = ambient;
                    effect.DiffuseColor = diffuse;

                    effect.World = this.localMatrices[mesh.ParentBone.Index] * world;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }

        }

        public bool getisAvail()
        {
            return this.isAvail;
        }
    }
}
