using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace PaperCraft
{
    class SkyBox
    {
        private GraphicsDevice device;
        private VertexBuffer vertices;
        private IndexBuffer indices;

        private TextureCube skyTex;
        private Effect skyEffect;
        private Matrix WVP;

        private const int number_of_vertices = 8;
        private const int number_of_indices = 36;

        public SkyBox() { 
        
        }

        public void Create(GraphicsDevice _device, ContentManager Content, string texName ="interstellar")
        {
            this.device = _device;
            skyEffect   = Content.Load<Effect>("SkyEffect").Clone();
            skyTex = Content.Load<TextureCube>(texName);
            skyEffect.Parameters["tex"].SetValue(skyTex);

            this.CreateCubeVertexBuffer();
            this.CreateCubeIndexBuffer();
        }

        void CreateCubeVertexBuffer()
        {
            Vector3[] cubeVertices = new Vector3[number_of_vertices];

            cubeVertices[0] = new Vector3(-1, -1, -1);
            cubeVertices[1] = new Vector3(-1, -1, 1);
            cubeVertices[2] = new Vector3(1, -1, 1);
            cubeVertices[3] = new Vector3(1, -1, -1);
            cubeVertices[4] = new Vector3(-1, 1, -1);
            cubeVertices[5] = new Vector3(-1, 1, 1);
            cubeVertices[6] = new Vector3(1, 1, 1);
            cubeVertices[7] = new Vector3(1, 1, -1);

            VertexDeclaration VertexPositionDeclaration = new VertexDeclaration
                (
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)
                );

            vertices = new VertexBuffer(device, VertexPositionDeclaration, number_of_vertices, BufferUsage.WriteOnly);
            vertices.SetData<Vector3>(cubeVertices);
        }

        void CreateCubeIndexBuffer()
        {
            UInt16[] cubeIndices = new UInt16[number_of_indices];

            //bottom face
            cubeIndices[0] = 0;
            cubeIndices[1] = 2;
            cubeIndices[2] = 3;
            cubeIndices[3] = 0;
            cubeIndices[4] = 1;
            cubeIndices[5] = 2;

            //top face
            cubeIndices[6] = 4;
            cubeIndices[7] = 6;
            cubeIndices[8] = 5;
            cubeIndices[9] = 4;
            cubeIndices[10] = 7;
            cubeIndices[11] = 6;

            //front face
            cubeIndices[12] = 5;
            cubeIndices[13] = 2;
            cubeIndices[14] = 1;
            cubeIndices[15] = 5;
            cubeIndices[16] = 6;
            cubeIndices[17] = 2;

            //back face
            cubeIndices[18] = 0;
            cubeIndices[19] = 7;
            cubeIndices[20] = 4;
            cubeIndices[21] = 0;
            cubeIndices[22] = 3;
            cubeIndices[23] = 7;

            //left face
            cubeIndices[24] = 0;
            cubeIndices[25] = 4;
            cubeIndices[26] = 1;
            cubeIndices[27] = 1;
            cubeIndices[28] = 4;
            cubeIndices[29] = 5;

            //right face
            cubeIndices[30] = 2;
            cubeIndices[31] = 6;
            cubeIndices[32] = 3;
            cubeIndices[33] = 3;
            cubeIndices[34] = 6;
            cubeIndices[35] = 7;

            indices = new IndexBuffer(device, IndexElementSize.SixteenBits, number_of_indices, BufferUsage.WriteOnly);
            indices.SetData<UInt16>(cubeIndices);

        }

        public void Update(float timeDelta,Camera theCamera) {

            WVP = theCamera.getWorldMat() * theCamera.getViewMat() * theCamera.getProjMat();
        }
        
        public void Update(float timeDelta,Matrix world, Matrix view, Matrix projection)
        {
            WVP = world * view * projection;
        }

        public void Draw() {

            device.SetVertexBuffer(vertices);
            device.Indices = indices;

            skyEffect.Parameters["WVP"].SetValue(WVP);
            skyEffect.CurrentTechnique.Passes[0].Apply();

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, number_of_vertices, 0, number_of_indices / 3);
        }
    }
}

// ref - http://iloveshaders.blogspot.kr/2011/05/creating-sky-box.html