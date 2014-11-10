using System;

namespace Forms3D
{
    public class Dodecahedron : PolyhedronBase
    {
        static readonly double G = (1 + Math.Sqrt(5)) / 2;      // approximately 1.618
        static readonly double H = 1 / G;                       // approximately 0.618
        static readonly double A = (3 * H + 4) / 5;             // approximately 1.171
        static readonly double B = (2 + G) / 5;                 // approximately 0.724

        static readonly Point3D[,] faces = new Point3D[12, 6]
        {
            { new Point3D( A, -B, 0), new Point3D( 1, -1, -1), new Point3D( G, 0, -H), new Point3D( G, 0,  H), new Point3D( 1, -1,  1), new Point3D( H, -G, 0) },
            { new Point3D(-A, -B, 0), new Point3D(-1, -1,  1), new Point3D(-G, 0,  H), new Point3D(-G, 0, -H), new Point3D(-1, -1, -1), new Point3D(-H, -G, 0) },
            { new Point3D(-A,  B, 0), new Point3D(-1,  1, -1), new Point3D(-G, 0, -H), new Point3D(-G, 0,  H), new Point3D(-1,  1,  1), new Point3D(-H,  G, 0) },
            { new Point3D( A,  B, 0), new Point3D( 1,  1,  1), new Point3D( G, 0,  H), new Point3D( G, 0, -H), new Point3D( 1,  1, -1), new Point3D( H,  G, 0) },
            { new Point3D(-B, 0, -A), new Point3D(-1,  1, -1), new Point3D(0,  H, -G), new Point3D(0, -H, -G), new Point3D(-1, -1, -1), new Point3D(-G, 0, -H) },
            { new Point3D(-B, 0,  A), new Point3D(-1, -1,  1), new Point3D(0, -H,  G), new Point3D(0,  H,  G), new Point3D(-1,  1,  1), new Point3D(-G, 0,  H) },
            { new Point3D( B, 0, -A), new Point3D( 1, -1, -1), new Point3D(0, -H, -G), new Point3D(0,  H, -G), new Point3D( 1,  1, -1), new Point3D( G, 0, -H) },
            { new Point3D( B, 0,  A), new Point3D( 1,  1,  1), new Point3D(0,  H,  G), new Point3D(0, -H,  G), new Point3D( 1, -1,  1), new Point3D( G, 0,  H) },
            { new Point3D(0, -A, -B), new Point3D( 1, -1, -1), new Point3D( H, -G, 0), new Point3D(-H, -G, 0), new Point3D(-1, -1, -1), new Point3D(0, -H, -G) },
            { new Point3D(0,  A, -B), new Point3D(-1,  1, -1), new Point3D(-H,  G, 0), new Point3D( H,  G, 0), new Point3D( 1,  1, -1), new Point3D(0,  H, -G) },
            { new Point3D(0, -A,  B), new Point3D(-1, -1,  1), new Point3D(-H, -G, 0), new Point3D( H, -G, 0), new Point3D( 1, -1,  1), new Point3D(0, -H,  G) },
            { new Point3D(0,  A,  B), new Point3D( 1,  1,  1), new Point3D( H,  G, 0), new Point3D(-H,  G, 0), new Point3D(-1,  1,  1), new Point3D(0,  H,  G) }
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
