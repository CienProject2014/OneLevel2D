using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OneLevelJson.Model;

namespace OneLevelJson.CustomList
{
    public class ComponentListView : CustomListView
    {
        private TableLayoutPanel statusBar;
        private PictureBox minusBox;
        private PictureBox downBox;
        private PictureBox upBox;
        private const string Title = "Component List";

        public ComponentListView()
        {
            titleName.Text = Title;

            InitializeComponent();
        }

        public void AddComponent(CienComponent component)
        {
            ComponentItem componentItem = new ComponentItem(component, new Point(0, GetY()))
            {
                Width = listPanel.Width,
                Anchor = (AnchorStyles.Left | AnchorStyles.Right)
            };
            AddItem(componentItem);

            SortListDescending();
        }

        public void SortListDescending()
        {
            var sorting = new List<ComponentItem>(items.Count);
            sorting.AddRange(items.Select(item => item as ComponentItem));
            // 내림차순 (component list)
            sorting.Sort((a, b) => b.Component.ZIndex.CompareTo(a.Component.ZIndex));
            items = new List<CustomItem>(sorting);

            UpdateListPanel();
        }

        public void RemoveComponent(CienComponent component)
        {
            RemoveItem(component.Id);
            State.Document.RemoveComponent(component.Id);
            State.Board.Invalidate();
        }

        private void RemoveSelectedComponent()
        {
            var removable = items.FindAll(x => x.IsSelected);
            foreach (var item in removable)
            {
                var component = State.Document.Components.Find(x => x.Id == item.Name);
                if (component != null)
                    RemoveComponent(component);
            }
        }

        private void upBox_Click(object sender, EventArgs e)
        {
            base.MoveSelectedUp();

            State.Selected.MoveSelectedUp();

            State.Document.SortComponentsAscending();
            State.Board.Invalidate();
        }

        private void downBox_Click(object sender, EventArgs e)
        {
            base.MoveSelectedDown();

            State.Selected.MoveSelectedDown();

            State.Document.SortComponentsAscending();
            State.Board.Invalidate();
        }

        private void minusBox_Click(object sender, EventArgs e)
        {
            RemoveSelectedComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentListView));
            this.statusBar = new System.Windows.Forms.TableLayoutPanel();
            this.upBox = new System.Windows.Forms.PictureBox();
            this.downBox = new System.Windows.Forms.PictureBox();
            this.minusBox = new System.Windows.Forms.PictureBox();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minusBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.ColumnCount = 4;
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusBar.Controls.Add(this.upBox, 0, 0);
            this.statusBar.Controls.Add(this.downBox, 0, 0);
            this.statusBar.Controls.Add(this.minusBox, 0, 0);
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 278);
            this.statusBar.Name = "statusBar";
            this.statusBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusBar.RowCount = 1;
            this.statusBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.statusBar.Size = new System.Drawing.Size(278, 20);
            this.statusBar.TabIndex = 6;
            // 
            // upBox
            // 
            this.upBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("upBox.BackgroundImage")));
            this.upBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.upBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upBox.Location = new System.Drawing.Point(221, 3);
            this.upBox.Name = "upBox";
            this.upBox.Size = new System.Drawing.Size(14, 14);
            this.upBox.TabIndex = 2;
            this.upBox.TabStop = false;
            this.upBox.Click += new System.EventHandler(this.upBox_Click);
            // 
            // downBox
            // 
            this.downBox.BackgroundImage = global::OneLevelJson.Properties.Resources.downarrow1;
            this.downBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.downBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downBox.Location = new System.Drawing.Point(241, 3);
            this.downBox.Name = "downBox";
            this.downBox.Size = new System.Drawing.Size(14, 14);
            this.downBox.TabIndex = 1;
            this.downBox.TabStop = false;
            this.downBox.Click += new System.EventHandler(this.downBox_Click);
            // 
            // minusBox
            // 
            this.minusBox.BackgroundImage = global::OneLevelJson.Properties.Resources.minus;
            this.minusBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minusBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.minusBox.Location = new System.Drawing.Point(261, 3);
            this.minusBox.Name = "minusBox";
            this.minusBox.Size = new System.Drawing.Size(14, 14);
            this.minusBox.TabIndex = 0;
            this.minusBox.TabStop = false;
            this.minusBox.Click += new System.EventHandler(this.minusBox_Click);
            // 
            // ComponentListView
            // 
            this.Controls.Add(this.statusBar);
            this.Name = "ComponentListView";
            this.Controls.SetChildIndex(this.statusBar, 0);
            this.statusBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.upBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minusBox)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
