using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using OneLevel2D.Model;

namespace OneLevel2D.CustomList
{
    public class LayerListView : CustomListView
    {
        private const string Title = "Layer List";
        private TableLayoutPanel statusBar;
        private PictureBox minusBox;
        private PictureBox plusBox;

        public LayerListView()
        {
            titleName.Text = Title;
            InitializeComponent();
        }

        public void ChangeLayerList(List<CienLayer> layerList)
        {
            ItemClear();
            foreach (var component in layerList)
            {
                AddLayer(component);
            }
        }

        public void AddLayer(CienLayer layer)
        {
            LayerItem layerItem = new LayerItem(layer, new Point(0, GetY()))
            {// item을 listPanel에 꽉 차게 하기 위해.
                Width = listPanel.Width,
                Anchor = (AnchorStyles.Left | AnchorStyles.Right)
            };

            Debug.Print(layerItem.TabIndex.ToString());

            AddItem(layerItem);
        }

        private void NewLayer()
        {
            CienLayer layer = new CienLayer("layer" + GetNumber(), true, false);
            State.CurrentScene.Layers.Add(layer);
            AddLayer(layer);
        }

        private void RemoveLayer(CienLayer layer)
        {
            RemoveItem(layer.Name);
            State.CurrentScene.Components.RemoveAll(x => x.LayerName == layer.Name);
            State.Board.Invalidate();
        }

        private void RemoveSelectedLayer()
        {
            var removable = items.FindAll(x => x.IsSelected);
            foreach (var item in removable)
            {
                var layer = State.CurrentScene.Layers.Find(x => x.Name == item.itemName.Text);
                RemoveLayer(layer);
            }
        }

        private void plusBox_Click(object sender, System.EventArgs e)
        {
            NewLayer();
        }

        private void minusBox_Click(object sender, System.EventArgs e)
        {
            RemoveSelectedLayer();
        }

        private void InitializeComponent()
        {
            this.statusBar = new System.Windows.Forms.TableLayoutPanel();
            this.plusBox = new System.Windows.Forms.PictureBox();
            this.minusBox = new System.Windows.Forms.PictureBox();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plusBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minusBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.ColumnCount = 3;
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.Controls.Add(this.plusBox, 0, 0);
            this.statusBar.Controls.Add(this.minusBox, 0, 0);
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 277);
            this.statusBar.Name = "statusBar";
            this.statusBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusBar.RowCount = 1;
            this.statusBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.statusBar.Size = new System.Drawing.Size(278, 21);
            this.statusBar.TabIndex = 6;
            // 
            // plusBox
            // 
            this.plusBox.BackgroundImage = global::OneLevel2D.Properties.Resources.plus;
            this.plusBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.plusBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plusBox.Location = new System.Drawing.Point(241, 3);
            this.plusBox.Name = "plusBox";
            this.plusBox.Size = new System.Drawing.Size(14, 15);
            this.plusBox.TabIndex = 4;
            this.plusBox.TabStop = false;
            this.plusBox.Click += new System.EventHandler(this.plusBox_Click);
            // 
            // minusBox
            // 
            this.minusBox.BackgroundImage = global::OneLevel2D.Properties.Resources.minus;
            this.minusBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minusBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.minusBox.Location = new System.Drawing.Point(261, 3);
            this.minusBox.Name = "minusBox";
            this.minusBox.Size = new System.Drawing.Size(14, 15);
            this.minusBox.TabIndex = 3;
            this.minusBox.TabStop = false;
            this.minusBox.Click += new System.EventHandler(this.minusBox_Click);
            // 
            // LayerListView
            // 
            this.Controls.Add(this.statusBar);
            this.Name = "LayerListView";
            this.Controls.SetChildIndex(this.statusBar, 0);
            this.statusBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plusBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minusBox)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
