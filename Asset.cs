using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OneLevelJson
{
    public enum AssetType
    {
        Image
    }

    public class Asset
    {
        [JsonConstructorAttribute]
        public Asset(AssetType type, string name)
        {
            Type = type;
            Name = name;
            Number = Number + 1;

            string projectDirectory = Document.SaveDirectory ?? Application.StartupPath;

            switch (Type)
            {
                case AssetType.Image:
                    Image img = Image.FromFile(projectDirectory + MainForm.ImageDataDirectory+@"\"+Name);
                    Data = img;
                    ImageSize = img.Size;
                    break;
            }
        }

        public static int Number { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public AssetType Type { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public readonly object Data;
        public Size ImageSize;
    }
}
