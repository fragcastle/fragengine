using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using Microsoft.Xna.Framework;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace FragEngine.Tests.Entities
{

    public class EntityTests
    {

        private readonly ITestOutputHelper output;

        public EntityTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private class GameObject1 : GameObject { }
        private class GameObject2 : GameObject { }

        [Fact]
        public void Should_Be_Able_To_Determine_Distance_To_Another_Entity()
        {
            var entityA = new GameObject1()
            {
                BoundingBox = new FragEngine.Collisions.HitBox
                {
                    Height = 1,
                    Width = 1
                }
            };
            var entityB = new GameObject2()
            {
                BoundingBox = new FragEngine.Collisions.HitBox
                {
                    Height = 1,
                    Width = 1
                }
            };

            entityA.Position = new Vector2( 100, 0 );

            entityA.DistanceTo( entityB ).ShouldEqual( 100 );

            entityA.Position = new Vector2( 0, 200 );

            entityA.DistanceTo( entityB ).ShouldEqual( 200 );
        }
    }
}
