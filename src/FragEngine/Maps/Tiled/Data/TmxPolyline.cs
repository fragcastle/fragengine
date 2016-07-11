using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    public class TmxPolyline
    {
        [XmlAttribute(AttributeName = "points")]
        public string Points { get; set; }
    }
}
