using System;
using System.Linq;
using System.Windows.Forms;
using FragEngine;

namespace FragEd.Forms {
    public partial class AddLayer : Form {
        private Editor _editorForm;

        public AddLayer( Editor editorForm ) {
            InitializeComponent();

            _editorForm = editorForm;

            Load += ( sender, args ) => {
                    ux_TileSet.Items.Clear();
                    foreach( var pair in ContentCacheManager.TextureCache ) {
                        ux_TileSet.Items.Add( pair.Key );
                    }
                };
        }

        public string LayerName
        {
            get { return ux_LayerName.Text; }
        }

        public int TileSize
        {
            get { return Int32.Parse( ux_TileSize.Text ); }
        }

        public string TileSet
        {
            get
            {
                var path = (string)ux_TileSet.SelectedItem;
                return path.Replace( "_", "/" );
            }
        }

        private void ux_AddLayer_Click( object sender, EventArgs e ) {
            if( Validate() )
            {
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private bool Validate()
        {
            if( String.IsNullOrWhiteSpace( ux_LayerName.Text ) ) {
                MessageBox.Show( "Please enter a layer name." );
                ux_LayerName.Focus();

                return false;
            }

            if( _editorForm.CurrentLevel.MapLayers.Any( ml => ml.Name == ux_LayerName.Text ) )
            {
                MessageBox.Show( "That layer name is already in use." );
                ux_LayerName.Focus();
                ux_LayerName.SelectAll();

                return false;
            }

            if( String.IsNullOrWhiteSpace( ux_TileSize.Text ) ) {
                MessageBox.Show( "Please enter a tile size." );
                ux_TileSize.Focus();

                return false;
            }

            var num = 0;
            if( !Int32.TryParse(ux_TileSize.Text, out num) )
            {
                MessageBox.Show( "Tile Size should be a whole number." );

                ux_TileSize.Focus();
                ux_TileSize.SelectAll();

                return false;
            }

            return true;
        }
    }
}
