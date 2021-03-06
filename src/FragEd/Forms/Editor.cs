﻿using System;
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
    public partial class Editor : Form
    {
        private Project _project;
        private Dictionary<Layer, int> _layerTileMap = new Dictionary<Layer, int>();

        public Project Project
        {
            get { return _project; }
            set
            {
                _project = value;
                UpdateLevelControl();
                UpdateUserInterface();
            }
        }

        public Level CurrentLevel
        {
            get
            {
                return ux_LevelEditor.Level;
            }
        }

        // statusbar variables
        private ToolStripStatusLabel _zoomText;
        private int[] _zoomLevels = new int [] { 50, 100, 200, 300, 400, 600, 800, 1200, 1400, 1600 };
        private bool _showGrid;

        protected string CurrentProjectFile { get; set; }

        public Editor()
        {
            InitializeComponent();

            ux_OpenProjectDialog.FileOk += ( sender, args ) => ReadProject( ux_OpenProjectDialog.FileName );

            ux_LayerList.ItemCheck += ( sender, args ) =>
            {
                var name = (string)ux_LayerList.Items[ args.Index ];
                if( name == "Collision" )
                {
                    ux_LevelEditor.Level.CollisionLayer.Alpha = args.NewValue == CheckState.Checked ? 255f : 0f;
                }
                else
                {
                    ux_LevelEditor.Level.MapLayers.First( ml => ml.Name == name ).Alpha = args.NewValue == CheckState.Checked ? 255f : 0f;
                }

            };
            ux_LayerList.SelectedIndexChanged += ( sender, args ) =>
            {
                var item = ux_LayerList.SelectedItem;
            };

            ux_LevelEntityList.ItemCheck += ( sender, args ) =>
            {
                var entity = (GameObject)ux_LevelEntityList.SelectedItem;

                if( entity != null )
                    entity.IsAlive = args.NewValue == CheckState.Checked;
            };

            ux_LevelEntityList.DoubleClick += UxLevelEntityListOnDoubleClick;

            ux_LevelEditor.MouseDown += UxLevelEditorOnMouseDown;
            ux_LevelEditor.MouseMove += UxLevelEditorOnMouseMove;
            ux_LevelEditor.KeyDown += UxLevelEditorOnKeyDown;

            KeyDown += UxLevelEditorOnKeyDown;

            Project.OnCompleteLoadContentDirectory += ( sender, args ) =>
            {
                var project = (Project)sender;
                project.ContentDirectories.ForEach( ContentCacheManager.AddContentDirectory );

                // TODO: disk op... show progress bar?
                ContentCacheManager.LoadContent( new ContentManager( ServiceLocator.Apply() ) );
            };

            ux_LevelEditor.MouseWheel += UxLevelEditorOnMouseWheel;

            // setup status bar controls
            AddZoomControlsToStatusBar( ux_StatusBar );

            ux_ShowGrid.Enabled = false;
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
                else if (currentTile == maxTiles - 1 && nextButtonPressed)
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
            ux_AddEntityMenu.DropDownItems.Clear();
            ux_AddEntity.DropDownItems.Clear();
            ux_GameLevels.DropDownItems.Clear();

            ux_ProjectMenu.Enabled = true;
            ux_SaveProjectMenu.Enabled = true;

            // enable all the children too
            foreach( ToolStripItem item in ux_ProjectMenu.DropDownItems )
            {
                item.Enabled = true;
            }

            Project.Entities.ForEach( AddEntityToUx );
            Project.Levels.ForEach( l => ux_GameLevels.DropDownItems.Add( Path.GetFileName( l.FilePath ), null, ( sender, args ) => EditLevel( l ) ) );

            SelectCurrentLevel();
        }

        private void AddEntityToUx( Type entity )
        {
            ux_AddEntityMenu.DropDownItems.Add( entity.Name, null, ( sender, args ) => AddEntityToLevel( entity ) );
            ux_AddEntity.DropDownItems.Add( entity.Name, null, ( sender, args ) => AddEntityToLevel( entity ) );
        }

        private void EditLevel( Level level )
        {
            if( ux_LevelEditor.Level != null )
            {
                var result = MessageBox.Show( "Do you want to save changes to the current level?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes )
                {
                    ux_LevelEditor.Level.Save();
                }

                if( result == DialogResult.Cancel )
                {
                    return;
                }
            }

            ux_LevelEditor.Level = level;

            RefreshLevelEntityList();

            RefreshLayerList();

            SelectCurrentLevel();
        }

        private void RefreshLevelEntityList()
        {
            ux_LevelEntityList.Items.Clear();

            // only show "actors" in the list for now
            // just to get the refactor done
            ux_LevelEditor.Level.Entities.Where( go => go as Actor != null ).ToList().ForEach( e => ux_LevelEntityList.Items.Add( e, true ) );
        }

        private void SelectCurrentLevel()
        {
            if( ux_LevelEditor.Level == null )
                return;

            foreach( ToolStripMenuItem item in ux_GameLevels.DropDownItems )
            {
                if( item.Text == ux_LevelEditor.Level.Name + ".json" )
                {
                    item.CheckState = CheckState.Checked;
                    break;
                }
            }

            ux_ShowGrid.Enabled = true;
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
                var centerOfEditorPane = new Vector2( ux_LevelEditor.Width/2, ux_LevelEditor.Height/2 );

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
            {
                UnPaintTile( layer, position );
            }
            else
            {
                PaintTile( layer, position );
            }
        }

        private void ux_NewProjectMenu_Click( object sender, EventArgs e )
        {
            if( Project != null )
            {
                var result = MessageBox.Show( "Do you want to save changes to the current project?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes )
                {
                    DiskStorage.SaveToDisk( CurrentProjectFile, Project );
                    Project.Levels.ForEach( l => l.Save() );
                }

                if( result == DialogResult.Cancel )
                {
                    return;
                }
            }

            // ask the user to pick a new project file
            var projectNameResult = ux_SaveProjectDialog.ShowDialog();
            if( projectNameResult == DialogResult.OK )
            {
                CurrentProjectFile = ux_SaveProjectDialog.FileName;
            }

            Project = new Project();
        }

        private void ux_OpenProjectMenu_Click( object sender, EventArgs e )
        {
            if( Project != null )
            {
                var result = MessageBox.Show( "Do you want to save changes to the current project?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes )
                {
                    DiskStorage.SaveToDisk( CurrentProjectFile, Project );
                    Project.Levels.ForEach( l => l.Save() );
                }

                if( result == DialogResult.Cancel )
                {
                    return;
                }
            }

            ux_OpenProjectDialog.ShowDialog();
        }

        private void ReadProject( string fileName )
        {
            var config = new ProjectConfiguration();
            // user chose a file
            CurrentProjectFile = fileName;

            // open the ProjectConfiguration
            var projectConfiguration = DiskStorage.LoadFromDisk<ProjectConfiguration>( fileName );

            if( projectConfiguration != null )
            {
                Project = new Project( projectConfiguration );

                UpdateUserInterface();
            }
        }

        private void ux_SaveProjectMenu_Click( object sender, EventArgs e )
        {
            DiskStorage.SaveToDisk( CurrentProjectFile, Project.GetConfiguration() );
            Project.Levels.ForEach( l => l.Save() );
        }

        private void ux_ExitApplicationMenu_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }

        private void ux_AddLevel_Click( object sender, EventArgs e )
        {
            var result = ux_SaveLevelDialog.ShowDialog();
            if( result == DialogResult.OK )
            {
                var fileName = ux_SaveLevelDialog.FileName;
                var level = new Level()
                {
                    FilePath = fileName,
                    Name = Path.GetFileNameWithoutExtension( fileName )
                };

                Project.Levels.Add( level );
                UpdateUserInterface();
            }
        }

        private void ux_AddExistingLevel_Click( object sender, EventArgs e )
        {
            var result = ux_OpenLevelDialog.ShowDialog();
            if( result == DialogResult.OK )
            {
                var fileName = ux_OpenLevelDialog.FileName;
                var level = Level.Load( new FileInfo( fileName ) );

                level.FilePath = Path.Combine( Path.GetDirectoryName( CurrentProjectFile ), Path.GetFileName( fileName ) );
                level.Save();

                Project.Levels.Add( level );
                UpdateUserInterface();
            }
        }

        private void ux_RemoveLayer_Click( object sender, EventArgs e )
        {
            var name = ux_LayerList.SelectedItem;
            if( name == "Collision" )
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

        private void ux_ManageGameAssemblies_Click( object sender, EventArgs e )
        {
            var gameAssemblies = new GameAssemblies( this );
            var result = gameAssemblies.ShowDialog();
            if( result == DialogResult.OK )
            {
                foreach( var type in gameAssemblies.RemovedEntities )
                    Project.Entities.Remove( type );

                Project.Entities.AddRange( gameAssemblies.AddedEntities );

                UpdateUserInterface();
            }
        }

        private void ux_ManageContentFolders_Click( object sender, EventArgs e )
        {
            var contentFolders = new ContentFolders( this );
            var result = contentFolders.ShowDialog();
            if( result == DialogResult.OK )
            {
                var dirs = Project.ContentDirectories.Where( d => contentFolders.RemovedFolders.Contains( d.FullName ) );
                dirs.ToList().ForEach( d => Project.ContentDirectories.Remove( d ) );

                foreach( var addedPath in contentFolders.AddedFolders )
                    Project.ContentDirectories.Add( new DirectoryInfo( addedPath ) );

                UpdateUserInterface();
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

            var trackBarZoom = new TrackBar
                {
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
            });
        }

        private void TrackBarZoomOnValueChanged(object sender, EventArgs eventArgs)
        {
            var trackBar = (TrackBar)sender;
            var zoom = _zoomLevels[ trackBar.Value ];

            ux_LevelEditor.Camera.Zoom = zoom/100f;

            _zoomText.Text = zoom + "%";
        }

        private void ux_ShowGrid_Click( object sender, EventArgs e )
        {
            _showGrid = !_showGrid;
            ux_ShowGrid.Checked = _showGrid;
            ux_ShowGrid.CheckState = _showGrid ? CheckState.Checked : CheckState.Unchecked;

            ux_LevelEditor.SetGridState( _showGrid );
        }
    }
}
