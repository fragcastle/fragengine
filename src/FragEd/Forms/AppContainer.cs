using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FragEd.Controls;
using FragEd.Data;
using FragEngine;
using FragEngine.Data;
using FragEngine.IO;
using FragEngine.Services;
using Microsoft.Xna.Framework.Content;

namespace FragEd.Forms
{
    public partial class AppContainer : Form
    {
        private Project _project;

        private Properties.Settings _settings = Properties.Settings.Default;

        public AppContainer()
        {
            InitializeComponent();

            if (_settings.FirstRun)
            {
                _settings.OpenProjectLastDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                _settings.FirstRun = false;

                _settings.Save();
            }

            Project.OnChange += ProjectOnOnChange;
        }

        protected string CurrentProjectFile { get; set; }

        public Project Project
        {
            get { return _project; }
            set
            {
                _project = value;
                UpdateUserInterface();
            }
        }
        private void NewFile(object sender, EventArgs e)
        {
            CreateProjectFile();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = _settings.OpenProjectLastDirectoryPath;
            openFileDialog.Filter = "FragEd Project|*.fed|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;

                if (File.Exists(fileName))
                {
                    LoadProjectFile(fileName);

                    _settings.OpenProjectLastDirectoryPath = Path.GetDirectoryName(fileName);

                    _settings.Save();
                }
            }
        }
        private void SaveFile(object sender, EventArgs e)
        {
            SaveProjectFile();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsProjectFile();
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            _settings.Save();

            Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLevel();
        }

        private void OpenChild(Form child)
        {
            child.MdiParent = this;
            child.Show();
        }
        private void CreateProjectFile()
        {
            var projectConfiguration = new ProjectConfiguration();

            Project = new Project(projectConfiguration);

            CurrentProjectFile = null;
        }

        private void LoadProjectFile(string fileName)
        {
            // user chose a file
            CurrentProjectFile = fileName;

            // open the ProjectConfiguration
            var projectConfiguration = DiskStorage.LoadFromDisk<ProjectConfiguration>(fileName) ?? new ProjectConfiguration();

            Project = new Project(projectConfiguration);
        }

        private void SaveProjectFile()
        {
            if (string.IsNullOrWhiteSpace(CurrentProjectFile))
            {
                SaveAsProjectFile();
            }
            else
            {
                DiskStorage.SaveToDisk<ProjectConfiguration>(CurrentProjectFile, Project.GetConfiguration());
                Project.Levels.ForEach(l => l.Save());
            }
        }

        private void SaveAsProjectFile()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = _settings.OpenProjectLastDirectoryPath;
            saveFileDialog.Filter = "FragEd Project|*.fed|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                CurrentProjectFile = saveFileDialog.FileName;

                DiskStorage.SaveToDisk<ProjectConfiguration>(CurrentProjectFile, Project.GetConfiguration());

                UpdateUserInterface();
            }
        }

        private void UpdateUserInterface()
        {
            var fileName = !string.IsNullOrWhiteSpace(CurrentProjectFile) ? Path.GetFileName(CurrentProjectFile) : "New file *";

            Text = string.Format("{0} - {1}", _settings.AppTitle, fileName);

            //ux_AddEntityMenu.DropDownItems.Clear();
            //ux_AddEntity.DropDownItems.Clear();
            //ux_GameLevels.DropDownItems.Clear();

            //ux_ProjectMenu.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;

            saveToolStripButton.Enabled = true;

            //// enable all the children too
            //foreach( ToolStripItem item in ux_ProjectMenu.DropDownItems ) {
            //    item.Enabled = true;
            //}

            //Project.Entities.ForEach( AddEntityToUx );
            //Project.Levels.ForEach( l => ux_GameLevels.DropDownItems.Add( Path.GetFileName( l.FilePath ), null, ( sender, args ) => EditLevel( l ) ) );

            //SelectCurrentLevel();

            // TODO: Refactor this to not refresh the whole list?
            if (levelsToolStripMenuItem.DropDownItems.Count > 1)
            {
                foreach (ToolStripItem item in levelsToolStripMenuItem.DropDownItems)
                {
                    levelsToolStripMenuItem.DropDownItems.Remove(item);
                }
            }

            Project.Levels.ForEach(l => levelsToolStripMenuItem.DropDownItems.Add(Path.GetFileName(l.FilePath), null, (sender, args) => EditLevel(l)));

            projectToolStripMenu.Visible = Project != null;
        }

        private void AddLevel()
        {
            var level = new Level();

            Project.Levels.Add(level);

            UpdateUserInterface();

            EditLevel(level);
        }

        private void EditLevel(Level level)
        {
            OpenChild(new LevelEditorForm(level));
        }

        private void ProjectOnOnChange(object sender, ProjectChangeEventArgs eventArgs)
        {
            Project = (Project)sender;
            if (eventArgs.PropertyName == "ContentDirectories")
            {
                // total hack, create a level editor control
                // so that a valid GraphicsDevice object is created
                new LevelEditorControl { Height = 100, Width = 100 }.CreateControl();

                Project.ContentDirectories.ForEach(ContentCacheManager.AddContentDirectory);

                // TODO: disk op... show progress bar?
                ContentCacheManager.LoadContent(new ContentManager(ServiceLocator.Apply()));
            }

            UpdateUserInterface();
        }

        private void assemblitesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
