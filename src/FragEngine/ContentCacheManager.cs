using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FragEngine.Properties;
using FragEngine.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace FragEngine
{
    public static class ContentCacheManager
    {
        private static readonly object _syncRoot = new object();

        public static Dictionary<string, Texture2D> TextureCache { get; set; }

        private static Dictionary<string, SpriteFont> FontCache { get; set; }

        private static Dictionary<string, SoundEffect> SoundCache { get; set; }

        private static Dictionary<string, Song> SongCache { get; set; }

        private static List<DirectoryInfo> _contentDirectories;

        public static void AddContentDirectory(DirectoryInfo dir)
        {
            lock (_syncRoot)
            {
                if (_contentDirectories == null)
                {
                    _contentDirectories = new List<DirectoryInfo>();
                }
            }

            _contentDirectories.Add(dir);
        }

        public static void RemoveContentDirectory(DirectoryInfo dir)
        {
            _contentDirectories.Remove(dir);
        }

        public static void LoadContent(ContentManager contentManager)
        {
            if (_contentDirectories != null && _contentDirectories.Any())
            {
                _contentDirectories.ForEach(dir => LoadContent(contentManager, dir));
            }
            else
            {
                LoadContent(contentManager, null);
            }
        }

        public static void LoadContent(ContentManager contentManager, DirectoryInfo dir)
        {
            // everything in the "Textures" folder is loaded as a Texture2D
            TextureCache = contentManager.LoadContent<Texture2D>(dir);

            FontCache = contentManager.LoadContent<SpriteFont>(dir);

            SoundCache = contentManager.LoadContent<SoundEffect>(dir);

            SongCache = contentManager.LoadContent<Song>(dir);
        }

        public static Font GetFont(string path)
        {
            var texture = GetTexture(path);
            return new Font(texture);
        }

        public static Texture2D GetTextureFromResource(string path, Assembly caller = null)
        {
            caller = caller ?? Assembly.GetCallingAssembly();

            var stream = caller.GetManifestResourceStream(path);

            if (stream == null)
            {
                throw new Exception("Texture could not be read from resource: " + path);
            }

            var graphicsDevice = ServiceLocator.Get<GraphicsDevice>();

            return Texture2D.FromStream(graphicsDevice, stream);
        }

        public static Texture2D GetTexture(string path, bool fallbackToResource = true)
        {
            // normalize the path syntax
            path = NormalizePath(path);

            Texture2D texture = null;
            TextureCache.TryGetValue(path, out texture);

            if (fallbackToResource && texture == null)
            {
                texture = GetTextureFromResource(path, Assembly.GetCallingAssembly());

                // cache the texture for future calls!
                TextureCache[path] = texture;
            }
            return texture;
        }

        public static SpriteFont GetSpriteFont(string path)
        {
            path = NormalizePath(path);
            SpriteFont font = null;
            FontCache.TryGetValue(path, out font);
            return font;
        }

        public static SoundEffect GetSound(string path)
        {
            path = NormalizePath(path);
            SoundEffect sound = null;
            SoundCache.TryGetValue(path, out sound);
            return sound;
        }

        public static Song GetSong(string path)
        {
            path = NormalizePath(path);
            Song sound = null;
            SongCache.TryGetValue(path, out sound);
            return sound;
        }

        public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager, DirectoryInfo directory)
        {
            if (String.IsNullOrWhiteSpace(contentManager.RootDirectory))
            {
                contentManager.RootDirectory = "Content";
            }

            var dirPath = directory == null ? AppDomain.CurrentDomain.BaseDirectory : directory.FullName;

            if (Path.GetDirectoryName(dirPath) != "Content" && !dirPath.Contains("Content"))
            {
                dirPath = Path.Combine(dirPath, "Content");
            }

            // contentCacheManager, because it's evil, requires a relative path
            var contentDirectory = new Uri(dirPath);
            var currentDirectory = new Uri(AppDomain.CurrentDomain.BaseDirectory);

            var relativeContentDirectory = currentDirectory.MakeRelativeUri(contentDirectory);

            contentManager.RootDirectory = relativeContentDirectory.ToString();

            dirPath = Path.Combine(dirPath, GetContentDirectoryName<T>());

            var result = LoadDirectory<T>(contentManager, new DirectoryInfo(dirPath));

            return result;
        }

        private static string GetContentDirectoryName<T>()
        {
            var types = new Dictionary<Type, String>()
                {
                    {typeof (Texture2D), "Textures"},
                    {typeof (SoundEffect), "Sounds"},
                    {typeof (SpriteFont), "Fonts"},
                    {typeof (Song), "Songs"}
                };

            return types[typeof(T)];
        }

        private static Dictionary<String, T> LoadDirectory<T>(this ContentManager contentManager, DirectoryInfo dir)
        {
            var result = new Dictionary<String, T>();

            var contentFolder = GetContentDirectoryName<T>();

            if (dir.Exists)
            {
                //Load all files that matches the file filter
                FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo file in files)
                {
                    var pathToFile = file.Directory.ToString() + "\\" + Path.GetFileNameWithoutExtension(file.Name);
                    if (File.Exists(pathToFile + ".xnb"))
                    {
                        var loadPath = pathToFile.Substring(pathToFile.IndexOf(contentFolder)).Replace("\\", "/");
                        var key = NormalizePath(pathToFile.Substring(pathToFile.IndexOf(contentFolder)));

                        result[key] = contentManager.Load<T>(loadPath);
                    }
                }
            }

            return result;
        }

        private static string NormalizePath(string path)
        {
            return path.Replace("/", "_").Replace("\\", "_");
        }
    }
}
