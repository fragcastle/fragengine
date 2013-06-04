using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEd.Data;
using FragEngine.IO;

namespace FragEd.Controllers {
    public class StateController : ControllerBase {
        private readonly static StateController CurrentState = new StateController();

        protected StateController()
        {
            
        }

        protected string ProjectFilePath { get; set; }

        public Project Project { get; set; }

        protected void Load(string filePath = null )
        {
            if( String.IsNullOrEmpty(filePath))
            {
                // new project
            }
        }

        protected void Save()
        {
            Persistant.Persist( ProjectFilePath, Project );
            Project.Levels.ForEach( l => l.Save() );
        }
        

        protected void SyncToTree()
        {
            _mainForm.ux_ProjectTree.Nodes.Clear();
        }
    }
}
