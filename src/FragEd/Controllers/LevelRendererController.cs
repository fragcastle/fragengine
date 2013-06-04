using System.Collections.Generic;
using System.Windows.Forms;
using FragEd.Controls;
using FragEd.Forms;
using FragEngine.Layers;
using FragEngine.View;
using Microsoft.Xna.Framework;

namespace FragEd.Controllers
{
    public class LevelRendererController : ControllerBase
    {
        private Dictionary<Layer, int> _layerTileMap;        

        public LevelRendererController( LevelEditorControl control )
        {
            _layerTileMap = new Dictionary<Layer, int>();

            ListenTo(control);
        }

        public void ListenTo( LevelEditorControl editor )
        {
            editor.MouseMove += EditorOnMouseMove;
            editor.MouseDown += EditorOnMouseDown;
        }

        private void EditorOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            // figure out what we're doing.
            var currentNode = GetSelectedNode();

            if( currentNode == null ) return;

            var position = new Vector2( mouseEventArgs.X, mouseEventArgs.Y );

            // Are we placing a tile?
            var layer = GetLayerFromNode( currentNode );
            if( layer != null )
            {
                if( !_layerTileMap.ContainsKey( layer ) )
                {
                    _layerTileMap.Add(layer, 0); // the first tile is the default
                }

                var lastTile = _layerTileMap[ layer ];

                layer.SetTile( position, lastTile );

                var level = ( (LevelEditorControl)sender ).Level;
                level.SetDirty();
            }

            // Are we selecting an Entity?

        }

        private void EditorOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            //var currentNode = GetSelectedNode();

            //if( mouseEventArgs.Button.HasFlag( MouseButtons.Left ) )
            //{
            //    // user has an entity selected, move the entity
            //    var entity = GetEntityFromNode( currentNode );
            //    if( entity != null )
            //    {
            //        entity.Position = new Vector2( mouseEventArgs.X, mouseEventArgs.Y );

            //        var level = ( (LevelEditorControl)sender ).Level;
            //        level.SetDirty();
            //    }
            //}
        }
    }
}
