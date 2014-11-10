using System;

namespace Forms3D
{
    public class Cube : PolyhedronBase
    {
        static readonly Point3D[,] faces = new Point3D[6, 5]
        {
            // front
            { new Point3D(0, 0, 1), 
              new Point3D(-1, 1, 1), new Point3D(-1, -1, 1), 
              new Point3D(1, -1, 1), new Point3D(1, 1, 1) },
            // back
            { new Point3D(0, 0, -1),
              new Point3D(1, 1, -1), new Point3D(1, -1, -1),
              new Point3D(-1, -1, -1), new Point3D(-1, 1, -1) },
            // left
            { new Point3D(-1, 0, 0),
              new Point3D(-1, 1, -1), new Point3D(-1, -1, -1),
              new Point3D(-1, -1, 1), new Point3D(-1, 1, 1) },
            // right
            { new Point3D(1, 0, 0),
              new Point3D(1, 1, 1), new Point3D(1, -1, 1),
              new Point3D(1, -1, -1), new Point3D(1, 1, -1) },
            // top
            { new Point3D(0, 1, 0),
              new Point3D(-1, 1, -1), new Point3D(-1, 1, 1),
              new Point3D(1, 1, 1), new Point3D(1, 1, -1) },
            // bottom
            { new Point3D(0, -1, 0),
              new Point3D(-1, -1, 1), new Point3D(-1, -1, -1),
              new Point3D(1, -1, -1), new Point3D(1, -1, 1) }
        };

        protected override Point3D[,] Faces
        {
            get
            {
                return faces;
            }
        }
    }
}
