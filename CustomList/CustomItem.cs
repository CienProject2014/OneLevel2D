using System;
using System.Drawing;
using System.Windows.Forms;

namespace OneLevelJson
{
    public abstract partial class CustomItem : UserControl, ICloneable
    {
        public bool IsSelected { get; protected set; }

        protected CustomItem(string name, Point location)
        {
            InitializeComponent();

            itemName.MouseEnter += itemName_MouseEnter;
            itemName.MouseLeave += itemName_MouseLeave;
            itemName.MouseDown += itemName_MouseDown;
            itemName.MouseUp += itemName_MouseUp;
            itemName.MouseDoubleClick += itemName_MouseDoubleClick;

            nameChangeBox.KeyDown += nameChangeBox_KeyDown;
            nameChangeBox.LostFocus += nameChangeBox_LostFocus;

            // TODO Name과 itemName.Text를 같게!
            Name = name;
            itemName.Text = Name;
            Location = location;
        }

        public void ShowSelected(Color color)
        {
            if (IsSelected)
                BackColor = color;
        }

        private void ShowUnSelected(Color color)
        {
            if (!IsSelected)
                BackColor = color;
        }

        protected abstract void ChangeItem(string text);

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawLine(pen, 0, Height, Width, Height);
            }
        }

        #region itemName Delegate
        private void itemName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            nameChangeBox.Text = itemName.Text;
            nameChangeBox.Visible = true;
            nameChangeBox.Focus();
            itemName.Visible = false;


        }

        private void itemName_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        void itemName_MouseLeave(object sender, EventArgs e)
        {
            ShowUnSelected(Color.DimGray);
        }

        void itemName_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        void itemName_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }
        #endregion

        private void nameChangeBox_LostFocus(object sender, EventArgs e)
        {
            ChangeItem(nameChangeBox.Text);

            nameChangeBox.Visible = false;
            itemName.Visible = true;
        }

        private void nameChangeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeItem(nameChangeBox.Text);

                nameChangeBox.Visible = false;
                itemName.Visible = true;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            ShowUnSelected(Color.LightGray);
            Parent.Focus();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            ShowUnSelected(Color.Gray);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsSelected = !IsSelected;
                ShowSelected(Color.WhiteSmoke);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ShowUnSelected(Color.LightGray);
        }


        #region Constant

        public const int ItemHeight = 30;

        #endregion

        public abstract object Clone();
    }
}
