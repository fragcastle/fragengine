namespace FragEd.Forms {
    partial class Editor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.ux_StatusBar = new System.Windows.Forms.StatusStrip();
            this.debugStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ux_MenuBar = new System.Windows.Forms.MenuStrip();
            this.ux_FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_NewProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_OpenProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_SaveProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ux_ExitApplicationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_ProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_AddEntityMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_GameLevels = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_ManageContentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_ManageGameAssemblies = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ux_AddLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_AddExistingLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_OpenProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.ux_SaveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.ux_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ux_LayerList = new System.Windows.Forms.CheckedListBox();
            this.ux_LayersToolStrip = new System.Windows.Forms.ToolStrip();
            this.ux_AddLayer = new System.Windows.Forms.ToolStripButton();
            this.ux_RemoveLayer = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ux_LevelEntityList = new System.Windows.Forms.CheckedListBox();
            this.ux_EntitiesToolStrip = new System.Windows.Forms.ToolStrip();
            this.ux_AddEntity = new System.Windows.Forms.ToolStripDropDownButton();
            this.ux_RemoveEntity = new System.Windows.Forms.ToolStripButton();
            this.ux_LevelEditor = new FragEd.Controls.LevelEditorControl();
            this.ux_SaveLevelDialog = new System.Windows.Forms.SaveFileDialog();
            this.ux_OpenLevelDialog = new System.Windows.Forms.OpenFileDialog();
            this.ux_StatusBar.SuspendLayout();
            this.ux_MenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_SplitContainer)).BeginInit();
            this.ux_SplitContainer.Panel1.SuspendLayout();
            this.ux_SplitContainer.Panel2.SuspendLayout();
            this.ux_SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ux_LayersToolStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.ux_EntitiesToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ux_StatusBar
            // 
            this.ux_StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugStatus});
            this.ux_StatusBar.Location = new System.Drawing.Point(0, 686);
            this.ux_StatusBar.Name = "ux_StatusBar";
            this.ux_StatusBar.Size = new System.Drawing.Size(998, 22);
            this.ux_StatusBar.TabIndex = 0;
            this.ux_StatusBar.Text = "statusStrip1";
            // 
            // debugStatus
            // 
            this.debugStatus.Name = "debugStatus";
            this.debugStatus.Size = new System.Drawing.Size(118, 17);
            this.debugStatus.Text = "toolStripStatusLabel1";
            // 
            // ux_MenuBar
            // 
            this.ux_MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_FileMenu,
            this.ux_ProjectMenu});
            this.ux_MenuBar.Location = new System.Drawing.Point(0, 0);
            this.ux_MenuBar.Name = "ux_MenuBar";
            this.ux_MenuBar.Size = new System.Drawing.Size(998, 24);
            this.ux_MenuBar.TabIndex = 1;
            // 
            // ux_FileMenu
            // 
            this.ux_FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_NewProjectMenu,
            this.ux_OpenProjectMenu,
            this.ux_SaveProjectMenu,
            this.toolStripMenuItem1,
            this.ux_ExitApplicationMenu});
            this.ux_FileMenu.Name = "ux_FileMenu";
            this.ux_FileMenu.Size = new System.Drawing.Size(37, 20);
            this.ux_FileMenu.Text = "&File";
            // 
            // ux_NewProjectMenu
            // 
            this.ux_NewProjectMenu.Name = "ux_NewProjectMenu";
            this.ux_NewProjectMenu.Size = new System.Drawing.Size(152, 22);
            this.ux_NewProjectMenu.Text = "&New Project...";
            this.ux_NewProjectMenu.Click += new System.EventHandler(this.ux_NewProjectMenu_Click);
            // 
            // ux_OpenProjectMenu
            // 
            this.ux_OpenProjectMenu.Name = "ux_OpenProjectMenu";
            this.ux_OpenProjectMenu.Size = new System.Drawing.Size(152, 22);
            this.ux_OpenProjectMenu.Text = "&Open Project...";
            this.ux_OpenProjectMenu.Click += new System.EventHandler(this.ux_OpenProjectMenu_Click);
            // 
            // ux_SaveProjectMenu
            // 
            this.ux_SaveProjectMenu.Enabled = false;
            this.ux_SaveProjectMenu.Name = "ux_SaveProjectMenu";
            this.ux_SaveProjectMenu.Size = new System.Drawing.Size(152, 22);
            this.ux_SaveProjectMenu.Text = "&Save Project";
            this.ux_SaveProjectMenu.Click += new System.EventHandler(this.ux_SaveProjectMenu_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // ux_ExitApplicationMenu
            // 
            this.ux_ExitApplicationMenu.Name = "ux_ExitApplicationMenu";
            this.ux_ExitApplicationMenu.Size = new System.Drawing.Size(152, 22);
            this.ux_ExitApplicationMenu.Text = "E&xit";
            this.ux_ExitApplicationMenu.Click += new System.EventHandler(this.ux_ExitApplicationMenu_Click);
            // 
            // ux_ProjectMenu
            // 
            this.ux_ProjectMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_AddEntityMenu,
            this.ux_GameLevels,
            this.ux_ManageContentFolders,
            this.ux_ManageGameAssemblies,
            this.toolStripSeparator1,
            this.ux_AddLevel,
            this.ux_AddExistingLevel});
            this.ux_ProjectMenu.Enabled = false;
            this.ux_ProjectMenu.Name = "ux_ProjectMenu";
            this.ux_ProjectMenu.RightToLeftAutoMirrorImage = true;
            this.ux_ProjectMenu.Size = new System.Drawing.Size(56, 20);
            this.ux_ProjectMenu.Text = "&Project";
            // 
            // ux_AddEntityMenu
            // 
            this.ux_AddEntityMenu.Enabled = false;
            this.ux_AddEntityMenu.Name = "ux_AddEntityMenu";
            this.ux_AddEntityMenu.Size = new System.Drawing.Size(213, 22);
            this.ux_AddEntityMenu.Text = "Game Entities";
            // 
            // ux_GameLevels
            // 
            this.ux_GameLevels.Enabled = false;
            this.ux_GameLevels.Name = "ux_GameLevels";
            this.ux_GameLevels.Size = new System.Drawing.Size(213, 22);
            this.ux_GameLevels.Text = "Game Levels";
            // 
            // ux_ManageContentFolders
            // 
            this.ux_ManageContentFolders.Enabled = false;
            this.ux_ManageContentFolders.Name = "ux_ManageContentFolders";
            this.ux_ManageContentFolders.Size = new System.Drawing.Size(213, 22);
            this.ux_ManageContentFolders.Text = "Manage Content Folders";
            this.ux_ManageContentFolders.Click += new System.EventHandler(this.ux_ManageContentFolders_Click);
            // 
            // ux_ManageGameAssemblies
            // 
            this.ux_ManageGameAssemblies.Enabled = false;
            this.ux_ManageGameAssemblies.Name = "ux_ManageGameAssemblies";
            this.ux_ManageGameAssemblies.Size = new System.Drawing.Size(213, 22);
            this.ux_ManageGameAssemblies.Text = "Manage Game Assemblies";
            this.ux_ManageGameAssemblies.Click += new System.EventHandler(this.ux_ManageGameAssemblies_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // ux_AddLevel
            // 
            this.ux_AddLevel.Enabled = false;
            this.ux_AddLevel.Name = "ux_AddLevel";
            this.ux_AddLevel.Size = new System.Drawing.Size(213, 22);
            this.ux_AddLevel.Text = "Add New Level";
            this.ux_AddLevel.Click += new System.EventHandler(this.ux_AddLevel_Click);
            // 
            // ux_AddExistingLevel
            // 
            this.ux_AddExistingLevel.Enabled = false;
            this.ux_AddExistingLevel.Name = "ux_AddExistingLevel";
            this.ux_AddExistingLevel.Size = new System.Drawing.Size(213, 22);
            this.ux_AddExistingLevel.Text = "Add Existing Level";
            this.ux_AddExistingLevel.Click += new System.EventHandler(this.ux_AddExistingLevel_Click);
            // 
            // ux_OpenProjectDialog
            // 
            this.ux_OpenProjectDialog.DefaultExt = "fed";
            this.ux_OpenProjectDialog.Filter = "FragEd Project|*.fed";
            this.ux_OpenProjectDialog.Title = "Open A Project";
            // 
            // ux_SaveProjectDialog
            // 
            this.ux_SaveProjectDialog.DefaultExt = "fed";
            this.ux_SaveProjectDialog.Filter = "FragEd Project|*.fed";
            this.ux_SaveProjectDialog.Title = "Save Project";
            // 
            // ux_SplitContainer
            // 
            this.ux_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_SplitContainer.Location = new System.Drawing.Point(0, 24);
            this.ux_SplitContainer.Name = "ux_SplitContainer";
            // 
            // ux_SplitContainer.Panel1
            // 
            this.ux_SplitContainer.Panel1.Controls.Add(this.splitContainer1);
            // 
            // ux_SplitContainer.Panel2
            // 
            this.ux_SplitContainer.Panel2.Controls.Add(this.ux_LevelEditor);
            this.ux_SplitContainer.Size = new System.Drawing.Size(998, 662);
            this.ux_SplitContainer.SplitterDistance = 320;
            this.ux_SplitContainer.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(320, 662);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ux_LayerList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ux_LayersToolStrip, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(320, 291);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ux_LayerList
            // 
            this.ux_LayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_LayerList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.ux_LayerList, 2);
            this.ux_LayerList.ColumnWidth = 1;
            this.ux_LayerList.FormattingEnabled = true;
            this.ux_LayerList.Location = new System.Drawing.Point(3, 28);
            this.ux_LayerList.MinimumSize = new System.Drawing.Size(0, 100);
            this.ux_LayerList.Name = "ux_LayerList";
            this.ux_LayerList.Size = new System.Drawing.Size(314, 255);
            this.ux_LayerList.TabIndex = 0;
            // 
            // ux_LayersToolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ux_LayersToolStrip, 2);
            this.ux_LayersToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_LayersToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ux_LayersToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_AddLayer,
            this.ux_RemoveLayer});
            this.ux_LayersToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ux_LayersToolStrip.MinimumSize = new System.Drawing.Size(320, 25);
            this.ux_LayersToolStrip.Name = "ux_LayersToolStrip";
            this.ux_LayersToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ux_LayersToolStrip.Size = new System.Drawing.Size(320, 25);
            this.ux_LayersToolStrip.TabIndex = 1;
            // 
            // ux_AddLayer
            // 
            this.ux_AddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_AddLayer.Image = global::FragEd.Properties.Resources.action_add_16xMD;
            this.ux_AddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_AddLayer.Name = "ux_AddLayer";
            this.ux_AddLayer.Size = new System.Drawing.Size(23, 22);
            this.ux_AddLayer.Text = "Add Layer";
            this.ux_AddLayer.Click += new System.EventHandler(this.ux_AddLayer_Click);
            // 
            // ux_RemoveLayer
            // 
            this.ux_RemoveLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_RemoveLayer.Image = global::FragEd.Properties.Resources.Offline_16xMD;
            this.ux_RemoveLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_RemoveLayer.Name = "ux_RemoveLayer";
            this.ux_RemoveLayer.Size = new System.Drawing.Size(23, 22);
            this.ux_RemoveLayer.Text = "Remove Layer";
            this.ux_RemoveLayer.Click += new System.EventHandler(this.ux_RemoveLayer_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ux_LevelEntityList, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.ux_EntitiesToolStrip, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(320, 367);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // ux_LevelEntityList
            // 
            this.ux_LevelEntityList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel2.SetColumnSpan(this.ux_LevelEntityList, 2);
            this.ux_LevelEntityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_LevelEntityList.FormattingEnabled = true;
            this.ux_LevelEntityList.Location = new System.Drawing.Point(3, 28);
            this.ux_LevelEntityList.Name = "ux_LevelEntityList";
            this.ux_LevelEntityList.Size = new System.Drawing.Size(314, 336);
            this.ux_LevelEntityList.TabIndex = 0;
            // 
            // ux_EntitiesToolStrip
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.ux_EntitiesToolStrip, 2);
            this.ux_EntitiesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_AddEntity,
            this.ux_RemoveEntity});
            this.ux_EntitiesToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ux_EntitiesToolStrip.Name = "ux_EntitiesToolStrip";
            this.ux_EntitiesToolStrip.Size = new System.Drawing.Size(320, 25);
            this.ux_EntitiesToolStrip.TabIndex = 1;
            this.ux_EntitiesToolStrip.Text = "toolStrip1";
            // 
            // ux_AddEntity
            // 
            this.ux_AddEntity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_AddEntity.Image = global::FragEd.Properties.Resources.action_add_16xMD;
            this.ux_AddEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_AddEntity.Name = "ux_AddEntity";
            this.ux_AddEntity.Size = new System.Drawing.Size(29, 22);
            this.ux_AddEntity.Text = "toolStripButton1";
            // 
            // ux_RemoveEntity
            // 
            this.ux_RemoveEntity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ux_RemoveEntity.Image = global::FragEd.Properties.Resources.Offline_16xMD;
            this.ux_RemoveEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ux_RemoveEntity.Name = "ux_RemoveEntity";
            this.ux_RemoveEntity.Size = new System.Drawing.Size(23, 22);
            this.ux_RemoveEntity.Text = "toolStripButton1";
            this.ux_RemoveEntity.Click += new System.EventHandler(this.ux_RemoveEntity_Click);
            // 
            // ux_LevelEditor
            // 
            this.ux_LevelEditor.BackColor = System.Drawing.Color.Black;
            this.ux_LevelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ux_LevelEditor.Level = null;
            this.ux_LevelEditor.Location = new System.Drawing.Point(0, 0);
            this.ux_LevelEditor.Name = "ux_LevelEditor";
            this.ux_LevelEditor.Size = new System.Drawing.Size(674, 662);
            this.ux_LevelEditor.TabIndex = 2;
            this.ux_LevelEditor.VSync = false;
            // 
            // ux_SaveLevelDialog
            // 
            this.ux_SaveLevelDialog.DefaultExt = "json";
            this.ux_SaveLevelDialog.Filter = "FragEngine Level|*.json";
            this.ux_SaveLevelDialog.Title = "Save Level";
            // 
            // ux_OpenLevelDialog
            // 
            this.ux_OpenLevelDialog.DefaultExt = "json";
            this.ux_OpenLevelDialog.Filter = "FragEngine Level|*.json";
            this.ux_OpenLevelDialog.Title = "Add Existing Level";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 708);
            this.Controls.Add(this.ux_SplitContainer);
            this.Controls.Add(this.ux_StatusBar);
            this.Controls.Add(this.ux_MenuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ux_MenuBar;
            this.Name = "Editor";
            this.Text = "FragEd";
            this.ux_StatusBar.ResumeLayout(false);
            this.ux_StatusBar.PerformLayout();
            this.ux_MenuBar.ResumeLayout(false);
            this.ux_MenuBar.PerformLayout();
            this.ux_SplitContainer.Panel1.ResumeLayout(false);
            this.ux_SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ux_SplitContainer)).EndInit();
            this.ux_SplitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ux_LayersToolStrip.ResumeLayout(false);
            this.ux_LayersToolStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ux_EntitiesToolStrip.ResumeLayout(false);
            this.ux_EntitiesToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ux_StatusBar;
        private System.Windows.Forms.MenuStrip ux_MenuBar;
        private System.Windows.Forms.ToolStripMenuItem ux_FileMenu;
        private System.Windows.Forms.ToolStripMenuItem ux_NewProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem ux_OpenProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem ux_SaveProjectMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ux_ExitApplicationMenu;
        private System.Windows.Forms.ToolStripMenuItem ux_ProjectMenu;
        private System.Windows.Forms.OpenFileDialog ux_OpenProjectDialog;
        private System.Windows.Forms.SaveFileDialog ux_SaveProjectDialog;
        private Controls.LevelEditorControl ux_LevelEditor;
        private System.Windows.Forms.ToolStripMenuItem ux_AddEntityMenu;
        private System.Windows.Forms.ToolStripMenuItem ux_GameLevels;
        private System.Windows.Forms.ToolStripMenuItem ux_ManageContentFolders;
        private System.Windows.Forms.ToolStripMenuItem ux_ManageGameAssemblies;
        private System.Windows.Forms.SplitContainer ux_SplitContainer;
        private System.Windows.Forms.CheckedListBox ux_LayerList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ux_AddLevel;
        private System.Windows.Forms.SaveFileDialog ux_SaveLevelDialog;
        private System.Windows.Forms.ToolStripMenuItem ux_AddExistingLevel;
        private System.Windows.Forms.OpenFileDialog ux_OpenLevelDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox ux_LevelEntityList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip ux_LayersToolStrip;
        private System.Windows.Forms.ToolStripButton ux_AddLayer;
        private System.Windows.Forms.ToolStripButton ux_RemoveLayer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip ux_EntitiesToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton ux_AddEntity;
        private System.Windows.Forms.ToolStripButton ux_RemoveEntity;
        private System.Windows.Forms.ToolStripStatusLabel debugStatus;
    }
}