using System;

namespace FragEngine.Content
{
    public abstract class ContentImporter<T> : IContentImporter
    {
        public abstract bool HandlesPath(string path);
        public abstract T Import(string path);

        Object IContentImporter.Import(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");
            return Import(filename);
        }
    }

    public interface IContentImporter
    {
        bool HandlesPath(string path);
        object Import(string path);
    }
}
