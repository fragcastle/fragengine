using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FragEngine.Services;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Should;
using Xunit;

namespace FragEngine.Tests
{
    public class ContentCacheManagerTests
    {
        private readonly ContentManager _content;
        public ContentCacheManagerTests()
        {
            var game = new TestGame();
            ServiceLocator.Add<IGraphicsDeviceService>(FragEngineGame.Graphics);
            ServiceLocator.Add(FragEngineGame.Graphics.GraphicsDevice);
            _content = new ContentManager(ServiceLocator.Apply());
            _content.RootDirectory = "Content";
            
            ContentCacheManager.LoadContent(_content, new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Content")));
        }

        public class LoadContent : ContentCacheManagerTests
        {
            [Fact]
            public void Should_Load_Png_As_Texture2D()
            {
                // var txtr = _content.Load<Texture2D>("Textures/crate");
                var txtr = ContentCacheManager.GetTexture("Textures/crate");
                txtr.ShouldNotBeNull();
            }
        }

        public class GetTextureFromResource : ContentCacheManagerTests
        {
            [Fact]
            public void Should_Allow_Loading_Of_Any_FragEngine_Resource()
            {
                var textures = new List<string>()
                {
                    "blank.png",
                    "collision_tiles.png",
                    "frag_castle_logo.png",
                    "editor_save_btn.png",
                    "font_helloplusplus_white_16.png"
                };

                foreach (var texture in textures)
                {
                    var txtr = ContentCacheManager.GetTextureFromResource("FragEngine.Resources." + texture,
                        Assembly.GetAssembly(typeof(ContentCacheManager)));
                    txtr.ShouldNotBeNull(texture);
                }
            }
        }
    }
}
