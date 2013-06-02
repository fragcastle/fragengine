using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FragEd.Controls;
using FragEd.Forms;
using FragEd.Services;
using FragEngine.Data;
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
            _mainForm.saveLevelToolStripMenuItem.Click += (sender, args) => Save();
            _mainForm.tabControl1.TabIndexChanged += ( sender, args ) => _mainForm.saveLevelToolStripMenuItem.Text = "Save " + ((LevelTabPage)sender).Level.Name;
            _mainForm.ux_ToolStripAddLevel.Click += ( sender, args ) => New();

            _mainForm.ux_ProjectTree.AfterLabelEdit += UxProjectTreeOnAfterLabelEdit;

            OpenLevels = new Dictionary<string, LevelTabPage>();

            EventAggregator.Current.On("add:entity").Add(OnAddEntity);
        }

        private void UxProjectTreeOnAfterLabelEdit(object sender, NodeLabelEditEventArgs nodeLabelEditEventArgs)
        {
            var node = nodeLabelEditEventArgs.Node;

            var label = nodeLabelEditEventArgs.Label;

            var level = GetLevelFromNode( node );

            if( level != null )
            {
                if( OpenLevels.ContainsKey( level.Name ) )
                {
                    var page = OpenLevels[ level.Name ];
                    page.Text = label;
                }

                level.Name = label;
            }
        }

        private void New()
        {
            var levelsNode = _mainForm.ux_ProjectTree.Nodes.Find( "levels", true ).First();

            var level = new Level()
                {
                    Name = "New Level" + _newLevelCount
                };

            _newLevelCount++;

            var node = levelsNode.Nodes.Add( level.Name );

            node.Tag = level;

            OpenLevel( level, node );

            if( !levelsNode.IsExpanded )
            {
                levelsNode.Expand();
            }

            node.Expand();

            node.BeginEdit();
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

                level.Layers.ForEach( l => l.Alpha = l != layer ? 128f : 255f );
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
                foreach(var layer in level.MapLayers())
                {
                    var layerNode = layers.Nodes.Add( layer.Name );
                    layerNode.Tag = layer;
                }

                // add the collision layer
                var collision = layers.Nodes.Add("Collision");
                collision.Tag = level.CollisionLayer();

                OpenLevels.Add(level.Name, tabPage);
                _mainForm.tabControl1.TabPages.Add( tabPage );
            }

            // set the level as the active tab
            tabPage.Select();

            _mainForm.saveLevelToolStripMenuItem.Text = string.Format("Save {0}", level.Name);
            _mainForm.saveLevelToolStripMenuItem.Enabled = true;
        }

        public void Save()
        {
            var tabPage = (LevelTabPage)_mainForm.tabControl1.SelectedTab;
            tabPage.Level.Save();
        }
    }
}
