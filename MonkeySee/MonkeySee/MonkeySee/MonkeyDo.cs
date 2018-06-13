using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Actions;

namespace MonkeySee
{
    public class MonkeyDo : Application
    {
        //      Node mainNode;
        Node cameraNode;

        Matrix3x4 cameraTransform;
/*
        Matrix3x4 monkeyLookAt = CreateLookAt(new Vector3(0, 0, -10),  //eye
                                              new Vector3(0, 3, 0),     // target (center of monkey)
                                              new Vector3(0, 1, 0));    // up

*/



        public MonkeyDo(ApplicationOptions options = null) : base(options)
        {

        }

        Quaternion orientation = Quaternion.Identity;

        public Quaternion Orientation
        {
            set
            {
                orientation = value;

                Matrix4 camXform = From3x4(cameraTransform);

          //      Matrix3x4 back = To3x4(camXform);

                orientation.ToAxisAngle(out Vector3 axis, out float angle);

                camXform = Matrix4.Mult(camXform, Matrix4.CreateFromAxisAngle(axis, -angle));



                cameraNode.SetTransform(To3x4(camXform));



           //     cameraNode.SetTransform()

           //     Matrix3x4 cameraMatrix = cameraNode.Transform;
             //   Matrix4 cameraMatrix4 = From3x4(cameraMatrix);


             //   Matrix4 matx = new Matrix4(cameraNode.Transform


       //         cameraNode.Rotation = orientation;

       //         System.Diagnostics.Debug.WriteLine("Position: {0} Direction {1}", cameraNode.Position, cameraNode.Direction);

            }
            get
            {
                return orientation;
            }
        }


        protected override void Start()
        {
            base.Start();

            // Create Scene and stuff
            Scene scene = new Scene();
            Octree octree = scene.CreateComponent<Octree>();
            Zone zone = scene.CreateComponent<Zone>();
            zone.AmbientColor = new Color(0.6f, 0.6f, 0.6f);

            // Create the root note
            Node rootNode = scene.CreateChild();
            rootNode.Position = new Vector3(0, 0, 0);

            // Create a light node (based on SimpleApplication)
            Node lightNode = rootNode.CreateChild();
            lightNode.Position = new Vector3(-5, 10, 0);
            Light light = lightNode.CreateComponent<Light>();
            light.Range = 100;
            light.Brightness = 0.5f;
            light.LightType = LightType.Point;

            // Create the camera
            cameraNode = scene.CreateChild();
            Camera camera = cameraNode.CreateComponent<Camera>();

            // Set Position and Direction above the monkey pointing down
            cameraNode.Position = new Vector3(0, 12, 0);
            cameraNode.SetDirection(new Vector3(0, 0, 0) - cameraNode.Position);

            // Save the camera transform 
            cameraTransform = cameraNode.Transform;

            // Xamarin monkey model created by Vic Wang at [http://vidavic.weebly.com](http://vidavic.weebly.com "Vidavic")
            Node monkeyNode = rootNode.CreateChild("monkeyNode");
            AnimatedModel monkey = monkeyNode.CreateComponent<AnimatedModel>();
            monkey.Model = ResourceCache.GetModel("monkey1.mdl");
            monkey.SetMaterial(ResourceCache.GetMaterial("Materials/phong1.xml"));

            // Move it down a smidge so it centered on the origin
            monkeyNode.Translate(new Vector3(0, -3, 0));

            // Set up the Viewport
            Viewport viewport = new Viewport(Context, scene, camera, null);
            Renderer.SetViewport(0, viewport);
            viewport.SetClearColor(new Color(0.88f, 0.88f, 0.88f));
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }


/*
        static Matrix3x4 CreateLookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 z = -Vector3.Normalize(eye - target);
            Vector3 x = Vector3.Normalize(Vector3.Cross(up, z));
            Vector3 y = Vector3.Normalize(Vector3.Cross(z, x));



            float offsetX = -Vector3.Dot(x, eye);
            float offsetY = -Vector3.Dot(y, eye);
            float offsetZ = -Vector3.Dot(z, eye);


            Matrix3x4 rot = new Matrix3x4(new Vector3(x.X, y.X, z.X),
                                        new Vector3(x.Y, y.Y, z.Y),
                                        new Vector3(x.Z, y.Z, z.Z),
                                        new Vector3(offsetX, offsetY, offsetZ));



            return rot;
        }
*/

        // Matrix conversion methods
        static Matrix3x4 To3x4(Matrix4 m)
        {
            return new Matrix3x4(m.M11, m.M21, m.M31, m.M41, m.M12, m.M22, m.M32, m.M42, m.M13, m.M23, m.M33, m.M43);
        }

        static Matrix4 From3x4(Matrix3x4 m)
        {
            return new Matrix4(m.m00, m.m10, m.m20, 0, 
                               m.m01, m.m11, m.m21, 0,
                               m.m02, m.m12, m.m22, 0,
                               m.m03, m.m13, m.m23, 1);
        }
    }
}
