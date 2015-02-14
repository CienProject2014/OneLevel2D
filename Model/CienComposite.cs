using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using MessageBox = System.Windows.Forms.MessageBox;

namespace OneLevel2D.Model
{
    public class CienComposite : CienBaseComponent
    {
        /* Variables ************************************************************/
        //public Composite composite { get; private set; }
        /*public List<CienImage> Images { get; set; }
        public List<CienLabel> Labels { get; set; }*/
        public List<CienBaseComponent> Composites { get; set; } 
        public List<CienLayer> Layers { get; set; }
            [JsonIgnore]
        public Image ImageData { get; private set; }
        /************************************************************************/

        public CienComposite(string id, Point position, 
            int zIndex, string layerName)
        {
            Id = id;
            Location = position;
            Tint = new List<float>(4) { 1, 1, 1, 1 };
            ZIndex = zIndex;
            LayerName = layerName;

            Layers = new List<CienLayer>(1)
            {
                new CienLayer(CienDocument.DefaultLayerName, true, false)
            };

            /*Images = new List<CienImage>();
            Labels = new List<CienLabel>();*/
            Composites = new List<CienBaseComponent>();

            /*composite = new Composite
            {
                Layers = new List<Composite.Layer>(1)
                {
                    new Composite.Layer(CienDocument.DefaultLayerName),
                },
                Images = new List<Composite.Image>(),
                Labels = new List<Composite.Label>()
            };*/
        }

        public void AddComponent(CienBaseComponent component)
        {
            if (Layers.Count == 0)
            {
                MessageBox.Show(@"Composite의 layer가 비어있습니다.");
                return;
            }

            component.SetLocation(new Point(0, 0));
            Composites.Add(component);

            UpdateImage();
        }

        public void AddImage(string imageName, string id)
        {
            /*Composite.Image newImage = new Composite.Image
            {
                ImageName = imageName,
                Id = id,
                LayerName = layerName,
                Tint = new List<float>(tint),
                X = Location.X,
                Y = Location.Y,
                Zindex = composite.Images.Count
            };
            composite.Images.Add(newImage);*/
            AddComponent(new CienImage(imageName, id, new Point(0, 0), Composites.Count, Layers[0].Name));
        }

        public void AddLabel(string text, string style, int size, List<float> tint, string id, string layerName)
        {
            /*Composite.Label newLabel = new Composite.Label
            {
                Text = text,
                Style = style,
                Size = size,
                Tint = new List<float>(tint),
                Id = id,
                LayerName = LayerName,
                Zindex = composite.Labels.Count
            };
            composite.Labels.Add(newLabel);*/
            AddComponent(new CienLabel(text, size, style, tint, id, Composites.Count, layerName));
        }

        public override string ToString()
        {
            return LayerName + " Composite: " + Id;
        }

        public override object Clone()
        {
            MessageBox.Show(@"CienImage CLone");
            return new CienComposite(Id, Location, ZIndex, LayerName)
            {
                //composite = (Composite) this.composite.Clone(),
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
            if (Composites.Count == 0)
            {
                MessageBox.Show(@"Composite에 그릴 Component가 없습니다.");
                return;
            }

            // sort ascending
            Composites.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));

            Image firstImage = Composites[0].GetImage();

            /*if (composite.Images.Count != 0)
            {
                firstImage =
                    Image.FromFile(CienDocument.ProjectDirectory + @"\" + CienDocument.Name + MainForm.ImageDirectory +
                                   @"\" +
                                   composite.Images[0].ImageName); // TODO ImageName에 확장자가 포함되어 있다.
            }
            else if (composite.Labels.Count != 0)
            {
                var firstLabel = composite.Labels[0];
//                firstImage = new Bitmap(firstLabel.);
            }
            else
            {
                return;
            }

            if (firstImage == null) return;
            */
            using (var e = Graphics.FromImage(firstImage))
            {
                foreach (var composite in Composites)
                {
                    e.DrawImage(composite.GetImage(), composite.Location);
                }
                /*
                for (int i = 1; i < composite.Images.Count; i++)
                {
                    var image =
                        Image.FromFile(CienDocument.ProjectDirectory + @"\" + CienDocument.Name +
                                       MainForm.ImageDirectory + @"\" +
                                       composite.Images[i].ImageName + ".png");

                    //Point converted = ConvertLocation(composite.Images[i], image);
                    var compositeLocation = Location;
                    var imageLocation = new Point(composite.Images[i].X, composite.Images[i].Y);
                    /*Point converted = CoordinateConverter.CompositeToBoard(compositeLocation, imageLocation, image.Width,
                        image.Height);
                    e.DrawImage(image, converted);#1#
                    e.DrawImage(image, imageLocation);
                }
                 */
            }

            ImageData = (Image) firstImage.Clone();
        }

        /************************************************************************/
        /* Internal Model														*/
        /************************************************************************/
        /*public class Composite : ICloneable
        {
            public List<Layer> Layers { get; set; }
            public List<Image> Images { get; set; }
            public List<Label> Labels { get; set; } 

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
                public string Id { get; set; }
                public string ImageName { get; set; } // with extension
                public List<float> Tint { get; set; }
                public int X;
                public int Y;
                public int Zindex;

                // Image (Custom)
                public object Clone()
                {
                    return new Image
                    {
                        ImageName = this.ImageName,
                        Id = this.Id,
                        LayerName = this.LayerName,
                        Tint = new List<float>(this.Tint),
                        X = this.X,
                        Y = this.Y,
                        Zindex = this.Zindex
                    };
                }
            }

            public class Label : ICloneable
            {
                public string LayerName { get; set; }
                public string Id { get; set; }
                public string Text { get; set; }
                public string Style { get; set; }
                public int Size { get; set; }
                public List<float> Tint { get; set; }
                public int X;
                public int Y;
                public int Zindex;

                public object Clone()
                {
                    return new Label
                    {
                        LayerName = this.LayerName,
                        Id = this.Id,
                        Size = this.Size,
                        Style = this.Style,
                        Text = this.Text,
                        Tint=new List<float>(this.Tint),
                        X = this.X,
                        Y = this.Y,
                        Zindex = this.Zindex
                    }
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
        }*/
    }
}
