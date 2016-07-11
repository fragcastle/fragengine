using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FragEngine;
using FragEngine.Animation;
using FragEngine.Collisions;
using FragEngine.GameObjects;
using Microsoft.Xna.Framework;

namespace JumpJoy.Entities
{
    public class Crate : GameObject
    {
        public Crate()
        {
            Group = GameObjectGroup.B;
            CheckAgainstGroup = GameObjectGroup.A;
            CollisionStyle = GameObjectCollisionStyle.Active;

            Animations = new AnimationSheet(ContentCacheManager.GetTextureFromResource(@"JumpJoy.Resources.crate.png", typeof(Crate).Assembly), 12, 12);

            Animations.Add("idle", 1f, false, 1);

            Friction = new Vector2(0, 0);
            Acceleration = Vector2.Zero;
            MaxVelocity = new Vector2(150, 180);

            BoundingBox = new HitBox { Height = 8, Width = 12 };
            Offset = new Vector2(0, 4);

            GravityFactor = 10f;
        }
    }
}
