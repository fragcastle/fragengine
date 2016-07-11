using System.IO;
using System.Xml.Serialization;
using FragEngine.Content;
using FragEngine.Data;
using FragEngine.Maps.Tiled.Data;

namespace FragEngine.Maps.Tiled
{
    public class TiledMapImporter : FileImporter<Level>
    {
        public override bool HandlesPath(string path)
        {
            return base.HandlesPath(path) && path.EndsWith(".tmx");
        }

        public override Level Import(string path)
        {
            Logger.Instance.LogMessage($"Importing {path}", subsystem: "ContentImport");
            var level = new Level();
            using (var stream = OpenStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new XmlSerializer(typeof(TmxMap));
                    var map = (TmxMap)serializer.Deserialize(reader);
                    var xmlSerializer = new XmlSerializer(typeof(TmxTileset));

                    Logger.Instance.LogMessage($"Importing {map.Tilesets.Count} tilesets", subsystem: "ContentImport");

                    for (var i = 0; i < map.Tilesets.Count; i++)
                    {
                        var tileset = map.Tilesets[i];

                        if (!string.IsNullOrWhiteSpace(tileset.Source))
                        {
                            var directoryName = Path.GetDirectoryName(path);
                            var tilesetLocation = tileset.Source.Replace('/', Path.DirectorySeparatorChar);
                            var filePath = Path.Combine(directoryName, tilesetLocation);

                            Logger.Instance.LogMessage($"Importing tileset {i}: {filePath}", subsystem: "ContentImport");
                            using (var file = new FileStream(filePath, FileMode.Open))
                            {
                                map.Tilesets[i] = (TmxTileset)xmlSerializer.Deserialize(file);
                            }
                        }
                    }
                }
            }
            return level;
        }
    }
}
