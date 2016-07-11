using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    public enum TmxOrientation
    {
        [XmlEnum(Name = "orthogonal")]
        Orthogonal = 1,
        [XmlEnum(Name = "isometric")]
        Isometric = 2,
        [XmlEnum(Name = "staggered")]
        Staggered = 3
    }
}
