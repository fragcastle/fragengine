using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Layers
{
    public sealed class CollisionLayer : MapLayer
    {
        private readonly Texture2D _collisionTiles;

        [IgnoreDataMember]
        public override string TileSetTexturePath
        {
            get
            {
                return @"FragEngine.Resources.collision_tiles.png";
            }
            set
            {

            }
        }

        [IgnoreDataMember]
        public override string Name
        {
            get
            {
                return "Collision";
            }
            set
            {

            }
        }

        [IgnoreDataMember]
        public override Texture2D TileSetTexture
        {
            get
            {
                return _collisionTiles;
            }
            set
            {

            }
        }

        [IgnoreDataMember]
        public override int TileSize
        {
            get
            {
                return 16;
            }
            set
            {

            }
        }

        public CollisionLayer()
            : base()
        {
            _collisionTiles = ContentCacheManager.GetTextureFromResource( TileSetTexturePath );

            _tileSetTextureIsDirty = true;
        }

        public override string ToString()
        {
            return "Collision";
        }
    }
}
