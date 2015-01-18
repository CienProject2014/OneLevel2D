using System.Collections.Generic;
using System.Drawing;

namespace OneLevelJson.Model
{
    public class CienComposite : Component
    {
        public CienComposite(string imageName, string id, Point position)
            : this(imageName, id, position, Document.DefaultLayerName)
        {
            
        }

        public CienComposite(string imageName, string id, Point position, string layerName)
        {
            Id = id;
            Location = position;
            Tint = new List<int>(4) { 1, 1, 1, 1 };
            LayerName = layerName;

            composite = new Composite()
            {
                Layers = new List<Composite.Layer>(3)
                {
                    new Composite.Layer(Document.DefaultLayerName),
                    new Composite.Layer(Document.PressedLayerName),
                    new Composite.Layer(Document.NomalLayerName)
                },
                Images = new List<Composite.Image>(1)
                {
                    new Composite.Image()
                    {
                        ImageName = imageName,
                        LayerName = Document.DefaultLayerName,
                        Tint = new List<float>(4){1, 1, 1, 1}
                    }
                }
            };
        }
        public override string ToString()
        {
            return LayerName + " Composite: " + Id;
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
            return Image.FromFile(Document.ProjectDirectory + MainForm.ImageDataDirectory + @"\" + composite.Images[0].ImageName);
        }

        /* Variables ************************************************************/
        public Composite composite { get; set; }
        /************************************************************************/

        /************************************************************************/
        /* Internal Model														*/
        /************************************************************************/
        public class Composite
        {
            public List<Layer> Layers { get; set; }
            public List<Image> Images { get; set; }

            public class Layer
            {
                public Layer(string name) : this(name, true, false)
                {
                    Name = name;
                }

                public Layer(string name, bool isVisible, bool isLocked)
                {
                    Name = name;
                    IsVisible = isVisible;
                    IsLocked = isLocked;
                }

                public string Name { get; set; }
                public bool IsVisible { get; set; }
                public bool IsLocked { get; set; }
            }

            public class Image
            {
                public string LayerName { get; set; }
                public string ImageName { get; set; } // with extension
                public List<float> Tint { get; set; }
            }
        }
    }
}
