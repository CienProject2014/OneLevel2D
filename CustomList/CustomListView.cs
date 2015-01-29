using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using OneLevel2D.CustomList;
using OneLevel2D.Model;

namespace OneLevel2D
{
    public partial class CustomListView : UserControl
    {
        #region Variables
        protected List<CustomItem> items;
        #endregion

        protected CustomListView()
        {
            InitializeComponent();

            items = new List<CustomItem>();
        }

        #region Add Item
        protected void AddTestItem(string name)
        {
/*            CustomItem item = new CustomItem(name, new Point(0, CustomItem.ItemHeight*GetNumber()))
            {
                Width = listPanel.Width,
                Anchor = (AnchorStyles.Left | AnchorStyles.Right)
            };

            AddItem(item);*/
        }

        protected void AddItem(CustomItem item)
        {
            if (items.Find(x => x.Name == item.Name) != null)
            {
                MessageBox.Show(item.Name + @": 이미 같은 이름의 Item이 List에 있습니다.");
                return;
            }

            items.Add(item);

            if (item is LayerItem)
            {
                LayerItem realItem = (LayerItem) item;
                listPanel.Controls.Add(realItem);
            }
            else if (item is ComponentItem)
            {
                ComponentItem realItem = (ComponentItem) item;
                listPanel.Controls.Add(realItem);
            }
            else
            {
                listPanel.Controls.Add(item);
            }
        }
        #endregion

        #region Remove Item

        protected void RemoveItem(string name)
        {
            var removable = items.Find(x => x.Name == name);
            if (removable != null)
                RemoveItem(removable);
        }

        protected void RemoveItem(CustomItem item)
        {
            items.Remove(item);
            var removable = listPanel.Controls.Find(item.Name, false);
            listPanel.Controls.Remove(removable[0]);
        }
        public void Clear()
        {
            items.Clear();
            listPanel.Controls.Clear();
        }
        #endregion

        #region Move Item

        protected void MoveSelectedUp()
        {
            // TODO Controls에서 순서를 바꿔주는 방법 개선
            var selected = items.FindAll(x => x.IsSelected);

            for (int i = 0; i < selected.Count; i++)
            {
                int index = items.IndexOf(selected[i]);
                if (index == 0) break;


                var temp = items[index];
                items[index] = items[index - 1];
                items[index - 1] = temp;

                UpdateListPanel();

                listPanel.ScrollControlIntoView(selected[0]);
            }
       }

        protected void MoveSelectedDown()
        {
            // TODO Controls에서 순서를 바꿔주는 방법 개선
            var selected = items.FindAll(x => x.IsSelected);

            for (int i = selected.Count-1; i >= 0; i--)
            {
                int index = items.IndexOf(selected[i]);
                if (index == items.Count-1) break;

                var temp = items[index];
                items[index] = items[index + 1];
                items[index + 1] = temp;

                UpdateListPanel();

                listPanel.ScrollControlIntoView(selected.Last());
            }
        }
        #endregion

        protected void SelectItem(CustomItem item)
        {
            item.ItemSelect();
            item.ShowSelected();

            listPanel.ScrollControlIntoView(item);
        }

        protected void UnselectItem(CustomItem item)
        {
            item.ItemUnselect();
            item.ShowLeave();
        }

        public void UnselectAll()
        {
            var selected = items.FindAll(x => x.IsSelected);
            foreach (var item in selected)
            {
                UnselectItem(item);
            }
        }

        protected void UpdateListPanel()
        {
            listPanel.Controls.Clear();
            foreach (var item in items)
            {
                listPanel.Controls.Add(item);
            }
            listPanel.Invalidate();
        }

        protected int GetY()
        {
            return CustomItem.ItemHeight*GetNumber();
        }

        protected int GetNumber()
        {
            return items.Count;
        }

    }

}
