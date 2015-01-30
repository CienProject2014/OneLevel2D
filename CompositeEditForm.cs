using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OneLevel2D.Model;

namespace OneLevel2D
{
    public partial class CompositeEditForm : Form
    {
        public CienComposite Composite { get; private set; }

        public CompositeEditForm()
        {
            InitializeComponent();
        }

        public void Set(CienComposite composite)
        {
            Composite = composite;
            //this.BackgroundImage = Composite.GetImage();
            Composite.AddImage(new CienImage("yongsa_sad", "yongsa", Point.Empty, 2));
            Size = Composite.GetSize();
        }

        public void AddImage(CienImage image)
        {
            Composite.AddImage(image);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Composite.GetImage(), new Rectangle(new Point(0, 0), new Size(Composite.GetImage().Width, Composite.GetImage().Height)));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
