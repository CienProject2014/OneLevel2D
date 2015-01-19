using System.Collections.Generic;
using System.Drawing;

namespace OneLevelJson.Model
{
    public class CienComposite : CienComponent
    {
        public CienComposite(string imageName, string id, Point position)
            : this(imageName, id, position, CienDocument.DefaultLayerName)
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
                    new Composite.Layer(CienDocument.DefaultLayerName),
                    new Composite.Layer(CienDocument.PressedLayerName),
                    new Composite.Layer(CienDocument.NomalLayerName)
                },
                Images = new List<Composite.Image>(1)
                {
                    new Composite.Image()
                    {
                        ImageName = imageName,
                        LayerName = CienDocument.DefaultLayerName,
                        Tint = new List<float>(4){1, 1, 1, 1}
                    }
                }
            };
        }

        public void SetComposite(Composite composite)
        {
            this.composite = composite;
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
            return Image.FromFile(CienDocument.ProjectDirectory + MainForm.ImageDataDirectory + @"\" + composite.Images[0].ImageName);
        }

        /* Variables ************************************************************/
        public Composite composite { get; private set; }
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
