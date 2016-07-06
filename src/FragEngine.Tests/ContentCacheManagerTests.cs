using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FragEngine.Services;
using Should;
using Xunit;

namespace FragEngine.Tests
{
    public class ContentCacheManagerTests
    {
        public ContentCacheManagerTests()
        {
            var game = new TestGame();

            ServiceLocator.Add(FragEngineGame.Graphics.GraphicsDevice);
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
