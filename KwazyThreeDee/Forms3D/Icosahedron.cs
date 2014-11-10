using System;

namespace Forms3D
{
    public class Icosahedron : PolyhedronBase
    {
        static readonly double G = (1 + Math.Sqrt(5)) / 2;

        static readonly Point3D[,] faces = new Point3D[20, 3]
        {
            { new Point3D(0, G, 1), new Point3D(-1, 0, G), new Point3D(1, 0, G) },
            { new Point3D(-1, 0, G), new Point3D(0, -G, 1), new Point3D(1, 0, G) },

            { new Point3D(0, G, 1), new Point3D(1, 0, G), new Point3D(G, 1, 0) },
            { new Point3D(1, 0, G), new Point3D(G, -1, 0), new Point3D(G, 1, 0) },

            { new Point3D(0, G, 1), new Point3D(G, 1, 0), new Point3D(0, G, -1) },
            { new Point3D(G, 1, 0), new Point3D(1, 0, -G), new Point3D(0, G, -1) },

            { new Point3D(0, G, 1), new Point3D(0, G, -1), new Point3D(-G, 1, 0) },
            { new Point3D(0, G, -1), new Point3D(-1, 0, -G), new Point3D(-G, 1, 0) },

            { new Point3D(0, G, 1), new Point3D(-G, 1, 0), new Point3D(-1, 0, G) },
            { new Point3D(-G, 1, 0), new Point3D(-G, -1, 0), new Point3D(-1, 0, G) },

            { new Point3D(1, 0, G), new Point3D(0, -G, 1), new Point3D(G, -1, 0) },
            { new Point3D(0, -G, 1), new Point3D(0, -G, -1), new Point3D(G, -1, 0) },

            { new Point3D(G, 1, 0), new Point3D(G, -1, 0), new Point3D(1, 0, -G) },
            { new Point3D(G, -1, 0), new Point3D(0, -G, -1), new Point3D(1, 0, -G) },

            { new Point3D(0, G, -1), new Point3D(1, 0, -G), new Point3D(-1, 0, -G) },
            { new Point3D(1, 0, -G), new Point3D(0, -G, -1), new Point3D(-1, 0, -G) },

            { new Point3D(-G, 1, 0), new Point3D(-1, 0, -G), new Point3D(-G, -1, 0) },
            { new Point3D(-1, 0, -G), new Point3D(0, -G, -1), new Point3D(-G, -1, 0) },

            { new Point3D(-1, 0, G), new Point3D(-G, -1, 0), new Point3D(0, -G, 1) },
            { new Point3D(-G, -1, 0), new Point3D(0, -G, -1), new Point3D(0, -G, 1) },

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
