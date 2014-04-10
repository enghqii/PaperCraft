using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PaperCraft.onGame
{
    interface GameObject
    {
        void Initialize(); // init vars
        void Create(Model _model); // create resources

        //void Update(float timeDelta); // 
        void Draw(Camera theCamera);
    }
}
