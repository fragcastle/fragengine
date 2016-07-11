using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FragEngine.Services;
using Microsoft.Xna.Framework.Content;

namespace FragEngine.Content
{
    public class ContentCacheLoader
    {
        private readonly IWriteableContentCache _cache;
        private readonly ContentManager _manager;
        private DirectoryInfo _workingDirectory = null;
        private readonly List<IContentImporter> _importers = new List<IContentImporter>();

        public ContentCacheLoader(IWriteableContentCache cache, ContentManager manager = null)
        {
            _cache = cache;
            _manager = manager ?? new ContentManager(ServiceLocator.Apply());
        }

        public void LoadContent()
        {
            SetContentDirectory();
            if (_workingDirectory.Exists)
            {
                //Load all files that matches the file filter
                FileInfo[] files = _workingDirectory.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo file in files)
                {
                    var pathToFile = file.Directory + "\\" + Path.GetFileName(file.Name);
                    // get the importer for this file
                    var importer = _importers.FirstOrDefault(i => i.HandlesPath(pathToFile));
                    if (importer != null)
                    {
                        var obj = importer.Import(pathToFile);
                        _cache.AddContent(pathToFile, obj);
                    }
                }
            }
        }

        public void RegisterImporter(IContentImporter importer)
        {
            _importers.Add(importer);
        }

        private void SetContentDirectory()
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory;

            if (Path.GetDirectoryName(dirPath) != "Content" && !dirPath.Contains("Content"))
            {
                dirPath = Path.Combine(dirPath, "Content");
            }

            _workingDirectory = new DirectoryInfo(dirPath);

            // contentCacheManager, because it's evil, requires a relative path
            var contentDirectory = new Uri(dirPath);
            var currentDirectory = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\");

            var relativeContentDirectory = currentDirectory.MakeRelativeUri(contentDirectory);

            _manager.RootDirectory = relativeContentDirectory.ToString();
        }
    }
}
