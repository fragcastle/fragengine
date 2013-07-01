using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Mapping;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Xunit;

namespace FragEngine.Tests.Mapping
{
    public class CollisionDetectorTests
    {

        private Vector2 _startPosition = new Vector2( 100, 100 );
        private Vector2 _smallVelocity = new Vector2( 5, 5 ); // diagonal velocity
        private Vector2 _size = new Vector2( 64, 64 );


        [Fact]
        public void Check_IncrementsYaxis_IfNoCollisions()
        {
            var detector = new CollisionDetector( CollisionMap.Empty );

            var result = detector.Check( _startPosition, _smallVelocity, _size );

            Assert.Equal( 105f, result.Position.X );
            Assert.Equal( 105f, result.Position.Y );
        }

        [Fact]
        public void Check_DecrementsYaxis_IfNoCollisions()
        {
            var detector = new CollisionDetector( CollisionMap.Empty );

            var result = detector.Check( _startPosition, new Vector2( 0, -2 ), _size );

            Assert.Equal( 100f, result.Position.X );
            Assert.Equal( 98f, result.Position.Y );
        }

        [Fact]
        public void Check_StopsObject_AtCollisionPoint()
        {
            // fake a collision along our path
            var map = CollisionMap.Empty;
            map.TileSize = 16;
            map.MapData[ 1 ] = 1;

            var detector = new CollisionDetector( map );

            var result = detector.Check( Vector2.Zero, new Vector2( 32, 0 ), new Vector2( 8, 8 ) );

            Assert.Equal( 8, result.Position.X );
            Assert.Equal( 0, result.Position.Y );
        }
    }
}
