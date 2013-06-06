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
    public partial class ContentFolders : Form {
        private readonly Editor _editor;

        public ContentFolders( Editor editor ) {
            AddedFolders = new List<string>();
            RemovedFolders = new List<string>();
            
            InitializeComponent();

            Load += OnLoad;

            _editor = editor;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            ux_ContentFolderList.Items.Clear();

            var paths = _editor.Project.ContentDirectories.Select( d => d.FullName ).ToArray();

            ux_ContentFolderList.Items.AddRange( paths );
        }

        public List<string> AddedFolders { get; private set; }

        public List<string> RemovedFolders { get; private set; } 

        private void ux_Done_Click( object sender, EventArgs e ) {
            this.Hide();
        }

        private void ux_Add_Click( object sender, EventArgs args ) {
            folderBrowserDialog1.ShowNewFolderButton = false;

            var result = folderBrowserDialog1.ShowDialog();
            if( result == DialogResult.OK )
            {
                var folder = folderBrowserDialog1.SelectedPath;

                ux_ContentFolderList.Items.Add( folder );

                AddedFolders.Add(folder);
            }
        }

        private void ux_Remove_Click( object sender, EventArgs args ) {
            var folder = (string)ux_ContentFolderList.SelectedItem;

            if( folder == null )
                return;

            RemovedFolders.Add( folder );
        }
    }
}
