using FragEngine.Collisions;
using Microsoft.Xna.Framework;
using Xunit;

namespace FragEngine.Tests.Collisions
{
    public class HitBoxTests
    {
        private HitBox _box;

        public HitBoxTests()
        {
            _box = new HitBox();
        }

        [Fact]
        public void CanBe_AddedWith_Vector2()
        {
            var vector = new Vector2( 200, 100 );

            Assert.Equal( 0, _box.Height );
            Assert.Equal( 0, _box.Width );

            _box += vector;

            Assert.Equal( 100, _box.Height );
            Assert.Equal( 200, _box.Width );
        }

        [Fact]
        public void CanBe_AddedWith_Rectangle()
        {
            Rectangle rectangle = new Rectangle(0, 0, 200, 100);

            _box.Height = 100;
            _box.Width = 200;

            _box += rectangle;

            Assert.Equal( 100, rectangle.Height );
            Assert.Equal( 200, rectangle.Width );
        }

        [Fact]
        public void CanBe_SubtractedWith_Vector2()
        {
            var vector = new Vector2( 100, 50 );

            _box.Height = 100;
            _box.Width = 200;

            _box -= vector;

            Assert.Equal( 50, _box.Height );
            Assert.Equal( 100, _box.Width );
        }

        [Fact]
        public void CanBe_SubtractedWith_Rectangle()
        {
            Rectangle rectangle = new Rectangle(0, 0, 100, 50 );

            _box.Height = 100;
            _box.Width = 200;

            _box -= rectangle;

            Assert.Equal( 50, rectangle.Height );
            Assert.Equal( 100, rectangle.Width );
        }

        [Fact]
        public void CanBe_AssignedTo_Vector2()
        {
            Vector2 vector;

            _box.Height = 100;
            _box.Width = 200;

            vector = _box;

            Assert.Equal( 100, vector.Y );
            Assert.Equal( 200, vector.X );
        }

        [Fact]
        public void CanBe_AssignedTo_Rectangle()
        {
            Rectangle rectangle;

            _box.Height = 100;
            _box.Width = 200; 
            
            rectangle = _box;

            Assert.Equal( 100, rectangle.Height );
            Assert.Equal( 200, rectangle.Width );
        }
        
        [Fact]
        public void ShouldBe_AssignableFrom_Vector2()
        {
            var vector = new Vector2( 200, 100 );
            
            _box = (HitBox)vector;

            Assert.Equal( 100, _box.Height );
            Assert.Equal( 200, _box.Width );
        }

        [Fact]
        public void ShouldBe_AssignableFrom_Rectangle()
        {
            var rectangle = new Rectangle( 0, 0, 200, 100 );

            _box = (HitBox)rectangle;

            Assert.Equal( 100, _box.Height );
            Assert.Equal( 200, _box.Width );
        }
    }
}
