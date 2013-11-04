using System.IO;
using Newtonsoft.Json;

namespace FragEngine.IO
{
    public class DiskStorage
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, MaxDepth = 8 };

        public static void SaveToDisk<T>(string filePath, T obj)
        {
            Directory.CreateDirectory( Directory.GetDirectoryRoot( filePath ) );

            var json = SaveToJson(obj);
            using( var file = File.Create( filePath ) )
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(json);
                }
            }
        }

        public static string SaveToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject( obj, _settings );
        }

        public static T LoadFromDisk<T>(string filepath)
        {
            T instance;
            using (var file = File.OpenRead(filepath))
            {
                using (var reader = new StreamReader(file))
                {
                    var json = reader.ReadToEnd();
                    instance = JsonConvert.DeserializeObject<T>( json, _settings );
                }
            }
            return instance;
        }
    }
}
