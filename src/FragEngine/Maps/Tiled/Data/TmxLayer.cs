using System.Collections.Generic;
using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    [XmlInclude(typeof(TmxTileLayer))]
    [XmlInclude(typeof(TmxImageLayer))]
    public abstract class TmxLayer
    {
        protected TmxLayer()
        {
            Opacity = 1.0f;
            Visible = true;
            Properties = new List<TmxProperty>();
        }

        public override string ToString()
        {
            return Name;
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "opacity")]
        public float Opacity { get; set; }

        [XmlAttribute(AttributeName = "visible")]
        public bool Visible { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<TmxProperty> Properties { get; set; }
    }
}
