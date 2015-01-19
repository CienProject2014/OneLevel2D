using System;
using System.Windows.Forms;

namespace OneLevelJson
{
    public partial class SelectedControl : UserControl
    {
        public SelectedControl()
        {
            InitializeComponent();
        }

        private void SelectedControl_Load(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource(State.Selected, null);

            componentIdLabel.DataBindings.Add("Text", bs, "Component.Id");
            xyValueLabel.DataBindings.Add("Text", bs, "Component.ConvertedLocation");
        }
    }
}
