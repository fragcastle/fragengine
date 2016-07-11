using System.IO;

namespace FragEngine.Content
{
    public abstract class FileImporter<T> : ContentImporter<T>
    {
        public override bool HandlesPath(string path)
        {
            return File.Exists(path);
        }

        public override T Import(string path)
        {
            return default(T);
        }

        protected Stream OpenStream(string path)
        {
            return File.OpenRead(path);
        }
    }
}
