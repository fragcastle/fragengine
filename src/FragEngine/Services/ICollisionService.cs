using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using FragEngine.Mapping;
using Microsoft.Xna.Framework;

namespace FragEngine.Services
{
    public interface ICollisionService
    {
        CollisionCheckResult Check( Vector2 currentPosition, Vector2 positionDelta, Vector2 objectSize );
    }
}
