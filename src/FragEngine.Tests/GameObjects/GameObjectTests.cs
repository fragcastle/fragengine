using FragEngine.Collisions;
using FragEngine.GameObjects;
using Microsoft.Xna.Framework;
using Should;
using Xunit;

namespace FragEngine.Tests.GameObjects
{

    public class GameObjectTests
    {

        private class BasicGameObject : GameObject
        {
            public BasicGameObject()
            {
                BoundingBox = new FragEngine.Collisions.HitBox { Height = 1, Width = 1 };
            }
        }

        private class GameObject1 : BasicGameObject { }
        private class GameObject2 : BasicGameObject { }

        public class HandleMovementTrace : GameObjectTests
        {
            public void Should_Set_Position_To_Result_Position()
            {
                var go = new BasicGameObject();
                go.Position.ShouldEqual(new Vector2(0, 0));
                go.HandleMovementTrace(new CollisionCheckResult()
                {
                    Position = Vector2.One,
                    XAxis = false,
                    YAxis = true
                });

                go.Position.ShouldEqual(new Vector2(1, 1));
            }
        }

        [Fact]
        public void Should_Be_Able_To_Determine_Distance_To_Another_Entity()
        {
            var entityA = new GameObject1();
            var entityB = new GameObject2();

            entityA.Position = new Vector2( 100, 0 );

            entityA.DistanceTo( entityB ).ShouldEqual( 100 );

            entityA.Position = new Vector2( 0, 200 );

            entityA.DistanceTo( entityB ).ShouldEqual( 200 );
        }
    }
}
