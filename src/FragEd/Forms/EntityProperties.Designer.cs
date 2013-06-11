namespace FragEd.Forms
{
    partial class EntityProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ux_Done = new System.Windows.Forms.Button();
            this.ux_AnimationList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ux_EntitySettings = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ux_Done
            // 
            this.ux_Done.Location = new System.Drawing.Point(313, 400);
            this.ux_Done.Name = "ux_Done";
            this.ux_Done.Size = new System.Drawing.Size(75, 23);
            this.ux_Done.TabIndex = 0;
            this.ux_Done.Text = "Done";
            this.ux_Done.UseVisualStyleBackColor = true;
            this.ux_Done.Click += new System.EventHandler(this.ux_Done_Click);
            // 
            // ux_AnimationList
            // 
            this.ux_AnimationList.FormattingEnabled = true;
            this.ux_AnimationList.Location = new System.Drawing.Point(141, 23);
            this.ux_AnimationList.Name = "ux_AnimationList";
            this.ux_AnimationList.Size = new System.Drawing.Size(247, 24);
            this.ux_AnimationList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Starting Animation";
            // 
            // ux_EntitySettings
            // 
            this.ux_EntitySettings.Location = new System.Drawing.Point(15, 136);
            this.ux_EntitySettings.Name = "ux_EntitySettings";
            this.ux_EntitySettings.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ux_EntitySettings.Size = new System.Drawing.Size(373, 258);
            this.ux_EntitySettings.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Settings";
            // 
            // EntityProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 435);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ux_EntitySettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ux_AnimationList);
            this.Controls.Add(this.ux_Done);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EntityProperties";
            this.Text = "Properties For";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ux_Done;
        private System.Windows.Forms.ComboBox ux_AnimationList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid ux_EntitySettings;
        private System.Windows.Forms.Label label2;
    }
}