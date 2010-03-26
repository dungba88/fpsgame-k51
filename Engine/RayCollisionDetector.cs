using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FPSGame.Engine
{
    public class RayCollisionDetector
    {
        /// <summary>
        /// Checks whether a ray intersects a model. This method needs to access
        /// the model vertex data, so the model must have been built using the
        /// custom TrianglePickingProcessor provided as part of this sample.
        /// Returns the distance along the ray to the point of intersection, or null
        /// if there is no intersection.
        /// </summary>
        public static float? RayIntersectsModel(Ray ray, Model model, Matrix modelTransform,
                                         out bool insideBoundingSphere,
                                         out Vector3 vertex1, out Vector3 vertex2,
                                         out Vector3 vertex3)
        {
            vertex1 = vertex2 = vertex3 = Vector3.Zero;

            // The input ray is in world space, but our model data is stored in object
            // space. We would normally have to transform all the model data by the
            // modelTransform matrix, moving it into world space before we test it
            // against the ray. That transform can be slow if there are a lot of
            // triangles in the model, however, so instead we do the opposite.
            // Transforming our ray by the inverse modelTransform moves it into object
            // space, where we can test it directly against our model data. Since there
            // is only one ray but typically many triangles, doing things this way
            // around can be much faster.

            Matrix inverseTransform = Matrix.Invert(modelTransform);

            ray.Position = Vector3.Transform(ray.Position, inverseTransform);
            ray.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

            // Look up our custom collision data from the Tag property of the model.
            Dictionary<string, object> tagData = (Dictionary<string, object>)model.Tag;

            //Start off with a fast bounding sphere test.
            BoundingSphere boundingSphere = (BoundingSphere)tagData["BoundingSphere"];

            if (boundingSphere.Intersects(ray) == null)
            {
                // If the ray does not intersect the bounding sphere, we cannot
                // possibly have picked this model, so there is no need to even
                // bother looking at the individual triangle data.
                insideBoundingSphere = false;

                return null;
            }
            else
            {
                // The bounding sphere test passed, so we need to do a full
                // triangle picking test.
                insideBoundingSphere = true;

                // Keep track of the closest triangle we found so far,
                // so we can always return the closest one.
                float? closestIntersection = null;

                // Loop over the vertex data, 3 at a time (3 vertices = 1 triangle).
                Vector3[] vertices = (Vector3[])tagData["Vertices"];

                for (int i = 0; i < vertices.Length; i += 3)
                {
                    // Perform a ray to triangle intersection test.
                    float? intersection;

                    RayIntersectsTriangle(ref ray,
                                          ref vertices[i],
                                          ref vertices[i + 1],
                                          ref vertices[i + 2],
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;

                            // Transform the three vertex positions into world space,
                            // and store them into the output vertex parameters.
                            Vector3.Transform(ref vertices[i],
                                              ref modelTransform, out vertex1);

                            Vector3.Transform(ref vertices[i + 1],
                                              ref modelTransform, out vertex2);

                            Vector3.Transform(ref vertices[i + 2],
                                              ref modelTransform, out vertex3);
                        }
                    }
                }

                return closestIntersection;
            }
        }

        /// <summary>
        /// Checks whether a ray intersects a triangle. This uses the algorithm
        /// developed by Tomas Moller and Ben Trumbore, which was published in the
        /// Journal of Graphics Tools, volume 2, "Fast, Minimum Storage Ray-Triangle
        /// Intersection".
        /// 
        /// This method is implemented using the pass-by-reference versions of the
        /// XNA math functions. Using these overloads is generally not recommended,
        /// because they make the code less readable than the normal pass-by-value
        /// versions. This method can be called very frequently in a tight inner loop,
        /// however, so in this particular case the performance benefits from passing
        /// everything by reference outweigh the loss of readability.
        /// </summary>
        public static void RayIntersectsTriangle(ref Ray ray,
                                          ref Vector3 vertex1,
                                          ref Vector3 vertex2,
                                          ref Vector3 vertex3, out float? result)
        {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
            {
                result = null;
                return;
            }

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float triangleU;
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
            triangleU *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleU < 0 || triangleU > 1)
            {
                result = null;
                return;
            }

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float triangleV;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
            triangleV *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleV < 0 || triangleU + triangleV > 1)
            {
                result = null;
                return;
            }

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            if (rayDistance < 0)
            {
                result = null;
                return;
            }

            result = rayDistance;
        }

        // CalculateCursorRay Calculates a world space ray starting at the camera's
        // "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
        // to accomplish this. see the accompanying documentation for more explanation
        // of the math behind this function.
        public static Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            MouseState ms = Mouse.GetState();
            Vector2 Position = new Vector2(ms.X, ms.Y);
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(Position, 0f);
            Vector3 farSource = new Vector3(Position, 1);
            GraphicsDevice graphDev = FPSGame.GetInstance().GraphicsDevice;

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = graphDev.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = graphDev.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        public static float? RayIntersectsModel(Ray ray, BoundingBox box, Matrix modelTransform,
                                         out bool insideBoundingSphere)
        {
            Matrix inverseTransform = Matrix.Invert(modelTransform);

            ray.Position = Vector3.Transform(ray.Position, inverseTransform);
            ray.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

            float? intersect =  box.Intersects(ray);
            if (intersect == null)
                insideBoundingSphere = false;
            else
                insideBoundingSphere = true;
            return intersect;
        }

                /// <summary>
        /// This helper function checks to see if a ray will intersect with a model.
        /// The model's bounding spheres are used, and the model is transformed using
        /// the matrix specified in the worldTransform argument.
        /// </summary>
        /// <param name="ray">the ray to perform the intersection check with</param>
        /// <param name="model">the model to perform the intersection check with.
        /// the model's bounding spheres will be used.</param>
        /// <param name="worldTransform">a matrix that positions the model
        /// in world space</param>
        /// <param name="absoluteBoneTransforms">this array of matrices contains the
        /// absolute bone transforms for the model. this can be obtained using the
        /// Model.CopyAbsoluteBoneTransformsTo function.</param>
        /// <returns>the intersection between the ray and model.</returns>
        public static float RayIntersectsModel(Ray ray, Model model,
            Matrix worldTransform, Matrix[] absoluteBoneTransforms)
        {
            float closetIntersection = float.MaxValue;
            // Each ModelMesh in a Model has a bounding sphere, so to check for an
            // intersection in the Model, we have to check every mesh.
            foreach (ModelMesh mesh in model.Meshes)
            {
                // the mesh's BoundingSphere is stored relative to the mesh itself.
                // (Mesh space). We want to get this BoundingSphere in terms of world
                // coordinates. To do this, we calculate a matrix that will transform
                // from coordinates from mesh space into world space....
                Matrix world =
                    absoluteBoneTransforms[mesh.ParentBone.Index] * worldTransform;

                // ... and then transform the BoundingSphere using that matrix.
                BoundingSphere sphere = 
                    TransformBoundingSphere(mesh.BoundingSphere, world);

                // now that the we have a sphere in world coordinates, we can just use
                // the BoundingSphere class's Intersects function. Intersects returns a
                // nullable float (float?). This value is the distance at which the ray
                // intersects the BoundingSphere, or null if there is no intersection.
                // so, if the value is not null, we have a collision.
                float? f = sphere.Intersects(ray);
                if (f != null)
                {
                    if (closetIntersection > f.Value)
                        closetIntersection = f.Value;
                }
            }

            return closetIntersection;
        }

        public static float RayIntersectsObject(Ray ray, BoundingSphere boundingSphere)
        {
            float closetIntersection = float.MaxValue;
            float? intersection = boundingSphere.Intersects(ray);
            if (intersection != null)
                return intersection.Value;
            return closetIntersection;
        }

        public static float RayIntersectsObject(Ray ray, BoundingBox boundingBox)
        {
            float closetIntersection = float.MaxValue;
            float? intersection = boundingBox.Intersects(ray);
            if (intersection != null)
                return intersection.Value;
            return closetIntersection;
        }

        /// <summary>
        /// This helper function takes a BoundingSphere and a transform matrix, and
        /// returns a transformed version of that BoundingSphere.
        /// </summary>
        /// <param name="sphere">the BoundingSphere to transform</param>
        /// <param name="world">how to transform the BoundingSphere.</param>
        /// <returns>the transformed BoundingSphere/</returns>
        public static BoundingSphere TransformBoundingSphere(BoundingSphere sphere,
            Matrix transform)
        {
            BoundingSphere transformedSphere;

            // the transform can contain different scales on the x, y, and z components.
            // this has the effect of stretching and squishing our bounding sphere along
            // different axes. Obviously, this is no good: a bounding sphere has to be a
            // SPHERE. so, the transformed sphere's radius must be the maximum of the 
            // scaled x, y, and z radii.

            // to calculate how the transform matrix will affect the x, y, and z
            // components of the sphere, we'll create a vector3 with x y and z equal
            // to the sphere's radius...
            Vector3 scale3 = new Vector3(sphere.Radius, sphere.Radius, sphere.Radius);

            // then transform that vector using the transform matrix. we use
            // TransformNormal because we don't want to take translation into account.
            scale3 = Vector3.TransformNormal(scale3, transform);

            // scale3 contains the x, y, and z radii of a squished and stretched sphere.
            // we'll set the finished sphere's radius to the maximum of the x y and z
            // radii, creating a sphere that is large enough to contain the original 
            // squished sphere.
            transformedSphere.Radius = Math.Max(scale3.X, Math.Max(scale3.Y, scale3.Z));

            // transforming the center of the sphere is much easier. we can just use 
            // Vector3.Transform to transform the center vector. notice that we're using
            // Transform instead of TransformNormal because in this case we DO want to 
            // take translation into account.
            transformedSphere.Center = Vector3.Transform(sphere.Center, transform);


            return transformedSphere;
        }
    }
}
