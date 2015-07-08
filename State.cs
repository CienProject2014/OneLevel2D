using System.Collections.Generic;
using System.ComponentModel;
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
        public static LayerListView LayerView;
        public static CienDocument Document;
        public static TabControl SceneTab;
        public static Blackboard Board;
        public static CienScene CurrentScene;
        /************************************************************************/
        
        public static List<CienBaseComponent> CopiedComponentList;
        public static SelectedNotifier _selected = new SelectedNotifier();
        public static SelectedNotifier Selected
        {
            get { return _selected; }
        }

        #region Scene

        public static void NewScene()
        {
            Document.NewScene();

            var lastScene = Document.Scenes.Last();
            if (lastScene == null) return;

            var newPage = new TabPage(lastScene.Name);
            newPage.Controls.Add(Board);
            SceneTab.TabPages.Add(newPage);

            SceneTab.SelectTab(newPage);
        }

        #endregion

        #region Component Control
        // Asset List에서 만들어지는 Image는 무조건 이 함수를 통해서 만들어져야 한다.
        public static void MakeNewImage(string assetName, Point location)
        {
            Asset asset = Document.Assets.Find(x => x.GetName() == assetName);

            string id = "image" + CurrentScene.Components.Count;
            id = Document.GetNewId(id);

            if (!IsLayerSelected())
            {
                MessageBox.Show(@"선택된 layer가 없습니다!");
                return;
            }

            var newImage = new CienImage(asset.GetNameWithExt(), id, location,
                CurrentScene.Components.Count, Selected.Layer.Name);

            // 1. Document에 추가하고
            Document.AddComponent(newImage);

            // 2. List View에 추가한다.
            ComponentView.AddComponent(newImage);

        }

        public static void MakeNewLabel(string text, int size, string fontName, List<float> tint)
        {
            string id = "label" + CurrentScene.Components.Count;
            id = Document.GetNewId(id);

            if (!IsLayerSelected())
            {
                MessageBox.Show(@"선택된 layer가 없습니다!");
                return;
            }

            var newLabel = new CienLabel(text, size, fontName, tint, id,
                CurrentScene.Components.Count, Selected.Layer.Name);

            // 1. Document에 추가하고
            Document.AddComponent(newLabel);

            // 2. List View에 추가한다.
            ComponentView.AddComponent(newLabel);
        }

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
            CienBaseComponent comp = CurrentScene.Components.Find(x => x.Id == id);
            if (comp is CienComposite) return;

            CienComposite newComposite;
            var composites = CurrentScene.Components.FindAll(x => x is CienComposite);
            string newId = "composite" + composites.Count;

            newComposite = new CienComposite(newId, comp.Location, Document.GetNewZindex(), comp.LayerName);
            newComposite.AddComponent(comp);
            RemoveSelectedComponent();
            Document.AddComponent(newComposite);
            ComponentView.AddComponent(newComposite);
        }

        #endregion

        #region Select
        public static void SelectComponent(CienBaseComponent component)
        {
            _selected.Component = component;

            if (!_selected.ComponentList.Contains(component) && component != CienBaseComponent.Empty)
                _selected.ComponentList.Add(component);

            ComponentView.SelectComponent(_selected.Component);
            Board.Invalidate();
        }

        public static void SelectOneComponent(CienBaseComponent component)
        {
            SelectedComponentAbandon();
            SelectComponent(component);
        }

        public static void UnselectComponent(CienBaseComponent component)
        {
            _selected.ComponentList.Remove(component);

            if (_selected.Component.Id == component.Id)
                ComponentView.UnselectComponent(_selected.Component);
        }

        public static void SelectedComponentAbandon()
        {
            _selected.Component = CienBaseComponent.Empty;
            _selected.ComponentList.Clear();

            ComponentView.UnselectAll();
        }

        public static bool IsComponentSelected()
        {
            return _selected.ComponentList.Count != 0;
        }

        public static void SelectOneLayer(CienLayer layer)
        {
            _selected.Layer = layer;

            LayerView.UnselectAll();
        }

        public static bool IsLayerSelected()
        {
            if (_selected.Layer == null) _selected.Layer = CienLayer.Empty;
            
            if (!CurrentScene.Layers.Contains(_selected.Layer)) _selected.Layer = CienLayer.Empty;

            return !_selected.Layer.Equals(CienLayer.Empty);
        }

        public static void SelectedLayerAbandon()
        {
            _selected.Layer = CienLayer.Empty;
        }
        #endregion

        #region Command

        public static void Copy()
        {
            if (_selected.ComponentList.Count == 0) return;

            CopiedComponentList = new List<CienBaseComponent>(_selected.ComponentList.Count);
            _selected.ComponentList.ForEach(item =>
            {
                CopiedComponentList.Add((CienBaseComponent)item.Clone());
            });
        }

        public static void Paste()
        {
            if (CopiedComponentList.Count == 0) return;

            foreach (var component in CopiedComponentList)
            {
                var newComponent = (CienBaseComponent)component.Clone();
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

            if (command.IsMoved())
            {
                _lastCommand = command;
                Commands.Push(_lastCommand);
            }
            
            _lastCommand = null;
        }

        public static void CommandAddComponent(List<CienBaseComponent> components)
        {
            var command = new AddCommand(Command.ADD, components);

            Commands.Push(command);
        }

        public static void CommandRemoveComponent(List<CienBaseComponent> components)
        {
            var command = new RemoveCommand(Command.REMOVE, components);

            Commands.Push(command);
        }

        public static void CommandResizeStart(Point startPoint)
        {
            var command = new ResizeCommand(Command.RESIZE, _selected.Component);

            command.StartPoint = startPoint;
            _lastCommand = command;
        }

        public static void CommandResizeEnd(Point endPoint)
        {
            if (_lastCommand == null) return;

            var command = (ResizeCommand)_lastCommand;
            command.EndPoint = endPoint;

            if (command.IsResized())
            {
                _lastCommand = command;
                Commands.Push(_lastCommand);
            }
            
            _lastCommand = null;
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
        private CienBaseComponent _component = CienBaseComponent.Empty;
        public CienBaseComponent Component
        {
            get { return _component; }
            set { _component = value; NotifyPropertyChanged(); }
        }
        private List<CienBaseComponent> _componentList = new List<CienBaseComponent>();
        public List<CienBaseComponent> ComponentList
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
            var toDown = State.CurrentScene.Components.Find(x => x.ZIndex == Component.ZIndex + 1);
            if (toDown != null)
            {
                toDown.SetZindex(Component.ZIndex);
            }
            Component.SetZindex(Component.ZIndex + 1);

            NotifyPropertyChanged();
        }

        private void MoveDown()
        {
            var toUp = State.CurrentScene.Components.Find(x => x.ZIndex == Component.ZIndex - 1);
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

            var maxZindex = State.CurrentScene.Components.Max(x => x.ZIndex);

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

            var minZindex = State.CurrentScene.Components.Min(x => x.ZIndex);

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
