using System;
using System.Numerics;

namespace AstronomyLib
{
    public struct EclipticCoordinate
    {
        public EclipticCoordinate(Angle longitude, Angle latitude, double radius) : this()
        {
            Longitude = longitude;
            Latitude = latitude;
            Radius = radius;
        }

        public EclipticCoordinate(Vector3 point) : this()
        {
            RectangularCoordinates = point;
        }

        public Angle Longitude { private set; get; }

        public Angle Latitude { private set; get; }

        public double Radius { private set; get; }

        public Vector3 RectangularCoordinates
        {
            set
            {
                Radius = Math.Sqrt(value.X * value.X +
                                   value.Y * value.Y +
                                   value.Z * value.Z);

                Longitude = Angle.ArcTangent(-value.Z, value.X);
                Latitude = Angle.ArcTangent(value.Y, Math.Sqrt(value.X * value.X + value.Z * value.Z));
            }
            get
            {
                double x = Radius * Latitude.Cosine * Longitude.Cosine;
                double y = Radius * Latitude.Sine;
                double z = -Radius * Latitude.Cosine * Longitude.Sine;

                return new Vector3((float)x, (float)y, (float)z);
            }
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", Longitude, Latitude, Radius);
        }
    }
}
