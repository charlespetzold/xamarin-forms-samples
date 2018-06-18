using System.Xml.Serialization;

namespace AstronomyLib
{
    public class Star : CelestialBody 
    {
        [XmlAttribute]
        public int Number { set; get; }

        [XmlAttribute]
        public string Name { set; get; }

        // J2000 in hours
        [XmlAttribute]
        public double RightAscension { set; get; }

        // J2000 in degrees
        [XmlAttribute]
        public double Declination { set; get; }

        [XmlAttribute]
        public double Magnitude { set; get; }

        // J2000 in arcsec / year
        [XmlAttribute]
        public double RightAscensionProperMotion { set; get; }

        // J2000 in arcsec / year
        [XmlAttribute]
        public double DeclinationProperMotion { set; get; }

        [XmlAttribute]
        public string LongName { set; get; }

        [XmlIgnore]
        public bool IsConnectedInConstellation { set; get; }

        protected override void OnTimeChanged()
        {
            int years = this.Time.UniversalTime.Year - 2000;
            Angle ra = Angle.FromHours(this.RightAscension) +
                       Angle.FromHours(0, 0, this.RightAscensionProperMotion * years);

            Angle dec = Angle.FromDegrees(this.Declination) +
                        Angle.FromDegrees(0, 0, this.DeclinationProperMotion * years);

            EquatorialCoordinate = new EquatorialCoordinate(ra, dec);

            base.OnTimeChanged();
        }
    }
}
