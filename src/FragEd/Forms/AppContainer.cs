﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FragEd.Controls;
using FragEd.Data;
using FragEd.Services;
using FragEngine;
using FragEngine.Data;
using FragEngine.IO;
using FragEngine.Services;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FragEd.Forms {
    public partial class AppContainer : Form {
        private Project _project;

        private Properties.Settings _settings = Properties.Settings.Default;

        public AppContainer() {
            InitializeComponent();

            if( _settings.FirstRun ) {
                _settings.OpenProjectLastDirectoryPath = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
                _settings.FirstRun = false;
            }

            Project.OnCompleteLoadContentDirectory += ( sender, args ) => {
                // total hack, create a level editor control
                // so that a valid GraphicsDevice object is created
                new LevelEditorControl { Height = 100, Width = 100 }.CreateControl();

                var project = (Project)sender;
                project.ContentDirectories.ForEach( ContentCacheManager.AddContentDirectory );

                // TODO: disk op... show progress bar?
                ContentCacheManager.LoadContent( new ContentManager( ServiceInjector.Apply() ) );
            };
        }

        protected string CurrentProjectFile { get; set; }

        protected Project Project {
            get { return _project; }
            set {
                _project = value;
                UpdateUserInterface();
            }
        }

        private void OpenFile( object sender, EventArgs e ) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _settings.OpenProjectLastDirectoryPath;
            openFileDialog.Filter = "FragEd Project|*.fed";
            if( openFileDialog.ShowDialog( this ) == DialogResult.OK ) {
                var fileName = openFileDialog.FileName;

                if( File.Exists( fileName ) ) {
                    LoadProjectFile( fileName );

                    _settings.OpenProjectLastDirectoryPath = Path.GetDirectoryName( fileName );
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click( object sender, EventArgs e ) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if( saveFileDialog.ShowDialog( this ) == DialogResult.OK ) {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click( object sender, EventArgs e ) {
            _settings.Save();
            
            Close();
        }

        private void CutToolStripMenuItem_Click( object sender, EventArgs e ) {
        }

        private void CopyToolStripMenuItem_Click( object sender, EventArgs e ) {
        }

        private void PasteToolStripMenuItem_Click( object sender, EventArgs e ) {
        }

        private void ToolBarToolStripMenuItem_Click( object sender, EventArgs e ) {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click( object sender, EventArgs e ) {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click( object sender, EventArgs e ) {
            LayoutMdi( MdiLayout.Cascade );
        }

        private void TileVerticalToolStripMenuItem_Click( object sender, EventArgs e ) {
            LayoutMdi( MdiLayout.TileVertical );
        }

        private void TileHorizontalToolStripMenuItem_Click( object sender, EventArgs e ) {
            LayoutMdi( MdiLayout.TileHorizontal );
        }

        private void ArrangeIconsToolStripMenuItem_Click( object sender, EventArgs e ) {
            LayoutMdi( MdiLayout.ArrangeIcons );
        }

        private void CloseAllToolStripMenuItem_Click( object sender, EventArgs e ) {
            foreach( Form childForm in MdiChildren ) {
                childForm.Close();
            }
        }

        private void OpenChild( Form child )
        {
            child.MdiParent = this;
            child.Show();
        }

        private void LoadProjectFile( string fileName ) {
            var config = new ProjectConfiguration();
            // user chose a file
            CurrentProjectFile = fileName;

            // open the ProjectConfiguration
            var projectConfiguration = Persistant.Load<ProjectConfiguration>( fileName ) ?? new ProjectConfiguration();

            Project = new Project( projectConfiguration );
        }

        private void UpdateUserInterface() {
            Text = _settings.AppTitle + Path.GetFileNameWithoutExtension( CurrentProjectFile );
            //ux_AddEntityMenu.DropDownItems.Clear();
            //ux_AddEntity.DropDownItems.Clear();
            //ux_GameLevels.DropDownItems.Clear();

            //ux_ProjectMenu.Enabled = true;
            //ux_SaveProjectMenu.Enabled = true;

            //// enable all the children too
            //foreach( ToolStripItem item in ux_ProjectMenu.DropDownItems ) {
            //    item.Enabled = true;
            //}

            //Project.Entities.ForEach( AddEntityToUx );
            //Project.Levels.ForEach( l => ux_GameLevels.DropDownItems.Add( Path.GetFileName( l.FilePath ), null, ( sender, args ) => EditLevel( l ) ) );

            //SelectCurrentLevel();

            levelsToolStripMenuItem.DropDownItems.Clear();
            Project.Levels.ForEach( l => levelsToolStripMenuItem.DropDownItems.Add( Path.GetFileName( l.FilePath ), null, ( sender, args ) => EditLevel( l ) ) );

            projectToolStripMenu.Visible = Project != null;
        }

        private void EditLevel( Level level ) {
            OpenChild( new LevelEditorForm( level ) );
        }
    }
}
