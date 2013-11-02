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
        private Actor _actor;

        public EntityProperties( Actor actor )
        {
            InitializeComponent();

            _actor = actor;

            Load += (sender, args) => PopulateForm();

            ux_AnimationList.SelectedIndexChanged +=
                (sender, args) => _actor.CurrentAnimation = (string)ux_AnimationList.SelectedItem;
        }

        private void PopulateForm()
        {
            ux_AnimationList.Items.Clear();
            ux_EntitySettings.SelectedObject = null;

            foreach (var animation in _actor.Animations.GetAnimations())
            {
                ux_AnimationList.Items.Add(animation.Name);
                if (animation.Name == _actor.CurrentAnimation)
                {
                    ux_AnimationList.SelectedIndex = ux_AnimationList.Items.Count - 1;
                }
            }

            ux_EntitySettings.SelectedObject = new EditableEntity( _actor );
        }

        private void ux_Done_Click( object sender, EventArgs e )
        {
            Hide();

            _actor.Settings = ( (EditableEntity)ux_EntitySettings.SelectedObject ).Settings;

            Close();
        }
    }
}
