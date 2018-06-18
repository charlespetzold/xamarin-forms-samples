using System.Numerics;

namespace AstronomyLib
{
    public class SolarSystemBody : CelestialBody
    {
        protected SolarSystemBody()
        {
        }

        protected SolarSystemBody(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public string Name { protected set; get; }

        public Color Color { protected set; get; }

        // Set by descendent classes, used here
        protected Vector3 HeliocentricLocation { set; private get; }

        protected override void OnTimeChanged()
        {
            // Calculate geocentric coordinates
            Vector3 bodyLocation = HeliocentricLocation - Earth.Instance.HeliocentricLocation;
            EclipticCoordinate geocentricCoordinate = new EclipticCoordinate(bodyLocation);
            EquatorialCoordinate = EquatorialCoordinate.From(geocentricCoordinate, Time);

            base.OnTimeChanged();
        }
    }
}
