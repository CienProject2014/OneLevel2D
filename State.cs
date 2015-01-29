using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using OneLevelJson.Annotations;
using OneLevelJson.CustomList;
using OneLevelJson.Model;

namespace OneLevelJson
{
    static class State
    {
        /************************************************************************/
        public static ComponentListView ComponentListView;
        /************************************************************************/

        public static CienDocument Document;
        public static Blackboard Board;
        public static CienComponent CopiedComponent;
        public static SelectedNotifier _selected = new SelectedNotifier();
        public static SelectedNotifier Selected
        {
            get { return _selected; }
        }

        public static void SetBoard(Blackboard board)
        {
            Board = board;
        }

        public static void SetComponentListView(ComponentListView clv)
        {
            ComponentListView = clv;
        }

        public static void SelectComponent(CienComponent component)
        {
            _selected.Component = component;

            if (!_selected.ComponentList.Contains(component))
                _selected.ComponentList.Add(component);
        }

        public static void UnSelectComponent(CienComponent component)
        {
            _selected.ComponentList.Remove(component);
        }

        public static void SelectLayer(CienLayer layer)
        {
            _selected.Layer = layer;
        }

        public static void SelectAbandon()
        {
            SelectComponent(CienComponent.Empty);
            _selected.ComponentList.Clear();
        }

        public static bool IsComponentSelected()
        {
            if (_selected.Component == null) _selected.Component = CienComponent.Empty;
            return !_selected.Component.Equals(CienComponent.Empty);
        }

        public static bool IsLayerSelected()
        {
            if (_selected.Layer == null) _selected.Layer = CienLayer.Empty;
            return !_selected.Layer.Equals(CienLayer.Empty);
        }

        #region Commands
        public static Stack<Command> Commands = new Stack<Command>();
        public static Command LastCommand;
        public static CienComponent LastComponent;

        public static void CommandMoveStart(Point startPoint)
        {
            var command = new MoveCommand();

            command.StartPoint = startPoint;

            LastCommand = command;

            LastComponent = _selected.Component;
        }

        public static void CommandMoveEnd(Point endPoint)
        {
            if (LastCommand == null) return;

            var command = (MoveCommand)LastCommand;

            command.EndPoint = endPoint;

            LastCommand = command;

            Commands.Push(LastCommand);
            LastCommand = null;
        }

        public static void ReverseLastCommand()
        {
            var command = Commands.Pop();
            if (command is MoveCommand)
            {
                var moveCommand = (MoveCommand)command;
                var offset = (Size)moveCommand.EndPoint - (Size)moveCommand.StartPoint;
                LastComponent.SetLocation(LastComponent.Location - offset);
            }
        }
        #endregion

    }

    class SelectedNotifier : INotifyPropertyChanged
    {
        private CienLayer _layer = new CienLayer(CienLayer.DefaultLayerName);
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
        private List<CienComponent> _componentList = new List<CienComponent>();
        public List<CienComponent> ComponentList
        {
            get { return _componentList; }
            set { _componentList = value; }
        }

        public void ChangeComponentId(string id)
        {
            Component.SetId(id);
            NotifyPropertyChanged();
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

        private void MoveUp()
        {
            var toDown = State.Document.Components.Find(x => x.ZIndex == Component.ZIndex + 1);
            if (toDown != null)
            {
                toDown.SetZindex(Component.ZIndex);
            }
            Component.SetZindex(Component.ZIndex + 1);

            NotifyPropertyChanged();
        }

        private void MoveDown()
        {
            var toUp = State.Document.Components.Find(x => x.ZIndex == Component.ZIndex - 1);
            if (toUp != null)
            {
                toUp.SetZindex(Component.ZIndex);
            }
            Component.SetZindex(Component.ZIndex - 1);

            NotifyPropertyChanged();
        }

        public void MoveSelectedUp()
        {
            // 내림차순
            ComponentList.Sort((a, b) => b.ZIndex.CompareTo(a.ZIndex));

            var maxZindex = State.Document.Components.Max(x => x.ZIndex);

            foreach (var component in ComponentList)
            {
                if (maxZindex == component.ZIndex) break;

                Component = component;
                MoveUp();
            }

            //State.ComponentListView.SortListDescending();
        }

        public void MoveSelectedDown()
        {
            // 오름차순
            ComponentList.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));

            var minZindex = State.Document.Components.Min(x => x.ZIndex);

            foreach (var component in ComponentList)
            {
                if (minZindex == component.ZIndex) break;

                Component = component;
                MoveDown();
            }

            //State.ComponentListView.SortListDescending();

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
