using System.Drawing;
using System.Windows.Forms;
using OneLevelJson.Model;

namespace OneLevelJson.CustomList
{
    public class ComponentItem : CustomItem
    {
        public CienComponent Component { get; private set; }

        public ComponentItem(CienComponent component, Point location)
            : base(component.Id, location)
        {

            Component = component;

            Name = Component.Id;
            itemName.Text = Component.Id;

            Invalidate();
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (IsSelected)
                    State.SelectComponent(Component);
                else
                    State.UnSelectComponent(Component);    
            }
        }

        protected override void ChangeItem(string newId)
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
