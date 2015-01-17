using System.Collections.Generic;
using System.Drawing;

namespace OneLevelJson.Model
{
    public class CienImage : Component
    {
        public CienImage(string imageName, string id, Point position, int zIndex)
            : this(imageName, id, position, zIndex, Document.DefaultLayerName)
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
            return Image.FromFile(Document.ProjectDirectory + @"\"
                                  + Document.Name + MainForm.ImageDataDirectory + @"\" + ImageName);
        }

        /* Variables ************************************************************/
        public string ImageName { get; private set; } // with extension
        /************************************************************************/
    }
}