using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using OneLevelJson.Annotations;

namespace OneLevelJson.Model
{
    abstract public class CienComponent
    {
        /*[JsonConstructor]
        protected Component(string id, Point position) 
            :this(id, position, Document.DefaultLayerName)
        {
        }

        protected Component(string id, Point position, string layerName)
        {
            Id = id;
            Position = position;
            ZIndex = Number++;
            LayerName = layerName;
        }*/

        public bool IsNull()
        {
            return Equals(Empty);
        }
        abstract public override string ToString();
        abstract public Size GetSize();
        abstract public Image GetImage();

        // public for Json Deserialization????? 잘모르겠다.
        //public Asset ParentAsset { get; set; }

        /* Variables ************************************************************/
        public static int Number = 1;

        public string Id { get; protected set; }
        public void SetId(string id)
        {
            Id = id; 
        }

        public Point Location { get; protected set; }
        private Point ConvertLocation(Point location)
        {
            Point translated = location - (Size) Blackboard.LeftTopPoint;
            int newX = translated.X;
            int newY = State.DocumentSize.Height - (translated.Y + GetSize().Height);
            return new Point(newX, newY);
            
        }
        public Point ConvertedLocation { get; protected set; }
        public void SetLocation(Point location)
        {
            Location = location;
            ConvertedLocation = ConvertLocation(Location);
        }

        public int ZIndex { get; protected set; }
        public void SetZindex(int zindex)
        {
            ZIndex = zindex; 
        }

        public List<int> Tint { get; protected set; }
        public void SetTint(List<int> tint)
        {
            Tint = tint;
        }

        public string LayerName { get; protected set; }
        public void SetLayerName(string layername)
        {
            LayerName = layername;
        }

        public static CienComponent Empty = new CienImage(EmptyId, EmptyId,Point.Empty, EmptyZindex);
        public const string EmptyImage = "EmptyImage";
        public const string EmptyId = "EmptyId";
        public const int EmptyZindex = -1;
        /************************************************************************/
    }
}
