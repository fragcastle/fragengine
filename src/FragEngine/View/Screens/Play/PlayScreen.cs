using System;
using System.Collections.Generic;
using System.Linq;
using FragEngine.Content;
using FragEngine.Data;
using FragEngine.Debug;
using FragEngine.GameObjects;
using FragEngine.Layers;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.View.Screens.Play
{
    public class PlayScreen : GameScreenBase
    {
        protected SpriteBatch _spriteBatch;

        protected SpriteFont _gameFont;

        protected Camera _camera;

        private const int CellSize = 64;

        private readonly IGameObjectService _gameObjectService = null;

        public PlayScreen( IGameObjectService gameObjectService = null, Camera camera = null )
        {
            _camera = camera ?? ServiceLocator.Get<Camera>();
            _gameObjectService = gameObjectService ?? ServiceLocator.Get<IGameObjectService>();
            Decorations = new List<Decoration>();
            Layers = new List<Layer>();
        }

        public List<Layer> Layers { get; set; } 

        public virtual void Initialize()
        {
#if DEBUG
            Decorations.Add(new DebugDecoration());
#endif
        }

        public void LoadLevel( string levelName )
        {
            var path = $"Levels\\{levelName}.tmx";
            var cache = ServiceLocator.Get<IReadableContentCache>();

            CurrentLevel = cache.GetContent<Level>(path);
            
            // CurrentLevel = Level.Load(levelName);

            // CurrentLevel = Level.Load( new FileInfo( path ) );

            // _gameObjectService.AttachGameObjects(CurrentLevel.GameObjects.ToArray());

            // replace the collision service with one setup for this level
            //var collisionMap = new CollisionMap( CurrentLevel );
            //var collisionService = new CollisionService( collisionMap );

            //ServiceLocator.Add<ICollisionService>( collisionService );
        }

        public List<Decoration> Decorations { get; set; }

        public Level CurrentLevel { get; private set; }

        private void CheckGameObjects()
        {
            // Insert all entities into a spatial hash and check them against any
            // other entity that already resides in the same cell. Entities that are
            // bigger than a single cell, are inserted into each one they intersect
            // with.

            // A list of entities, which the current one was already checked with,
            // is maintained for each entity.
            var hash = new Dictionary<int, Dictionary<int, GameObject[]>>();
            foreach (var gameObject in _gameObjectService.GameObjects.ToList())
            {
                if (gameObject.Group == GameObjectGroup.None &&
                    gameObject.CheckAgainstGroup == GameObjectGroup.None &&
                    gameObject.CollisionStyle == GameObjectCollisionStyle.Never)
                {
                    continue;
                }

                var checkedGameObjectIds = new List<Guid>();
                var xmin = (int)Math.Floor(gameObject.Position.X/CellSize);
                var ymin = (int)Math.Floor(gameObject.Position.Y/CellSize);
                var xmax = (int)Math.Floor((gameObject.Position.X + gameObject.BoundingBox.Width)/CellSize) + 1;
                var ymax = (int)Math.Floor((gameObject.Position.Y + gameObject.BoundingBox.Height)/CellSize) + 1;

                for (var x = xmin; x < xmax; x++)
                {
                    for (var y = ymin; y < ymax; y++)
                    {
                        // Current cell is empty - create it and insert!
                        if (!hash.ContainsKey(x))
                        {
                            hash[x] = new Dictionary<int, GameObject[]>();
                            hash[x].Add(y, new [] { gameObject });
                        }
                        else if (!hash[x].ContainsKey(y))
                        {
                            hash[x].Add(y, new[] {gameObject});
                        }
                        // Check against each entity in this cell, then insert
                        else
                        {
                            var cell = hash[x][y];
                            for (var c = 0; c < cell.Length; c++)
                            {
                                // Intersects and wasn't already checkd?
                                if (gameObject.Touches(cell[c]) && !checkedGameObjectIds.Contains(cell[c].Id))
                                {
                                    checkedGameObjectIds.Add(cell[c].Id);
                                    GameObject.CheckPair(gameObject, cell[c]);
                                }
                            }
                            var newCell = new GameObject[cell.Length + 1];
                            cell.CopyTo(newCell, 0);
                            newCell[newCell.Length - 1] = gameObject;
                            hash[x][y] = newCell;
                        }
                    }
                }

            }
        }

        public override void LoadContent()
        {
            Initialize();

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch( ScreenManager.GraphicsDevice );

            base.LoadContent();
        }

        public override void Draw( GameTime gameTime )
        {
            _camera.Update();

            foreach (Layer layer in Layers)
            {
                layer.Alpha = TransitionAlpha;
                layer.Draw( _spriteBatch );
            }

            if (Decorations.Count > 0)
            {
                Decorations.Sort((a, b) => a.ZIndex < b.ZIndex ? -1 : 1);
                Decorations.ForEach(d => d.Draw(gameTime));
            }   
        }

        public override void Update( GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen )
        {
            // copy the GameObjects collection in case it is modified while we
            // are updating the objects
            // TODO: maybe don't update these objects inside the screen? This could be expensive if we have 10k objects
            foreach (var gameObject in _gameObjectService.GameObjects.ToList())
            {
                if (gameObject.IsAlive)
                {
                    gameObject.Update(gameTime);
                }
            }
            CheckGameObjects();

            _gameObjectService.CleanUp();

            base.Update( gameTime, otherScreenHasFocus, coveredByOtherScreen );
        }
    }
}
