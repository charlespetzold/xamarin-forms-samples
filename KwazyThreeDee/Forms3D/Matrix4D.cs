using System;
using Xamarin.Forms;

namespace Forms3D
{
    public struct Matrix4D
    {
        double m11, m22, m33, m44;

        public Matrix4D(double m11, double m12, double m13, double m14,
                        double m21, double m22, double m23, double m24,
                        double m31, double m32, double m33, double m34,
                        double m41, double m42, double m43, double m44) : this()
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public double M11
        {
            // Causes parameterless constructor to create identity matrix.
            private set { m11 = value - 1; }
            get { return m11 + 1; }
        }

        public double M12 { private set; get; }
        public double M13 { private set; get; }
        public double M14 { private set; get; }
        public double M21 { private set; get; }

        public double M22
        {
            // Causes parameterless constructor to create identity matrix.
            private set { m22 = value - 1; }
            get { return m22 + 1; }
        }

        public double M23 { private set; get; }
        public double M24 { private set; get; }
        public double M31 { private set; get; }
        public double M32 { private set; get; }

        public double M33
        {
            // Causes parameterless constructor to create identity matrix.
            private set { m33 = value - 1; }
            get { return m33 + 1; }
        }

        public double M34 { private set; get; }
        public double M41 { private set; get; }
        public double M42 { private set; get; }
        public double M43 { private set; get; }

        public double M44
        {
            // Causes parameterless constructor to create identity matrix.
            private set { m44 = value - 1; }
            get { return m44 + 1; }
        }

        public static Matrix4D RotationX(double radians)
        {
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            return new Matrix4D(1,    0,   0, 0,
                                0,  cos, sin, 0,
                                0, -sin, cos, 0,
                                0,    0,   0, 1);
        }

        public static Matrix4D RotationY(double radians)
        {
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            return new Matrix4D(cos, 0, -sin, 0,
                                  0, 1,    0, 0,
                                sin, 0,  cos, 0,
                                  0, 0,    0, 1);
        }

        public static Matrix4D RotationZ(double radians)
        {
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            return new Matrix4D( cos, sin, 0, 0, 
                                -sin, cos, 0, 0,
                                   0,   0, 1, 0,
                                   0,   0, 0, 1);
        }

        // Enormously simplified
        public static Matrix4D OrthographicView(Point center, double width)
        {
            return new Matrix4D(width, 0, 0, 0,
                                0, width, 0, 0,
                                0, 0, width, 0,
                                center.X, center.Y, 0, 1);
        }

        public static Matrix4D operator * (Matrix4D m1, Matrix4D m2)
        {
            return new Matrix4D(m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41,
                                m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42,
                                m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43,
                                m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44,

                                m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41,
                                m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42,
                                m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43,
                                m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44,

                                m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M24 * m2.M41,
                                m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M24 * m2.M42,
                                m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M24 * m2.M43,
                                m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M24 * m2.M44,

                                m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41,
                                m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42,
                                m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43,
                                m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44);
        }

        public static Point4D operator * (Point4D pt, Matrix4D m)
        {
            return new Point4D(pt.X * m.M11 + pt.Y * m.M21 + pt.Z * m.M31 + pt.W * m.M41,
                               pt.X * m.M12 + pt.Y * m.M22 + pt.Z * m.M32 + pt.W * m.M42,
                               pt.X * m.M13 + pt.Y * m.M23 + pt.Z * m.M33 + pt.W * m.M43,
                               pt.X * m.M14 + pt.Y * m.M24 + pt.Z * m.M34 + pt.W * m.M44);
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}\r\n" +
                                 "{4} {5} {6} {7}\r\n" +
                                 "{8} {9} {10} {11}\r\n" +
                                 "{12} {13} {14} {15}\r\n",
                                 M11, M12, M13, M14, 
                                 M21, M22, M23, M24,
                                 M31, M31, M33, M34, 
                                 M41, M42, M43, M44);
        }
    }
}
