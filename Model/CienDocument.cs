using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace OneLevelJson.Model
{
    public class CienDocument
    {
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
            State.DocumentSize = new Size(width, height);
            Layers = new List<CienLayer>(1){new CienLayer(DefaultLayerName)};
        }

        #region Asset: Get, Add, Remove
        public Asset GetAsest(string assetName)
        {
            return Assets.Find(x => x.GetName() == assetName);
        }

        public void AddAsset(Asset asset)
        {
            // TODO assetList에 같은 이름의 asset이 있는지 중복 검사를 해야 한다.
            Assets.Add(asset);
        }

        public void RemoveAsset(Asset asset)
        {
            Assets.Remove(asset);
        }
        #endregion

        #region Component: Add, Remove, Rename
        public void AddComponent(string name, Point location)
        {
            Asset asset = Assets.Find(x => x.GetName() == name);
            string id = "image" + Components.Count;
            if(State.IsLayerSelected())
                Components.Add(new CienImage(asset.GetNameWithExt(), id, location, CienComponent.Number++, State.Selected.Layer.Name));
        }

        public void RemoveComponent(string id)
        {
            Components.Remove(Components.Find(x => x.Id == id));
        }

        public void RenameComponent(string id, string newId)
        {
            CienComponent component = Components.Find(x => x.Id == id);
            component.SetId(newId);
        }
        #endregion

        #region RenameLayer, ConvertToComposite
        public void RenameLayer(string name, string newName)
        {
            CienLayer layer = Layers.Find(x => x.Name == name);
            if(layer != null) layer.SetName(newName);
        }

        /*
         * Convert CienImage instance to CienComposite
         */
        public void ConvertToComposite(string id)
        {
            CienComponent comp = Components.Find(x => x.Id == id);
            if (comp is CienComposite) return;

            CienImage img = comp as CienImage;
            if (img == null) return;

            CienComposite newComp = new CienComposite(img.ImageName, img.Id, img.Location);
            Components.Add(newComp);
        }
        #endregion


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
        /*public List<CienImage> Images { get; private set; }
        public List<CienComposite> Composites { get; private set; }*/

        [JsonIgnore]
        public List<CienLayer> Layers;

        [JsonIgnore]
        public const string DefaultLayerName = "Default";
        [JsonIgnore]
        public const string PressedLayerName = "pressed";
        [JsonIgnore]
        public const string NomalLayerName = "normal";

    }
}