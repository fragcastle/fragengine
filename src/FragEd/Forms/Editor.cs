﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FragEd.Controllers;
using FragEd.Controls;
using FragEd.Data;
using FragEngine;
using FragEngine.Data;
using FragEngine.Entities;
using FragEngine.IO;
using FragEngine.Layers;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FragEd.Forms {
    // TODO: Remove Level
    //
    // TODO: Add Game Assembly
    // TODO: Remove Game Assembly
    //
    // TODO: load content folders
    // TODO: Add Content Folder
    // TODO: Remove Content Folder
    // 
    // TODO: Add Layer
    // TODO: Remove Layer
    //
    // TODO: Add Entity
    // TODO: Remove Entity
    public partial class Editor : Form {
        private Project _project;
        private Dictionary<Layer, int> _layerTileMap = new Dictionary<Layer, int>();

        public Project Project
        {
            get { return _project; }
            set { 
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

        protected string CurrentProjectFile { get; set; }

        public Editor() {
            InitializeComponent();

            ux_OpenProjectDialog.FileOk += ( sender, args ) => ReadProject( ux_OpenProjectDialog.FileName );

            ux_LayerList.ItemCheck += ( sender, args ) => {
                    var name = (string)ux_LayerList.Items[ args.Index ];
                    if( name == "Collision")
                    {
                        ux_LevelEditor.Level.CollisionLayer.Alpha = args.NewValue == CheckState.Checked ? 255f : 0f;
                    }
                    else
                    {
                        ux_LevelEditor.Level.MapLayers.First( ml => ml.Name == name ).Alpha = args.NewValue == CheckState.Checked ? 255f : 0f;
                    }
                    
                };
            ux_LayerList.SelectedIndexChanged += ( sender, args ) => {
                    var item = ux_LayerList.SelectedItem;
                };

            ux_LevelEditor.MouseDown += UxLevelEditorOnMouseDown;
        }

        private void UpdateUserInterface() {
            ux_AddEntityMenu.DropDownItems.Clear();
            ux_GameLevels.DropDownItems.Clear();

            ux_ProjectMenu.Enabled = true;
            ux_SaveProjectMenu.Enabled = true;

            // enable all the children too
            foreach( ToolStripItem item in ux_ProjectMenu.DropDownItems ) {
                item.Enabled = true;
            }

            Project.Entities.ForEach( e => ux_AddEntityMenu.DropDownItems.Add( e.Name, null, ( sender, args ) => AddEntityToLevel( e ) ) );
            Project.Levels.ForEach( l => ux_GameLevels.DropDownItems.Add( Path.GetFileName( l.FilePath ), null, ( sender, args ) => EditLevel( l ) ) );

            Project.ContentDirectories.ForEach( ContentCacheManager.AddContentDirectory );

            // TODO: disk op... show progress bar?
            ContentCacheManager.LoadContent( new ContentManager( ServiceInjector.Apply() ) );
        }

        private void EditLevel( Level level ) {
            if( ux_LevelEditor.Level != null ) {
                var result = MessageBox.Show( "Do you want to save changes to the current level?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes ) {
                    ux_LevelEditor.Level.Save();
                }

                if( result == DialogResult.Cancel ) {
                    return;
                }
            }

            ux_LevelEditor.Level = level;

            ux_LevelEntityList.Items.Clear();            

            RefreshLayerList();

            foreach(ToolStripMenuItem item in ux_GameLevels.DropDownItems )
            {
                if( item.Text == level.Name )
                {
                    item.CheckState = CheckState.Checked;
                    break;
                }
            }

            level.Entities.ForEach( e => ux_LevelEntityList.Items.Add( e, true ));
        }

        private void RefreshLayerList()
        {
            ux_LayerList.Items.Clear();

            ux_LayerList.Items.Add( "Collision", true );
            CurrentLevel.MapLayers.ForEach( ml => ux_LayerList.Items.Add( ml.Name, true ) );
        }

        private void AddEntityToLevel( Type type ) {
            var entity = (EntityBase)Activator.CreateInstance( type );
            ux_LevelEditor.Level.Entities.Add( entity );
        }

        private void UpdateLevelControl( Level level = null ) {
            ux_LevelEditor.Level = level;
        }

        private void UxLevelEditorOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            if( ux_LayerList.SelectedItem == null ) return;

            var name = (string)ux_LayerList.SelectedItem;

            var layer = name == "Collision" ? ux_LevelEditor.Level.CollisionLayer : ux_LevelEditor.Level.MapLayers.First( ml => ml.Name == name );

            var position = new Vector2( mouseEventArgs.X, mouseEventArgs.Y );

            if( !_layerTileMap.ContainsKey( layer ) ) {
                _layerTileMap.Add( layer, 0 ); // the first tile is the default
            }

            var lastTile = _layerTileMap[ layer ];

            layer.SetTile( position, lastTile );
        }

        private void ux_NewProjectMenu_Click( object sender, EventArgs e ) {
            if( Project != null )
            {
                var result = MessageBox.Show( "Do you want to save changes to the current project?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes )
                {
                    Persistant.Persist( CurrentProjectFile, Project );
                    Project.Levels.ForEach( l => l.Save() );
                }

                if( result == DialogResult.Cancel )
                {
                    return;
                }
            }
            Project = new Project();
        }

        private void ux_OpenProjectMenu_Click( object sender, EventArgs e ) {
            if( Project != null ) {
                var result = MessageBox.Show( "Do you want to save changes to the current project?", "Save Changes?", MessageBoxButtons.YesNoCancel );
                if( result == DialogResult.Yes ) {
                    Persistant.Persist( CurrentProjectFile, Project );
                    Project.Levels.ForEach( l => l.Save() );
                }

                if( result == DialogResult.Cancel ) {
                    return;
                }
            }

            ux_OpenProjectDialog.ShowDialog();
        }

        private void ReadProject( string fileName ) {
            var config = new ProjectConfiguration();
            // user chose a file
            CurrentProjectFile = fileName;

            // open the ProjectConfiguration
            var projectConfiguration = Persistant.Load<ProjectConfiguration>( fileName );

            if( projectConfiguration != null ) {
                Project = new Project( projectConfiguration );

                UpdateUserInterface();
            }
        }

        private void ux_SaveProjectMenu_Click( object sender, EventArgs e ) {
            Persistant.Persist( CurrentProjectFile, Project.GetConfiguration() );
            Project.Levels.ForEach( l => l.Save() );
        }

        private void ux_ExitApplicationMenu_Click( object sender, EventArgs e ) {
            Application.Exit();
        }

        private void ux_AddLevel_Click( object sender, EventArgs e ) {
            var result = ux_SaveLevelDialog.ShowDialog();
            if( result == DialogResult.OK )
            {
                var fileName = ux_SaveLevelDialog.FileName;
                var level = new Level() {
                    FilePath = fileName,
                    Name = Path.GetFileNameWithoutExtension( fileName )
                };

                Project.Levels.Add( level );
                UpdateUserInterface();
            }
        }

        private void ux_AddExistingLevel_Click( object sender, EventArgs e ) {
            var result = ux_OpenLevelDialog.ShowDialog();
            if( result == DialogResult.OK )
            {
                var fileName = ux_OpenLevelDialog.FileName;
                var level = Level.Load( new FileInfo( fileName ) );

                level.FilePath = Path.Combine( Path.GetDirectoryName( CurrentProjectFile ), Path.GetFileName( fileName ) );
                level.Save();

                Project.Levels.Add(level);
                UpdateUserInterface();
            }
        }

        private void ux_RemoveLayer_Click( object sender, EventArgs e ) {
            var name = ux_LayerList.SelectedItem;
            if( name == "Collision" ) {
                MessageBox.Show( "Sorry, you can't remove the Collision layer.", "Sorry Dave...", MessageBoxButtons.OK, MessageBoxIcon.Hand );
                return;
            }

            CurrentLevel.MapLayers.RemoveAll( ml => ml.Name == name );
            
            RefreshLayerList();
        }

        private void ux_AddLayer_Click( object sender, EventArgs e ) {
            var dialog = new AddLayer( this );
            var result = dialog.ShowDialog();
            if( result == DialogResult.OK ) {
                // create a layer object
                var layer = new MapLayer();

                // TODO: make sure this is relative to one of the content directories...
                layer.TileSetTexturePath = dialog.TileSet;
                layer.TileSize = dialog.TileSize;
                layer.Name = dialog.LayerName;

                CurrentLevel.MapLayers.Add( layer );

                RefreshLayerList();
            }
        }
    }
}
