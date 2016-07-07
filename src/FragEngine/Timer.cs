using Microsoft.Xna.Framework;
using System;

namespace FragEngine
{
    public class Timer
    {
        private static float _time;
        private const float MaxStep = 0.05f;

        [Obsolete("Use FragEngineGame.TimeScale instead. This property may be removed in future releases.")]
        public static float TimeScale => FragEngineGame.TimeScale;

        public float Base { get; private set; }
        public float Last { get; private set; }
        public float Target { get; private set; }
        public float PausedAt { get; private set; }

        public Timer(float seconds = 0f)
        {
            Base = _time;
            Last = _time;

            Target = seconds;
        }

        public void Set(float seconds = 0f)
        {
            Target = seconds;
            Base = _time;
            PausedAt = 0;
        }

        public void Reset()
        {
            Base = _time;
            PausedAt = 0;
        }

        public float Tick()
        {
            var delta = _time - Last;
            Last = _time;
            return (PausedAt == 0 ? 0 : delta);
        }

        public float Delta()
        {
            var start = PausedAt == 0 ? _time : PausedAt;
            return start - Base - Target;
        }

        public static void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time += Math.Min(delta, MaxStep) * FragEngineGame.TimeScale;
        }
    }
}
