using System;

namespace Forms3D
{
    public struct Point4D
    {
        public Point4D(double x, double y, double z, double w) : this()
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Point4D(double x, double y, double z) : this(x, y, z, 1)
        {
        }

        public double X { private set; get; }

        public double Y { private set; get; }

        public double Z { private set; get; }

        public double W { private set; get; }

        public static implicit operator Point3D (Point4D pt)
        {
            return new Point3D(pt.X / pt.W, pt.Y / pt.W, pt.Z / pt.W);
        }

        public override string ToString()
        {
            return String.Format("(X={0} Y={1} Z={2}, W={3})", X, Y, Z, W);
        }
    }
}
