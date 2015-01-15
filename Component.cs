using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OneLevelJson
{
    public class Component
    {
        [JsonConstructorAttribute]
        public Component(Asset asset, string id, Point position)
        {
            ParentAsset = asset;
            Id = id;
            Position = position;

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

        // public for Json Deserialization????? 잘모르겠다.
        public Asset ParentAsset { get; set; }
        public string Id { get; private set; }
        public Point Position { get; private set; }
    }
}
