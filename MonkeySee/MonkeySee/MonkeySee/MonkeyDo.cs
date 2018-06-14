using System;
using Urho;

namespace MonkeySee
{
    public class MonkeyDo : Application
    {
        Node cameraNode;
        Node monkeyNode;
        Matrix4 cameraTransform;
        Quaternion orientation = Quaternion.Identity;

        // Used for the head rotation
        static readonly Quaternion qSittingUp = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), 90);

        public MonkeyDo(ApplicationOptions options = null) : base(options)
        {
        }

        public Quaternion Orientation
        {
            set
            {
                orientation = value;

                // First calculate the camera rotation based on the device orientation
                orientation.ToAxisAngle(out Vector3 axis, out float angle);
                Matrix4 rotatedCameraTransform = Matrix4.Mult(cameraTransform, 
                                                              Matrix4.CreateFromAxisAngle(axis, -angle));
                cameraNode.SetTransform(To3x4(rotatedCameraTransform));

                // Now calculate the head swerve
                Quaternion difference = orientation * Quaternion.Invert(qSittingUp);

                // ToAxisAngle returns angle in radians
                difference.ToAxisAngle(out axis, out angle);

                // Empirically, this is always positive
                angle *= 180 / (float)Math.PI;

                // Swap some components for the difference in coordinates
                Vector3 swerveAxis = new Vector3(-axis.Z, -axis.Y, axis.X);

                // Now calculate the swerve angle
                const float FOLLOW = 30;    // degrees
                const float IGNORE = 45;

                float swerveAngle = 0;

                if (angle > IGNORE)
                {
                    swerveAngle = 0;
                }
                else if (angle >= FOLLOW)
                {
                    swerveAngle = FOLLOW / (IGNORE - FOLLOW) * (IGNORE - angle);
                }
                else
                {
                    swerveAngle = angle;
                }

                // Calculate the head rotation (FromAxisAngle takes angle in degrees)
                Quaternion headRotation = Quaternion.FromAxisAngle(swerveAxis, swerveAngle);

                // Rotate the head
                monkeyNode.GetChild("head", true).Rotation = headRotation;
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
            zone.AmbientColor = new Color(0.75f, 0.75f, 0.75f);

            // Create the root note
            Node rootNode = scene.CreateChild();
            rootNode.Position = new Vector3(0, 0, 0);

            // Create a light node
            Node lightNode = rootNode.CreateChild();
            Light light = lightNode.CreateComponent<Light>();
            light.Color = new Color(0.75f, 0.75f, 0.75f);
            light.LightType = LightType.Directional;
            lightNode.SetDirection(new Vector3(2, -3, -1));

            // Create the camera
            cameraNode = scene.CreateChild();
            Camera camera = cameraNode.CreateComponent<Camera>();

            // Set Position and Direction above the monkey pointing down
            cameraNode.Position = new Vector3(0, 12, 0);
            cameraNode.SetDirection(new Vector3(0, 0, 0) - cameraNode.Position);

            // Save the camera transform 
            cameraTransform = From3x4(cameraNode.Transform);

            // Xamarin monkey model created by Vic Wang at [http://vidavic.weebly.com](http://vidavic.weebly.com "Vidavic")
            monkeyNode = rootNode.CreateChild("monkeyNode");
            AnimatedModel monkey = monkeyNode.CreateComponent<AnimatedModel>();
            monkey.Model = ResourceCache.GetModel("monkey1.mdl");
            monkey.SetMaterial(ResourceCache.GetMaterial("Materials/phong1.xml"));

            // Move the monkey down a bit so it's centered on the origin
            monkeyNode.Translate(new Vector3(0, -3, 0));

            // Set up the Viewport
            Viewport viewport = new Viewport(Context, scene, camera, null);
            Renderer.SetViewport(0, viewport);
            viewport.SetClearColor(new Color(0.88f, 0.88f, 0.88f));
        }

        // Matrix conversion methods
        static Matrix3x4 To3x4(Matrix4 m)
        {
            return new Matrix3x4(m.M11, m.M21, m.M31, m.M41, 
                                 m.M12, m.M22, m.M32, m.M42, 
                                 m.M13, m.M23, m.M33, m.M43);
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
