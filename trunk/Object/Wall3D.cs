using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FPSGame.Core;
using FPSGame.Engine;
using FPSGame.Factory;

namespace FPSGame.Object
{
    public class Wall3D : IDisplayObject3D, Collidable, IBoxShaped
    {
        private Vector3 pos;
        private Vector3 up;
        private Vector3 normal;
        private bool dead;
        public float height {get; protected set;}
        public float width { get; protected set; }
        private int[] Indexes;
        private BasicEffect quadEffect;
        private VertexDeclaration quadVertexDecl;
        private Texture2D texture;
        private Brick brick;

        public Brick GetBrick()
        {
            return brick;
        }

        public void SetBrick(Brick brick)
        {
            this.brick = brick;
        }

        private VertexPositionNormalTexture[] Vertices;

        public float? CollideWith(Ray ray)
        {
            return RayCollisionDetector.RayIntersectsObject(ray, GetBoundingBox());
        }

        public Texture2D GetTexture()
        {
            return texture;
        }
        
        public Wall3D()
        {
        }

        public Wall3D(Vector3 pos, Vector3 normal, Vector3 up, float width, float height, Texture2D texture)
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new int[6];
            dead = false;
            this.texture = texture;
            Initialize(pos, normal, up, width, height);

            quadEffect = new BasicEffect(FPSGame.GetInstance().GraphicsDevice, null);
            quadEffect.EnableDefaultLighting();
            quadEffect.World = Matrix.Identity;

            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
            quadVertexDecl = new VertexDeclaration(FPSGame.GetInstance().GraphicsDevice,
                VertexPositionNormalTexture.VertexElements);
        }

        private void Initialize(Vector3 pos, Vector3 normal, Vector3 up, float width, float height)
        {
            this.pos = pos;
            //System.Windows.Forms.MessageBox.Show(pos.X + "/" + pos.Y + "/" + pos.Z); 
            this.normal = normal;
            this.up = up;
            this.width = width;
            this.height = height;
            Vector3 Left = Vector3.Cross(this.normal, this.up);   //left vector
            Vector3 uppercenter = (this.up * this.height / 2) + this.pos;
            Vector3 UpperLeft = uppercenter + (Left * this.width / 2);
            Vector3 UpperRight = uppercenter - (Left * this.width / 2);
            Vector3 LowerLeft = UpperLeft - (this.up * this.height);
            Vector3 LowerRight = UpperRight - (this.up * this.height);

            FillVertices(UpperLeft, UpperRight, LowerLeft, LowerRight);
            // Set the index buffer for each vertex, using
            // clockwise winding
            Indexes[0] = 0;
            Indexes[1] = 1;
            Indexes[2] = 2;
            Indexes[3] = 2;
            Indexes[4] = 1;
            Indexes[5] = 3;
        }

        public virtual void Begin()
        {
            ObjectManager.GetInstance().RegisterObject(this);
        }

        private void FillVertices(Vector3 topleft, Vector3 topright, Vector3 bottomleft, Vector3 bottomright)
        {
            // Fill in texture coordinates to display full texture
            // on quad
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = bottomleft;
            Vertices[0].TextureCoordinate = textureLowerLeft;
            Vertices[1].Position = topleft;
            Vertices[1].TextureCoordinate = textureUpperLeft;
            Vertices[2].Position = bottomright;
            Vertices[2].TextureCoordinate = textureLowerRight;
            Vertices[3].Position = topright;
            Vertices[3].TextureCoordinate = textureUpperRight;
        }

        public Vector3 GetPosition()
        {
            return pos;
        }

        public void SetPosition(Vector3 pos)
        {
            this.pos = pos;
            Initialize(pos, normal, up, width, height);
        }

        public Vector3 GetRotation()
        {
            return Vector3.Zero;
        }

        public void SetRotation(Vector3 pos)
        {
            
        }

        public Matrix GetWorld()
        {
            return Matrix.Identity;
        }

        public void SetWorld(Matrix world)
        {
        }

        public virtual void Draw3D(GameTime gameTime)
        {
            FPSGame.GetInstance().GraphicsDevice.VertexDeclaration = quadVertexDecl;
            quadEffect.Begin();
            //System.Windows.Forms.MessageBox.Show("Drawing at " + pos.X + "/" + pos.Y + "/" + pos.Z);
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                FPSGame.GetInstance().GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    this.Vertices, 0, 4,
                    this.Indexes, 0, 2);

                pass.End();
            }
            quadEffect.End();
        }

        public void Draw(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
            normal.Normalize();
            up.Normalize();
            ICamera camera = FPSGame.GetInstance().GetFPSCamera();
            quadEffect.View = camera.GetView();
            quadEffect.Projection = camera.GetProjection();
        }

        public bool IsDead()
        {
            return dead;
        }

        public void End()
        {
            dead = true;
            ObjectManager.GetInstance().UnregisterObject(this);
        }

        public BoundingBox GetBoundingBox()
        {
            return MathUtils.GetBoundingBox(pos, normal, up, width, 1, height);
        }

        public virtual bool CollideWith(IBoxShaped obj)
        {
            return GetBoundingBox().Intersects(obj.GetBoundingBox());
        }

        public virtual bool CollideWith(ISphereShaped obj)
        {
            return GetBoundingBox().Intersects(obj.GetBoundingSphere());
        }
    }
}
