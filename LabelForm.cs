using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using OneLevel2D.Model;

namespace OneLevel2D
{
    public partial class LabelForm : Form
    {
        public LabelForm()
        {
            InitializeComponent();

            LoadFont();

            AddEvent();
        }

        private void LoadFont()
        {
            var fontList = State.Document.Assets.FindAll(x => x.Type == AssetType.Font);
            for (int i = 0; i < fontList.Count; i++)
            {
                var a = new FontItem(fontList[i].GetName());
                fontComboBox.Items.Add(a);
            }
        }

        private void AddEvent()
        {
            sizeBox.KeyPress += color_KeyPress;
            rBox.KeyPress += color_KeyPress;
            gBox.KeyPress += color_KeyPress;
            bBox.KeyPress += color_KeyPress;
            aBox.KeyPress += color_KeyPress;
            rBox.TextChanged += color_TextChanged;
            gBox.TextChanged += color_TextChanged;
            bBox.TextChanged += color_TextChanged;
            aBox.TextChanged += color_TextChanged;
        }

        void color_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox) sender;
            int ipno = 0;
            if (!string.IsNullOrEmpty(textBox.Text))
                ipno = Convert.ToInt32(textBox.Text);

            if (ipno > 255)
            {
                textBox.Text = "255";
                textBox.Focus();
            } 
        }

        void color_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 )
            //8:백스페이스,45:마이너스,46:소수점
            {
                e.Handled = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var item = fontComboBox.SelectedItem as FontItem;
            if (item == null)
            {
                MessageBox.Show(@"폰트를 선택해주세요.");
                return;
            }
            var fontName = item.Name;
            var text = textBox.Text ?? "label";
            var tint = new List<float>(4)
            {
                (!string.IsNullOrEmpty(rBox.Text)) ? ToFloat(int.Parse(rBox.Text)) : ToFloat(255),
                (!string.IsNullOrEmpty(gBox.Text)) ? ToFloat(int.Parse(gBox.Text)) : ToFloat(255),
                (!string.IsNullOrEmpty(bBox.Text)) ? ToFloat(int.Parse(bBox.Text)) : ToFloat(255),
                (!string.IsNullOrEmpty(aBox.Text)) ? ToFloat(int.Parse(aBox.Text)) : ToFloat(255)
            };
            int size = (!string.IsNullOrEmpty(sizeBox.Text)) ? int.Parse(sizeBox.Text) : 100;

            State.MakeNewLabel(text, size, fontName, tint);

            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private float ToFloat(int value)
        {
            return value/255.0f;
        }
    }

    public class FontItem
    {
        public string Name { get; set; }
        public FontItem(string name){Name = name;}
        public override string ToString(){return Name;}
    }
}
