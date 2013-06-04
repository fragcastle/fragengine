using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FragEd.Controls;
using FragEd.Forms;
using FragEd.Services;
using FragEngine.Data;
using FragEngine.IO;
using FragEngine.Layers;

namespace FragEd.Controllers
{
    public class LevelController : ControllerBase
    {
        public EventHandler<SelectedLevelEventArgs> LevelSelected;

        public Dictionary<string, LevelTabPage> OpenLevels { get; private set; }

        private int _newLevelCount = 0;

        public static LevelController BoundTo(Main form)
        {
            return new LevelController(form);
        }

        private LevelController( Main form )
        {
            _mainForm = form;
            _mainForm.ux_ProjectTree.AfterSelect += UxProjectTreeOnAfterSelect;
            _mainForm.ux_ProjectTree.NodeMouseDoubleClick += UxProjectTreeOnNodeMouseDoubleClick;
            _mainForm.ux_ToolStripAddLevel.Click += ( sender, args ) => New();

            _mainForm.saveLevelDialog.FileOk += SaveFileDialog1OnFileOk;

            OpenLevels = new Dictionary<string, LevelTabPage>();

            EventAggregator.Current.On("add:entity").Add(OnAddEntity);
        }

        private void SaveFileDialog1OnFileOk(object sender, CancelEventArgs cancelEventArgs)
        {
            var levelFilePath = _mainForm.saveLevelDialog.FileName;

            var levelsNode = _mainForm.ux_ProjectTree.Nodes.Find( "levels", true ).First();

            var level = new Level() {
                Name = Path.GetFileNameWithoutExtension(levelFilePath),
                FilePath = levelFilePath
            };

            level.Save();

            var node = levelsNode.Nodes.Add( level.Name );

            node.Tag = level;

            OpenLevel( level, node );

            if( !levelsNode.IsExpanded ) {
                levelsNode.Expand();
            }

            node.Expand();

            EventAggregator.Current.Trigger("add:level", level );
        }

        private void New()
        {
            _mainForm.saveLevelDialog.CheckPathExists = false;
            _mainForm.saveLevelDialog.AddExtension = true;
            _mainForm.saveLevelDialog.Filter = "FragEd Level|*.json";
            _mainForm.saveLevelDialog.DefaultExt = ".json";
            _mainForm.saveLevelDialog.FileName = "NewLevel";
            _mainForm.saveLevelDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
            _mainForm.saveLevelDialog.SupportMultiDottedExtensions = true;
            _mainForm.saveLevelDialog.Title = "Save your new level";
            _mainForm.saveLevelDialog.ShowDialog();
        }

        private void OnAddEntity( Object sender, EventArgs e )
        {
            var level = GetLevelFromNode( (TreeNode)sender );
            var entity = GetEntityFromNode( (TreeNode)sender );
            level.Entities.Add( entity );
        }

        private void UxProjectTreeOnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs treeNodeMouseClickEventArgs)
        {
            var node = treeNodeMouseClickEventArgs.Node;
            var level = GetLevelFromNode( node );

            if( level != null )
            {
                OpenLevel( level, treeNodeMouseClickEventArgs.Node );
            }
        }

        private void UxProjectTreeOnAfterSelect( object sender, TreeViewEventArgs treeViewEventArgs )
        {
            var node = treeViewEventArgs.Node;
            var level = GetLevelFromNode( node );
            var entity = GetEntityFromNode( node );
            var layer = GetLayerFromNode( node );

            if( entity != null )
            {
                // open entity in the properties window
                _mainForm.ux_PropertiesGrid.SelectedObject = entity;
            }

            if( layer != null )
            {
                _mainForm.ux_PropertiesGrid.SelectedObject = layer;

                level.MapLayers.ForEach( l => l.Alpha = l != layer ? 128f : 255f );
            }
        }

        private void OpenLevel( Level level, TreeNode node )
        {
            LevelTabPage tabPage;
            // check if the level is already open
            if( OpenLevels.ContainsKey(level.Name) )
            {
                // set the tab index so this tab will be selected
                tabPage = OpenLevels[ level.Name ];
            }
            else
            {
                // level isn't already open, create a new tab page for it
                tabPage = new LevelTabPage( level );

                // create the layer and entities sub nodes
                var layers = node.Nodes.Add( "Layers" );
                var entities = node.Nodes.Add( "Entities" );

                // load the entities for this level
                foreach( var ent in level.Entities )
                {
                    var entityNode = entities.Nodes.Add( ent.GetType().Name );
                    entityNode.Tag = ent;
                }

                // load the layers
                foreach(var layer in level.MapLayers)
                {
                    var layerNode = layers.Nodes.Add( layer.Name );
                    layerNode.Tag = layer;
                }

                // add the collision layer
                var collision = layers.Nodes.Add("Collision");
                collision.Tag = level.CollisionLayer;

                OpenLevels.Add(level.Name, tabPage);
                _mainForm.tabControl1.TabPages.Add( tabPage );
            }

            // set the level as the active tab
            tabPage.Select();
        }

        public void Save()
        {
            var tabPage = (LevelTabPage)_mainForm.tabControl1.SelectedTab;
            tabPage.Level.Save();
        }

        public void SaveAll()
        {
            foreach(var page in OpenLevels)
            {
                page.Value.Level.Save();
            }
        }
    }
}
