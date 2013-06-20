using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FragEd.Data;
using FragEngine.Entities;

namespace FragEd.Forms
{
    public partial class EntityProperties : Form
    {
        private EntityBase _entity;

        public EntityProperties( EntityBase entity )
        {
            InitializeComponent();

            _entity = entity;

            Load += (sender, args) => PopulateForm();

            ux_AnimationList.SelectedIndexChanged +=
                (sender, args) => _entity.CurrentAnimation = (string)ux_AnimationList.SelectedItem;
        }

        private void PopulateForm()
        {
            ux_AnimationList.Items.Clear();
            ux_EntitySettings.SelectedObject = null;
            
            foreach (var animation in _entity.Animations.GetAnimations())
            {
                ux_AnimationList.Items.Add(animation.Name);
                if (animation.Name == _entity.CurrentAnimation)
                {
                    ux_AnimationList.SelectedIndex = ux_AnimationList.Items.Count - 1;
                }
            }

            ux_EntitySettings.SelectedObject = new EditableEntity( _entity );
        }

        private void ux_Done_Click( object sender, EventArgs e )
        {
            Hide();

            _entity.Settings = ( (EditableEntity)ux_EntitySettings.SelectedObject ).Settings;

            Close();
        }
    }
}
