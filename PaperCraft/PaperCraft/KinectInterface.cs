using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace PaperCraft
{
    class KinectInterface
    {
        private KinectSensor kinect = null;
        private bool kinectAvailable = false;

        private Skeleton[] skeletons = new Skeleton[0];

        private SkeletonPoint leftHandPosition;
        private SkeletonPoint rightHandPosition;
        private SkeletonPoint headPosition;

        private Vector3 filteredLHPos = new Vector3(0,0,0);
        private Vector3 filteredRHPos = new Vector3(0,0,0);
        private Vector3 filteredHDPos = new Vector3(0,0,0);

        private int nTracked = 0;

        public KinectInterface() {

            Initialize();
        }

        public void Initialize() {

            if (KinectSensor.KinectSensors.Count != 0) {

                this.kinectAvailable = true;
                this.kinect = KinectSensor.KinectSensors[0];

                //kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                //kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

                kinect.SkeletonStream.Enable();
                kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                kinect.Start();
            }
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e){

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            nTracked = 0;
            foreach (Skeleton skel in skeletons)
            {

                if (skel.TrackingState == SkeletonTrackingState.Tracked)
                {
                    nTracked += 1;
                    this.leftHandPosition = skel.Joints[JointType.HandLeft].Position;
                    this.rightHandPosition = skel.Joints[JointType.HandRight].Position;

                    this.headPosition = skel.Joints[JointType.Head].Position;
                    return;
                }
            }

        }

        public Vector3 getLeftHandPosition()    { return new Vector3(leftHandPosition.X, leftHandPosition.Y, leftHandPosition.Z); }
        public Vector3 getRightHandPosition()   { return new Vector3(rightHandPosition.X, rightHandPosition.Y, rightHandPosition.Z); }
        public Vector3 getHeadPosition()        { return new Vector3(headPosition.X, headPosition.Y, headPosition.Z); }

        public Vector3 getFilteredLHPos() { return this.filteredLHPos; }
        public Vector3 getFilteredRHPos() { return this.filteredRHPos; }
        public Vector3 getFilteredHDPos() { return this.filteredHDPos; }

        public bool getIsAvail() { return this.kinectAvailable; }

        public float getRollAngle() {

            if (this.kinectAvailable == false)
            {
                return 0;
            }

            float dx = rightHandPosition.X - leftHandPosition.X;
            float dy = rightHandPosition.Y - leftHandPosition.Y;

            return (float) -Math.Atan2(dy, dx);
        }

        public float getPitchAngle(){

            if (this.kinectAvailable == false)
            {
                return 0;
            }

            float mid = 0.4f;
            float dz = (rightHandPosition.Z + leftHandPosition.Z) / 2.0f ;
            float p = headPosition.Z - dz;

            return -(p - mid);
        }

        public float getYawAngle() {

            if (this.kinectAvailable == false)
            {
                return 0;
            }

            float mx = (rightHandPosition.X + leftHandPosition.X)/2.0f;
            //float my = (rightHandPosition.Y + leftHandPosition.Y)/2.0f;

            return headPosition.X - mx;
        }

        public void Update(float timeDelta){

            float div = 5.0f;

            filteredLHPos.X += (this.leftHandPosition.X - filteredLHPos.X) / div;
            filteredLHPos.Y += (this.leftHandPosition.Y - filteredLHPos.Y) / div;
            filteredLHPos.Z += (this.leftHandPosition.Z - filteredLHPos.Z) / div;

            filteredRHPos.X += (this.rightHandPosition.X - filteredRHPos.X) / div;
            filteredRHPos.Y += (this.rightHandPosition.Y - filteredRHPos.Y) / div;
            filteredRHPos.Z += (this.rightHandPosition.Z - filteredRHPos.Z) / div;

            filteredHDPos.X += (this.headPosition.X - filteredHDPos.X) / div;
            filteredHDPos.Y += (this.headPosition.Y - filteredHDPos.Y) / div;
            filteredHDPos.Z += (this.headPosition.Z - filteredHDPos.Z) / div;

            if (nTracked == 0)
            {
                this.kinectAvailable = false;
            }
            else
            {
                this.kinectAvailable = true;
            }
        }

        public void Stop() {
            if (this.kinectAvailable)
            {
                kinect.Stop();
            }
        }
    }

}
