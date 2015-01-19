using System.Collections.Generic;
using System.Drawing;

namespace OneLevelJson.Model
{
    public class CienImage : CienComponent
    {
        public CienImage(string imageName, string id, Point position, int zIndex)
            : this(imageName, id, position, zIndex, CienDocument.DefaultLayerName)
        {
        }

        public CienImage(string imageName, string id, Point position, int zIndex, string layerName)
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
            using (Image image = GetImage())
            {
                return image.Size;
            }
        }

        public override Image GetImage()
        {
            return Image.FromFile(CienDocument.ProjectDirectory + @"\"
                                  + CienDocument.Name + MainForm.ImageDataDirectory + @"\" + ImageName);
        }

        /* Variables ************************************************************/
        public string ImageName { get; private set; } // with extension
        /************************************************************************/
    }
}