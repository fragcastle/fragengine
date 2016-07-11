using System.Collections.Generic;
using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    public class TmxTerrain
    {
        public TmxTerrain()
        {
            Properties = new List<TmxProperty>();
        }

        public override string ToString()
        {
            return Name;
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "tile")]
        public string TileId { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<TmxProperty> Properties { get; set; }
    }
}
