using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using OneLevelJson.Annotations;
using OneLevelJson.Model;

namespace OneLevelJson
{
    static class State
    {
        //public static Log log = new Log();
        public static CienDocument Document;
        public static SelectedNotifier _selected = new SelectedNotifier();
        public static SelectedNotifier Selected
        {
            get { return _selected; }
            private set { _selected = value; }
        }

        public static void SelectComponent(CienComponent component)
        {
            _selected.Component = component;
        }

        public static void SelectLayer(CienLayer layer)
        {
            _selected.Layer = layer;
        }

        public static void SelectAbandon()
        {
            SelectComponent(CienComponent.Empty);
        }

        public static bool IsComponentSelected()
        {
            if (Selected.Component == null) Selected.Component = CienComponent.Empty;
            return !Selected.Component.Equals(CienComponent.Empty);
        }

        public static bool IsLayerSelected()
        {
            if (Selected.Layer == null) Selected.Layer = CienLayer.Empty;
            return !Selected.Layer.Equals(CienLayer.Empty);
        }
    }

    class SelectedNotifier : INotifyPropertyChanged
    {
        public CienLayer _layer = new CienLayer(CienLayer.DefaultLayerName);
        public CienLayer Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }
        private CienComponent _component = CienComponent.Empty;
        public CienComponent Component
        {
            get { return _component; }
            set { _component = value; NotifyPropertyChanged(); }
        }

        public void Move(Point offset)
        {
            Component.SetLocation(Component.Location + (Size)offset);
            NotifyPropertyChanged();
        }

        public void Move(int dx, int dy)
        {
            Component.SetLocation(Component.Location + new Size(dx, dy));
            NotifyPropertyChanged();
        }

        public void MoveUp()
        {
            Component.SetZindex(Component.ZIndex + 1);
            NotifyPropertyChanged();
        }

        public void MoveDown()
        {
            Component.SetZindex(Component.ZIndex - 1);
            NotifyPropertyChanged();
        }

        #region Binding

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotifyPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        #endregion

    }
}
