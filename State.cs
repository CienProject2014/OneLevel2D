using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using OneLevel2D.Annotations;
using OneLevel2D.CustomList;
using OneLevel2D.Model;

namespace OneLevel2D
{
    static class State
    {
        /************************************************************************/
        public static ComponentListView ComponentView;
        public static CienDocument Document;
        public static Blackboard Board;
        public static CompositeEditForm CompositeEditForm = new CompositeEditForm();
        /************************************************************************/
        
        public static List<CienComponent> CopiedComponentList;
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
            ComponentView = clv;
        }

        #region Component Control
        // from document and view
        public static void RemoveSelectedComponent()
        {
            foreach (var component in _selected.ComponentList)
            {
                Document.RemoveComponent(component.Id);
                ComponentView.RemoveComponent(component);
            }
        }

        /*
         * Convert CienImage instance to CienComposite
         */
        public static void ConvertToComposite(string id)
        {
            // TODO sImage to sComposite
            CienComponent comp = Document.CurrentScene.Components.Find(x => x.Id == id);
            if (comp is CienComposite) return;

            CienImage image = comp as CienImage;
            if (image == null) return;

            CienComposite newComposite = new CienComposite(image.ImageName, image.Id, image.Location, image.ZIndex);
            RemoveSelectedComponent();
            Document.AddComponent(newComposite);
            ComponentView.AddComponent(newComposite);
        }

        #endregion

        #region Select
        public static void SelectComponent(CienComponent component)
        {
            _selected.Component = component;

            if (!_selected.ComponentList.Contains(component) && component != CienComponent.Empty)
                _selected.ComponentList.Add(component);

            ComponentView.SelectComponent(_selected.Component);
            Board.Invalidate();
        }

        public static void SelectOneComponent(CienComponent component)
        {
            SelectedComponentAbandon();
            SelectComponent(component);
        }

        public static void UnselectComponent(CienComponent component)
        {
            _selected.ComponentList.Remove(component);

            if (_selected.Component.Id == component.Id)
                ComponentView.UnselectComponent(_selected.Component);
        }

        public static void SelectedComponentAbandon()
        {
            SelectComponent(CienComponent.Empty);
            _selected.ComponentList.Clear();

            ComponentView.UnselectAll();
        }

        public static bool IsComponentSelected()
        {
            return _selected.ComponentList.Count != 0;
        }

        public static void SelectLayer(CienLayer layer)
        {
            _selected.Layer = layer;
        }

        public static bool IsLayerSelected()
        {
            if (_selected.Layer == null) _selected.Layer = CienLayer.Empty;
            return !_selected.Layer.Equals(CienLayer.Empty);
        }
        #endregion

        #region Command

        public static void Copy()
        {
            if (_selected.ComponentList.Count == 0) return;

            CopiedComponentList = new List<CienComponent>(_selected.ComponentList.Count);
            _selected.ComponentList.ForEach((item) =>
            {
                CopiedComponentList.Add((CienComponent)item.Clone());
            });
        }

        public static void Paste()
        {
            if (CopiedComponentList.Count == 0) return;

            foreach (var component in CopiedComponentList)
            {
                var newComponent = (CienComponent)component.Clone();
                newComponent.SetZindex(Document.GetNewZindex());
                Document.AddComponent(newComponent);
                ComponentView.AddComponent(newComponent);
            }
        }

        #endregion

        #region Command Class
        public static readonly Stack<Command> Commands = new Stack<Command>();
        public static readonly Stack<Command> RevertedCommands = new Stack<Command>();
        private static Command _lastCommand;

        public static void CommandMoveStart(Point startPoint)
        {
            var command = new MoveCommand(Command.MOVE, _selected.ComponentList);

            command.StartPoint = startPoint;
            _lastCommand = command;
        }

        public static void CommandMoveEnd(Point endPoint)
        {
            if (_lastCommand == null) return;

            var command = (MoveCommand)_lastCommand;
            command.EndPoint = endPoint;

            if (!command.IsMoved())
            {
                _lastCommand = null;
                return;
            }

            _lastCommand = command;
            Commands.Push(_lastCommand);
            _lastCommand = null;
        }

        public static void CommandAddComponent(List<CienComponent> components)
        {
            var command = new AddCommand(Command.ADD, components);

            Commands.Push(command);
        }

        public static void CommandRemoveComponent(List<CienComponent> components)
        {
            var command = new RemoveCommand(Command.REMOVE, components);

            Commands.Push(command);
        }

        private static void DoCommand(Command command)
        {
            switch (command.Name)
            {
                case Command.MOVE:
                    var moveCommand = (MoveCommand)command;
                    var offset = (Size)moveCommand.EndPoint - (Size)moveCommand.StartPoint;

                    foreach (var component in moveCommand.ComponentList)
                    {
                        component.SetLocation(component.Location + offset);
                    }

                    break;
                case Command.ADD:

                    break;
                case Command.REMOVE:
                    var removeCommand = (RemoveCommand)command;
                    foreach (var component in removeCommand.ComponentList)
                    {
                        Document.RemoveComponent(component.Id);
                        ComponentView.RemoveComponent(component);
                    }
                    break;
            }
        }

        private static void DoReverse(Command command)
        {
            switch (command.Name)
            {
                case Command.MOVE:
                    var moveCommand = (MoveCommand)command;
                    var offset = (Size)moveCommand.EndPoint - (Size)moveCommand.StartPoint;

                    foreach (var component in moveCommand.ComponentList)
                    {
                        component.SetLocation(component.Location - offset);
                    }

                    break;
                case Command.ADD:
                    var addCommand = (AddCommand)command;
                    foreach (var component in addCommand.ComponentList)
                    {
                        Document.RemoveComponent(component.Id);
                        ComponentView.RemoveComponent(component);
                    }
                    break;
                case Command.REMOVE:
                    
                    break;
            }
        }

        public static void ReverseLastCommand()
        {
            var command = Commands.Pop();
            DoReverse(command);
            RevertedCommands.Push(command);
        }

        public static void ReverseLastReverse()
        {
            var command = RevertedCommands.Pop();
            DoCommand(command);
            Commands.Push(command);
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
            if (ComponentList.Count != 1)
            {
                foreach (var component in ComponentList)
                {
                    component.SetLocation(component.Location + (Size)offset);
                }
            }
            else
            {
                Component.SetLocation(Component.Location + (Size)offset);
            }
            NotifyPropertyChanged();
        }

        private void MoveUp()
        {
            var toDown = State.Document.CurrentScene.Components.Find(x => x.ZIndex == Component.ZIndex + 1);
            if (toDown != null)
            {
                toDown.SetZindex(Component.ZIndex);
            }
            Component.SetZindex(Component.ZIndex + 1);

            NotifyPropertyChanged();
        }

        private void MoveDown()
        {
            var toUp = State.Document.CurrentScene.Components.Find(x => x.ZIndex == Component.ZIndex - 1);
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

            var maxZindex = State.Document.CurrentScene.Components.Max(x => x.ZIndex);

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

            var minZindex = State.Document.CurrentScene.Components.Min(x => x.ZIndex);

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
