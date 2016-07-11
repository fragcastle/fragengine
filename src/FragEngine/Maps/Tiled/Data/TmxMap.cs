﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace FragEngine.Maps.Tiled.Data
{
    [XmlRoot(ElementName = "map")]
    public class TmxMap
    {
        public TmxMap()
        {
            Properties = new List<TmxProperty>();
            Tilesets = new List<TmxTileset>();
            Layers = new List<TmxLayer>();
            ObjectGroups = new List<TmxObjectGroup>();
        }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "orientation")]
        public TmxOrientation Orientation { get; set; }

        [XmlAttribute(AttributeName = "renderorder")]
        public TmxRenderOrder RenderOrder { get; set; }

        [XmlAttribute(AttributeName = "backgroundcolor")]
        public string BackgroundColor { get; set; }

        [XmlAttribute(AttributeName = "width")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }

        [XmlAttribute(AttributeName = "tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute(AttributeName = "tileheight")]
        public int TileHeight { get; set; }

        [XmlElement(ElementName = "tileset")]
        public List<TmxTileset> Tilesets { get; set; }

        [XmlElement(ElementName = "objectgroup")]
        public List<TmxObjectGroup> ObjectGroups { get; set; }

        [XmlElement(ElementName = "layer", Type = typeof(TmxTileLayer))]
        [XmlElement(ElementName = "imagelayer", Type = typeof(TmxImageLayer))]
        public List<TmxLayer> Layers { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<TmxProperty> Properties { get; set; }
    }
}
