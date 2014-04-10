using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaperCraft.onGame;

namespace PaperCraft
{
    class Craft : GameObject
    {
        // model
        private Model craftModel;

        // matrices
        private Matrix world;
        private Matrix Translation;
        private Matrix Rotation;

        private Matrix[] transforms;

        // rotational factor
        private float roll;
        private float pitch;
        private float yaw;

        // positional vector
        private Vector3 position;
        private Vector3 direction;
        private Vector3 accel;

        private Vector3 diffuse = new Vector3(1, 1, 1);

        private float extForce;

        private float HP = 100;

        private bool unCollidable = false;
        private float unCollidableTime = 0;
        private const float UNCOLLIDABLE_TIME = 1;

        public Craft() {

        }

        public void Initialize()
        {
            world = Matrix.Identity;
            Translation = Matrix.Identity;
            Rotation = Matrix.Identity;

            position = new Vector3(0, 10, 10);
            direction = new Vector3(0, 0, -1);
            accel = Vector3.Zero;

            extForce = 0;

            roll = 0;
            pitch = 0;
            yaw = 0;

            HP = 100;
        }

        public void Create(Model _model) {

            craftModel = _model;
            
            // possible cause is has no animation
            transforms = new Matrix[craftModel.Bones.Count];
            this.craftModel.CopyAbsoluteBoneTransformsTo(transforms);
        }

        public void Update(float timeDelta,KinectInterface kIntrfc) {

            HP += 0.05f;
            if (HP > 100) { HP = 100; }

            if (unCollidable == true) {
                unCollidableTime += timeDelta;
                if (unCollidableTime > UNCOLLIDABLE_TIME) {
                    unCollidable = false;
                    unCollidableTime = 0;
                }
            }

            const float mov = 0.003f;
            direction /= 5;

            float kRoll = kIntrfc.getRollAngle();
            float kYaw = kIntrfc.getYawAngle();
            float kPitch = kIntrfc.getPitchAngle();
            
            this.roll += kRoll/500.0f;
            this.yaw += kYaw / 500.0f;
            this.pitch += kPitch / 250.0f;

            if(Keyboard.GetState().IsKeyDown(Keys.Up)){
                pitch -= mov;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Down)){
                pitch += mov;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                roll -= mov;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                roll += mov;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                yaw += mov;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                yaw -= mov;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //extForce *= 1.5f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //extForce /= 1.5f;
            }

            // Rotational begin
            Rotation *= Matrix.CreateFromAxisAngle(Rotation.Up, yaw);
            Rotation *= Matrix.CreateFromAxisAngle(Rotation.Right, pitch);
            Rotation *= Matrix.CreateFromAxisAngle(Rotation.Forward, roll);

            yaw *= 0.9f;
            pitch *= 0.9f;
            roll *= 0.9f;
            // Rotational end

            // movement begin
            accel = Rotation.Forward;
            accel.Normalize();
            direction += (accel * (1 + extForce));


            if (!Keyboard.GetState().IsKeyDown(Keys.Space)) {
                position += direction;
            }

            direction *= 0.98f;
            extForce *= 0.995f;
            // movement end
            
            // traslation
            Translation = Matrix.CreateTranslation(position);

            world = Rotation * Translation;
        }

        public void Draw(Camera theCamera) {

            foreach (ModelMesh mesh in craftModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.DiffuseColor = diffuse;

                    effect.World = transforms[mesh.ParentBone.Index] * this.world;
                    effect.Projection = theCamera.getProjMat();
                    effect.View = theCamera.getViewMat();
                }

                mesh.Draw(); 
                
                /*
                BoundingSphere sp1 = mesh.BoundingSphere;
                //sp1.Radius = 4;
                sp1 = sp1.Transform(this.transforms[mesh.ParentBone.Index] * world);
                sp1.Radius /= 2;
                BoundingSphereRenderer.Render(sp1, theCamera.getViewMat(), theCamera.getProjMat(), Color.Blue);
                */
            }
        }

        public void giveForce(float force) {
            this.extForce += force;
        }

        public void onCollide() {

            if (unCollidable == false)
            {
                extForce = -(this.direction.Length()/4 < 1.5f ? 1.5f : this.direction.Length()/4);// -0.1f;
                this.HP -= 15 * Math.Abs(direction.Length());
                //this.direction = Vector3.Zero;
                this.direction = -this.direction;
                unCollidable = true;
            }
        }

        public float getHP() {
            return this.HP;
        }

        public float getExtForce() {
            return this.extForce;
        }
        
        public Matrix getWorldMat() {
            return world;
        }

        public Matrix getMeshWorldMat(int index) {

            //index += 1;

            if (transforms.Length <= index)
            {
                return Matrix.Identity;
            }
            else
            {
                return this.transforms[index] * this.world;
            }
        }

        public Vector3 getPosition() {
            return this.position;
        }

        public Vector3 getDirection() {
            return this.direction;
            //return this.Rotation.Forward;
        }

        public Vector3 getAccel() {
            return this.accel;
        }

        public Vector3 getUp() {
            return this.Rotation.Up;
        }

        public Model getCraftModel() {
            return this.craftModel;    
        }
    }
}
