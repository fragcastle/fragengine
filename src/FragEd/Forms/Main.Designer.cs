using FragEd.Controls;

namespace FragEd.Forms
{
    partial class Main
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

        /// <summary>-
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ux_SolutionTabs = new System.Windows.Forms.TabControl();
            this.ux_ProjectTab = new System.Windows.Forms.TabPage();
            this.ux_projectTabTable = new System.Windows.Forms.TableLayoutPanel();
            this.ux_ProjectTree = new System.Windows.Forms.TreeView();
            this.ux_projectToolbar = new System.Windows.Forms.ToolStrip();
            this.ux_ToolStripSaveProject = new System.Windows.Forms.ToolStripButton();
            this.ux_ToolStripAddLayer = new System.Windows.Forms.ToolStripButton();
            this.ux_ToolStripAddEntityList = new System.Windows.Forms.ToolStripSplitButton();
            this.ux_ToolStripAddLevel = new System.Windows.Forms.ToolStripButton();
            this.ux_PropertiesGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.levelEditorControl1 = new FragEd.Controls.LevelEditorControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mousePosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.ux_SolutionTabs.SuspendLayout();
            this.ux_ProjectTab.SuspendLayout();
            this.ux_projectTabTable.SuspendLayout();
            this.ux_projectToolbar.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1436, 825);
            this.splitContainer1.SplitterDistance = 416;
            this.splitContainer1.TabIndex = 0;
            //
            // splitContainer2
            //
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitContainer2.Panel1
            //
            this.splitContainer2.Panel1.Controls.Add(this.ux_SolutionTabs);
            //
            // splitContainer2.Panel2
            //
            this.splitContainer2.Panel2.Controls.Add(this.ux_PropertiesGrid);
            this.splitContainer2.Size = new System.Drawing.Size(416, 825);
            this.splitContainer2.SplitterDistance = 352;
            this.splitContainer2.TabIndex = 2;
            //
            // ux_SolutionTabs
            //
            this.ux_SolutionTabs.Controls.Add(this.ux_ProjectTab);
            this.ux_SolutionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_SolutionTabs.Location = new System.Drawing.Point(0, 0);
            this.ux_SolutionTabs.Name = "ux_SolutionTabs";
            this.ux_SolutionTabs.SelectedIndex = 0;
            this.ux_SolutionTabs.Size = new System.Drawing.Size(416, 352);
            this.ux_SolutionTabs.TabIndex = 0;
            //
            // ux_ProjectTab
            //
            this.ux_ProjectTab.Controls.Add(this.ux_projectTabTable);
            this.ux_ProjectTab.Location = new System.Drawing.Point(4, 22);
            this.ux_ProjectTab.Margin = new System.Windows.Forms.Padding(2);
            this.ux_ProjectTab.Name = "ux_ProjectTab";
            this.ux_ProjectTab.Padding = new System.Windows.Forms.Padding(2);
            this.ux_ProjectTab.Size = new System.Drawing.Size(408, 326);
            this.ux_ProjectTab.TabIndex = 2;
            this.ux_ProjectTab.Text = "Project";
            this.ux_ProjectTab.UseVisualStyleBackColor = true;
            //
            // ux_projectTabTable
            //
            this.ux_projectTabTable.ColumnCount = 2;
            this.ux_projectTabTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ux_projectTabTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ux_projectTabTable.Controls.Add(this.ux_ProjectTree, 0, 1);
            this.ux_projectTabTable.Controls.Add(this.ux_projectToolbar, 0, 0);
            this.ux_projectTabTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_projectTabTable.Location = new System.Drawing.Point(2, 2);
            this.ux_projectTabTable.Margin = new System.Windows.Forms.Padding(2);
            this.ux_projectTabTable.Name = "ux_projectTabTable";
            this.ux_projectTabTable.RowCount = 2;
            this.ux_projectTabTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.695652F));
            this.ux_projectTabTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.30434F));
            this.ux_projectTabTable.Size = new System.Drawing.Size(404, 322);
            this.ux_projectTabTable.TabIndex = 1;
            //
            // ux_ProjectTree
            //
            this.ux_projectTabTable.SetColumnSpan(this.ux_ProjectTree, 2);
            this.ux_ProjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_ProjectTree.HideSelection = false;
            this.ux_ProjectTree.LabelEdit = true;
            this.ux_ProjectTree.Location = new System.Drawing.Point(2, 30);
            this.ux_ProjectTree.Margin = new System.Windows.Forms.Padding(2);
            this.ux_ProjectTree.Name = "ux_ProjectTree";
            this.ux_ProjectTree.Size = new System.Drawing.Size(400, 290);
            this.ux_ProjectTree.TabIndex = 0;
            //
            // ux_projectToolbar
            //
            this.ux_projectTabTable.SetColumnSpan(this.ux_projectToolbar, 2);
            this.ux_projectToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_projectToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_ToolStripSaveProject,
            this.ux_ToolStripAddLayer,
            this.ux_ToolStripAddEntityList,
            this.ux_ToolStripAddLevel});
            this.ux_projectToolbar.Location = new System.Drawing.Point(0, 0);
            this.ux_projectToolbar.Name = "ux_projectToolbar";
            this.ux_projectToolbar.Size = new System.Drawing.Size(404, 28);
            this.ux_projectToolbar.TabIndex = 1;
            //
            // ux_ToolStripSaveProject
            //
            this.ux_ToolStripSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_ToolStripSaveProject.Image = global::FragEd.Properties.Resources.Save_6530;
            this.ux_ToolStripSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_ToolStripSaveProject.Name = "ux_ToolStripSaveProject";
            this.ux_ToolStripSaveProject.Size = new System.Drawing.Size(23, 25);
            this.ux_ToolStripSaveProject.Text = "Save Project";
            //
            // ux_ToolStripAddLayer
            //
            this.ux_ToolStripAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_ToolStripAddLayer.Image = global::FragEd.Properties.Resources.AddControl_371;
            this.ux_ToolStripAddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_ToolStripAddLayer.Name = "ux_ToolStripAddLayer";
            this.ux_ToolStripAddLayer.Size = new System.Drawing.Size(23, 25);
            this.ux_ToolStripAddLayer.Text = "Add Layer";
            //
            // ux_ToolStripAddEntityList
            //
            this.ux_ToolStripAddEntityList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_ToolStripAddEntityList.Image = global::FragEd.Properties.Resources.AddVariable_5541;
            this.ux_ToolStripAddEntityList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_ToolStripAddEntityList.Name = "ux_ToolStripAddEntityList";
            this.ux_ToolStripAddEntityList.Size = new System.Drawing.Size(32, 25);
            this.ux_ToolStripAddEntityList.Text = "Add Entity";
            //
            // ux_ToolStripAddLevel
            //
            this.ux_ToolStripAddLevel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_ToolStripAddLevel.Image = global::FragEd.Properties.Resources.AddNewItem_6273;
            this.ux_ToolStripAddLevel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_ToolStripAddLevel.Name = "ux_ToolStripAddLevel";
            this.ux_ToolStripAddLevel.Size = new System.Drawing.Size(23, 25);
            this.ux_ToolStripAddLevel.Text = "Add Level";
            //
            // ux_PropertiesGrid
            //
            this.ux_PropertiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_PropertiesGrid.Location = new System.Drawing.Point(0, 0);
            this.ux_PropertiesGrid.Name = "ux_PropertiesGrid";
            this.ux_PropertiesGrid.Size = new System.Drawing.Size(416, 469);
            this.ux_PropertiesGrid.TabIndex = 0;
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1016, 825);
            this.tabControl1.TabIndex = 0;
            //
            // tabPage1
            //
            this.tabPage1.Controls.Add(this.levelEditorControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1008, 799);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // levelEditorControl1
            //
            this.levelEditorControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.levelEditorControl1.BackColor = System.Drawing.Color.Black;
            this.levelEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelEditorControl1.Level = null;
            this.levelEditorControl1.Location = new System.Drawing.Point(3, 3);
            this.levelEditorControl1.Margin = new System.Windows.Forms.Padding(4);
            this.levelEditorControl1.Name = "levelEditorControl1";
            this.levelEditorControl1.Size = new System.Drawing.Size(1002, 793);
            this.levelEditorControl1.TabIndex = 0;
            this.levelEditorControl1.VSync = false;
            //
            // tabPage2
            //
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1008, 799);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1436, 24);
            this.menuStrip1.TabIndex = 1;
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveLevelToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            //
            // newProjectToolStripMenuItem
            //
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.newProjectToolStripMenuItem.Text = "New ProjectConfiguration...";
            //
            // openToolStripMenuItem
            //
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            //
            // saveToolStripMenuItem
            //
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
            | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.saveToolStripMenuItem.Text = "Save &ProjectConfiguration";
            //
            // saveLevelToolStripMenuItem
            //
            this.saveLevelToolStripMenuItem.Name = "saveLevelToolStripMenuItem";
            this.saveLevelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveLevelToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.saveLevelToolStripMenuItem.Text = "&Save Level";
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(281, 6);
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            //
            // openFileDialog1
            //
            this.openFileDialog1.FileName = "openFileDialog1";
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mousePosition});
            this.statusStrip1.Location = new System.Drawing.Point(0, 827);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1436, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            //
            // mousePosition
            //
            this.mousePosition.Name = "mousePosition";
            this.mousePosition.Size = new System.Drawing.Size(0, 17);
            //
            // Main
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1436, 849);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "FragEd";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ux_SolutionTabs.ResumeLayout(false);
            this.ux_ProjectTab.ResumeLayout(false);
            this.ux_projectTabTable.ResumeLayout(false);
            this.ux_projectTabTable.PerformLayout();
            this.ux_projectToolbar.ResumeLayout(false);
            this.ux_projectToolbar.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel mousePosition;
        private System.Windows.Forms.SplitContainer splitContainer2;
        public System.Windows.Forms.PropertyGrid ux_PropertiesGrid;
        public System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.TabControl ux_SolutionTabs;
        private System.Windows.Forms.TabPage ux_ProjectTab;
        private System.Windows.Forms.TableLayoutPanel ux_projectTabTable;
        public System.Windows.Forms.TreeView ux_ProjectTree;
        private System.Windows.Forms.ToolStrip ux_projectToolbar;
        public LevelEditorControl levelEditorControl1;
        public System.Windows.Forms.ToolStripMenuItem saveLevelToolStripMenuItem;
        public System.Windows.Forms.ToolStripButton ux_ToolStripSaveProject;
        private System.Windows.Forms.ToolStripButton ux_ToolStripAddLayer;
        public System.Windows.Forms.ToolStripSplitButton ux_ToolStripAddEntityList;
        public System.Windows.Forms.ToolStripButton ux_ToolStripAddLevel;
        public System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}

