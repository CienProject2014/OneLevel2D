using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    abstract public class CienBaseComponent : ICloneable
    {
        public static int Number = 1;
        /* Variables ************************************************************/
        // should be public for Serialization/Deserialization
        public string Id { get; set; }
        public Point Location { get; set; }
        public Point ConvertedLocation { get; set; }
        public int ZIndex { get; set; }
        public List<float> Tint { get; set; }
        public string LayerName { get; set; }
        /************************************************************************/
        public const string EmptyImage = "EmptyImage";
        public const string EmptyId = "EmptyId";
        public const int EmptyZindex = -1;
        public static CienBaseComponent Empty = new CienImage(EmptyId, EmptyId, Point.Empty, EmptyZindex);
        /************************************************************************/

        abstract public override string ToString();
        abstract public object Clone();
        abstract public Size GetSize();
        abstract public Image GetImage();

        public void SetId(string id)
        {
            Id = id;
        }

        public void SetLocation(Point location)
        {
            Location = location;
            ConvertedLocation = CoordinateConverter.ToOrigin(Location);
        }

        public void SetZindex(int zindex)
        {
            ZIndex = zindex;
        }

        public void SetTint(List<float> tint)
        {
            Tint = tint;
        }

        public void SetLayerName(string layername)
        {
            LayerName = layername;
        }
    }
}
