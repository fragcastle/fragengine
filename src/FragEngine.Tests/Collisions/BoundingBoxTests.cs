using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            Assert.Equal( _box.Height, 0 );
            Assert.Equal( _box.Width, 0 );

            _box += vector;

            Assert.Equal( _box.Height, 100 );
            Assert.Equal( _box.Width, 200 );
        }

        [Fact]
        public void CanBe_AddedWith_Rectangle()
        {
            var rectangle = new Rectangle( 0, 0, 200, 100 );

            Assert.Equal( _box.Height, 0 );
            Assert.Equal( _box.Width, 0 );

            _box += rectangle;

            Assert.Equal( _box.Height, 100 );
            Assert.Equal( _box.Width, 200 );
        }

        [Fact]
        public void ShouldBe_AssignableFrom_Vector2()
        {
            var vector = new Vector2( 200, 100 );
            
            _box = (HitBox)vector;

            Assert.Equal( _box.Height, 100 );
            Assert.Equal( _box.Width, 200 );
        }

        [Fact]
        public void ShouldBe_AssignableFrom_Rectangle()
        {
            var rectangle = new Rectangle( 0, 0, 200, 100 );

            _box = (HitBox)rectangle;

            Assert.Equal( _box.Height, 100 );
            Assert.Equal( _box.Width, 200 );
        }
    }
}
