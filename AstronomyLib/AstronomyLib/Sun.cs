using System.Numerics;

namespace AstronomyLib
{
    public class Sun : SolarSystemBody
    {
        public Sun() : base("Sun", Color.White)
        {
            HeliocentricLocation = Vector3.Zero;
        }
    }
}
