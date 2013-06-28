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

        public static Action Throttle( Action toThrottle, int wait ) {
            var lastCall = DateTime.Now;
            return () => {
                    if( DateTime.Now.Subtract( lastCall ).TotalMilliseconds >= wait ) {
                        toThrottle();
                        lastCall = DateTime.Now;
                    }
                };
        }

        public static Action<T1, T2> Throttle<T1, T2>( Action<T1, T2> toThrottle, int wait )
        {
            var lastCall = DateTime.Now;
            return (first, second) => {
                if( DateTime.Now.Subtract( lastCall ).TotalMilliseconds >= wait ) {
                    toThrottle(first, second);
                    lastCall = DateTime.Now;
                }
            };
        }
    }
}
