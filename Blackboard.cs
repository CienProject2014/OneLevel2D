using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OneLevelJson.Model;

namespace OneLevelJson
{
    public partial class Blackboard : UserControl
    {
        public Blackboard()
        {
            InitializeComponent();

            /* Commands for Flicker *************************************************/
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
            /************************************************************************/

            AddEvent();
        }

        public void SetDocument(Document document)
        {
            PresentDocument = document;
            UpdateRectangle();
        }

        private void AddEvent()
        {
            blackboardContextMenu.ItemClicked += blackboardContextMenu_ItemClicked;
        }

        private bool IsInside(Component component, Point clicked)
        {
            // if clicked is inside the component, then return true
            if (clicked.X > component.Location.X &&
                clicked.X < component.Location.X + component.GetSize().Width &&
                clicked.Y > component.Location.Y &&
                clicked.Y < component.Location.Y + component.GetSize().Height)
            {
                return true;
            }

            return false;
        }

        public void RemoveSelected()
        {
            State.SelectedComponent = null;
        }

        private void DrawComponent(PaintEventArgs e, Component component)
        {
            using (Image img = component.GetImage())
            {
                e.Graphics.DrawImage(img, new Rectangle(component.Location,
                    new Size(img.Width, img.Height)));
            }
        }

        private void DrawComponentList(PaintEventArgs e)
        {
            foreach (var component in PresentDocument.Components)
            {
                DrawComponent(e, component);
            }
        }

        private void DrawSelectedComponentBorder(PaintEventArgs e)
        {
            var component = State.SelectedComponent;
            if (component == null) return;

            Point[] points =
                {
                    new Point(component.Location.X-BorderOffset, component.Location.Y-BorderOffset),
                    new Point(component.Location.X+component.GetSize().Width+BorderOffset, component.Location.Y-BorderOffset),
                    new Point(component.Location.X+BorderOffset+component.GetSize().Width, component.Location.Y+component.GetSize().Height+BorderOffset),
                    new Point(component.Location.X-BorderOffset, component.Location.Y+component.GetSize().Height+BorderOffset),
                    new Point(component.Location.X-BorderOffset, component.Location.Y-BorderOffset)
                };

            using (Pen pen = new Pen(Color.Black))
            {
                e.Graphics.DrawLines(pen, points);
            }

        }

        private void DrawBoundary(PaintEventArgs e)
        {
            if (Points == null) return;

            using (Pen pen = new Pen(Color.Black))
            {
                e.Graphics.DrawLines(pen, Points);
            }
        }

        private void UpdateBlackboardContextMenu(List<Component> componentList)
        {
            blackboardContextMenu.Items.Clear();
            foreach (var component in componentList)
            {
                blackboardContextMenu.Items.Add(component.Id);
            }
        }

        private void UpdateRectangle()
        {
            if (PresentDocument == null) return;
            Point leftTop = new Point(0, 0);

            LeftTopPoint = leftTop + LeftTopOffset;

            Points = new[]
            {
                LeftTopPoint,
                LeftTopPoint + new Size(PresentDocument.Width, 0),
                LeftTopPoint + new Size(PresentDocument.Width, PresentDocument.Height),
                LeftTopPoint + new Size(0, PresentDocument.Height),
                LeftTopPoint
            };
        }

        /************************************************************************/
        /* Delegate Event Handler												*/
        /************************************************************************/
        #region Delegate Method
        void blackboardContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            State.SelectedComponent = PresentDocument.Components.Find(x => x.Id == e.ClickedItem.Text);
            Invalidate();
        }
        #endregion

        /************************************************************************/
        /* Override 															*/
        /************************************************************************/
        #region Override Method
        protected override void OnPaint(PaintEventArgs e)
        {
            State.log.Write("OnPaint is called");
            if (PresentDocument == null) return;

            DrawComponentList(e);
            DrawSelectedComponentBorder(e);
            DrawBoundary(e);

            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRectangle();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (PresentDocument == null) return;

            ClickedPoint = new Point((Size)e.Location);
            MovingPoint = new Point((Size)ClickedPoint);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    // 선택된 List중에서 ZIndex가 가장 큰 Component를 선택한다.
                    var selectedList = PresentDocument.Components.FindAll(x => IsInside(x, ClickedPoint));
                    State.SelectedComponent = selectedList.Find(x => x.ZIndex == selectedList.Max(y => y.ZIndex));
                    break;
                case MouseButtons.Right:
                    var componentList = PresentDocument.Components.FindAll(x => IsInside(x, ClickedPoint));
                    UpdateBlackboardContextMenu(componentList);
                    blackboardContextMenu.Show(PointToScreen(e.Location));
                    break;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (State.SelectedComponent == null) return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    Point offset = e.Location - (Size)MovingPoint;
                    State.SelectedComponent.Move(offset);
                    MovingPoint = new Point((Size) e.Location);
                    State.log.Write(offset.ToString());
                    break;
            }
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Left:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            int dx = 0, dy=0;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    dx -= 1;
                    break;
                case Keys.Right:
                    dx += 1;
                    break;
                case Keys.Up:
                    dy -= 1;
                    break;
                case Keys.Down:
                    dy += 1;
                    break;
            }
            if (e.Shift) 
            {
                dx *= 10;
                dy *= 10;
            }
            if (State.SelectedComponent != null) State.SelectedComponent.Move(dx, dy);

            Invalidate();
        }
        #endregion

        /************************************************************************/
        /* Variables															*/
        /************************************************************************/
        public Document PresentDocument { get; set; }
        public Point ClickedPoint { get; private set; }
        public Point MovingPoint { get; private set; }

        public static Point LeftTopPoint;
        private readonly Size LeftTopOffset = new Size(50, 50);
        public Point[] Points { get; private set; }

        private const int BorderOffset = 0;
    }
}