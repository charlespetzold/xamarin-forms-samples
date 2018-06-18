using System;

namespace AstronomyLib
{
    public struct GeographicCoordinate
    {
        public GeographicCoordinate(Angle longitude, Angle latitude) : this()
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public Angle Longitude { private set; get; }

        public Angle Latitude { private set; get; }

        public static GeographicCoordinate NewYorkCity
        {
            get
            {
                return new GeographicCoordinate(Angle.FromDegrees(73.99),
                                                Angle.FromDegrees(40.73));
            }
        }

        // Subtraction operator returns distance along unit sphere.
        public static double operator -(GeographicCoordinate gc1, GeographicCoordinate gc2)
        {
            Angle angBetween = Angle.Zero;
            angBetween.Cosine = gc1.Latitude.Sine * gc2.Latitude.Sine +
                                gc1.Latitude.Cosine * gc2.Latitude.Cosine * 
                                (gc1.Longitude - gc2.Longitude).Cosine;
            return angBetween.Radians;
        }

        // Operators and overrides
        public static bool operator == (GeographicCoordinate geo1, GeographicCoordinate geo2)
        {
            return geo1.Equals(geo2);
        }

        public static bool operator !=(GeographicCoordinate geo1, GeographicCoordinate geo2)
        {
            return !geo1.Equals(geo2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GeographicCoordinate))
                return false;

            return Equals((Angle)obj);
        }

        public bool Equals(GeographicCoordinate that)
        {
            return this.Longitude.Equals(that.Longitude) && this.Latitude.Equals(that.Latitude);
        }

        public override int GetHashCode()
        {
            Angle longitude = this.Longitude;
            Angle latitude = this.Latitude;

            longitude.NormalizePositive();
            latitude.NormalizePositive();

            return (180 * longitude + latitude).GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0} longitude {1} latitude", Longitude, Latitude);
        }
    }
}
