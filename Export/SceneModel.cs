using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneLevelJson.Export
{
    public class SceneModel
    {
        public Composite1 composite { get; set; }
        public List<float> ambientColor { get; set; }
        public Physics physcisPropertiesV0 { get; set; }
        public string sceneName { get; set; }
    }

    public class Composite1
    {
        public List<Layer> layers { get; set; }
        public List<sImage> sImages { get; set; }
        public List<sComposite> sComposites { get; set; }
    }
    public class Physics
    {
    }

    public class Layer
    {
        public string layerName { get; set; }
    }

    public class sImage
    {
        public string layerName { get; set; }
        public string itemIdentifier { get; set; }
        public string imageName { get; set; }
        public int zIndex { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public List<int> tint { get; set; }
    }

    public class sComposite
    {
        public string layerName { get; set; }
        public string itemIdentifier { get; set; }
        public int zIndex { get; set; }
        public Composite2 composite { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public List<int> tint { get; set; }
    }

    public class Composite2
    {
        public List<Layer> layers { get; set; }
        public List<sImage2> sImages { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<sLabel> sLabels { get; set; }
    }

    public class sImage2
    {
        public string layerName { get; set; }
        public string imageName { get; set; }
        public List<float> tint { get; set; }
    }

    public class sLabel
    {

    }
}