using System.Collections.Generic;
using System.Xml.Serialization;

namespace AstronomyLib
{
    public class Constellation : CelestialBody
    {
        public Constellation()
        {
            Stars = new List<Star>();
            Connectors = new List<Connector>();
        }

        [XmlAttribute]
        public string Name { set; get; }

        [XmlAttribute]
        public string LongName { set; get; }

        [XmlElement(ElementName = "Star")]
        public List<Star> Stars { set; get; }

        [XmlElement(ElementName = "Connector")]
        public List<Connector> Connectors { set; get; }

        [XmlIgnore]
        public List<StarConnector> StarConnectors { set; get; }

        public void Initialize()
        {
            foreach (Connector connector in this.Connectors)
            {
                FindStar(connector.From).IsConnectedInConstellation = true;
                FindStar(connector.To).IsConnectedInConstellation = true;
            }

            // The following calculation is solely for positioning the name
            //  of the constellation relative to its contents.

            // First determine if the constellation crosses the 0° right ascension line
            int numLessThan8Hours = 0;
            int numGreaterThan16Hours = 0;

            foreach (Star star in this.Stars)
            {
                if (star.RightAscension < 8)
                    numLessThan8Hours += 1;

                else if (star.RightAscension > 16)
                    numGreaterThan16Hours += 1;

            }

            bool straddles = numLessThan8Hours > 0 && numGreaterThan16Hours > 0;
            bool countAll = this.Connectors.Count == 0;

            int count = 0;
            double rightAscensionTotal = 0;
            double declinationTotal = 0;

            foreach (Star star in this.Stars)
                if (countAll || star.IsConnectedInConstellation)
                {
                    count += 1;
                    rightAscensionTotal += star.RightAscension - (straddles && star.RightAscension > 16 ? 24 : 0);
                    declinationTotal += star.Declination;
                }

            this.EquatorialCoordinate = new EquatorialCoordinate(Angle.FromHours(rightAscensionTotal / count),
                                                                 Angle.FromDegrees(declinationTotal / count));
        }

        public Star FindStar(int number)
        {
            foreach (Star star in this.Stars)
                if (star.Number == number)
                    return star;

            return null;
        }
    }
}
