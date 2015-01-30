using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    abstract public class CienComponent : ICloneable
    {
        abstract public override string ToString();
        abstract public object Clone();
        abstract public Size GetSize();
        abstract public Image GetImage();

        public static CienComponent Empty = new CienImage(EmptyId, EmptyId, Point.Empty, EmptyZindex);
        public const string EmptyImage = "EmptyImage";
        public const string EmptyId = "EmptyId";
        public const int EmptyZindex = -1;

        // public for Json Deserialization????? 잘모르겠다.
        //public Asset ParentAsset { get; set; }

        /* Variables ************************************************************/
        public static int Number = 1;

        public string Id { get; set; }
        public void SetId(string id)
        {
            Id = id; 
        }

        public Point Location { get; set; }
        public Point ConvertedLocation { get; set; }
        public void SetLocation(Point location)
        {
            Location = location;
            ConvertedLocation = CoordinateConverter.ToOrigin(Location);
        }

        public int ZIndex { get; set; }
        public void SetZindex(int zindex)
        {
            ZIndex = zindex; 
        }

        public List<int> Tint { get; set; }
        public void SetTint(List<int> tint)
        {
            Tint = tint;
        }

        public string LayerName { get; set; }
        public void SetLayerName(string layername)
        {
            LayerName = layername;
        }

        /************************************************************************/
    }
}
