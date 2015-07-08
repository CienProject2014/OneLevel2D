namespace OneLevel2D
{
    partial class TitleBarControl
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
            this.closeBox = new System.Windows.Forms.PictureBox();
            this.minimizeBox = new System.Windows.Forms.PictureBox();
            this.maximizeBox = new System.Windows.Forms.PictureBox();
            this.titleLogo = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.closeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // closeBox
            // 
            this.closeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBox.BackgroundImage = global::OneLevel2D.Properties.Resources.xbutton;
            this.closeBox.Location = new System.Drawing.Point(403, 0);
            this.closeBox.Name = "closeBox";
            this.closeBox.Size = new System.Drawing.Size(34, 28);
            this.closeBox.TabIndex = 0;
            this.closeBox.TabStop = false;
            // 
            // minimizeBox
            // 
            this.minimizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeBox.BackgroundImage = global::OneLevel2D.Properties.Resources.minimizebutton;
            this.minimizeBox.Location = new System.Drawing.Point(335, 0);
            this.minimizeBox.Name = "minimizeBox";
            this.minimizeBox.Size = new System.Drawing.Size(34, 28);
            this.minimizeBox.TabIndex = 1;
            this.minimizeBox.TabStop = false;
            // 
            // maximizeBox
            // 
            this.maximizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maximizeBox.BackgroundImage = global::OneLevel2D.Properties.Resources.maximizebutton;
            this.maximizeBox.Location = new System.Drawing.Point(369, 0);
            this.maximizeBox.Name = "maximizeBox";
            this.maximizeBox.Size = new System.Drawing.Size(34, 28);
            this.maximizeBox.TabIndex = 2;
            this.maximizeBox.TabStop = false;
            // 
            // titleLogo
            // 
            this.titleLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleLogo.Image = global::OneLevel2D.Properties.Resources.package7;
            this.titleLogo.Location = new System.Drawing.Point(12, 8);
            this.titleLogo.Name = "titleLogo";
            this.titleLogo.Size = new System.Drawing.Size(20, 20);
            this.titleLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.titleLogo.TabIndex = 3;
            this.titleLogo.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("NanumBarunGothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.titleLabel.ForeColor = System.Drawing.Color.Silver;
            this.titleLabel.Location = new System.Drawing.Point(38, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(56, 15);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "noname";
            // 
            // TitleBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = global::OneLevel2D.Properties.Resources.background;
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.titleLogo);
            this.Controls.Add(this.maximizeBox);
            this.Controls.Add(this.minimizeBox);
            this.Controls.Add(this.closeBox);
            this.Name = "TitleBarControl";
            this.Size = new System.Drawing.Size(437, 31);
            ((System.ComponentModel.ISupportInitialize)(this.closeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox closeBox;
        private System.Windows.Forms.PictureBox minimizeBox;
        private System.Windows.Forms.PictureBox maximizeBox;
        private System.Windows.Forms.PictureBox titleLogo;
        private System.Windows.Forms.Label titleLabel;
    }
}
