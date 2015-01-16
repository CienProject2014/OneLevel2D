using System.Collections.Generic;
using System.Drawing;

namespace OneLevelJson.Model
{
    public class CienImage : Component
    {
        public CienImage(string imageName, string id, Point position)
            : this(imageName, id, position, Document.DefaultLayerName)
        {
        }

        public CienImage(string imageName, string id, Point position, string layerName)
        {
            ImageName = imageName;
            Id = id;
            Position = position;
            Tint = new List<int>(4) {1, 1, 1, 1};
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