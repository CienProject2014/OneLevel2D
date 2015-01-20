using System.ComponentModel;
using System.Windows.Forms;

namespace OneLevelJson
{
    partial class Blackboard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.blackboardContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // blackboardContextMenu
            // 
            this.blackboardContextMenu.Name = "blackboardContextMenu";
            this.blackboardContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // Blackboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "Blackboard";
            this.Size = new System.Drawing.Size(254, 248);
            this.ResumeLayout(false);

        }
        
        #endregion

        private ContextMenuStrip blackboardContextMenu;
    }
}
