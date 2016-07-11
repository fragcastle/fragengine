using System;
using Microsoft.Xna.Framework;
using Should;
using Xunit;

namespace FragEngine.Tests
{
    public class TimerTests
    {
        private void AdvanceTime(GameTime start, int seconds, int fps = 60)
        {
            float frameTime = (1f/fps) * 1000;
            var frames = fps*seconds;
            var time = start;
            for (int i = 0; i < frames; i++)
            {
                time.TotalGameTime.Add(TimeSpan.FromMilliseconds(frameTime));
                time.ElapsedGameTime = TimeSpan.FromMilliseconds(frameTime);
                Timer.Update(time);
            }
        }

        public class Delta : TimerTests
        {
            [Fact]
            public void Should_Increment_As_It_Approaches_Target()
            {
                var timer = new Timer(5);
                var baseTime = new GameTime(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(0));

                timer.Delta().ShouldEqual(-5);

                AdvanceTime(baseTime, 1);

                timer.Delta().ShouldBeInRange(-4f, -3.9f);

                AdvanceTime(baseTime, 1);

                timer.Delta().ShouldBeInRange(-3f, -2.9f);

                AdvanceTime(baseTime, 1);

                timer.Delta().ShouldBeInRange(-2f, -1.9f);

                AdvanceTime(baseTime, 1);

                timer.Delta().ShouldBeInRange(-1f, -0.9f);

                AdvanceTime(baseTime, 1);

                timer.Delta().ShouldBeInRange(0f, 0.9f);
            }
        }
    }
}
