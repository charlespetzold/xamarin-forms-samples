using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace AstronomyLib
{
    public class Constellations
    {
        public Constellations()
        {
            this.ConstellationList = new List<Constellation>();
        }

        public static Constellations Load()
        {
            Assembly assembly = typeof(Constellations).Assembly;
            string filename = "AstronomyLib.Data.constellations.sml";

            XmlSerializer serializer = new XmlSerializer(typeof(Constellations));
            Constellations constellations = null;

            using (Stream stream = assembly.GetManifestResourceStream(filename))
            {
                constellations = serializer.Deserialize(stream) as Constellations;
            }

            foreach (Constellation constellation in constellations.ConstellationList)
                constellation.Initialize();

            return constellations;
        }

        [XmlElement(ElementName = "Constellation")]
        public List<Constellation> ConstellationList { set; get; }
    }
}
