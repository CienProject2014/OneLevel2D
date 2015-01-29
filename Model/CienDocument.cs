using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Navigation;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienDocument
    {
        // static 변수는 자동으로 제외하기 때문에 이 attribute를 추가해줘야 한다.
        [JsonProperty]
        public static string ProjectDirectory { get; set; }

        [JsonProperty]
        public static string ExportDirectory { get; set; }

        [JsonProperty]  // static을 이렇게 남발해도 되는가? 필요하긴 한데, 필요를 없애는게 맞진 않은가?
        public static string Name { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public List<Asset> Assets { get; private set; }
        public List<CienComponent> Components { get; private set; }

        [JsonIgnore]
        public List<CienLayer> Layers;

        public List<string> Resolutions { get; private set; }

        [JsonIgnore]
        public const string DefaultLayerName = "Default";
        [JsonIgnore]
        public const string PressedLayerName = "pressed";
        [JsonIgnore]
        public const string NomalLayerName = "normal";

        public CienDocument() : this("noname", 1920, 1080) {}
        public CienDocument(string name, int width, int height)
        {
            Name = name;
            Assets = new List<Asset>();
            /*Images = new List<CienImage>();
            Composites = new List<CienComposite>();*/
            Components = new List<CienComponent>();
            Width = width;
            Height = height;
            Layers = new List<CienLayer>(1){new CienLayer(DefaultLayerName)};
            Resolutions = new List<string>(1){"orig"};
        }

        #region Asset: Get, Add, Remove
        public Asset GetAsest(string assetName)
        {
            return Assets.Find(x => x.GetName() == assetName);
        }

        public void AddAsset(Asset asset)
        {
            // TODO assetList에 같은 이름의 asset이 있는지 중복 검사를 해야 한다.
            Asset newasseAsset = Assets.Find(x => x.GetName() == asset.GetName());
            if (newasseAsset != null)
            {
                MessageBox.Show(@"같은 이름의 Asset이 이미 프로젝트내에 존재합니다.");
                return;
            }
            Assets.Add(asset);

        }

        public void RemoveAsset(string name)
        {
            // 관련된 Component부터 다 지운다.
            List<string> removableList = new List<string>();
            foreach (var component in Components)
            {
                if (component is CienImage)
                {
                    CienImage image = (CienImage) component;
                    if(image.ImageName.Split('.')[0] == name)
                        removableList.Add(image.Id);
                }
                else if (component is CienComposite)
                {
                    CienComposite composite = (CienComposite) component;
                    if (composite.composite.Images[0].ImageName.Split('.')[0] == name)
                        removableList.Add(composite.Id);
                }
            }

            foreach (var removable in removableList)
            {
                Components.Remove(Components.Find(x => x.Id.Equals(removable)));
            }

            Assets.Remove(Assets.Find(x => x.GetName() == name));
        }
        #endregion

        #region Component: Add, Remove, Rename

        // TODO 무조건 이 함수를 통해서만 Component를 추가한다!
        public void AddComponent(CienComponent component)
        {
            if (State.Document.Components.Find(x => x.Id == component.Id) != null)
            {
                MessageBox.Show(@"같은 Id의 Component가 존재합니다.");
                return;
            }

            // TODO Zindex를 정리해줄 필요가 있다.
            component.SetZindex(GetNewZindex());

            Components.Add(component);

            CienComponent.Number++;
        }

        // Asset에서 만들어지는 Component는 무조건 이 함수를 통해서 만들어져야 한다.
        public void NewComponent(string name, Point location)
        {
            Asset asset = Assets.Find(x => x.GetName() == name);
            string id = "image" + Components.Count;
            if (State.IsLayerSelected())
                AddComponent(new CienImage(asset.GetNameWithExt(), id, location, CienComponent.Number,
                    State.Selected.Layer.Name));
            else
                MessageBox.Show("선택된 layer가 없습니다!");
        }

        public void RemoveComponent(string id)
        {
            Components.Remove(Components.Find(x => x.Id == id));
        }

        #endregion

        #region ConvertToComposite
        /*
         * Convert CienImage instance to CienComposite
         */
        public void ConvertToComposite(string id)
        {
            CienComponent comp = Components.Find(x => x.Id == id);
            if (comp is CienComposite) return;

            CienImage img = comp as CienImage;
            if (img == null) return;

            CienComposite newComp = new CienComposite(img.ImageName, img.Id, img.Location, img.ZIndex);
            Components.Add(newComp);
        }
        #endregion

        public int GetNewZindex()
        {
            if (Components.Count == 0) return 1;
            return Components.Max(x => x.ZIndex) + 1;
        }

        public void SortComponentsAscending()
        {
            Components.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
        }
    }
}