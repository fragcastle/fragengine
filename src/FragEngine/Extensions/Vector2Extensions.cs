using System;
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

        public static Vector2 Floor ( this Vector2 vector )
        {
            return new Vector2( (float)Math.Floor(vector.X), (float)Math.Floor(vector.Y));
        }

        public static Vector2 Ceiling( this Vector2 vector )
        {
            return new Vector2( (float)Math.Ceiling( vector.X ), (float)Math.Ceiling( vector.Y ) );
        }

        public static Vector2 Round( this Vector2 vector, int decimals = 0 )
        {
            return new Vector2( (float)Math.Round( vector.X, decimals ), (float)Math.Round( vector.Y, decimals ) );
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.Y);
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            return new Vector2(vector.X, y);
        }

        public static Vector2 SetX(this Vector2 vector, double x)
        {
            return new Vector2((float)x, vector.Y);
        }

        public static Vector2 SetY(this Vector2 vector, double y)
        {
            return new Vector2(vector.X, (float)y);
        }
    }
}
