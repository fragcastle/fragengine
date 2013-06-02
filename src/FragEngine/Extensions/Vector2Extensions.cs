using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// stick these at the root namespace
namespace FragEngine
{
    public static class Vector2Extensions
    {
        public static Vector2 ToAbsoluteValue( this Vector2 vector )
        {
            return new Vector2( Math.Abs(vector.X), Math.Abs(vector.Y) );
        }
    }
}
