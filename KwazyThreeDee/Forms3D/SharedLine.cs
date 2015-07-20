using System;

namespace Forms3D
{
    public struct SharedLine
    {
        public SharedLine(Point3D point1, Point3D point2, Vector3D normal1, Vector3D normal2)
            : this()
        {
            Point1 = point1;
            Point2 = point2;
            Normal1 = normal1;
            Normal2 = normal2;
        }
        public SharedLine(Point3D point1, Point3D point2)
            : this(point1, point2, new Vector3D(), new Vector3D())
        {
        }

        public Point3D Point1 { private set; get; }

        public Point3D Point2 { private set; get; }

        public Vector3D Normal1 { set; get; }

        public Vector3D Normal2 { set; get; }
    }
}
