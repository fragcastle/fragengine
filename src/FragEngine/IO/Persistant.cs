using System.IO;
using Newtonsoft.Json;

namespace FragEngine.IO
{
    public class Persistant
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, MaxDepth = 8 };

        public static void Persist<T>(string filePath, T obj)
        {
            Directory.CreateDirectory( Directory.GetDirectoryRoot( filePath ) );

            var json = PersistToJson(obj);
            using( var file = File.Create( filePath ) )
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(json);
                }
            }
        }

        public static string PersistToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject( obj, _settings );
        }

        public static T Load<T>(string filepath)
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
