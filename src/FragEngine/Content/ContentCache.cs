using System;
using System.Collections.Generic;

namespace FragEngine.Content
{
    public class ContentAlreadyCachedException : Exception
    {
        private readonly string _key;

        public ContentAlreadyCachedException(string key, object content)
        {
            _key = key;
            ContentBeingCached = content;
        }

        public override string Message => $"The cache key {_key} is already taken in the cache.";
        public object ContentBeingCached { get; }
    }

    public class ContentCache : IReadableContentCache, IWriteableContentCache
    {
        private readonly Dictionary<string, object> _cache= new Dictionary<string, object>();
        public T GetContent<T>(string path)
        {
            path = NormalizePath(path).ToLowerInvariant();
            return (T)_cache[path];
        }

        public void AddContent(string path, object content)
        {
            path = NormalizePath(path);
            if (_cache.ContainsKey(path))
            {
                throw new ContentAlreadyCachedException(path, content);
            }
            _cache.Add(path.ToLowerInvariant(), content);
        }

        private string RemoveExtension(string path)
        {
            if (path.Contains("."))
                return path.Substring(0, path.LastIndexOf("."));
            else
                return path;
        }

        private string Slash(string path)
        {
            return path.Replace("\\", "/");
        }

        internal string NormalizePath(string path)
        {
            path = path.ToLowerInvariant();
            path = Slash(path);
            if (path.Contains("/"))
            {
                if (path.Contains("content"))
                    path = path.Substring(path.IndexOf("content") + 8);
                path = path.Replace("/", "_");
            }
            else
            {
                // assume it's a resource path
                // resources are cached as if they are in a
                // Resources folder in the content directory
                var parts = path.Split('.');
                if (parts.Length > 1)
                {
                    path = $"resources_{parts[parts.Length - 2]}";
                }
            }
            return RemoveExtension(path);
        }
    }

    public interface IWriteableContentCache
    {
        void AddContent(string path, object content);
    }

    public interface IReadableContentCache
    {
        T GetContent<T>(string path);
    }
}
