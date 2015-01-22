using System;
using System.Drawing;
using System.Windows.Forms;
using OneLevelJson.Properties;

namespace OneLevelJson
{
    public partial class TitleBarControl : UserControl
    {
        public TitleBarControl()
        {
            InitializeComponent();
            AddEvent();

        }

        public void SetTitleName(string name)
        {
            titleLabel.Text = name;
        }

        private void AddEvent()
        {
            closeBox.MouseEnter += Control_MouseEnter;
            maximizeBox.MouseEnter += Control_MouseEnter;
            minimizeBox.MouseEnter += Control_MouseEnter;

            closeBox.MouseLeave += Control_MouseLeave;
            maximizeBox.MouseLeave += Control_MouseLeave;
            minimizeBox.MouseLeave += Control_MouseLeave;

            closeBox.MouseClick += Control_MouseClick;
            maximizeBox.MouseClick += Control_MouseClick;
            minimizeBox.MouseClick += Control_MouseClick;

            MouseUp += titlePanel_MouseUp;
            MouseDown += titlePanel_MouseDown;
            MouseMove += titlePanel_MouseMove;
            DoubleClick += titlePanel_DoubleClick;

            titleLabel.MouseUp += titlePanel_MouseUp;
            titleLabel.MouseDown += titlePanel_MouseDown;
            titleLabel.MouseMove += titlePanel_MouseMove;

            titleLogo.MouseUp += titlePanel_MouseUp;
            titleLogo.MouseDown += titlePanel_MouseDown;
            titleLogo.MouseMove += titlePanel_MouseMove;
            titleLogo.MouseDoubleClick += titleLogo_MouseDoubleClick;
        }

        private void titlePanel_DoubleClick(object sender, EventArgs e)
        {

            if (Program.Form.WindowState == FormWindowState.Normal)
                Program.Form.WindowState = FormWindowState.Maximized;
            else if (Program.Form.WindowState == FormWindowState.Maximized)
                Program.Form.WindowState = FormWindowState.Normal;
        }

        private void titlePanel_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void titlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _startPoint = e.Location;
        }

        private void titlePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Program.Form.Location = new Point(p.X - _startPoint.X, p.Y - _startPoint.Y);
            }
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            if (sender.Equals(closeBox))
                closeBox.Image = Resources.xbuttonhover;
            else if (sender.Equals(maximizeBox))
                maximizeBox.Image = Resources.maximizebuttonhover;
            else if (sender.Equals(minimizeBox))
                minimizeBox.Image = Resources.minimizebuttonhover;
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            if (sender.Equals(closeBox))
                closeBox.Image = null;
            else if (sender.Equals(maximizeBox))
                maximizeBox.Image = null;
            else if (sender.Equals(minimizeBox))
                minimizeBox.Image = null;
        }

        private void Control_MouseClick(object sender, EventArgs e)
        {
            if (sender.Equals(closeBox))
                Program.Form.Close();
            else if (sender.Equals(maximizeBox))
            {
                if (Program.Form.WindowState == FormWindowState.Normal)
                    Program.Form.WindowState = FormWindowState.Maximized;
                else if (Program.Form.WindowState == FormWindowState.Maximized)
                    Program.Form.WindowState = FormWindowState.Normal;
            }
            else if (sender.Equals(minimizeBox))
                Program.Form.WindowState = FormWindowState.Minimized;
        }

        void titleLogo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Program.Form.Close();
        }

        private bool _dragging;
        private Point _startPoint = new Point(0, 0);
    }
}
