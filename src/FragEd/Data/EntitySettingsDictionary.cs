using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FragEngine.Entities;
using Wexman.Design;

namespace FragEd.Data {
    public class EditableEntity
    {
        private Entity _entity;

        [Editor(typeof(GenericDictionaryEditor<string,string>), typeof(UITypeEditor))]
        public Dictionary<string, string> Settings
        {
            get { return _entity.Settings; }
            set { _entity.Settings = value; }
        }

        public EditableEntity( Entity entity )
        {
            _entity = entity;
        }
    }
}
