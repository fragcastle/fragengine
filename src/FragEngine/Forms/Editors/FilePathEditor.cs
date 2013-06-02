using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FragEngine.Forms.Editors
{
    public class FilePathEditor : UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            string path = (string)value;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.FileName = path;
            if( openFile.ShowDialog() == DialogResult.OK )
            {
                path = openFile.FileName;
            }

            return path;
        }
    }
}
