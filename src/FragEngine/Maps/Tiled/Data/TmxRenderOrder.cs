﻿using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    public enum TmxRenderOrder
    {
        [XmlEnum(Name = "right-down")]
        RightDown = 1,
        [XmlEnum(Name = "right-up")]
        RightUp = 2,
        [XmlEnum(Name = "left-down")]
        LeftDown = 3,
        [XmlEnum(Name = "left-up")]
        LeftUp = 4
    }
}
