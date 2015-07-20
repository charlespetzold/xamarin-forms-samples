using System;

namespace Forms3D
{
    public class Octahedron : PolyhedronBase
    {
        static readonly Point3D[,] faces = new Point3D[8, 3]
        {
            // front upper right
            { new Point3D(0, 1, 0), new Point3D(0, 0, 1), new Point3D(1, 0, 0) },

            // front upper left
            { new Point3D(0, 1, 0), new Point3D(-1, 0, 0), new Point3D(0, 0, 1) },

            // front lower left
            { new Point3D(0, -1, 0), new Point3D(0, 0, 1), new Point3D(-1, 0, 0) },

            // front lower right
            { new Point3D(0, -1, 0), new Point3D(1, 0, 0), new Point3D(0, 0, 1) },

            // back lower right
            { new Point3D(0, -1, 0), new Point3D(0, 0, -1), new Point3D(1, 0, 0) },

            // back lower left
            { new Point3D(0, -1, 0), new Point3D(-1, 0, 0), new Point3D(0, 0, -1) },

            // back upper left
            { new Point3D(0, 1, 0), new Point3D(0, 0, -1), new Point3D(-1, 0, 0) },

            // back upper right
            { new Point3D(0, 1, 0), new Point3D(1, 0, 0), new Point3D(0, 0, -1) }
        };

        static Octahedron()
        {
            // Double it in size
            for (int face = 0; face < 8; face++)
                for (int vertex = 0; vertex < 3; vertex++)
                {
                    Point3D point = faces[face, vertex];
                    faces[face, vertex] = new Point3D(2 * point.X, 2 * point.Y, 2 * point.Z);
                }
        }

        protected override Point3D[,] Faces
        {
            get
            {
                return faces;
            }
        }
    }
}
