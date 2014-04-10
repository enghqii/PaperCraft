using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PaperCraft.scene
{
    abstract class Scene
    {
        protected SceneManager smgr;

        abstract public void Initialize();
        abstract public void Create(ContentManager content);

        abstract public void Draw();
        abstract public void Update(float timeDelta);

        public void setSceneManager(SceneManager smgr)
        {
            this.smgr = smgr;
        }
    }
}
