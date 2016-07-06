using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FragEngine.Entities;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Should;
using Xunit;

namespace FragEngine.Tests.Entities
{
    public class TestGameObject : GameObject
    {
        
    }

    public class GameObjectServiceTests
    {
        private readonly IGameObjectService _gameObjectService;

        public GameObjectServiceTests()
        {
            _gameObjectService = new GameObjectService();
        }

        public class GetGameObjectsByType : GameObjectServiceTests
        {
            [Fact]
            public void Should_Return_An_Empty_Array_If_Type_Is_Not_Found()
            {
                var gos = _gameObjectService.GetGameObjectsByType(typeof (TestGameObject));
                gos.Count.ShouldEqual(0);
            }

            [Fact]
            public void Should_Return_GameObjects_If_Found()
            {
                _gameObjectService.SpawnGameObject<TestGameObject>(Vector2.One);
                _gameObjectService.SpawnGameObject<TestGameObject>(Vector2.One);
                var gos = _gameObjectService.GetGameObjectsByType(typeof(TestGameObject));
                gos.Count.ShouldEqual(2);
            }
        }

        public class GetGameObjectByName : GameObjectServiceTests
        {
            [Fact]
            public void Should_Return_Null_If_GameObject_Is_Not_Found()
            {
                var go = _gameObjectService.GetGameObjectByName("name");
                go.ShouldBeNull();
            }

            [Fact]
            public void Should_Return_GameObject_If_Found()
            {
                var go = _gameObjectService.SpawnGameObject<TestGameObject>(Vector2.One, tgo => tgo.Name = "name");
                var fgo = _gameObjectService.GetGameObjectByName("name");
                fgo.ShouldNotBeNull().ShouldEqual(go);
            }
        }

        public class SpawnGameObject : GameObjectServiceTests
        {
            [Fact]
            public void Should_Add_GameObject_To_GameObjects_List()
            {
                _gameObjectService.GameObjects.Count.ShouldEqual(0);
                _gameObjectService.SpawnGameObject<TestGameObject>(Vector2.One);
                _gameObjectService.GameObjects.Count.ShouldEqual(1);
            }

            [Fact]
            public void Should_Return_The_GameObject()
            {
                var go = _gameObjectService.SpawnGameObject<TestGameObject>(Vector2.One);
                _gameObjectService.GameObjects[0].ShouldEqual(go);
            }

            [Fact]
            public void Should_Add_GameObject_To_DrawQueue()
            {
                var go = _gameObjectService.SpawnGameObject<TestGameObject>(Vector2.One, tgo =>
                {
                    tgo.ZIndex = 100;
                });
                _gameObjectService.DrawQueues.Keys.ShouldContain(10);
                _gameObjectService.DrawQueues[10].ShouldContain(go);
                _gameObjectService.DrawQueues[10][0].ShouldEqual(go);
            }
        }
    }
}
