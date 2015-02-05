using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Markup.Localizer;
using System.Windows.Navigation;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienDocument
    {
        // static 변수는 자동으로 제외하기 때문에 이 attribute를 추가해줘야 한다.
        // static을 이렇게 남발해도 되는가? 필요하긴 한데, 필요를 없애는게 맞진 않은가?
        // set도 Serialization을 위해 public으로 두었다. 이렇게 해도 되는가?
        [JsonProperty]
        public static string ProjectDirectory { get; set; }
        [JsonProperty]
        public static string ExportDirectory { get; set; }
        [JsonProperty]
        public static string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Resolutions { get; set; }
        public List<Asset> Assets { get; set; }
        //public CienScene CurrentScene { get; set; }
        public List<CienScene> Scenes { get; set; }

        public const string DefaultLayerName = "Default";
        public const string PressedLayerName = "pressed";
        public const string NomalLayerName = "normal";

        public CienDocument()
        {

        }

        public void Init(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
            Resolutions = new List<string>(1) { "orig" };
            Assets = new List<Asset>();

            CurrentScene = new CienScene();
            CurrentScene.InitScene();
            Scenes = new List<CienScene>();
            Scenes.Add(CurrentScene);
        }

        #region Asset: Get, Add, Remove
        public Asset GetAsest(string assetName)
        {
            return Assets.Find(x => x.GetName() == assetName);
        }

        public void AddAsset(Asset asset)
        {
            // TODO assetList에 같은 이름의 asset이 있는지 중복 검사를 해야 한다.
            if (Assets.Find(x => x.GetNameWithExt() == asset.GetNameWithExt()) != null)
            {
                MessageBox.Show(@"같은 이름의 Asset이 이미 프로젝트내에 존재합니다.");
                return;
            }

            Assets.Add(asset);
        }

        public void RemoveAsset(string name)
        {
            // 선택되었으면 버린다.
            State.SelectedComponentAbandon();

            // 관련된 Component부터 다 지운다.
            List<string> removableList = new List<string>();

            foreach (var scene in Scenes)
            {
                foreach (var component in scene.Components)
                {
                    if (component is CienImage)
                    {
                        CienImage image = (CienImage)component;
                        image.ImageData.Dispose();
                        if (image.ImageName.Split('.')[0] == name)
                            removableList.Add(image.Id);
                    }
                    else if (component is CienComposite)
                    {
                        CienComposite composite = (CienComposite)component;
                        // TODO Composite 안에 Composite이 있을 경우에도 Remove 할수있어야 함.
                        foreach (var image in composite.Composites.FindAll(x => x is CienImage))
                        {
                            var cienImage = (CienImage) image;
                            if (cienImage.ImageName == name)
                            {
                                removableList.Add(composite.Id);
                                break;
                            }
                        }
                    }
                }

                foreach (var removable in removableList)
                {
                    scene.Components.Remove(scene.Components.Find(x => x.Id.Equals(removable)));
                }
            }

            // Asset을 지운다.
            Assets.Remove(Assets.Find(x => x.GetName() == name));
        }
        #endregion

        #region Component: Add, Remove, Rename

        // TODO 무조건 이 함수를 통해서만 Component를 추가한다!
        public void AddComponent(CienBaseComponent component)
        {
            // TODO Zindex를 정리해줄 필요가 있다.
            component.SetZindex(GetNewZindex());

            CurrentScene.Components.Add(component);

            CienBaseComponent.Number++;
        }

        // Asset에서 만들어지는 Component는 무조건 이 함수를 통해서 만들어져야 한다.
        public void MakeNewImage(string assetName, Point location)
        {
            Asset asset = Assets.Find(x => x.GetName() == assetName);

            string id = "image" + CurrentScene.Components.Count;
            id = GetNewId(id);

            if (State.IsLayerSelected())
            {
                AddComponent(new CienImage(asset.GetNameWithExt(), id, location, CienBaseComponent.Number,
                    State.Selected.Layer.Name));
            }
            else
            {
                MessageBox.Show(@"선택된 layer가 없습니다!");
            }
        }

        public void MakeNewLabel(string text, int size, string style, List<float> tint)
        {
            string id = "label" + CurrentScene.Components.Count;
            id = GetNewId(id);

            if (State.IsLayerSelected())
            {
                AddComponent(new CienLabel(text, size, style, tint, id, CienBaseComponent.Number, State.Selected.Layer.Name));
            }
            else
            {
                MessageBox.Show(@"선택된 layer가 없습니다!");
            }
        }

        private string GetNewId(string id)
        {
            string newId = (string) id.Clone();

            for (int i = CurrentScene.Components.Count; CurrentScene.Components.Find(x => x.Id == id) != null; i++)
            {
                 newId = Regex.Replace(id, @"[\d-]", "") + (i);
            }

            return newId;
        }

        public void RemoveComponent(string id)
        {
            CurrentScene.Components.Remove(CurrentScene.Components.Find(x => x.Id == id));
        }

        public void RemoveComponent(CienBaseComponent component)
        {
            RemoveComponent(component.Id);
        }

        #endregion

        #region Label



        #endregion

        public int GetNewZindex()
        {
            if (CurrentScene.Components.Count == 0) return 1;
            return CurrentScene.Components.Max(x => x.ZIndex) + 1;
        }

        public void SortComponentsAscending()
        {
            CurrentScene.Components.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
        }
    }
}