namespace AstronomyLib
{
    public struct EquatorialCoordinate
    {
        public EquatorialCoordinate(Angle rightAscension, Angle declination) : this()
        {
            RightAscension = rightAscension;
            Declination = declination;
        }

        public Angle RightAscension { private set; get; }

        public Angle Declination { private set; get; }

        // Time not used in current calculation, but might be need in future refinements
        public static EquatorialCoordinate From(EclipticCoordinate eclipticCoordinate, Time time)
        {
            // Calculate right ascension and declination
            Angle angleEpsilon = Angle.FromDegrees(23.4392911);     // obliquity of the ecliptic -- see Meeus, pg. 92

            Angle angleRightAscension = Angle.ArcTangent(eclipticCoordinate.Longitude.Sine * angleEpsilon.Cosine -
                                                         eclipticCoordinate.Latitude.Tangent * angleEpsilon.Sine,
                                                         eclipticCoordinate.Longitude.Cosine);

            angleRightAscension.NormalizePositive();

            Angle angleDeclination = Angle.Zero;

            angleDeclination.Sine = eclipticCoordinate.Latitude.Sine * angleEpsilon.Cosine +
                                    eclipticCoordinate.Latitude.Cosine * angleEpsilon.Sine * eclipticCoordinate.Longitude.Sine;

            angleDeclination.NormalizeAroundZero();

            return new EquatorialCoordinate(angleRightAscension, angleDeclination);
        }
    }
}
