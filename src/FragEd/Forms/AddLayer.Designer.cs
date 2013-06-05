namespace FragEd.Forms {
    partial class AddLayer {
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
            this.label1 = new System.Windows.Forms.Label();
            this.ux_LayerName = new System.Windows.Forms.TextBox();
            this.ux_AddLayer = new System.Windows.Forms.Button();
            this.ux_Cancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ux_TileSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ux_TileSet = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Layer Name";
            // 
            // ux_LayerName
            // 
            this.ux_LayerName.Location = new System.Drawing.Point(16, 30);
            this.ux_LayerName.Name = "ux_LayerName";
            this.ux_LayerName.Size = new System.Drawing.Size(259, 20);
            this.ux_LayerName.TabIndex = 1;
            // 
            // ux_AddLayer
            // 
            this.ux_AddLayer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ux_AddLayer.Location = new System.Drawing.Point(16, 170);
            this.ux_AddLayer.Name = "ux_AddLayer";
            this.ux_AddLayer.Size = new System.Drawing.Size(75, 23);
            this.ux_AddLayer.TabIndex = 4;
            this.ux_AddLayer.Text = "Add Layer";
            this.ux_AddLayer.UseVisualStyleBackColor = true;
            this.ux_AddLayer.Click += new System.EventHandler(this.ux_AddLayer_Click);
            // 
            // ux_Cancel
            // 
            this.ux_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ux_Cancel.Location = new System.Drawing.Point(182, 170);
            this.ux_Cancel.Name = "ux_Cancel";
            this.ux_Cancel.Size = new System.Drawing.Size(75, 23);
            this.ux_Cancel.TabIndex = 5;
            this.ux_Cancel.Text = "Cancel";
            this.ux_Cancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tile Size (pixels)";
            // 
            // ux_TileSize
            // 
            this.ux_TileSize.Location = new System.Drawing.Point(16, 70);
            this.ux_TileSize.Name = "ux_TileSize";
            this.ux_TileSize.Size = new System.Drawing.Size(100, 20);
            this.ux_TileSize.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Tile Set Image";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            this.openFileDialog1.Filter = "Image Files|*.png,*.jpg";
            this.openFileDialog1.Title = "Select a Tile Set";
            // 
            // ux_TileSet
            // 
            this.ux_TileSet.FormattingEnabled = true;
            this.ux_TileSet.Location = new System.Drawing.Point(19, 114);
            this.ux_TileSet.Name = "ux_TileSet";
            this.ux_TileSet.Size = new System.Drawing.Size(256, 21);
            this.ux_TileSet.TabIndex = 3;
            // 
            // AddLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 205);
            this.Controls.Add(this.ux_TileSet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ux_TileSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ux_Cancel);
            this.Controls.Add(this.ux_AddLayer);
            this.Controls.Add(this.ux_LayerName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddLayer";
            this.Text = "Add Layer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ux_LayerName;
        private System.Windows.Forms.Button ux_AddLayer;
        private System.Windows.Forms.Button ux_Cancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ux_TileSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox ux_TileSet;
    }
}