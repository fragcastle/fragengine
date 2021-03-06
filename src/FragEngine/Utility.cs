﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FragEngine {
    public static class Utility {
        private static readonly Random random = new Random();

        public static float Clerp( float from, float to, float step ) {
            float t = ( ( MathHelper.WrapAngle( to - from ) * ( step ) ) );
            return from + t;
        }

        /// <summary>
        /// Helper to generate a random float in the range of [-1, 1].
        /// </summary>
        public static float NextFloat() {
            return (float)random.NextDouble() * 2f - 1f;
        }

        public static float Limit( float target, float min, float max )
        {
            return Math.Min( max, Math.Max( min, target ) );
        }

        public static int Limit( int target, int min, int max )
        {
            return Math.Min( max, Math.Max( min, target ) );
        }

        public static double Limit( double target, double min, double max )
        {
            return Math.Min( max, Math.Max( min, target ) );
        }

        public static Vector2 Limit( Vector2 target, Vector2 Range)
        {
            return new Vector2( Limit( target.X, -Range.X, Range.X ), Limit( target.Y, -Range.Y, Range.Y ) );
        }
    }
}
