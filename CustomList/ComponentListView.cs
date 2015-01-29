using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OneLevel2D.Model;
using OneLevel2D.Properties;

namespace OneLevel2D.CustomList
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

            SortDescending();
        }

        public void SortDescending()
        {
            var sorting = new List<ComponentItem>(items.Count);
            sorting.AddRange(items.Select(item => item as ComponentItem));
            // 내림차순 (component list)
            sorting.Sort((a, b) => b.Component.ZIndex.CompareTo(a.Component.ZIndex));

            items.Clear();
            items = new List<CustomItem>(sorting);

            UpdateListPanel();
        }

        public void RemoveComponent(CienComponent component)
        {
            RemoveItem(component.Id);
        }

        public void RemoveSelectedComponent()
        {
            var removable = items.FindAll(x => x.IsSelected);
            foreach (var item in removable)
            {
                RemoveItem(item);
            }
        }

        public void SelectComponent(CienComponent component)
        {
            foreach (var componentItem in items.Cast<ComponentItem>().Where(componentItem => componentItem.Component.Id == component.Id))
            {
                SelectItem(componentItem);
            }
        }

        public void UnselectComponent(CienComponent component)
        {
            foreach (var componentItem in items.Cast<ComponentItem>().Where(componentItem => componentItem.Component.Id == component.Id))
            {
                UnselectItem(componentItem);
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
            State.RemoveSelectedComponent();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ComponentListView));
            this.statusBar = new TableLayoutPanel();
            this.upBox = new PictureBox();
            this.downBox = new PictureBox();
            this.minusBox = new PictureBox();
            this.statusBar.SuspendLayout();
            ((ISupportInitialize)(this.upBox)).BeginInit();
            ((ISupportInitialize)(this.downBox)).BeginInit();
            ((ISupportInitialize)(this.minusBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.ColumnCount = 4;
            this.statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.statusBar.Controls.Add(this.upBox, 0, 0);
            this.statusBar.Controls.Add(this.downBox, 0, 0);
            this.statusBar.Controls.Add(this.minusBox, 0, 0);
            this.statusBar.Dock = DockStyle.Bottom;
            this.statusBar.Location = new Point(0, 278);
            this.statusBar.Name = "statusBar";
            this.statusBar.RightToLeft = RightToLeft.Yes;
            this.statusBar.RowCount = 1;
            this.statusBar.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.statusBar.Size = new Size(278, 20);
            this.statusBar.TabIndex = 6;
            // 
            // upBox
            // 
            this.upBox.BackgroundImage = ((Image)(resources.GetObject("upBox.BackgroundImage")));
            this.upBox.BackgroundImageLayout = ImageLayout.Stretch;
            this.upBox.Dock = DockStyle.Fill;
            this.upBox.Location = new Point(221, 3);
            this.upBox.Name = "upBox";
            this.upBox.Size = new Size(14, 14);
            this.upBox.TabIndex = 2;
            this.upBox.TabStop = false;
            this.upBox.Click += new EventHandler(this.upBox_Click);
            // 
            // downBox
            // 
            this.downBox.BackgroundImage = Resources.downarrow1;
            this.downBox.BackgroundImageLayout = ImageLayout.Stretch;
            this.downBox.Dock = DockStyle.Fill;
            this.downBox.Location = new Point(241, 3);
            this.downBox.Name = "downBox";
            this.downBox.Size = new Size(14, 14);
            this.downBox.TabIndex = 1;
            this.downBox.TabStop = false;
            this.downBox.Click += new EventHandler(this.downBox_Click);
            // 
            // minusBox
            // 
            this.minusBox.BackgroundImage = Resources.minus;
            this.minusBox.BackgroundImageLayout = ImageLayout.Stretch;
            this.minusBox.Dock = DockStyle.Fill;
            this.minusBox.Location = new Point(261, 3);
            this.minusBox.Name = "minusBox";
            this.minusBox.Size = new Size(14, 14);
            this.minusBox.TabIndex = 0;
            this.minusBox.TabStop = false;
            this.minusBox.Click += new EventHandler(this.minusBox_Click);
            // 
            // ComponentListView
            // 
            this.Controls.Add(this.statusBar);
            this.Name = "ComponentListView";
            this.Controls.SetChildIndex(this.statusBar, 0);
            this.statusBar.ResumeLayout(false);
            ((ISupportInitialize)(this.upBox)).EndInit();
            ((ISupportInitialize)(this.downBox)).EndInit();
            ((ISupportInitialize)(this.minusBox)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
