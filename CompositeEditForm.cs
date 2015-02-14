using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OneLevel2D.Model;
using OneLevel2D.Properties;

namespace OneLevel2D
{
    public partial class CompositeEditForm : Form
    {
        private CompositeBoard Board { get; set; }
        private CienComposite Composite { get; set; }

        public CompositeEditForm()
        {
            InitializeComponent();

            InitBoard();
        }

        private void InitBoard()
        {
            Board = new CompositeBoard()
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(Board);
            Board.BringToFront();
            AddEvent();
        }

        private void AddEvent()
        {
            closeBox.MouseEnter += (sender, e) => { closeBox.Image = Resources.xbuttonhover; };
            closeBox.MouseLeave += (sender, e) => { closeBox.Image = null; };
            closeBox.MouseClick += (sender, e) => { Close(); };
        }

        public void Set(CienComposite composite)
        {
            Composite = composite;
            Size = Composite.GetSize() + new Size(0, titleBar.Size.Height);
            Board.SetImage(composite.GetImage());
        }

        public void AddImage(CienImage image)
        {
            Composite.AddComponent(image);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            MessageBox.Show("asdf");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //e.Cancel = true;
            // TODO 이 form에서 수정한 사항을 여기서 적용한다.
        }

        public class CompositeBoard : UserControl
        {
            private Image drawImage;

            public CompositeBoard()
            {

            }

            public void SetImage(Image image)
            {
                drawImage = image;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (drawImage != null)
                    e.Graphics.DrawImage(drawImage, new Rectangle(new Point(0, 0), drawImage.Size));
            }
        }
    }
}
