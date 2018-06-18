using System.Xml.Serialization;

namespace AstronomyLib
{
    public class Connector
    {
        [XmlAttribute]
        public int From { set; get; }

        [XmlAttribute]
        public int To { set; get; }
    }
}
