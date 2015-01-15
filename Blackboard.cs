using System;
using System.Drawing;
using System.Windows.Forms;

namespace OneLevelJson
{
    public partial class Blackboard : UserControl
    {
        private Log log = new Log();
        public Document PresentDocument { get; set; }
        public Point ClickedPoint { get; private set; }
        public Point MovingPoint { get; private set; }
        public Component SelectedComponent { get; private set; }

        private const int borderOffset = 0;

        public Blackboard()
        {
            InitializeComponent();

            
            /* Commands for Flicker *************************************************/
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
            /************************************************************************/
            
        }

        private bool IsInside(Component component, Point clicked)
        {
            // if clicked is inside the component, then return true
            if (clicked.X > component.Position.X &&
                clicked.X < component.Position.X + component.GetSize().Width &&
                clicked.Y > component.Position.Y &&
                clicked.Y < component.Position.Y + component.GetSize().Height)
            {
                return true;
            }

            return false;
        }

        private Point CalcOffset(Point p1, Point p2)
        {
            return p2 - (Size)p1;
        }

        private void DrawBorder(PaintEventArgs e, Component component)
        {
            Point[] points =
                {
                    new Point(component.Position.X-borderOffset, component.Position.Y-borderOffset),
                    new Point(component.Position.X+component.GetSize().Width+borderOffset, component.Position.Y-borderOffset),
                    new Point(component.Position.X+borderOffset+component.GetSize().Width, component.Position.Y+component.GetSize().Height+borderOffset),
                    new Point(component.Position.X-borderOffset, component.Position.Y+component.GetSize().Height+borderOffset),
                    new Point(component.Position.X-borderOffset, component.Position.Y-borderOffset)
                };

            using (Pen pen = new Pen(Color.Black))
            {
                e.Graphics.DrawLines(pen, points);
            }

            log.Write("size is : " + component.GetSize().ToString());
        }

        public void RemoveSelected()
        {
            SelectedComponent = null;
        }

        /************************************************************************/
        /* Override 															*/
        /************************************************************************/
        protected override void OnPaint(PaintEventArgs e)
        {
            log.Write("OnPaint is called");
            if (PresentDocument == null) return;


            foreach (var component in PresentDocument.Components)
            {
                Object data = component.ParentAsset.Data;
                Point pos = component.Position;
                switch (component.ParentAsset.Type)
                {
                    case AssetType.Image:
                        Image img = data as Image;
                        e.Graphics.DrawImage(img, new Rectangle(pos, new Size(img.Width, img.Height)));
                        break;
                }
            }

            if (SelectedComponent != null)
            {
                DrawBorder(e, SelectedComponent);
            }

            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ClickedPoint = new Point((Size)e.Location);
            MovingPoint = new Point((Size)ClickedPoint);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (PresentDocument != null)
                        SelectedComponent = PresentDocument.Components.Find(x => IsInside(x, ClickedPoint));
                    break;
                case MouseButtons.Right:

                    break;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (SelectedComponent == null) return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    Point offset = CalcOffset(MovingPoint, e.Location);
                    SelectedComponent.Move(offset);
                    MovingPoint = new Point((Size) e.Location);
                    log.Write(offset.ToString());
                    break;
            }
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Invalidate();
        }
    }
}