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

        [Fact]
        public void Check_StopsObject_EvenIfItIsMovingLessThanOnePixel()
        {
            var map = CollisionMap.Empty;
            map.TileSize = 16;
            map.MapData.Width = 4;

            // create a fake map
            // 0,  0,  0,  0,
            // 0, -1, -1,  0,
            // 0, -1, -1,  0,
            // 0,  0,  0,  0
            map.MapData[  0 ] = 0; map.MapData[  1 ] =  0; map.MapData[  2 ] =  0; map.MapData[   3 ] = 0;
            map.MapData[  4 ] = 0; map.MapData[  5 ] = -1; map.MapData[  6 ] = -1; map.MapData[   7 ] = 0;
            map.MapData[  8 ] = 0; map.MapData[  9 ] = -1; map.MapData[ 10 ] = -1; map.MapData[  11 ] = 0;
            map.MapData[ 12 ] = 0; map.MapData[ 13 ] =  0; map.MapData[ 14 ] =  0; map.MapData[  15 ] = 0;

            var detector = new CollisionDetector( map );

            // now, simulate a collision where the entity wants to travel 0.32000000023f on X and 4.8f on Y
            var result = detector.Check( new Vector2( 16, 16 ), new Vector2( -0.3200000023f, 4.8f ), new Vector2( 64, 64 ) );

            // X position should be pushed back (entity was in a bad spot before)
            Assert.Equal( 32f, result.Position.X );

            // Y should increase as expected
            Assert.Equal( 20.8f, result.Position.Y );
        }
    }
}
