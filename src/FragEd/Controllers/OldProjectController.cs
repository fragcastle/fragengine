using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using FragEd.Data;
using FragEd.Forms;
using FragEd.Properties;
using FragEd.Services;
using FragEngine;
using FragEngine.Data;
using FragEngine.IO;
using FragEngine.Services;
using Microsoft.Xna.Framework.Content;

namespace FragEd.Controllers
{
    public class SelectedLevelEventArgs : EventArgs {
        public Level SelectedLevel { get; set; }
    }

    public class OldProjectController : StateController
    {
        private string _currentProjectFile;

        public Project CurrentProject { get; private set; }

        public static OldProjectController BoundTo( Main form )
        {
            return new OldProjectController( form );
        }

        private OldProjectController( Main form )
        {
            _mainForm = form;

            _mainForm.openFileDialog1.FileOk += OpenFileDialog1OnFileOk;
            _mainForm.saveFileDialog1.FileOk += SaveFileDialog1OnFileOk;

            _mainForm.openToolStripMenuItem.Click += ( o, e ) => OpenProject();
            _mainForm.saveToolStripMenuItem.Click += ( o, e ) => Save();
            _mainForm.ux_ToolStripSaveProject.Click += ( o, e ) => Save();
            _mainForm.newProjectToolStripMenuItem.Click += ( o, e ) => New();

            Project.OnLoad += ( sender, args ) => LoadProject( (Project)sender );

            EventAggregator.Current.On("add:level").Add( (o,e) =>
                {
                    var level = (Level)o;
                    CurrentProject.Levels.Add(level);
                    Save();
                } );

            _mainForm.ux_ToolStripAddEntityList.Enabled = false;

            // cancel any node label edits if the nodes are not levels
            _mainForm.ux_ProjectTree.BeforeLabelEdit += ( sender, args ) => args.CancelEdit = GetLevelFromNode( args.Node ) == null;
        }

        private void SaveFileDialog1OnFileOk(object sender, CancelEventArgs cancelEventArgs)
        {
            if( !cancelEventArgs.Cancel )
            {
                _currentProjectFile = _mainForm.saveFileDialog1.FileName;
                Save();
            }
        }

        private void OpenFileDialog1OnFileOk( object sender, CancelEventArgs cancelEventArgs )
        {
            // user chose a file
            _currentProjectFile = _mainForm.openFileDialog1.FileName;

            // open the ProjectConfiguration
            var projectConfiguration = Persistant.Load<ProjectConfiguration>( _currentProjectFile );

            if( projectConfiguration != null )
            {
                CurrentProject = new Project( projectConfiguration );
            }
        }

        public void New()
        {

            // user wants to start a new project
            // are we currently editing a project?
            //  if so, ask the user if they want to save changes
            //      if they do and that project is new (hasn't been saved) open the save dialog
            //  if not, create a new project
            //      ask the user where the project will be saved
            //      better to structure this like the visual studio project creator,
            //      ask where the project goes, create some directories and assume files will be saved relative to the project file.

            CurrentProject = new Project();

            PromptForProjectName();
        }

        public void LoadProject( Project project )
        {
            _mainForm.ux_ProjectTree.Nodes.Clear();
            _mainForm.ux_ToolStripAddEntityList.DropDownItems.Clear();

            var projectNode = _mainForm.ux_ProjectTree.Nodes.Add( "project", "Project" );
            var entitiesRoot = projectNode.Nodes.Add( "entities", "Entities" );
            var levelsRoot = projectNode.Nodes.Add( "levels", "Levels" );

            project.Entities.ForEach( e => AddEntity(e, entitiesRoot) );

            project.Levels.ForEach( l => AddLevel(l, levelsRoot ) );

            project.ContentDirectories.ForEach( ContentCacheManager.AddContentDirectory );

            // load the content
            LoadContent();

            project.OnLoadContentDirectory += ( o, e ) =>
                {
                    var dir = (DirectoryInfo)o;
                    ContentCacheManager.AddContentDirectory( dir );
                };

            projectNode.Expand();
        }

        private void LoadContent()
        {
            ContentCacheManager.LoadContent( new ContentManager( ServiceInjector.Apply() ) );
        }

        private void AddLevel( Level level, TreeNode root )
        {
            var node = root.Nodes.Add( level.Name, level.Name );

            node.Tag = level;
        }

        private void AddEntity( Type type, TreeNode root )
        {
            _mainForm.ux_ToolStripAddEntityList.Enabled = true;
            var node = root.Nodes.Add( type.Name );
            node.Tag = type;

            var item = _mainForm.ux_ToolStripAddEntityList.DropDownItems.Add( type.Name );

            item.Click += ( o, e ) => EventAggregator.Current.Trigger( "add:entity", node );
        }

        public void OpenProject()
        {
            _mainForm.openFileDialog1.DefaultExt = ".fed";
            _mainForm.openFileDialog1.CheckFileExists = true;
            _mainForm.openFileDialog1.Title = Resources.Dialog_Title_Open_Project_File;
            _mainForm.openFileDialog1.InitialDirectory = Path.GetDirectoryName( Application.ExecutablePath );
            _mainForm.openFileDialog1.ShowDialog();
        }

        public void PromptForProjectName()
        {
            _mainForm.saveFileDialog1.CheckPathExists = false;
            _mainForm.saveFileDialog1.DefaultExt = ".fed";
            _mainForm.saveFileDialog1.FileName = "myProject";
            _mainForm.saveFileDialog1.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
            _mainForm.saveFileDialog1.SupportMultiDottedExtensions = true;
            _mainForm.saveFileDialog1.ShowDialog();
        }

        public void Save()
        {
            if( String.IsNullOrWhiteSpace(_currentProjectFile))
            {
                PromptForProjectName();
            }
            else
            {
                var config = CurrentProject.GetConfiguration();
                Persistant.Persist( _currentProjectFile, config);
            }
        }
    }
}