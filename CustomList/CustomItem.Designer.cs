namespace OneLevel2D
{
    partial class CustomItem
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
            this.itemName = new System.Windows.Forms.Label();
            this.nameChangeBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // itemName
            // 
            this.itemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemName.AutoSize = true;
            this.itemName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.itemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.itemName.Location = new System.Drawing.Point(3, 9);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(37, 12);
            this.itemName.TabIndex = 0;
            this.itemName.Text = "name";
            // 
            // nameChangeBox
            // 
            this.nameChangeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameChangeBox.Location = new System.Drawing.Point(3, 4);
            this.nameChangeBox.Name = "nameChangeBox";
            this.nameChangeBox.Size = new System.Drawing.Size(111, 21);
            this.nameChangeBox.TabIndex = 1;
            this.nameChangeBox.Visible = false;
            // 
            // CustomItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.nameChangeBox);
            this.Controls.Add(this.itemName);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CustomItem";
            this.Size = new System.Drawing.Size(224, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label itemName;
        private System.Windows.Forms.TextBox nameChangeBox;
    }
}
