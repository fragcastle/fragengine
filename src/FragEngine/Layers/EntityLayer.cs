﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Layers
{
    // EntityLayer is never serialized. It's only used by the game engine
    public class EntityLayer : Layer
    {
        public EntityLayer( Camera camera, Vector2? parallax = null )
            : base( camera, parallax )
        {
        }

        public List<EntityBase> Entities { get; set; }

        public override void Draw( SpriteBatch spriteBatch )
        {
            base.Draw( spriteBatch );

            foreach( var entity in Entities )
            {
                if( entity.IsAlive )
                {
                    entity.Alpha = Alpha;
                    entity.Draw( spriteBatch );
                }
            }
        }
    }
}
