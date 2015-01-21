using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace OneLevelJson.Model
{
    public class CienComposite : CienComponent
    {
        public CienComposite(string imageName, string id, Point position, string layerName = CienDocument.DefaultLayerName)
        {
            Id = id;
            Location = position;
            Tint = new List<int>(4) { 1, 1, 1, 1 };
            LayerName = layerName;

            composite = new Composite
            {
                Layers = new List<Composite.Layer>(3)
                {
                    new Composite.Layer(CienDocument.DefaultLayerName),
                    new Composite.Layer(CienDocument.PressedLayerName),
                    new Composite.Layer(CienDocument.NomalLayerName)
                },
                Images = new List<Composite.Image>(1)
                {
                    new Composite.Image
                    {
                        ImageName = imageName,
                        LayerName = CienDocument.DefaultLayerName,
                        Tint = new List<float>(4){1, 1, 1, 1}
                    }
                }
            };
        }

        public void AddImage(string imageName, Point location, string LayerName)
        {
            Composite.Image image = new Composite.Image()
            {
                LayerName = LayerName,
                ImageName =  imageName,
                Tint = new List<float>(4) { 1, 1, 1, 1 },
                X = location.X,
                Y = location.Y
            };
            composite.Images.Add(image);

            UpdateImage();
        }

        public override string ToString()
        {
            return LayerName + " Composite: " + Id;
        }

        public override Size GetSize()
        {
            Image image = GetImage();

            return image.Size;
        }

        private void UpdateImage()
        {
            var firstImage =
                Image.FromFile(CienDocument.ProjectDirectory + @"\" + CienDocument.Name + MainForm.AssetDirectory +
                               MainForm.ImageDirectory + @"\" +
                               composite.Images[0].ImageName + ".png");

            using (var e = Graphics.FromImage(firstImage))
            {
                for (int i = 1; i < composite.Images.Count; i++)
                {
                    var image =
                        Image.FromFile(CienDocument.ProjectDirectory + @"\" + CienDocument.Name +
                                       MainForm.AssetDirectory + MainForm.ImageDirectory + @"\" +
                                       composite.Images[i].ImageName + ".png");

                    //Point converted = ConvertLocation(composite.Images[i], image);
                    var compositeLocation = Location;
                    var imageLocation = new Point(composite.Images[i].X, composite.Images[i].Y);
                    Point converted = CoordinateConverter.CompositeToBoard(compositeLocation, imageLocation, image.Width,
                        image.Height);
                    e.DrawImage(image, converted);
                }
            }

            ImageData = (Image) firstImage.Clone();
        }

        public override Image GetImage()
        {
            if (ImageData == null)
                UpdateImage();

            return ImageData;
        }

        /* Variables ************************************************************/
        public Composite composite { get; private set; }
        [JsonIgnore]
        public Image ImageData { get; private set; }
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
                public Layer(string name)
                    : this(name, true, false)
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
                public int X;
                public int Y;
            }
        }
    }
}
