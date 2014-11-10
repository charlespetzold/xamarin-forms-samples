using System;

namespace Forms3D
{
    public class Tetrahedron : PolyhedronBase
    {
        static readonly Point3D[,] faces = new Point3D[4, 3]
        {
            // upper-left front
            { new Point3D(-1, 1, -1), new Point3D(-1, -1, 1), new Point3D(1, 1, 1) },

            // lower-right front 
            { new Point3D(1, -1, -1), new Point3D(1, 1, 1), new Point3D(-1, -1, 1) },

            // upper-right back
            { new Point3D(1, 1, 1), new Point3D(1, -1, -1), new Point3D(-1, 1, -1) },

            // lower-left back
            { new Point3D(-1, -1, 1), new Point3D(-1, 1, -1), new Point3D(1, -1, -1) }
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
