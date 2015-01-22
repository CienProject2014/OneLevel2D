using System.Collections.Generic;
using System.Drawing;
using System.Windows.Navigation;

namespace OneLevelJson.Model
{
    public class CienImage : CienComponent
    {
        public CienImage(string imageName, string id, Point position, int zIndex, string layerName = CienDocument.DefaultLayerName)
        {
            ImageName = imageName;
            Id = id;
            Location = position;
            Tint = new List<int>(4) {1, 1, 1, 1};
            ZIndex = zIndex;
            LayerName = layerName;
        }

        public void SetImageName(string imagename)
        {
            ImageName = imagename;
        }

        public override string ToString()
        {
            return LayerName + " Image: " + Id + " " + ImageName;
        }

        public override Size GetSize()
        {
            Image image = GetImage();

            return image.Size;
        }

        public override Image GetImage()
        {
            if (ImageData == null)
                ImageData = Image.FromFile(CienDocument.ProjectDirectory + @"\"
                                  + CienDocument.Name + MainForm.AssetDirectory + MainForm.ImageDirectory + @"\" + ImageName);
            return ImageData;
        }

        /* Variables ************************************************************/
        public string ImageName { get; private set; } // with extension
        public Image ImageData { get; private set; }
        /************************************************************************/
    }
}