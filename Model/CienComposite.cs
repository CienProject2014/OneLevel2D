using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienComposite : CienComponent
    {
        /* Variables ************************************************************/
        public Composite composite { get; private set; }
        [JsonIgnore]
        public Image ImageData { get; private set; }
        /************************************************************************/

        public CienComposite(string imageName, string id, Point position, 
            int zIndex, string layerName = CienDocument.DefaultLayerName)
        {
            Id = id;
            Location = position;
            Tint = new List<int>(4) { 1, 1, 1, 1 };
            ZIndex = zIndex;
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
                        ImageName = imageName.Split('.')[0],
                        LayerName = CienDocument.DefaultLayerName,
                        Tint = new List<float>(4){1, 1, 1, 1}
                    }
                }
            };
        }

        public void AddImage(CienImage image)
        {
            AddImage(image.ImageName, image.LayerName);
        }

        public void AddImage(string imageName, string layerName)
        {
            AddImage(imageName, new Point(), layerName);
        }

        // for load
        public void AddImage(string imageName, Point location, string layerName)
        {
            Composite.Image image = new Composite.Image()
            {
                LayerName = layerName,
                ImageName = imageName,
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

        public override object Clone()
        {
            MessageBox.Show(@"CienImage CLone");
            return new CienComposite(composite.Images[0].ImageName, Id, Location, ZIndex, LayerName)
            {
                composite = (Composite) this.composite.Clone(),
                ImageData = (Image) GetImage().Clone()
            };
        }

        public override Image GetImage()
        {
            if (ImageData == null)
                UpdateImage();

            return ImageData;
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
                               composite.Images[0].ImageName);  // TODO ImageName에 확장자가 포함되어 있다.

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
                    /*Point converted = CoordinateConverter.CompositeToBoard(compositeLocation, imageLocation, image.Width,
                        image.Height);
                    e.DrawImage(image, converted);*/
                    e.DrawImage(image, imageLocation);
                }
            }

            ImageData = (Image) firstImage.Clone();
        }

        /************************************************************************/
        /* Internal Model														*/
        /************************************************************************/
        public class Composite : ICloneable
        {
            public List<Layer> Layers { get; set; }
            public List<Image> Images { get; set; }

            public class Layer : ICloneable
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

                // Layer
                public object Clone()
                {
                    return new Layer(Name, IsVisible, IsLocked);
                }
            }

            public class Image : ICloneable
            {
                public string LayerName { get; set; }
                public string ImageName { get; set; } // with extension
                public List<float> Tint { get; set; }
                public int X;
                public int Y;

                // Image (Custom)
                public object Clone()
                {
                    return new Image()
                    {
                        ImageName = this.ImageName,
                        LayerName = this.LayerName,
                        Tint = new List<float>() {1, 1, 1, 1},
                        X = this.X,
                        Y = this.Y
                    };
                }
            }

            // Composite
            public object Clone()
            {
                List<Image> newImages = new List<Image>(Images.Count);
                newImages.AddRange((IEnumerable<Image>) Images.Select(image => image.Clone()));

                List<Layer> newLayers = new List<Layer>(Layers.Count);
                newLayers.AddRange((IEnumerable<Layer>) Layers.Select(layer => layer.Clone()));

                return new Composite()
                {
                    Images = newImages,
                    Layers = newLayers
                };
            }
        }
    }
}
