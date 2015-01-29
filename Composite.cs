using System.Collections.Generic;
using System.Drawing;

namespace OneLevel2D
{
    class Composite
    {

        public string LayerName { get; private set; }
        public string Id { get; private set; }
        public Point Position { get; private set; }
        public int ZIndex { get; private set; }
        public List<float> tint { get; private set; }
        //List<Component> 
    }
}
