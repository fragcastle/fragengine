using Should;
using Xunit;

namespace FragEngine.Tests
{
    public class FragEngineGameTests
    {
        [Fact]
        public void Should_Create_A_Graphics_Device_When_Instantiated()
        {
            var game = new TestGame();
            game.GraphicsDevice.ShouldNotBeNull();
        }
    }
}
