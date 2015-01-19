namespace OneLevelJson
{
    partial class SelectedControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.xyValueLabel = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.componentIdLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "X,Y";
            // 
            // xyValueLabel
            // 
            this.xyValueLabel.AutoSize = true;
            this.xyValueLabel.Location = new System.Drawing.Point(63, 66);
            this.xyValueLabel.Name = "xyValueLabel";
            this.xyValueLabel.Size = new System.Drawing.Size(25, 12);
            this.xyValueLabel.TabIndex = 1;
            this.xyValueLabel.Text = "0, 0";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(18, 31);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(15, 12);
            this.idLabel.TabIndex = 4;
            this.idLabel.Text = "Id";
            // 
            // componentIdLabel
            // 
            this.componentIdLabel.AutoSize = true;
            this.componentIdLabel.Location = new System.Drawing.Point(63, 31);
            this.componentIdLabel.Name = "componentIdLabel";
            this.componentIdLabel.Size = new System.Drawing.Size(84, 12);
            this.componentIdLabel.TabIndex = 5;
            this.componentIdLabel.Text = "Component Id";
            // 
            // SelectedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.componentIdLabel);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.xyValueLabel);
            this.Controls.Add(this.label1);
            this.Name = "SelectedControl";
            this.Size = new System.Drawing.Size(213, 194);
            this.Load += new System.EventHandler(this.SelectedControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label xyValueLabel;
        private System.Windows.Forms.Label componentIdLabel;
        private System.Windows.Forms.Label idLabel;


    }
}
