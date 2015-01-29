namespace OneLevel2D
{
    partial class CustomListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titlePanel = new System.Windows.Forms.Panel();
            this.titleName = new System.Windows.Forms.Label();
            this.listPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.titlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.SkyBlue;
            this.titlePanel.Controls.Add(this.titleName);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(278, 21);
            this.titlePanel.TabIndex = 3;
            // 
            // titleName
            // 
            this.titleName.AutoSize = true;
            this.titleName.Location = new System.Drawing.Point(3, 5);
            this.titleName.Name = "titleName";
            this.titleName.Size = new System.Drawing.Size(105, 12);
            this.titleName.TabIndex = 0;
            this.titleName.Text = "Custom List View";
            // 
            // listPanel
            // 
            this.listPanel.AutoScroll = true;
            this.listPanel.BackColor = System.Drawing.Color.DimGray;
            this.listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPanel.Location = new System.Drawing.Point(0, 21);
            this.listPanel.Name = "listPanel";
            this.listPanel.Size = new System.Drawing.Size(278, 277);
            this.listPanel.TabIndex = 4;
            // 
            // CustomListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.listPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "CustomListView";
            this.Size = new System.Drawing.Size(278, 298);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public System.Windows.Forms.Label titleName;
        public System.Windows.Forms.FlowLayoutPanel listPanel;


    }
}
