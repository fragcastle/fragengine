using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FragEd.Controls;
using FragEd.Data;
using FragEngine;
using FragEngine.Animation;
using FragEngine.Data;
using FragEngine.Entities;
using FragEngine.IO;
using FragEngine.Layers;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FragEd.Forms
{
    // TODO: Remove Level
    // TODO: Undo changes
    // TODO: organize this massive file
    public partial class LevelEditorForm : Form
    {
        private Project _project;
        private Dictionary<Layer, int> _layerTileMap = new Dictionary<Layer, int>();

        public Level CurrentLevel
        {
            get
            {
                return ux_LevelEditor.Level;
            }
        }

        // statusbar variables
        private ToolStripStatusLabel _zoomText;
        private int[] _zoomLevels = new int[] { 50, 100, 200, 300, 400, 600, 800, 1200, 1400, 1600 };
        private bool _showGrid;

        protected string CurrentProjectFile { get; set; }

        public LevelEditorForm( Level level )
        {
            InitializeComponent();

            EditLevel( level );

            ux_LayerList.ItemCheck += ( sender, args ) => {
                var name = (string)ux_LayerList.Items[ args.Index ];
                if( name == "Collision" )
                    ux_LevelEditor.Level.CollisionLayer.Alpha = args.NewValue == CheckState.Checked ? 255f : 0f;
                else
                    ux_LevelEditor.Level.MapLayers.First( ml => ml.Name == name ).Alpha = args.NewValue == CheckState.Checked ? 255f : 0f;

            };
            ux_LayerList.SelectedIndexChanged += ( sender, args ) => {
                var item = ux_LayerList.SelectedItem;
            };

            ux_LevelEntityList.ItemCheck += ( sender, args ) => {
                var entity = (GameObject)ux_LevelEntityList.SelectedItem;

                if( entity != null )
                    entity.IsAlive = args.NewValue == CheckState.Checked;
            };

            ux_LevelEntityList.DoubleClick += UxLevelEntityListOnDoubleClick;

            ux_LevelEditor.MouseDown += UxLevelEditorOnMouseDown;
            ux_LevelEditor.MouseMove += UxLevelEditorOnMouseMove;
            ux_LevelEditor.KeyDown += UxLevelEditorOnKeyDown;

            KeyDown += UxLevelEditorOnKeyDown;

            ux_LevelEditor.MouseWheel += UxLevelEditorOnMouseWheel;

            // setup status bar controls
            AddZoomControlsToStatusBar( ux_StatusBar );

            Text = Path.GetFileName( level.FilePath );

            Project.OnChange += ( sender, args ) => UpdateUserInterface();

            LostFocus += ( sender, args ) => {
                ( (AppContainer)MdiParent ).ux_AddEntity.Visible = false;
            };

            GotFocus += ( sender, args ) => {
                ( (AppContainer)MdiParent ).ux_AddEntity.Visible = true;
                UpdateUserInterface();
            };
        }

        private void UxLevelEditorOnMouseWheel( object sender, MouseEventArgs mouseEventArgs )
        {
            // TODO: handle the zoom in/out here
            var zoom = ux_LevelEditor.Camera.Zoom + mouseEventArgs.Delta;
            zoom = zoom > 500 ? 500 : zoom;
            ux_LevelEditor.Camera.Zoom = zoom;
        }

        private void UxLevelEditorOnKeyDown( object sender, KeyEventArgs keyEventArgs )
        {
            if( ux_LayerList.SelectedItem == null ) return;
            var name = (string)ux_LayerList.SelectedItem;
            var layer = name == "Collision" ? ux_LevelEditor.Level.CollisionLayer : ux_LevelEditor.Level.MapLayers.First( ml => ml.Name == name );

            if( keyEventArgs.KeyCode == Keys.Q || keyEventArgs.KeyCode == Keys.E )
            {
                var maxTiles = layer.TileSheet.GetAnimations().Count;
                var currentTile = _layerTileMap.ContainsKey( layer ) ? _layerTileMap[ layer ] : 0;
                var previousButtonPressed = keyEventArgs.KeyData.HasFlag( Keys.Q );
                var nextButtonPressed = keyEventArgs.KeyData.HasFlag( Keys.E );

                if( currentTile == -1 && previousButtonPressed )
                {
                    currentTile = maxTiles;
                }
                else if( currentTile == maxTiles - 1 && nextButtonPressed )
                {
                    currentTile = -1;
                }
                else
                {
                    currentTile += ( nextButtonPressed ? 1 : -1 );
                }

                if( !_layerTileMap.ContainsKey( layer ) )
                {
                    _layerTileMap.Add( layer, currentTile );
                }

                _layerTileMap[ layer ] = currentTile;

                debugStatus.Text = string.Format( "curr: {0}", currentTile );
            }
        }

        private void UxLevelEntityListOnDoubleClick( object sender, EventArgs eventArgs )
        {
            var actor = (Actor)ux_LevelEntityList.SelectedItem;
            if( actor != null )
            {
                // todo: open a new properties dialog for this entity
                var dialog = new EntityProperties( actor );
                dialog.Show();
            }
        }

        private void UpdateUserInterface()
        {
            ux_AddEntity.DropDownItems.Clear();

            ( (AppContainer)MdiParent ).Project.Entities.ForEach( AddEntityToUx );
        }

        private void AddEntityToUx( Type entity )
        {
            ( (AppContainer)MdiParent ).ux_AddEntity.DropDownItems.Add( entity.Name, null, ( sender, args ) => AddEntityToLevel( entity ) );
        }

        private void EditLevel( Level level )
        {
            if( ux_LevelEditor.Level != null )
            {
                var result = MessageBox.Show( "Do you want to save changes to the current level?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes )
                    ux_LevelEditor.Level.Save();

                if( result == DialogResult.Cancel )
                    return;
            }

            ux_LevelEditor.Level = level;

            RefreshLevelEntityList();

            RefreshLayerList();
        }

        private void RefreshLevelEntityList()
        {
            ux_LevelEntityList.Items.Clear();
            ux_LevelEditor.Level.Entities.ForEach( e => ux_LevelEntityList.Items.Add( e, true ) );
        }

        private void RefreshLayerList()
        {
            ux_LayerList.Items.Clear();

            ux_LayerList.Items.Add( "Collision", true );
            CurrentLevel.MapLayers.ForEach( ml => ux_LayerList.Items.Add( ml.Name, true ) );
        }

        private void AddEntityToLevel( Type type )
        {
            var entity = (GameObject)Activator.CreateInstance( type );
            ux_LevelEditor.Level.Entities.Add( entity );

            RefreshLevelEntityList();
        }

        private void UpdateLevelControl( Level level = null )
        {
            ux_LevelEditor.Level = level;
        }

        private void UxLevelEditorOnMouseMove( object sender, MouseEventArgs mouseEventArgs )
        {
            debugStatus.Text = string.Format( "{0}, {1}", mouseEventArgs.X, mouseEventArgs.Y );

            var name = (string)ux_LayerList.SelectedItem;
            var position = new Vector2( mouseEventArgs.X, mouseEventArgs.Y );

            if( mouseEventArgs.Button.HasFlag( MouseButtons.Right ) && ux_LayerList.SelectedItem != null )
                UnPaintTile( GetLayerByName( name ), position );

            if( mouseEventArgs.Button.HasFlag( MouseButtons.Left ) )
            {
                // user has an entity selected, move the entity
                var entity = (GameObject)ux_LevelEntityList.SelectedItem;
                if( entity != null )
                    entity.Position = new Vector2( mouseEventArgs.X, mouseEventArgs.Y );

                if( ux_LayerList.SelectedItem != null )
                    PaintTile( GetLayerByName( name ), position );
            }

            if( mouseEventArgs.Button.HasFlag( MouseButtons.Middle ) )
            {
                var centerOfEditorPane = new Vector2( ux_LevelEditor.Width / 2, ux_LevelEditor.Height / 2 );

                var delta = position - centerOfEditorPane;

                ux_LevelEditor.Camera.Origin += delta * ux_LevelEditor.LatestTick;
            }
        }

        private void UxLevelEditorOnMouseDown( object sender, MouseEventArgs mouseEventArgs )
        {
            if( ux_LayerList.SelectedItem == null ) return;

            var name = (string)ux_LayerList.SelectedItem;
            var position = new Vector2( mouseEventArgs.X, mouseEventArgs.Y );
            var layer = GetLayerByName( name );

            if( mouseEventArgs.Button == MouseButtons.Right )
                UnPaintTile( layer, position );
            else
                PaintTile( layer, position );
        }

        private void ux_RemoveLayer_Click( object sender, EventArgs e )
        {
            var name = ux_LayerList.SelectedItem;
            if( name.ToString().ToLowerInvariant() == "collision" )
            {
                MessageBox.Show( "Sorry, you can't remove the Collision layer.", "Sorry Dave...", MessageBoxButtons.OK, MessageBoxIcon.Hand );
                return;
            }

            CurrentLevel.MapLayers.RemoveAll( ml => ml.Name == name );

            RefreshLayerList();
        }

        private void ux_AddLayer_Click( object sender, EventArgs e )
        {
            var dialog = new AddLayer( CurrentLevel.MapLayers );
            var result = dialog.ShowDialog();
            if( result == DialogResult.OK )
            {
                // create a layer object
                // TODO: make sure this is relative to one of the content directories...
                var layer = new MapLayer {
                    TileSetTexturePath = dialog.TileSet,
                    TileSize = dialog.TileSize,
                    Name = dialog.LayerName
                };

                CurrentLevel.MapLayers.Add( layer );

                RefreshLayerList();
            }
        }

        private void ux_RemoveEntity_Click( object sender, EventArgs e )
        {
            if( ux_LevelEntityList.SelectedItem != null )
            {
                var entity = (GameObject)ux_LevelEntityList.SelectedItem;
                ux_LevelEditor.Level.Entities.Remove( entity );

                RefreshLevelEntityList();
            }
        }

        private MapLayer GetLayerByName( string name )
        {
            var layer = name == "Collision" ?
                    ux_LevelEditor.Level.CollisionLayer :
                    ux_LevelEditor.Level.MapLayers.First( ml => ml.Name == name );
            return layer;
        }

        private void PaintTile( MapLayer layer, Vector2 position )
        {
            if( !_layerTileMap.ContainsKey( layer ) )
                _layerTileMap.Add( layer, 0 ); // the first tile is the default

            layer.SetTile( position, _layerTileMap[ layer ] );
        }

        private void UnPaintTile( MapLayer layer, Vector2 position )
        {
            layer.SetTile( position, -1 );
        }

        private void AddZoomControlsToStatusBar( StatusStrip statusBar )
        {

            _zoomText = new ToolStripStatusLabel( "100%" ) { AutoSize = false, Width = 35, Padding = new Padding( 6, 3, 0, 2 ), TextAlign = ContentAlignment.MiddleRight };

            var trackBarZoom = new TrackBar {
                AutoSize = false,
                Height = 22,
                TickStyle = TickStyle.None,
                Anchor = AnchorStyles.Right,
                Minimum = 0,
                Maximum = _zoomLevels.Length - 1,
                Value = 1,
                BackColor = SystemColors.ControlLightLight
            };

            trackBarZoom.ValueChanged += TrackBarZoomOnValueChanged;

            var trackBarZoomItem = new ToolStripControlHost( trackBarZoom );

            debugStatus.Width = 80;
            debugStatus.AutoSize = false;
            debugStatus.BorderSides = ToolStripStatusLabelBorderSides.Right;

            statusBar.Items.AddRange( new ToolStripItem[] {
                debugStatus,
                _zoomText,
                trackBarZoomItem
            } );
        }

        private void TrackBarZoomOnValueChanged( object sender, EventArgs eventArgs )
        {
            var trackBar = (TrackBar)sender;
            var zoom = _zoomLevels[ trackBar.Value ];

            ux_LevelEditor.Camera.Zoom = zoom / 100f;

            _zoomText.Text = zoom + "%";
        }
    }
}
