using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FragEd.Controllers;
using FragEngine.Data;

namespace FragEd.Controls
{
    public class LevelTabPage : TabPage
    {
        public LevelEditorControl Editor { get; private set; }

        public Level Level { get; private set; }

        public LevelTabPage( Level level )
        {
            Editor = new LevelEditorControl()
            {
                Name = "Editor",
                Level = level,
                Dock = DockStyle.Fill
            };

            Level = level;

            level.OnDataChanged += ( sender, args ) => Text = level.Name + "*";
            level.OnPersisted += ( sender, args ) => Text = level.Name;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            Controls.Add( Editor );

            // LevelRendererController.Current.ListenTo( Editor );

            Text = Level.Name;
        }
    }
}
