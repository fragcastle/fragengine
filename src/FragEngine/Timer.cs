using Microsoft.Xna.Framework;
using System;

namespace FragEngine
{
    public class Timer
    {
        private static long last;
        private static long current;
        private static float time;
        private static float maxStep = 0.05f;
        private static float timeScale = 1f;

        public static float TimeScale
        {
            get
            {
                return timeScale;
            }
        }

        public float Base { get; private set; }
        public float Last { get; private set; }
        public float Target { get; private set; }
        public float PausedAt { get; private set; }

        public Timer( float seconds = 0f )
        {
            Base = Timer.time;
            Last = Timer.time;

            Target = seconds;
        }

        public void Set(float seconds = 0f)
        {
            Target = seconds;
            Base = Timer.time;
            PausedAt = 0;
        }

        public void Reset()
        {
            Base = Timer.time;
            PausedAt = 0;
        }

        public float Tick()
        {
            var delta = Timer.time - Last;
            Last = Timer.time;
            return (PausedAt == 0 ? 0 : delta);
        }

        public float Delta()
        {
            var start = PausedAt == 0 ? Timer.time : PausedAt;
            return start - Base - Target;
        }

        public static void Update( GameTime gameTime )
        {
            var current = DateTime.Now.SinceEpoch();
            var delta = (current - last) / 1000;
            time += Math.Min(delta, maxStep) * timeScale;
            last = current;
        }
    }
}
