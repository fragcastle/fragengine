using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    [XmlRoot(ElementName = "tileoffset")]
    public class TmxTileOffset
    {
        public TmxTileOffset()
        {
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }

        [XmlAttribute(AttributeName = "x")]
        public int X { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public int Y { get; set; }
    }
}
