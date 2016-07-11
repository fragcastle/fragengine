using System;
using FragEngine.Content;
using Should;
using Xunit;

namespace FragEngine.Tests.Content
{
    public class ContentCacheTests
    {
        public class NormalizePath : ContentCacheTests
        {
            [Fact]
            public void Should_Convert_Local_Path_To_Directory_File()
            {
                var cache = new ContentCache();
                var test = cache.NormalizePath("C:\\test\\content\\textures\\somefile.png");
                test.ShouldEqual("textures_somefile");
            }

            [Fact]
            public void Should_Convert_Resource_Path_To_Directory_File()
            {
                var cache = new ContentCache();
                var test = cache.NormalizePath("FragEngine.Resources.somefile.png");
                test.ShouldEqual("resources_somefile");
            }

            [Fact]
            public void Should_Convert_Directory_Slash_File_To_Key_Name()
            {
                var cache = new ContentCache();
                var test = cache.NormalizePath("Resources/somefile.png");
                test.ShouldEqual("resources_somefile");
            }
        }

        public class AddContent : ContentCacheTests
        {
            [Fact]
            public void Should_Normalize_Path_Correctly()
            {
                var cache = new ContentCache();
                cache.AddContent("C:\\test\\content\\textures\\somefile.png", new Object());
                var obj = cache.GetContent<object>("Textures/somefile.png");

                obj.ShouldNotBeNull();
            }
        }
    }
}
