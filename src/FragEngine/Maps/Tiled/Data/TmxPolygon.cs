using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    public class TmxPolygon
    {
        [XmlAttribute(AttributeName = "points")]
        public string Points { get; set; }
    }
}
