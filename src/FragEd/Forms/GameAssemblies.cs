using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FragEd.Controllers;

namespace FragEd.Forms {
    public partial class GameAssemblies : Form {
        private readonly Editor _editor;

        public GameAssemblies( Editor editor ) {
            AddedEntities = new List<Type>();
            RemovedEntities = new List<Type>();

            InitializeComponent();

            Load += OnLoad;

            _editor = editor;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            ux_AssemblyList.Items.Clear();
            var asmPaths = _editor.Project.Entities.Select( e => Path.GetFullPath( e.Assembly.Location ) ).Distinct().ToArray();
            ux_AssemblyList.Items.AddRange( asmPaths );
        }

        public IEnumerable<Type> AddedEntities { get; private set; }

        public IEnumerable<Type> RemovedEntities { get; private set; }

        private void ux_Done_Click( object sender, EventArgs e ) {
            this.Hide();
        }

        private void ux_Add_Click( object sender, EventArgs args ) {
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var result = openFileDialog1.ShowDialog();
            if( result == DialogResult.OK )
            {
                var filename = openFileDialog1.FileName;
                var assembly = AssemblyResolutionManager.Current.Add( filename );

                AddedEntities = assembly.GetEntities();

                ux_AssemblyList.Items.Add( filename );
            }
        }

        private void ux_Remove_Click( object sender, EventArgs args ) {
            var filename = (string)ux_AssemblyList.SelectedItem;

            if( filename == null )
                return;

            var removedEntities = _editor.Project.Entities.Where( e => e.Assembly.Location == filename );

            RemovedEntities = RemovedEntities.Concat( removedEntities );
        }
    }
}
