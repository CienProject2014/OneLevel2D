using System;
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
            if (e.Button == MouseButtons.Left)
            {
                if (!IsSelected)
                {
                    if (false) // TODO Component List View에서 여려개를 선택할 때 사용.
                    {
                        Debug.Print("select with ctrl");
                        State.SelectComponent(Component);
                    }
                    else
                    {
                        Debug.Print("select without ctrl");
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
