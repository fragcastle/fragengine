using System;
using System.Collections.Generic;
using System.Linq;
using FragEngine.Data;
using FragEngine.Layers;
using Microsoft.Xna.Framework.Graphics;
using Should;
using Xunit;

namespace FragEngine.Tests.Data
{
    public class FakeLayer : Layer
    {
        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
    public class LevelTests
    {
        public class Layers : LevelTests
        {
            [Fact]
            public void Should_Reorder_Layers_When_Layers_Is_Retrieved()
            {
                var level = new Level();
                level.Layers = new List<Layer>()
                {
                    new FakeLayer() { Order = 100 },
                    new FakeLayer() { Order = 1 }
                };

                level.Layers.First().Order.ShouldEqual(1);
            }
        }
    }
}
