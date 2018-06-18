using System;
using System.Numerics;

namespace AstronomyLib
{
    public struct HorizontalCoordinate
    {
        public HorizontalCoordinate(Angle azimuth, Angle altitude, Angle tilt)
            : this()
        {
            Azimuth = azimuth;
            Altitude = altitude;
            Tilt = tilt;
        }

        public HorizontalCoordinate(Angle azimuth, Angle altitude)
            : this(azimuth, altitude, Angle.Zero)
        {
        }

        // Eastward from north
        public Angle Azimuth { private set; get; }

        public Angle Altitude { private set; get; }

        public Angle Tilt { private set; get; }

        public static HorizontalCoordinate From(Vector3 vector)
        {
            Angle altitude = Angle.Zero;
            altitude.Sine = vector.Z;

            // Make X and Y components negative for zero azimuth at south
            //  with westward increasing values.
            // Then change x and y calculations likewise in ToVector method
            //  and remove adjustment in From(EquatorialCoordinates) method.
            Angle azimuth = Angle.ArcTangent(vector.X, vector.Y);

            return new HorizontalCoordinate(azimuth, altitude);
        }

        public static HorizontalCoordinate From(Matrix4x4 matrix)
        {
            // Transform (0, 0, -1) -- the vector extending from the lens
            Vector3 zAxisTransformed = Vector3.Transform(-Vector3.UnitZ, matrix);

            // Get the horizontal coordinates
            HorizontalCoordinate horzCoord = From(zAxisTransformed);

            // Find the theoretical HorizontalCoordinate for the transformed +Y vector if the phone is upright
            Angle yUprightAltitude = Angle.Zero;
            Angle yUprightAzimuth = Angle.Zero;

            if (horzCoord.Altitude.Degrees > 0)
            {
                yUprightAltitude = Angle.Right - horzCoord.Altitude;
                yUprightAzimuth = Angle.Straight + horzCoord.Azimuth;
            }
            else
            {
                yUprightAltitude = Angle.Right + horzCoord.Altitude;
                yUprightAzimuth = horzCoord.Azimuth;
            }
            Vector3 yUprightVector = new HorizontalCoordinate(yUprightAzimuth, yUprightAltitude).ToVector();

            // Find the real transformed +Y vector
            Vector3 yAxisTransformed = Vector3.Transform(Vector3.UnitY, matrix);

            // Get the angle between the upright +Y vector and the real transformed +Y vector
            double dotProduct = Vector3.Dot(yUprightVector, yAxisTransformed);
            Vector3 crossProduct = Vector3.Cross(yUprightVector, yAxisTransformed);
            crossProduct = Vector3.Normalize(crossProduct);

            // Sometimes dotProduct is slightly greater than 1, which 
            // raises an exception in the angleBetween calculation, so....
            dotProduct = Math.Min(dotProduct, 1);

            Angle angleBetween = Angle.FromRadians(Vector3.Dot(zAxisTransformed, crossProduct) * Math.Acos(dotProduct));
            horzCoord.Tilt = angleBetween;

            return horzCoord;
        }

        public static HorizontalCoordinate From(EquatorialCoordinate equatorialCoordinate,
                                                GeographicCoordinate geographicCoordinate, Time time)
        {
            // Calculate hour angle
            Angle localHourAngle = time.GreenwichSiderealTime - geographicCoordinate.Longitude - equatorialCoordinate.RightAscension;

            // Calculate azimuth
            Angle azimuth = Angle.ArcTangent(localHourAngle.Sine,
                                             localHourAngle.Cosine * geographicCoordinate.Latitude.Sine -
                                             equatorialCoordinate.Declination.Tangent * geographicCoordinate.Latitude.Cosine);

            // Adjustment for azimuth eastward from north
            azimuth += Angle.Straight;

            azimuth.NormalizeAroundZero();

            // Calculate altitude
            Angle altitude = Angle.Zero;
            altitude.Sine = geographicCoordinate.Latitude.Sine * equatorialCoordinate.Declination.Sine +
                            geographicCoordinate.Latitude.Cosine * equatorialCoordinate.Declination.Cosine * localHourAngle.Cosine;
            altitude.NormalizeAroundZero();

            return new HorizontalCoordinate(azimuth, altitude);
        }

        public Vector3 ToVector()
        {
            double x = Altitude.Cosine * Azimuth.Sine;
            double y = Altitude.Cosine * Azimuth.Cosine;
            double z = Altitude.Sine;

            return new Vector3((float)x, (float)y, (float)z);
        }

        public override string ToString()
        {
            return String.Format("Azi: {0} Alt: {1} Tilt: {2}", Azimuth, Altitude, Tilt);
        }
    }
}
