namespace FragEd.Forms {
    partial class GameAssemblies {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ux_AssemblyList = new System.Windows.Forms.ListBox();
            this.ux_Done = new System.Windows.Forms.Button();
            this.ux_Remove = new System.Windows.Forms.Button();
            this.ux_Add = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // ux_AssemblyList
            // 
            this.ux_AssemblyList.FormattingEnabled = true;
            this.ux_AssemblyList.Location = new System.Drawing.Point(12, 12);
            this.ux_AssemblyList.Name = "ux_AssemblyList";
            this.ux_AssemblyList.Size = new System.Drawing.Size(552, 225);
            this.ux_AssemblyList.TabIndex = 0;
            // 
            // ux_Done
            // 
            this.ux_Done.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ux_Done.Location = new System.Drawing.Point(13, 244);
            this.ux_Done.Name = "ux_Done";
            this.ux_Done.Size = new System.Drawing.Size(75, 23);
            this.ux_Done.TabIndex = 1;
            this.ux_Done.Text = "Done";
            this.ux_Done.UseVisualStyleBackColor = true;
            this.ux_Done.Click += new System.EventHandler(this.ux_Done_Click);
            // 
            // ux_Remove
            // 
            this.ux_Remove.Location = new System.Drawing.Point(489, 244);
            this.ux_Remove.Name = "ux_Remove";
            this.ux_Remove.Size = new System.Drawing.Size(75, 23);
            this.ux_Remove.TabIndex = 2;
            this.ux_Remove.Text = "Remove";
            this.ux_Remove.UseVisualStyleBackColor = true;
            this.ux_Remove.Click += new System.EventHandler(this.ux_Remove_Click);
            // 
            // ux_Add
            // 
            this.ux_Add.Location = new System.Drawing.Point(408, 244);
            this.ux_Add.Name = "ux_Add";
            this.ux_Add.Size = new System.Drawing.Size(75, 23);
            this.ux_Add.TabIndex = 3;
            this.ux_Add.Text = "Add...";
            this.ux_Add.UseVisualStyleBackColor = true;
            this.ux_Add.Click += new System.EventHandler(this.ux_Add_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Game Binaries|*.exe,*.dll";
            // 
            // GameAssemblies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 279);
            this.Controls.Add(this.ux_Add);
            this.Controls.Add(this.ux_Remove);
            this.Controls.Add(this.ux_Done);
            this.Controls.Add(this.ux_AssemblyList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameAssemblies";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Game Assemblies";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ux_AssemblyList;
        private System.Windows.Forms.Button ux_Done;
        private System.Windows.Forms.Button ux_Remove;
        private System.Windows.Forms.Button ux_Add;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}