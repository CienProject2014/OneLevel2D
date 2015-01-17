using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace OneLevelJson.Model
{
    abstract public class Component
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


        public void Move(Point offset)
        {
            Location = Location + (Size)offset;
        }

        public void Move(int dx, int dy)
        {
            Location = Location + new Size(dx, dy);
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public void MoveUp()
        {
            ZIndex++;
        }

        public void MoveDown()
        {
            ZIndex--;
        }

        abstract public override string ToString();
        abstract public Size GetSize();
        public abstract Image GetImage();

        public static int Number = 1;

        // public for Json Deserialization????? 잘모르겠다.
        //public Asset ParentAsset { get; set; }
        
        /* Variables ************************************************************/
        public string Id { get; protected set; }
        public Point Location { get; protected set; }
        public int ZIndex { get; protected set; }
        public List<int> Tint { get; protected set; }
        public string LayerName { get; protected set; }
        /************************************************************************/
    }
}
