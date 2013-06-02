using System;
using System.Windows.Forms;
using FragEd.Controllers;

namespace FragEd.Forms
{
    public partial class Main : Form
    {
        private ProjectController _projectController;
        private LevelController _levelController;
        private LevelRendererController _levelRenderController;

        public Main()
        {
            InitializeComponent();

            Load += ( sender, args ) => tabControl1.TabPages.Clear();

            saveToolStripMenuItem.Enabled = false;
            saveLevelToolStripMenuItem.Enabled = false;

            _levelRenderController = LevelRendererController.BoundTo( this );
            _projectController = ProjectController.BoundTo(this);
            _levelController = LevelController.BoundTo( this );
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }
    }
}
