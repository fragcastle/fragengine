using System;
using System.Collections.Generic;
using FragEngine.Content;
using Should;
using Xunit;

namespace FragEngine.Tests.Content
{
    public class FakeFileImporter : FileImporter<object>
    {
        private readonly bool _handles = false;
        public FakeFileImporter(bool handles = false)
        {
            HandlesPatchCalledWith = new List<string>();
            ImportCalledWith = new List<string>();
            _handles = handles;
        }

        public bool DidMatch { get; set; }

        public List<string> HandlesPatchCalledWith { get; set; }

        public List<string> ImportCalledWith { get; set; } 

        public override bool HandlesPath(string path)
        {
            HandlesPatchCalledWith.Add(path);
            return _handles;
        }

        public override object Import(string path)
        {
            ImportCalledWith.Add(path);
            return new Object();
        }
    }


    public class ContentCacheLoaderTests
    {
        public class LoadContent : ContentCacheLoaderTests
        {
            [Fact]
            public void Should_Call_Each_Importer_With_Every_File_In_Content()
            {
                var cache = new ContentCache();
                var loader = new ContentCacheLoader(cache);
                var importer = new FakeFileImporter();
                loader.RegisterImporter(importer);

                loader.LoadContent();

                importer.HandlesPatchCalledWith.Count.ShouldEqual(1);
                importer.HandlesPatchCalledWith[0].EndsWith("crate.png").ShouldBeTrue();
            }

            [Fact]
            public void Should_Not_Call_Import_If_HandlesPath_Returns_False()
            {
                var cache = new ContentCache();
                var loader = new ContentCacheLoader(cache);
                var importer = new FakeFileImporter();
                loader.RegisterImporter(importer);

                loader.LoadContent();

                importer.ImportCalledWith.Count.ShouldEqual(0);
            }

            [Fact]
            public void Should_Call_Import_If_HandlesPath_Returns_True()
            {
                var cache = new ContentCache();
                var loader = new ContentCacheLoader(cache);
                var importer = new FakeFileImporter(true);
                loader.RegisterImporter(importer);

                loader.LoadContent();

                importer.ImportCalledWith.Count.ShouldEqual(1);
            }

            [Fact]
            public void Should_Fill_Cache_With_Key_And_Returned_Object()
            {
                var cache = new ContentCache();
                var loader = new ContentCacheLoader(cache);
                var importer = new FakeFileImporter(true);
                loader.RegisterImporter(importer);

                loader.LoadContent();

                var obj = cache.GetContent<object>("Textures/crate.png");
                obj.ShouldNotBeNull();
            }
        }
    }
}
