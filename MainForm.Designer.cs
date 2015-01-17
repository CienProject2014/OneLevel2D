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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
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
            this.componentImageList = new System.Windows.Forms.ImageList(this.components);
            this.imageImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.componentList = new System.Windows.Forms.ListView();
            this.componentMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.saveFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.blackboardMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.exportFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.deleteLayer = new System.Windows.Forms.Button();
            this.layerMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.blackboard = new OneLevelJson.Blackboard();
            this.addLayer = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.layerList = new System.Windows.Forms.ListView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.componentMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.blackboardMenuStrip.SuspendLayout();
            this.layerMenuStrip.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.assetsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "MenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.jsonExportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // jsonExportToolStripMenuItem
            // 
            this.jsonExportToolStripMenuItem.Name = "jsonExportToolStripMenuItem";
            this.jsonExportToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.jsonExportToolStripMenuItem.Text = "Json Export";
            this.jsonExportToolStripMenuItem.Click += new System.EventHandler(this.jsonExportToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // assetsToolStripMenuItem
            // 
            this.assetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem});
            this.assetsToolStripMenuItem.Name = "assetsToolStripMenuItem";
            this.assetsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.assetsToolStripMenuItem.Text = "Assets";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tESTToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tESTToolStripMenuItem
            // 
            this.tESTToolStripMenuItem.Name = "tESTToolStripMenuItem";
            this.tESTToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.tESTToolStripMenuItem.Text = "TEST";
            this.tESTToolStripMenuItem.Click += new System.EventHandler(this.tESTToolStripMenuItem_Click);
            // 
            // assetList
            // 
            this.assetList.AllowDrop = true;
            this.assetList.Location = new System.Drawing.Point(3, 279);
            this.assetList.Name = "assetList";
            this.assetList.Size = new System.Drawing.Size(146, 93);
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
            // componentImageList
            // 
            this.componentImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.componentImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.componentImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageImportDialog
            // 
            this.imageImportDialog.Filter = "png files|*.png|jpg files|*.jpg|All image files|*.*";
            this.imageImportDialog.Multiselect = true;
            // 
            // componentList
            // 
            this.componentList.LabelEdit = true;
            this.componentList.Location = new System.Drawing.Point(3, 116);
            this.componentList.Name = "componentList";
            this.componentList.Size = new System.Drawing.Size(146, 157);
            this.componentList.SmallImageList = this.componentImageList;
            this.componentList.TabIndex = 3;
            this.componentList.UseCompatibleStateImageBehavior = false;
            this.componentList.View = System.Windows.Forms.View.SmallIcon;
            // 
            // componentMenuStrip
            // 
            this.componentMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.componentMenuStrip.Name = "contextMenuStrip1";
            this.componentMenuStrip.Size = new System.Drawing.Size(115, 48);
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
            // picBox
            // 
            this.picBox.AllowDrop = true;
            this.picBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.picBox.Location = new System.Drawing.Point(3, 3);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(146, 107);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBox.TabIndex = 4;
            this.picBox.TabStop = false;
            // 
            // blackboardMenuStrip
            // 
            this.blackboardMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem});
            this.blackboardMenuStrip.Name = "blackboardMenuStrip";
            this.blackboardMenuStrip.Size = new System.Drawing.Size(108, 26);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.imageToolStripMenuItem.Text = "image";
            // 
            // openProjectDialog
            // 
            this.openProjectDialog.Filter = "Proejct File(.dt)|*.dt";
            // 
            // deleteLayer
            // 
            this.deleteLayer.Location = new System.Drawing.Point(32, 509);
            this.deleteLayer.Name = "deleteLayer";
            this.deleteLayer.Size = new System.Drawing.Size(23, 23);
            this.deleteLayer.TabIndex = 8;
            this.deleteLayer.Text = "-";
            this.deleteLayer.UseVisualStyleBackColor = true;
            this.deleteLayer.Click += new System.EventHandler(this.deleteLayer_Click);
            // 
            // layerMenuStrip
            // 
            this.layerMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem1});
            this.layerMenuStrip.Name = "layerMenuStrip";
            this.layerMenuStrip.Size = new System.Drawing.Size(115, 26);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.renameToolStripMenuItem1.Text = "rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.layerRenameToolStripMenuItem_Click);
            // 
            // blackboard
            // 
            this.blackboard.AllowDrop = true;
            this.blackboard.BackColor = System.Drawing.SystemColors.ControlDark;
            this.blackboard.ContextMenuStrip = this.blackboardMenuStrip;
            this.blackboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackboard.Location = new System.Drawing.Point(0, 0);
            this.blackboard.Name = "blackboard";
            this.blackboard.PresentDocument = null;
            this.blackboard.Size = new System.Drawing.Size(800, 659);
            this.blackboard.TabIndex = 5;
            // 
            // addLayer
            // 
            this.addLayer.Location = new System.Drawing.Point(3, 509);
            this.addLayer.Name = "addLayer";
            this.addLayer.Size = new System.Drawing.Size(23, 23);
            this.addLayer.TabIndex = 7;
            this.addLayer.Text = "+";
            this.addLayer.UseVisualStyleBackColor = true;
            this.addLayer.Click += new System.EventHandler(this.addLayer_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(559, 51);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.55658F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65.44343F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14191F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(0, 14231);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // layerList
            // 
            this.layerList.Location = new System.Drawing.Point(3, 378);
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(145, 125);
            this.layerList.TabIndex = 6;
            this.layerList.UseCompatibleStateImageBehavior = false;
            this.layerList.View = System.Windows.Forms.View.SmallIcon;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.picBox);
            this.flowLayoutPanel1.Controls.Add(this.componentList);
            this.flowLayoutPanel1.Controls.Add(this.assetList);
            this.flowLayoutPanel1.Controls.Add(this.layerList);
            this.flowLayoutPanel1.Controls.Add(this.addLayer);
            this.flowLayoutPanel1.Controls.Add(this.deleteLayer);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(647, 24);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(153, 635);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 659);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.blackboard);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.componentMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.blackboardMenuStrip.ResumeLayout(false);
            this.layerMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
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
        private System.Windows.Forms.PictureBox picBox;
        private OneLevelJson.Blackboard blackboard;
        private ImageList componentImageList;
        private ToolStripMenuItem newToolStripMenuItem;
        private ContextMenuStrip componentMenuStrip;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private FolderBrowserDialog saveFolderBrowserDialog;
        private ContextMenuStrip blackboardMenuStrip;
        private ToolStripMenuItem imageToolStripMenuItem;
        private OpenFileDialog openProjectDialog;
        private FolderBrowserDialog exportFolderBrowser;
        private ToolStripMenuItem tESTToolStripMenuItem;
        private Button deleteLayer;
        private ContextMenuStrip layerMenuStrip;
        private ToolStripMenuItem renameToolStripMenuItem1;
        private Button addLayer;
        private TableLayoutPanel tableLayoutPanel1;
        private ListView layerList;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}

