using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using OneLevelJson.Annotations;
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

            ViewMatrix = new Matrix();
            ViewMatrix.Reset();

            AddEvent();
        }

        public void SetDocument(CienDocument document)
        {
            State.Document = document;
            UpdateRectangle();
        }

        private void AddEvent()
        {
            blackboardContextMenu.ItemClicked += blackboardContextMenu_ItemClicked;
        }

        private bool IsInside(CienComponent component, Point clicked)
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

        private bool IsSelectable(CienComponent component)
        {
            var layer = State.Document.Layers.Find(x => x.Name == component.LayerName);
            return layer.IsVisible && !layer.IsLocked;
        }

        
        /************************************************************************/
        /* Drawing																*/
        /************************************************************************/
        #region Drawing Method

        protected override void OnPaint(PaintEventArgs e)
        {
            if (State.Document == null) return;

            e.Graphics.Transform = ViewMatrix;

            DrawComponentList(e);
            DrawSelectedComponentBorder(e);
            DrawBoundary(e);
        }

        private void DrawComponent(PaintEventArgs e, CienComponent component)
        {
            Image img = component.GetImage();

            e.Graphics.DrawImage(img, new Rectangle(component.Location,
                new Size(img.Width, img.Height)));
        }

        private void DrawComponentList(PaintEventArgs e)
        {
            // TODO 그리기 전에도 한 번 정렬해준다. 속도 이슈가 발생하면 삭제해야할듯.
            State.Document.Components.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));

            foreach (var component in State.Document.Components)
            {
                if (State.Document.Layers.Find(x => x.Name == component.LayerName).IsVisible)
                    DrawComponent(e, component);
            }
        }

        private void DrawSelectedComponentBorder(PaintEventArgs e)
        {
            var component = State.Selected.Component;
            if (!State.IsComponentSelected()) return;

            Point[] points =
                {
                    new Point(component.Location.X-BorderOffset, component.Location.Y-BorderOffset),
                    new Point(component.Location.X+component.GetSize().Width+BorderOffset, component.Location.Y-BorderOffset),
                    new Point(component.Location.X+BorderOffset+component.GetSize().Width, component.Location.Y+component.GetSize().Height+BorderOffset),
                    new Point(component.Location.X-BorderOffset, component.Location.Y+component.GetSize().Height+BorderOffset),
                    new Point(component.Location.X-BorderOffset, component.Location.Y-BorderOffset)
                };

            using (Pen pen = new Pen(Color.FromArgb(62, 62, 66)))
            {
                e.Graphics.DrawLines(pen, points);
            }

        }

        private void DrawBoundary(PaintEventArgs e)
        {
            if (RectanglePoints == null) return;

            using (Pen pen = new Pen(BoundaryColor))
            {
                e.Graphics.DrawLines(pen, RectanglePoints);
            }
        }
        #endregion

        /************************************************************************/
        /* Override 															*/
        /************************************************************************/
        #region Override Method

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateRectangle();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!Focused) Focus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (State.Document == null) return;

            ClickedPoint = PointTransform(e.Location);
            PreviousPoint = new Point((Size)ClickedPoint);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    // 선택된 List중에서 ZIndex가 가장 큰 Component를 선택한다.
                    var selectables = State.Document.Components.FindAll(x => IsInside(x, ClickedPoint) && IsSelectable(x));

                    if (selectables.Count == 0)
                    {
                        State.SelectAbandon();
                        break;
                    }

                    var candidate = selectables.Find(x => x.ZIndex == selectables.Max(y => y.ZIndex));
                    State.SelectComponent(candidate);

                    State.CommandMoveStart(e.Location);

                    break;
                case MouseButtons.Right:
                    var componentList = State.Document.Components.FindAll(x => IsInside(x, ClickedPoint));
                    UpdateBlackboardContextMenu(componentList);
                    blackboardContextMenu.Show(PointToScreen(e.Location));
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            CursorPosition = PointTransform(new Point(e.X, e.Y));

            Point offset = CursorPosition - (Size)PreviousPoint;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!State.IsComponentSelected()) break;
                    State.Selected.Move(offset);
                    break;
                case MouseButtons.Middle:
                    TranslateX = offset.X;
                    TranslateY = offset.Y;
                    TranslateBoard();
                    break;
            }

            PreviousPoint = PointTransform(new Point(e.X, e.Y));

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            State.CommandMoveEnd(e.Location);
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // e.Delta값이 음수이면 뒤쪽(사용자), 양수이면 앞쪽(반대)
            //State.log.Write(e.Delta.ToString());
            //MessageBox.Show(e.Delta.ToString());

            if (e.Delta < 0)
            {
                _zoom = 1.0f - ScaleFactor;
                _zoomFiled *= _zoom;
            }
            else if (e.Delta > 0)
            {
                _zoom = 1.0f + ScaleFactor;
                _zoomFiled *= _zoom;
            }

            if (_zoomFiled <= MinimumZoom) { _zoomFiled = MinimumZoom; return; }
            if (_zoomFiled >= MaximumZoom) { _zoomFiled = MaximumZoom; return; }

            ScaleBoard();
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

            Invalidate();
        }
        #endregion

        /************************************************************************/
        /* Etc.																    */
        /************************************************************************/
        #region Trasformation
        private void TranslateBoard()
        {
            ViewMatrix.Translate(TranslateX, TranslateY);
        }

        private void ScaleBoard()
        {
            Point CursorCenterOffset = CursorPosition - (Size)(new Point(Width / 2, Height / 2));
            ViewMatrix.Translate(CursorCenterOffset.X, CursorCenterOffset.Y);
            ViewMatrix.Scale(_zoom, _zoom);
            ViewMatrix.Translate(-CursorCenterOffset.X, -CursorCenterOffset.Y);
        }

        public Point PointTransform(Point point)
        {
            Point[] points = { point };
            using (Matrix invertViewMatrix = ViewMatrix.Clone())
            {
                invertViewMatrix.Invert();
                invertViewMatrix.TransformPoints(points);
            }
            return points[0];
        }
        #endregion

        #region Delegate Method
        void blackboardContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var selected = State.Document.Components.Find(x => x.Id == e.ClickedItem.Text);
            if (selected != null)
            {
                State.SelectComponent(selected);
            }
            else
            {
                switch (e.ClickedItem.Text)
                {
                    case ConvertToButton:
                        State.Document.ConvertToComposite("sf");
                        break;
                }
            }
            Invalidate();
        }
        #endregion

        #region Update Method
        private void UpdateBlackboardContextMenu(List<CienComponent> componentList)
        {
            blackboardContextMenu.Items.Clear();
            if (componentList.Count != 0)
            {
                blackboardContextMenu.Items.Add(ConvertToButton);
            }

            foreach (var component in componentList)
            {
                blackboardContextMenu.Items.Add(component.Id);
            }
        }

        private void UpdateRectangle()
        {
            if (State.Document == null) return;
            Point leftTop = new Point(0, 0);

            leftTop = leftTop + LeftTopOffset;

            RectanglePoints = new[]
            {
                leftTop,
                leftTop + new Size(State.Document.Width, 0),
                leftTop + new Size(State.Document.Width, State.Document.Height),
                leftTop + new Size(0, State.Document.Height),
                leftTop
            };
        }
        #endregion

        /* Variables ************************************************************/
        public Point[] RectanglePoints { get; private set; }

        public Point CursorPosition { get; private set; }
        public Point ClickedPoint { get; private set; }
        public Point PreviousPoint { get; private set; }

        public Matrix ViewMatrix { get; private set; }
        public int TranslateX { get; private set; }
        public int TranslateY { get; private set; }

        private float _zoom = 1.0f;
        private float _zoomFiled = 1.0f;

        public static readonly Size LeftTopOffset = new Size(50, 50);
        private readonly Color BoundaryColor = Color.FromArgb(96, 96, 102);
        private const float ScaleFactor = 0.08f;
        private const float MinimumZoom = 0.2f;
        private const float MaximumZoom = 2.5f;
        private const int BorderOffset = 0;

        private const string ConvertToButton = "Convert to button";
        /************************************************************************/

    }
}