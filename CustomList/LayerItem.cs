using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OneLevel2D.Model;
using OneLevel2D.Properties;

namespace OneLevel2D.CustomList
{
    public class LayerItem : CustomItem
    {
        private PictureBox lockBox;
        private PictureBox eyeBox;

        private CienLayer _layer;

        public LayerItem(CienLayer layer, Point location) : base(layer.Name, location)
        {
            InitializeComponent();

            MouseDown += LayerItem_MouseDown;

            eyeBox.MouseClick += eyeBox_MouseClick;
            lockBox.MouseClick += lockBox_MouseClick;

            _layer = layer;

            Name = _layer.Name;
            itemName.Text = _layer.Name;

            UpdateBox();
        }

        void LayerItem_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (IsSelected)
                {
                }
                else
                {
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                State.SelectOneLayer(_layer);
                
                ItemSelect();
            }
        }

        public void UpdateBox()
        {
            eyeBox.BackgroundImage = _layer.IsVisible ? Resources.eye : Resources.paleeye;
            lockBox.BackgroundImage = _layer.IsLocked ? Resources.locked : Resources.paleunlocked;
            Invalidate();
        }

        void eyeBox_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.Print("eye");
            _layer.VisibleToggle();
            eyeBox.BackgroundImage = _layer.IsVisible ? Resources.eye : Resources.paleeye;

            Invalidate();
            State.Board.Invalidate();
        }

        void lockBox_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.Print("lock");
            _layer.LockToggle();
            lockBox.BackgroundImage = _layer.IsLocked ? Resources.locked : Resources.paleunlocked;

            Invalidate();
            State.Board.Invalidate();
        }

        protected override void ChangeItemName(string newName)
        {
            if (newName == null) return;

            itemName.Text = newName;
            _layer.SetName(newName);
        }

        public override object Clone()
        {
            return new LayerItem(_layer, Location);
        }

        private void InitializeComponent()
        {
            this.lockBox = new System.Windows.Forms.PictureBox();
            this.eyeBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lockBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eyeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lockBox
            // 
            this.lockBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lockBox.BackColor = System.Drawing.Color.Transparent;
            this.lockBox.BackgroundImage = global::OneLevel2D.Properties.Resources.paleunlocked;
            this.lockBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lockBox.Location = new System.Drawing.Point(203, 6);
            this.lockBox.Name = "lockBox";
            this.lockBox.Size = new System.Drawing.Size(18, 18);
            this.lockBox.TabIndex = 1;
            this.lockBox.TabStop = false;
            // 
            // eyeBox
            // 
            this.eyeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.eyeBox.BackColor = System.Drawing.Color.Transparent;
            this.eyeBox.BackgroundImage = global::OneLevel2D.Properties.Resources.paleeye;
            this.eyeBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.eyeBox.Location = new System.Drawing.Point(179, 6);
            this.eyeBox.Name = "eyeBox";
            this.eyeBox.Size = new System.Drawing.Size(18, 18);
            this.eyeBox.TabIndex = 2;
            this.eyeBox.TabStop = false;
            // 
            // LayerItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.eyeBox);
            this.Controls.Add(this.lockBox);
            this.Name = "LayerItem";
            this.Controls.SetChildIndex(this.itemName, 0);
            this.Controls.SetChildIndex(this.lockBox, 0);
            this.Controls.SetChildIndex(this.eyeBox, 0);
            ((System.ComponentModel.ISupportInitialize)(this.lockBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eyeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
