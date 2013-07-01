using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FragEngine.Mapping
{
    public class CollisionCheckResult
    {
        public bool YAxis { get; set; }
        public bool XAxis { get; set; }
        public Vector2 Position { get; set; }
    }
}
