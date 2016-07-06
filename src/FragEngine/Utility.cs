using System;
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

        public static int RndRange(int min = 0, int max = 1)
        {
            var r = new Random();
            return r.Next(min, max);
        }

        public static float RndRange(float min = 0, float max = 1)
        {
            var r = new Random();
            return (float)(r.NextDouble() * (max - min) + min);
        }

        public static T Random<T>(T[] arr)
        {
            var idx = RndRange(0, arr.Length - 1);
            return arr[idx];
        }

        public static Vector2 RndPos(Vector2 min, Vector2 max, Vector2? offset = null)
        {
            if (!offset.HasValue)
            {
                offset = Vector2.Zero;
            }
            return new Vector2(offset.Value.X + RndRange(min.X, max.X), offset.Value.Y + RndRange(min.Y, max.Y));
        }

        public static bool CoinFlip()
        {
            var x = RndRange(0, 100);
            return x >= 50;
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
