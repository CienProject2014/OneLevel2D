using System.Windows.Forms;

namespace OneLevelJson
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jsonExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tESTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetList = new System.Windows.Forms.ListView();
            this.assetImageList = new System.Windows.Forms.ImageList(this.components);
            this.imageImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.componentList = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.zindex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.componentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.exportFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.deleteLayer = new System.Windows.Forms.Button();
            this.layerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lockunlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLayer = new System.Windows.Forms.Button();
            this.layerList = new System.Windows.Forms.ListView();
            this.componentDownBtn = new System.Windows.Forms.Button();
            this.componentUpBtn = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.toolContainer1 = new System.Windows.Forms.SplitContainer();
            this.selectedControl = new OneLevelJson.SelectedControl();
            this.toolContainer2 = new System.Windows.Forms.SplitContainer();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.blackboard = new OneLevelJson.Blackboard();
            this.titleBarControl1 = new OneLevelJson.TitleBarControl();
            this.assetContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.componentContextMenu.SuspendLayout();
            this.layerContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolContainer1)).BeginInit();
            this.toolContainer1.Panel1.SuspendLayout();
            this.toolContainer1.Panel2.SuspendLayout();
            this.toolContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolContainer2)).BeginInit();
            this.toolContainer2.Panel1.SuspendLayout();
            this.toolContainer2.Panel2.SuspendLayout();
            this.toolContainer2.SuspendLayout();
            this.assetContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStrip.Font = new System.Drawing.Font("NanumBarunGothic", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.menuStrip.ForeColor = System.Drawing.Color.White;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.assetsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 31);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.jsonExportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // jsonExportToolStripMenuItem
            // 
            this.jsonExportToolStripMenuItem.Name = "jsonExportToolStripMenuItem";
            this.jsonExportToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.jsonExportToolStripMenuItem.Text = "Json Export";
            this.jsonExportToolStripMenuItem.Click += new System.EventHandler(this.jsonExportToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // assetsToolStripMenuItem
            // 
            this.assetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem});
            this.assetsToolStripMenuItem.Name = "assetsToolStripMenuItem";
            this.assetsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.assetsToolStripMenuItem.Text = "Assets";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tESTToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tESTToolStripMenuItem
            // 
            this.tESTToolStripMenuItem.Name = "tESTToolStripMenuItem";
            this.tESTToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.tESTToolStripMenuItem.Text = "TEST";
            this.tESTToolStripMenuItem.Click += new System.EventHandler(this.tESTToolStripMenuItem_Click);
            // 
            // assetList
            // 
            this.assetList.AllowDrop = true;
            this.assetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetList.Location = new System.Drawing.Point(0, 0);
            this.assetList.Name = "assetList";
            this.assetList.Size = new System.Drawing.Size(190, 150);
            this.assetList.SmallImageList = this.assetImageList;
            this.assetList.TabIndex = 2;
            this.assetList.UseCompatibleStateImageBehavior = false;
            this.assetList.View = System.Windows.Forms.View.SmallIcon;
            // 
            // assetImageList
            // 
            this.assetImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.assetImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.assetImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageImportDialog
            // 
            this.imageImportDialog.Filter = "png files|*.png|jpg files|*.jpg|All image files|*.*";
            this.imageImportDialog.Multiselect = true;
            // 
            // componentList
            // 
            this.componentList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.zindex});
            this.componentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentList.LabelEdit = true;
            this.componentList.Location = new System.Drawing.Point(0, 0);
            this.componentList.Name = "componentList";
            this.componentList.Size = new System.Drawing.Size(190, 157);
            this.componentList.TabIndex = 3;
            this.componentList.UseCompatibleStateImageBehavior = false;
            this.componentList.View = System.Windows.Forms.View.Details;
            // 
            // id
            // 
            this.id.Text = "Id";
            // 
            // zindex
            // 
            this.zindex.Text = "zIndex";
            // 
            // componentContextMenu
            // 
            this.componentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.componentContextMenu.Name = "contextMenuStrip1";
            this.componentContextMenu.Size = new System.Drawing.Size(115, 48);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.renameToolStripMenuItem.Text = "rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.componentRenameToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.removeToolStripMenuItem.Text = "remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.conponentRemoveToolStripMenuItem_Click);
            // 
            // openProjectDialog
            // 
            this.openProjectDialog.Filter = "Proejct File(.dt)|*.dt|Overlap2D Project File(.pit)|*.pit";
            // 
            // deleteLayer
            // 
            this.deleteLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteLayer.Location = new System.Drawing.Point(161, 125);
            this.deleteLayer.Name = "deleteLayer";
            this.deleteLayer.Size = new System.Drawing.Size(23, 23);
            this.deleteLayer.TabIndex = 8;
            this.deleteLayer.Text = "-";
            this.deleteLayer.UseVisualStyleBackColor = true;
            this.deleteLayer.Click += new System.EventHandler(this.deleteLayer_Click);
            // 
            // layerContextMenu
            // 
            this.layerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem1,
            this.lockunlockToolStripMenuItem});
            this.layerContextMenu.Name = "layerMenuStrip";
            this.layerContextMenu.Size = new System.Drawing.Size(138, 48);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.renameToolStripMenuItem1.Text = "rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.layerRenameToolStripMenuItem_Click);
            // 
            // lockunlockToolStripMenuItem
            // 
            this.lockunlockToolStripMenuItem.Name = "lockunlockToolStripMenuItem";
            this.lockunlockToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.lockunlockToolStripMenuItem.Text = "lock/unlock";
            this.lockunlockToolStripMenuItem.Click += new System.EventHandler(this.lockunlockToolStripMenuItem_Click);
            // 
            // addLayer
            // 
            this.addLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addLayer.Location = new System.Drawing.Point(132, 125);
            this.addLayer.Name = "addLayer";
            this.addLayer.Size = new System.Drawing.Size(23, 23);
            this.addLayer.TabIndex = 7;
            this.addLayer.Text = "+";
            this.addLayer.UseVisualStyleBackColor = true;
            this.addLayer.Click += new System.EventHandler(this.addLayer_Click);
            // 
            // layerList
            // 
            this.layerList.CheckBoxes = true;
            this.layerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerList.Location = new System.Drawing.Point(0, 0);
            this.layerList.MultiSelect = false;
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(190, 150);
            this.layerList.TabIndex = 6;
            this.layerList.UseCompatibleStateImageBehavior = false;
            this.layerList.View = System.Windows.Forms.View.SmallIcon;
            // 
            // componentDownBtn
            // 
            this.componentDownBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.componentDownBtn.Location = new System.Drawing.Point(161, 133);
            this.componentDownBtn.Name = "componentDownBtn";
            this.componentDownBtn.Size = new System.Drawing.Size(23, 23);
            this.componentDownBtn.TabIndex = 13;
            this.componentDownBtn.Text = "↓";
            this.componentDownBtn.UseVisualStyleBackColor = true;
            this.componentDownBtn.Click += new System.EventHandler(this.componentDownBtn_Click);
            // 
            // componentUpBtn
            // 
            this.componentUpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.componentUpBtn.Location = new System.Drawing.Point(132, 133);
            this.componentUpBtn.Name = "componentUpBtn";
            this.componentUpBtn.Size = new System.Drawing.Size(23, 23);
            this.componentUpBtn.TabIndex = 15;
            this.componentUpBtn.Text = "↑";
            this.componentUpBtn.UseVisualStyleBackColor = true;
            this.componentUpBtn.Click += new System.EventHandler(this.componentUpBtn_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitContainer.Location = new System.Drawing.Point(610, 55);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.toolContainer1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.toolContainer2);
            this.splitContainer.Size = new System.Drawing.Size(190, 604);
            this.splitContainer.SplitterDistance = 296;
            this.splitContainer.TabIndex = 16;
            // 
            // toolContainer1
            // 
            this.toolContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolContainer1.Name = "toolContainer1";
            this.toolContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // toolContainer1.Panel1
            // 
            this.toolContainer1.Panel1.Controls.Add(this.selectedControl);
            // 
            // toolContainer1.Panel2
            // 
            this.toolContainer1.Panel2.Controls.Add(this.componentDownBtn);
            this.toolContainer1.Panel2.Controls.Add(this.componentUpBtn);
            this.toolContainer1.Panel2.Controls.Add(this.componentList);
            this.toolContainer1.Size = new System.Drawing.Size(190, 296);
            this.toolContainer1.SplitterDistance = 135;
            this.toolContainer1.TabIndex = 17;
            // 
            // selectedControl
            // 
            this.selectedControl.AutoSize = true;
            this.selectedControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.selectedControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedControl.Font = new System.Drawing.Font("NanumGothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.selectedControl.Location = new System.Drawing.Point(0, 0);
            this.selectedControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectedControl.Name = "selectedControl";
            this.selectedControl.Size = new System.Drawing.Size(190, 135);
            this.selectedControl.TabIndex = 17;
            // 
            // toolContainer2
            // 
            this.toolContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolContainer2.Name = "toolContainer2";
            this.toolContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // toolContainer2.Panel1
            // 
            this.toolContainer2.Panel1.Controls.Add(this.assetList);
            // 
            // toolContainer2.Panel2
            // 
            this.toolContainer2.Panel2.Controls.Add(this.addLayer);
            this.toolContainer2.Panel2.Controls.Add(this.deleteLayer);
            this.toolContainer2.Panel2.Controls.Add(this.layerList);
            this.toolContainer2.Size = new System.Drawing.Size(190, 304);
            this.toolContainer2.SplitterDistance = 150;
            this.toolContainer2.TabIndex = 17;
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // blackboard
            // 
            this.blackboard.AllowDrop = true;
            this.blackboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.blackboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackboard.Location = new System.Drawing.Point(0, 55);
            this.blackboard.Name = "blackboard";
            this.blackboard.Size = new System.Drawing.Size(610, 604);
            this.blackboard.TabIndex = 5;
            // 
            // titleBarControl1
            // 
            this.titleBarControl1.AutoSize = true;
            this.titleBarControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.titleBarControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleBarControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("titleBarControl1.BackgroundImage")));
            this.titleBarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarControl1.Location = new System.Drawing.Point(0, 0);
            this.titleBarControl1.Name = "titleBarControl1";
            this.titleBarControl1.Size = new System.Drawing.Size(800, 31);
            this.titleBarControl1.TabIndex = 18;
            // 
            // assetContextMenu
            // 
            this.assetContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem1});
            this.assetContextMenu.Name = "assetContextMenu";
            this.assetContextMenu.Size = new System.Drawing.Size(115, 26);
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.removeToolStripMenuItem1.Text = "remove";
            this.removeToolStripMenuItem1.Click += new System.EventHandler(this.assetRemoveToolStripMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 659);
            this.Controls.Add(this.blackboard);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.titleBarControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.componentContextMenu.ResumeLayout(false);
            this.layerContextMenu.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolContainer1.Panel1.ResumeLayout(false);
            this.toolContainer1.Panel1.PerformLayout();
            this.toolContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolContainer1)).EndInit();
            this.toolContainer1.ResumeLayout(false);
            this.toolContainer2.Panel1.ResumeLayout(false);
            this.toolContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolContainer2)).EndInit();
            this.toolContainer2.ResumeLayout(false);
            this.assetContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jsonExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListView assetList;
        private System.Windows.Forms.ImageList assetImageList;
        private System.Windows.Forms.OpenFileDialog imageImportDialog;
        private System.Windows.Forms.ListView componentList;
        private OneLevelJson.Blackboard blackboard;
        private ToolStripMenuItem newToolStripMenuItem;
        private ContextMenuStrip componentContextMenu;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private FolderBrowserDialog saveFolderBrowserDialog;
        private OpenFileDialog openProjectDialog;
        private FolderBrowserDialog exportFolderBrowser;
        private ToolStripMenuItem tESTToolStripMenuItem;
        private Button deleteLayer;
        private ContextMenuStrip layerContextMenu;
        private ToolStripMenuItem renameToolStripMenuItem1;
        private Button addLayer;
        private ListView layerList;
        private ColumnHeader zindex;
        private ColumnHeader id;
        private Button componentDownBtn;
        private Button componentUpBtn;
        private SplitContainer splitContainer;
        private SplitContainer toolContainer1;
        private SplitContainer toolContainer2;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private SelectedControl selectedControl;
        private TitleBarControl titleBarControl1;
        private ToolStripMenuItem lockunlockToolStripMenuItem;
        private ContextMenuStrip assetContextMenu;
        private ToolStripMenuItem removeToolStripMenuItem1;
    }
}

