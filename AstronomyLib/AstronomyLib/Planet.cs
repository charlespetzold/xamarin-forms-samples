using System;
using System.Windows;
using System.Numerics;

namespace AstronomyLib
{
    public class Planet : SolarSystemBody
    {
        VsopCruncher vsop;

        public Planet(string strPlanetAbbreviation, string name, Color color) : 
            this(strPlanetAbbreviation)
        {
            Name = name;
            Color = color;
        }

        protected Planet(string strPlanetAbbreviation) 
        {
            vsop = new VsopCruncher(strPlanetAbbreviation);
        }

        protected override void OnTimeChanged()
        {
            Angle latitude = Angle.FromRadians(vsop.GetLatitude(Time.Tau));
            Angle longitude = Angle.FromRadians(vsop.GetLongitude(Time.Tau));
            double radius = vsop.GetRadius(Time.Tau);

            HeliocentricLocation = new EclipticCoordinate(longitude, latitude, radius).RectangularCoordinates;

            base.OnTimeChanged();
        }
    }
}
