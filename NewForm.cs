using System;
using System.Windows.Forms;

namespace OneLevelJson
{
    public partial class NewForm : Form
    {
        public new string Name { get; private set; }
        public new int Width { get; private set; }
        public new int Height { get; private set; }

        public NewForm()
        {
            InitializeComponent();

            widthBox.KeyPress += widthBox_KeyPress;
            heightBox.KeyPress += heightBox_KeyPress;
        }

        void widthBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }

        void heightBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Name = nameBox.Text;
            Width = int.Parse(widthBox.Text);
            Height = int.Parse(heightBox.Text);

            DialogResult = DialogResult.OK;
        }

    }
}
