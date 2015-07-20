using System;

namespace Forms3D
{
    public struct Vector3D
    {
        public Vector3D(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { private set; get; }

        public double Y { private set; get; }

        public double Z { private set; get; }

        public static Vector3D CrossProduct(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Y * v2.Z - v1.Z * v2.Y,
                                v1.Z * v2.X - v1.X * v2.Z,
                                v1.X * v2.Y - v1.Y * v2.X);
        }

        public static implicit operator Point3D(Vector3D v)
        {
            return new Point3D(v.X, v.Y, v.Z);
        }
    }
}
