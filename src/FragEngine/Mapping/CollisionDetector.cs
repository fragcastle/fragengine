using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using Microsoft.Xna.Framework;

namespace FragEngine.Mapping
{
    public class CollisionDetector : IDisposable
    {

        private EntityBase _entity;
        private CollisionMap _map;

        private Vector2 _positionDelta;
        private Vector2 _finalPosition;
        private Vector2 _entityPosition;

        public CollisionCheckResult Result { get; private set; }

        public CollisionDetector( EntityBase entity, Vector2 positionDelta, CollisionMap map )
        {
            Result = new CollisionCheckResult();

            _entity = entity;

            // positionDelta is the change in position from the entitys current position.
            _positionDelta = positionDelta;
            _map = map;

            // copy the entitys position otherwise when we update this in Check() the entity will actually move o_O
            _entityPosition = entity.Position;
        }

        public CollisionCheckResult Check()
        {
            var collision = new CollisionCheckResult();

            _finalPosition = new Vector2( _positionDelta.X, _positionDelta.Y );

            // determine if we need to perform multiple checks, e.g. the entitys proposed
            // position is more than one tile away.
            var steps = (float)Math.Ceiling( Math.Max( Math.Abs( _entity.Position.X ), Math.Abs( _entity.Position.Y ) ) / _map.TileSize );

            if( steps > 1 )
            {
                var stepDelta = _positionDelta / steps;

                if( stepDelta.X > 0 || stepDelta.Y > 0 )
                {
                    for( var i = 0; i < steps; i++ )
                    {
                        collision = PeekStep( stepDelta, i );

                        // "move" the entity along the path
                        collision.Position = _entityPosition = _finalPosition;

                        // if the trace finds a collision anywhere along the way, zero out the offending part of the vector
                        if( collision.XAxis )
                        {
                            stepDelta.X = 0;
                            _positionDelta.X = 0;
                        }
                        if( collision.YAxis )
                        {
                            stepDelta.Y = 0;
                            _positionDelta.Y = 0;
                        }
                    }
                }
            }
            else
            {
                collision = PeekStep( _positionDelta, 0f );
            }

            Result = collision;

            return Result;
        }

        public void Dispose()
        {

        }

        private CollisionCheckResult PeekStep( Vector2 stepDelta, float stepIndex )
        {
            _finalPosition += stepDelta;

            var correctedPos = _finalPosition / _map.TileSize; // convert gameworld pos to map pos

            var tileIndex = _map.MapData.GetCellIndex( correctedPos );


            if( IsMovingHorizontally() )
            {

            }

            if( IsMovingHorizontally() )
            {

            }

            return new CollisionCheckResult();
        }

        private bool IsMovingHorizontally()
        {
            return _positionDelta.X != 0f;
        }

        private bool IsMovingVertically()
        {
            return _positionDelta.Y != 0f;
        }
    }
}
