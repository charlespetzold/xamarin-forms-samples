using System;
using Xamarin.Forms;

namespace Forms3D
{
    public struct Point3D
    {
        public Point3D(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { private set; get; }

        public double Y { private set; get; }

        public double Z { private set; get; }

        public static implicit operator Point4D (Point3D pt)
        {
            return new Point4D(pt.X, pt.Y, pt.Z, 1);
        }

        public static implicit operator Point (Point3D pt)
        {
            return new Point(pt.X, pt.Y);
        }

        public static implicit operator Vector3D (Point3D pt)
        {
            return new Vector3D(pt.X, pt.Y, pt.Z);
        }

        public static Point3D operator + (Point3D pt, Vector3D v)
        {
            return new Point3D(pt.X + v.X, pt.Y + v.Y, pt.Z + v.Z);
        }
        public static Point3D operator +(Vector3D v, Point3D pt)
        {
            return new Point3D(pt.X + v.X, pt.Y + v.Y, pt.Z + v.Z);
        }

        public static Vector3D operator - (Point3D pt1, Point3D pt2)
        {
            return new Vector3D(pt1.X - pt2.X, pt1.Y - pt2.Y, pt1.Z - pt2.Z);
        }

        public static bool operator ==(Point3D pt1, Point3D pt2)
        {
            return (pt1.X == pt2.X) && (pt1.Y == pt2.Y) && (pt1.Z == pt2.Z);
        }

        public static bool operator !=(Point3D pt1, Point3D pt2)
        {
            return (pt1.X != pt2.X) || (pt1.Y != pt2.Y) || (pt1.Z != pt2.Z);
        }

        public override bool Equals(object o)
        {
            if (!(o is Point))
                return false;

            return (this == (Point)o);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() * 397) ^ (Z.GetHashCode() * 54321);
        }

        public override string ToString()
        {
            return String.Format("(X={0} Y={1} Z={2})", X, Y, Z);
        }
    }
}
