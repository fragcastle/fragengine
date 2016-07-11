using System.Reflection;
using FragEngine.Services;
using Should;
using Xunit;

namespace FragEngine.Tests
{
    public class FontTests
    {
        protected const string HelloWorld = "HELLO WORLD";

        public FontTests()
        {
            var game = new TestGame();

            ServiceLocator.Add(FragEngineGame.Graphics.GraphicsDevice);
        }

        public class WidthMap : FontTests
        {
            [Fact]
            public void Should_Store_The_Widths_For_All_Characters_In_The_Font()
            {
                var txtr = ContentCacheManager.GetTextureFromResource("FragEngine.Resources.font_04b03_white_16.png", Assembly.GetAssembly(typeof(FragEngineGame)));
                var font = new Font(txtr);

                var width = font.WidthMap['W' - Font.FirstChar];

                width.ShouldEqual(12);
            }
        }

        public class Indices : FontTests
        {
            [Fact]
            public void Should_Store_The_Offsets_For_All_Characters_In_The_Font()
            {
                var txtr = ContentCacheManager.GetTextureFromResource("FragEngine.Resources.font_04b03_white_16.png", Assembly.GetAssembly(typeof(FragEngineGame)));
                var font = new Font(txtr);

                var idx = font.Indices['W' - Font.FirstChar];

                idx.ShouldEqual(543);
            }
        }

        public class WidthForString : FontTests
        {
            [Fact]
            public void Should_Return_The_Correct_Width_For_A_String_Of_Text()
            {
                var txtr = ContentCacheManager.GetTextureFromResource("FragEngine.Resources.font_04b03_white_16.png", Assembly.GetAssembly(typeof(FragEngineGame)));
                var font = new Font(txtr);

                var width = font.WidthForString(HelloWorld);
                width.ShouldEqual(10 + 8 + 8 + 8 + 10 + 8 + 12 + 10 + 10 + 8 + 10);
            }
        }

        public class HeightForString : FontTests
        {
            [Fact]
            public void Should_Return_The_Correct_Width_For_A_String_Of_Text()
            {
                var txtr = ContentCacheManager.GetTextureFromResource("FragEngine.Resources.font_04b03_white_16.png", Assembly.GetAssembly(typeof(FragEngineGame)));
                var font = new Font(txtr);

                var width = font.HeightForString(HelloWorld);
                width.ShouldEqual(txtr.Height - 1);
            }
        }
    }
}
