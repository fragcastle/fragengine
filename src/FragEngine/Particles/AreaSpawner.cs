using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FragEngine.Entities;
using FragEngine.Services;
using Microsoft.Xna.Framework;

namespace FragEngine.Particles
{
    public class AreaSpawner : GameObject
    {
        private readonly IGameObjectService _gameObjectService;

        public AreaSpawner() : this(Vector2.Zero)
        {
            
        }
        public AreaSpawner(Vector2 initialPosition, IGameObjectService gameObjectService = null)
        {
            if (SpawnDelayTime == 0f)
            {
                SpawnDelayTime = 0.05f;
            }
            SpawnDelay = new Timer(SpawnDelayTime);

            _gameObjectService = gameObjectService ?? ServiceLocator.Get<IGameObjectService>();
        }

        public Action<Particle> ParticleOptions { get; set; }

        public float SpawnDelayTime { get; set; }

        public Timer SpawnDelay { get; set; }

        public int Particles { get; set; }

        public override void ReceiveDamage(GameObject @from, int amount)
        {
            // invincible
        }

        public override void Update(GameTime gameTime)
        {
            if (SpawnDelay.Delta() >= 0)
            {
                for (var i = 0; i < Particles; i++)
                {
                    var x = Utility.RndRange(Position.X, Position.X + BoundingBox.Width);
                    var y = Utility.RndRange(Position.Y, Position.Y + BoundingBox.Height);

                    _gameObjectService.SpawnGameObject(new Vector2(x, y), ParticleOptions);
                }
                SpawnDelay.Reset();
            }
        }
    }
}
