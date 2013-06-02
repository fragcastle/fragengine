using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FragEd.Forms;
using FragEngine.Data;
using FragEngine.Entities;
using FragEngine.Layers;
using FragEngine.View;

namespace FragEd.Controllers
{
    public abstract class ControllerBase
    {
        protected Main _mainForm;

        protected Level GetLevelFromNode( TreeNode node )
        {
            Level level = null;
            if( node.Tag as Level != null )
            {
                // this is a level
                level = (Level)node.Tag;
            }
            else if ( node.Parent != null )
            {
                level = GetLevelFromNode( node.Parent );
            }
            return level;
        }

        protected EntityBase GetEntityFromNode( TreeNode node )
        {
            EntityBase entity = null;
            if( node.Tag as Type != null )
            {
                entity = (EntityBase)Activator.CreateInstance( (Type)node.Tag );
            }

            if( node.Tag as EntityBase != null )
            {
                entity = (EntityBase)node.Tag;
            }
            return entity;
        }

        protected MapLayer GetLayerFromNode( TreeNode node )
        {
            return node.Tag as MapLayer;
        }

        protected TreeNode GetSelectedNode()
        {
            return _mainForm.ux_ProjectTree.SelectedNode;
        }
    }
}
