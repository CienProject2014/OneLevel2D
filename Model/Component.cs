using System.Drawing;
using Newtonsoft.Json;

namespace OneLevelJson.Model
{
    public class Component
    {
        [JsonConstructor]
        public Component(Asset asset, string id, Point position)
        {
            ParentAsset = asset;
            Id = id;
            Position = position;
            LayerName = Document.DefaultLayerName;
        }

        public override string ToString()
        {
            return Id + ": " + ParentAsset.Name;
        }

        public void Move(Point offset)
        {
            Position = Position + (Size)offset;
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public Size GetSize()
        {
            return ParentAsset.ImageSize;
        }

        public void ConvertToButton()
        {
            isButton = true;
        }

        // public for Json Deserialization????? 잘모르겠다.
        public Asset ParentAsset { get; set; }
        public string Id { get; private set; }
        public Point Position { get; private set; }
        public string LayerName;

        [JsonIgnore] 
        private bool isButton = false;
    }
}
