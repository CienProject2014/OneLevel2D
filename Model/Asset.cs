using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OneLevelJson.Model
{
    public enum AssetType
    {
        Image
    }

    public class Asset
    {
        [JsonConstructor]
        public Asset(AssetType type, string name)
        {
            Type = type;
            Name = name;
            Number = Number + 1;

            string projectDirectory = CienDocument.ProjectDirectory ?? Application.StartupPath;

            switch (Type)
            {
                case AssetType.Image:
                    Image img = Image.FromFile(projectDirectory + @"\" + CienDocument.Name
                        + MainForm.AssetDirectory + MainForm.ImageDirectory+@"\"+Name);
                    Data = img;
                    ImageSize = img.Size;
                    break;
            }
        }

        public string GetName()
        {
            return Name.Split('.')[0];
        }

        public string GetNameWithExt()
        {
            return Name;
        }

        public static int Number { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public AssetType Type { get; set; }
        private string Name { get; set; }    // 확장자 포함
        public Size ImageSize;

        [JsonIgnore]
        public readonly object Data;
    }
}
