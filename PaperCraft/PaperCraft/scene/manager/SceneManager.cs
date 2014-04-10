using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PaperCraft.scene
{
    class SceneManager
    {

        private KinectInterface kInterface;

        private GraphicsDeviceManager graphics;
        private Dictionary<string, Scene> scenePool;
        private Scene curScene;

        public SceneManager(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            scenePool = new Dictionary<string,Scene>();
            curScene = null;
        }

        public GraphicsDeviceManager getGDM() {
            return this.graphics;
        }

        public void AddScene(string index, Scene scene) {
            scene.setSceneManager(this);
            scenePool.Add(index, scene);
        }

        public Scene getScene(string index) {
            
            if (scenePool.ContainsKey(index))
            {
                return scenePool[index];
            }
            return null;
        }

        public void Initialize()
        {

            foreach (KeyValuePair<string, Scene> kvp in scenePool) 
            {
                kvp.Value.Initialize();
            }
        }

        public void Stop() {
            if (this.kInterface != null) {
                this.kInterface.Stop();
            }
        }

        public void Create(ContentManager content)
        {

            kInterface = new KinectInterface();

            foreach (KeyValuePair<string, Scene> kvp in scenePool)
            {
                kvp.Value.Create(content);
            }
        }

        public void ChangeScene(string index) {

            if (scenePool.ContainsKey(index)) {

                curScene = scenePool[index];
                curScene.Initialize();
            }
        }

        public void Update(float timeDelta) {

            if (curScene != null)
            {
                curScene.Update(timeDelta);
            }
        }

        public void Draw() {
            
            if (curScene != null)
            {
                curScene.Draw();
            }
        }

        public KinectInterface getKinect() {
            return this.kInterface;
        }
    }
}
