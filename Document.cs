using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OneLevelJson
{
    public class Document
    {
        public Document() : this("noname", 1920, 1080) {}
        public Document(string name, int width, int height)
        {
            Name = name;
            Assets = new List<Asset>();
            Components = new List<Component>();
            Width = width;
            Height = height;
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

        public void AddComponent(string name, Point location)
        {
            Asset asset = Assets.Find(x => x.Name == name);
            string id = "component" + Components.Count;
            Components.Add(new Component(asset, id, location));
        }

        public void RemoveComponent(string id)
        {
            Components.Remove(Components.Find(x => x.Id == id));
        }

        public void ReNameComponent(string id, string newId)
        {
            Component comp = Components.Find(x => x.Id == id);
            if (comp != null) comp.SetId(newId);
        }
        // TODO Save할때 static 변수도 나가는지 확인
        public static string Directory { get; set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public List<Asset> Assets { get; private set; }
        public List<Component> Components { get; private set; }
    }
}