using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FragEngine.Animation;
using FragEngine.Data;
using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Layers
{
    [DataContract]
    public class MapLayer : Layer
    {
        private Vector2 _start = Vector2.Zero;
        private string _tileSetTexturePath;
        protected bool _tileSetTextureIsDirty;
        private int _tileSize;

        // TODO make this a dictionary and static so each map layer caches the animation
        // rectangles for each texture
        public List<Rectangle> TextureFrameMap { get; set; }

        [DataMember]
        public virtual string TileSetTexturePath
        {
            get
            {
                return _tileSetTexturePath;
            }
            set
            {
                _tileSetTexturePath = value;

                if( !String.IsNullOrWhiteSpace( value ) )
                {
                    // don't load the animation sheet yet as we can't garauntee that the TileSize
                    // is correct, which will throw off our frame calculations.
                    // instead, set a flag that tells draw that we are "dirty" and need to reload
                    // the animation sheet.
                    _tileSetTextureIsDirty = true;
                }
            }
        }

        [DataMember]
        public Map MapData { get; set; }

        [DataMember]
        public virtual int TileSize
        {
            get { return _tileSize; }
            set
            {
                _tileSize = value;

                // if the tilesize changes the tile set texture needs to be reloaded
                // well, not really, but the animation sheet needs to be reloaded
                // and refreshing the tilesheet will do that for us :D
                _tileSetTextureIsDirty = true;
            }
        }

        [IgnoreDataMember]
        public bool DrawGrid { get; set; }

        public virtual Texture2D TileSetTexture { get; set; }

        [IgnoreDataMember]
        public AnimationSheet TileSheet { get; private set; }

        public MapLayer()
            : this( null )
        { }

        public MapLayer( Vector2? parallax, int tileSize = 16, int[] mapData = null )
            : base( parallax )
        {
            MapData = new Map();

            TileSize = tileSize;

            if( mapData != null )
                MapData.Data = mapData;

            DrawMethod = DrawMap;

            SamplerState = SamplerState.PointClamp;

            if( MapData.Data == null || MapData.IsEmpty() )
            {
                MapData.Data = new int[ 45 * 80 ];
                MapData.Width = 80;
                Prefill();
            }
        }

        public void DrawMap( SpriteBatch spriteBatch )
        {
            if( _tileSetTextureIsDirty )
            {
                TileSetTexture = ContentCacheManager.GetTexture( TileSetTexturePath );

                TileSheet = new AnimationSheet( TileSetTexture, TileSize, TileSize );

                // add the animations
                for( int frame = 0; frame < TileSheet.TotalFrames; frame++ )
                {
                    TileSheet.Add( frame, 1f, false, frame );
                }
            }

            if( TileSetTexture != null )
            {
                MapData.EachCell( ( cell, tile ) =>
                {
                    // adjust the vector position according to the tilesize
                    cell *= TileSize;

                    // only draw tiles that have data
                    if( tile > -1 )
                        TileSheet[ tile ].Draw( spriteBatch, cell, Color.White * Opacity );

                    if( DrawGrid )
                    {
                        var frame = new Rectangle( (int)cell.X, (int)cell.Y, TileSize, TileSize );
                        
                        var whiteTexture = new Texture2D( spriteBatch.GraphicsDevice, 1, 1 );
                        whiteTexture.SetData( new Color[] { Color.White } );

                        spriteBatch.Draw( whiteTexture, new Rectangle( frame.Left, frame.Top, frame.Width, 1 ), Color.White );
                        spriteBatch.Draw( whiteTexture, new Rectangle( frame.Left, frame.Bottom, frame.Width, 1 ), Color.White );
                        spriteBatch.Draw( whiteTexture, new Rectangle( frame.Left, frame.Top, 1, frame.Height ), Color.White );
                        spriteBatch.Draw( whiteTexture, new Rectangle( frame.Right, frame.Top, 1, frame.Height + 1 ), Color.White );
                    }
                } );
            }
        }

        public int? GetTile( int x, int y )
        {
            var tx = (int)Math.Floor( (double)x / TileSize );
            var ty = (int)Math.Floor( (double)y / TileSize );

            return MapData.GetTile( new Vector2( tx, ty ) );
        }

        public void SetTile( Vector2 gameWorldPosition, int tileValue )
        {
            // truncate both the values, keeping as floats will produce unexpected tile placements
            var correctedPosition = new Vector2(
                (int)( gameWorldPosition.X / TileSize ),
                (int)( gameWorldPosition.Y / TileSize ) );

            var index = MapData.GetCellIndex( correctedPosition );

            MapData[ index ] = tileValue;
        }

        public void SetTile( int x, int y, int tile )
        {
            var tx = (int)Math.Floor( (double)x / TileSize );
            var ty = (int)Math.Floor( (double)y / TileSize );

            var position = new Vector2( tx, ty );

            MapData[ position ] = tile;
        }

        private void Prefill( int dummy = -1 )
        {
            MapData.EachCell( ( cell, index ) => dummy );
        }
    }
}
