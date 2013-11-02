using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using Microsoft.Xna.Framework;

namespace FragEngine.Mapping
{
    public class CollisionDetector
    {

        private GameObject _gameObject;
        private CollisionMap _map;

        private Vector2 _positionDelta;
        private Vector2 _finalPosition;
        private Vector2 _currentPosition;
        private Vector2 _objectSize;

        public CollisionCheckResult Result { get; private set; }

        public CollisionDetector( CollisionMap map )
        {
            _map = map;
        }

        public CollisionCheckResult Check( Vector2 currentPosition, Vector2 positionDelta, Vector2 objectSize  )
        {
            Result = new CollisionCheckResult() { Position = currentPosition };

            _currentPosition = currentPosition.Ceiling();
            _positionDelta = positionDelta.Ceiling();
            _objectSize = objectSize.Ceiling();

            _finalPosition = new Vector2( _positionDelta.X, _positionDelta.Y );

            // short-circuit if there is nothing to do
            if( positionDelta == Vector2.Zero )
                return Result;

            // determine if we need to perform multiple checks, e.g. the entitys proposed
            // position is more than one tile away.
            var steps = (float)Math.Ceiling( Math.Max( Math.Abs( _positionDelta.X ), Math.Abs( _positionDelta.Y ) ) / _map.TileSize );

            if( steps > 1 )
            {
                var stepDelta = _positionDelta / steps;

                if( stepDelta.X > 0 || stepDelta.Y > 0 )
                {
                    for( var i = 0; i < steps; i++ )
                    {
                        PeekStep( stepDelta, i );

                        // if the trace finds a collision anywhere along the way, zero out the offending part of the vector
                        if( Result.XAxis )
                        {
                            stepDelta.X = 0;
                            positionDelta.X = 0;
                        }
                        if( Result.YAxis )
                        {
                            stepDelta.Y = 0;
                            positionDelta.Y = 0;
                        }
                    }
                }
            }
            else
            {
                PeekStep( positionDelta, 0f );
            }

            return Result;
        }

        private void PeekStep( Vector2 stepDelta, float stepIndex )
        {
            var resultPosition = Result.Position + stepDelta;

            var t = 0;

            var mapHeight = _map.MapData.Data.Length / _map.MapData.Width;

            if( stepDelta.X != 0f )
            {
                var pxOffsetX = ( stepDelta.X > 0 ? _objectSize.X : 0 );
                var tileOffsetX = ( stepDelta.X < 0 ? _map.TileSize : 0 );

                var firstTileY = (int)Math.Max( Math.Floor( _currentPosition.Y / _map.TileSize ), 0 );
                var lastTileY = (int)Math.Min( Math.Ceiling( ( _currentPosition.Y + _objectSize.Y ) / _map.TileSize ), mapHeight );
                var tileX = (int)Math.Floor( ( resultPosition.X + pxOffsetX ) / _map.TileSize );

                if( tileX >= 0 && tileX < _map.MapData.Width )
                {
                    for( var tileY = firstTileY; tileY < lastTileY; tileY++ )
                    {
                        t = _map.MapData.GetTile( new Vector2( tileX, tileY ) );

                        if( t != -1 )
                        {
                            // full tile collision!

                            Result.XAxis = true;
                            resultPosition.X = tileX * _map.TileSize - pxOffsetX + tileOffsetX;

                            // assign the new result X to the currentPosition
                            // so our checks against vertical collision will
                            // start from the position that the character /will/
                            // be at.
                            _currentPosition = resultPosition;
                            break;
                        }
                    }
                }
            }

            if( stepDelta.Y != 0f )
            {
                var pxOffsetY = ( stepDelta.Y > 0 ? _objectSize.Y : 0 );
                var tileOffsetY = ( stepDelta.Y < 0 ? _map.TileSize : 0 );

                var firstTileX = (int)Math.Max( Math.Floor( _currentPosition.X / _map.TileSize ), 0 );
                var lastTileX = (int)Math.Min( Math.Ceiling( ( _currentPosition.X + _objectSize.X ) / _map.TileSize ), _map.MapData.Width );
                var tileY = (int)Math.Floor( ( resultPosition.Y + pxOffsetY ) / _map.TileSize );

                if( tileY >= 0 && tileY < mapHeight )
                {
                    for( var tileX = firstTileX; tileX < lastTileX; tileX++ )
                    {
                        t = _map.MapData.GetTile( new Vector2( tileX, tileY ) );

                        if( t != -1 )
                        {
                            // full tile collision!

                            Result.YAxis = true;
                            resultPosition.Y = tileY * _map.TileSize - pxOffsetY + tileOffsetY;

                            _currentPosition = resultPosition;
                            break;
                        }
                    }
                }
            }

            Result.Position = resultPosition;
        }
    }
}
