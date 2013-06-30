using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FragEngine.Services
{
    public interface ICollisionService
    {
        Vector2 Check( Vector2 currentPosition, Vector2 newPosition );
    }
}
