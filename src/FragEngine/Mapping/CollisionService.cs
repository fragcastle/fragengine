using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Services;
using Microsoft.Xna.Framework;

namespace FragEngine.Mapping
{
    public class CollisionService : ICollisionService
    {
        public Vector2 Check(Vector2 currentPosition, Vector2 newPosition)
        {
            return newPosition;
        }
    }
}
