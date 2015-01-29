using System;
using System.Windows.Forms;

namespace OneLevel2D
{
    public partial class ContextRenameForm : Form
    {
        public string Result { get; private set; }

        public ContextRenameForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result = textBox1.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
