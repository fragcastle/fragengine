using System;
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
        public EntityLayer( Vector2? parallax = null )
            : base( parallax )
        {
            DrawMethod = DrawEntities;
        }

        public List<Entity> Entities = new List<Entity>();

        public void DrawEntities( SpriteBatch spriteBatch )
        {
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
