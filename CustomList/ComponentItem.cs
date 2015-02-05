using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OneLevel2D.Model;

namespace OneLevel2D.CustomList
{
    public class ComponentItem : CustomItem
    {
        public CienBaseComponent Component { get; private set; }

        public ComponentItem(CienBaseComponent component, Point location)
            : base(component.Id, location)
        {

            Component = component;

            Name = Component.Id;
            itemName.Text = Component.Id;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //base.OnMouseDown(e);
            var parentList = (ComponentListView) Parent.Parent;

            if (e.Button == MouseButtons.Left)
            {
                if (!IsSelected)
                {
                    if (parentList.MultipleSelect)
                    {
                        State.SelectComponent(Component);
                    }
                    else
                    {
                        State.SelectOneComponent(Component);
                    }

                    ItemSelect();
                    State.Board.Focus();
                }
                else
                {
                    State.UnselectComponent(Component);

                    ItemUnselect();
                }
            }

        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Control)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var parentList = (ComponentListView)Parent.Parent;
            if (e.Control)
                parentList.MultipleSelect = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            var parentList = (ComponentListView)Parent.Parent;
            if (parentList.MultipleSelect)
                parentList.MultipleSelect = false;
        }

        protected override void ChangeItemName(string newId)
        {
            if (newId == null) return;

            // TODO Name과 itemName.Text를 같게!
            Name = newId;
            itemName.Text = newId;
            State.Selected.ChangeComponentId(newId);
        }

        public override object Clone()
        {
            var component = new ComponentItem(Component, Location)
            {
                IsSelected = this.IsSelected
            };
            return component;
        }
    }
}
