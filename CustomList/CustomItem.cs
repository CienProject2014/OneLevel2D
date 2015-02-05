using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace OneLevel2D
{
    public abstract partial class CustomItem : UserControl, ICloneable
    {
        public bool IsSelected { get; protected set; }
        private static readonly Color SelecteColor = Color.WhiteSmoke;
        private static readonly Color EnterColor = Color.LightGray;
        private static readonly Color LeaveColor = Color.Gray;

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

        public void ItemSelect()
        {
            IsSelected = true;
            ShowSelected();
        }

        public void ItemUnselect()
        {
            IsSelected = false;
            ShowEnter();
        }

        public void ShowSelected()
        {
            if (IsSelected)
                BackColor = SelecteColor;
        }

        public void ShowEnter()
        {
            if (!IsSelected)
            {
                BackColor = EnterColor;
            }
        }

        public void ShowLeave()
        {
            if (!IsSelected)
            {
                BackColor = LeaveColor;
            }
        }

        protected abstract void ChangeItemName(string text);

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
            ShowLeave();
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
            ChangeItemName(nameChangeBox.Text);

            nameChangeBox.Visible = false;
            itemName.Visible = true;
        }

        private void nameChangeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeItemName(nameChangeBox.Text);

                nameChangeBox.Visible = false;
                itemName.Visible = true;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            ShowEnter();
            // TODO Compoennt List View에 문제가 생기면 여기를 확인.
            //Parent.Focus();
            //Focus();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            ShowLeave();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsSelected)
                    ItemSelect();
                else
                    ItemUnselect();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ShowEnter();
        }


        #region Constant

        public const int ItemHeight = 30;

        #endregion

        public abstract object Clone();
    }
}
