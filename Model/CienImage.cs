﻿using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienImage : CienBaseComponent
    {
        /* Variables ************************************************************/
        public string ImageName { get; private set; } // with extension
        [JsonIgnore]
        public Image ImageData { get; private set; }
        /************************************************************************/

        public CienImage(string imageName, string id, Point location, int zIndex, string layerName = CienDocument.DefaultLayerName)
        {
            ImageName = imageName;
            Id = id;
            Location = location;
            Tint = new List<float>(4) { 1, 1, 1, 1 };
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

        public override object Clone()
        {
            return new CienImage(ImageName, Id, Location, ZIndex, LayerName)
            {
                ImageData = (Image) GetImage().Clone()
            };
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
                                  + CienDocument.Name + MainForm.ImageDirectory + @"\" + ImageName);
            return ImageData;
        }

    }
}