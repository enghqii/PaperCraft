using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaperCraft
{
    class Camera
    {
        private Vector3 position = new Vector3(0, 0, 50);
        private Vector3 lookAt = new Vector3(0, 0, -1);
        private Vector3 up = new Vector3(0, 1, 0);

        private Matrix projMat;
        private Matrix ViewMat;
        private Matrix worldMat;

        private float aspectRatio;

        private BoundingFrustum viewFrustum;

        public Camera(float _aspectRatio) {

            this.aspectRatio = _aspectRatio;
            UpdateMatrices();

            viewFrustum = new BoundingFrustum(this.ViewMat * this.projMat);
        }

        public void Update() {

            UpdateMatrices();
            viewFrustum.Matrix = this.ViewMat * this.projMat;
        }

        public void UpdateMatrices() {

            projMat = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f);
            ViewMat = Matrix.CreateLookAt(this.position, this.lookAt, this.up);
            worldMat = Matrix.CreateTranslation(position);
        }

        public void setPosition(Craft theCraft) {

            Vector3 targetPosition = theCraft.getPosition();
            Vector3 targetLookAt = Vector3.Normalize(theCraft.getAccel());
            Vector3 targetUp = Vector3.Normalize(theCraft.getUp());
            Matrix targetWorld = theCraft.getWorldMat();

            this.lookAt = Vector3.Transform(targetLookAt,targetWorld);
            this.position = (targetPosition - (targetLookAt * 50)) + 10 * targetUp;
            this.up = targetUp;
        }

        public Matrix getProjMat() {
            return this.projMat;
        }

        public Matrix getViewMat() {
            return this.ViewMat;
        }

        public Matrix getWorldMat() {
            return this.worldMat;
        }

        public BoundingFrustum getViewFrustum() {
            return viewFrustum;
        }

        public Vector3 getLookAt() {
            return this.lookAt;
        }

        public Vector3 getUpVector() {
            return this.up;
        }

        public Vector3 getRightVector() {
            return Vector3.Cross(this.lookAt, this.up);
            //return new Vector3(ViewMat.M11,ViewMat.M12,ViewMat.M13);
            return ViewMat.Right;
        }

        public Vector3 getPositionVector() {
            return this.position;
        }

        internal void DrawVectors(SpriteBatch spr, Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, Vector2 vector2)
        {
            spr.DrawString(spriteFont, "LOOK AT  x;" + this.lookAt.X + " y ; " + this.lookAt.Y + " z ; " + this.lookAt.Z, new Vector2(0, 0) + vector2, Color.Purple);
            spr.DrawString(spriteFont, "POSITION x;" + this.position.X + " y ; " + this.position.Y + " z ; " + this.position.Z, new Vector2(0, 20) + vector2, Color.Purple);
            spr.DrawString(spriteFont, "UP x;" + this.up.X + " y ; " + this.up.Y + " z ; " + this.up.Z, new Vector2(0, 40) + vector2, Color.White);
            spr.DrawString(spriteFont, "RIGHT x;" + ViewMat.M11 + " y ; " + ViewMat.M12 + " z ; " + ViewMat.M13, new Vector2(0, 60) + vector2, Color.White);
        }
    }
}
