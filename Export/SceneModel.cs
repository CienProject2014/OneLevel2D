using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneLevel2D.Export
{
    public class SceneModel
    {
        public ExportComposite1 composite { get; set; }
        public List<float> ambientColor { get; set; }
        public ExportPhysics physicsPropertiesVO { get; set; }
        public string sceneName { get; set; }
    }

    public class ExportComposite1
    {
        public List<ExportLayer> layers { get; set; }
        public List<ExportsImage> sImages { get; set; }
        public List<ExportsComposite> sComposites { get; set; }
        public List<ExportsLabel> sLabels { get; set; } 
    }
    public class ExportPhysics
    {
    }

    public class ExportLayer
    {
        public string layerName { get; set; }
        public bool isVisible { get; set; }
        public bool isLocked { get; set; }
    }

    public class ExportsImage
    {
        public string layerName { get; set; }
        public string itemIdentifier { get; set; }
        public string imageName { get; set; }
        public int zIndex { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public List<float> tint { get; set; }
    }

    public class ExportsComposite
    {
        public string layerName { get; set; }
        public string itemIdentifier { get; set; }
        public int zIndex { get; set; }
        public ExportComposite2 composite { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public List<float> tint { get; set; }
    }

    public class ExportComposite2
    {
        public List<ExportLayer> layers { get; set; }
        public List<ExportsImage2> sImages { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ExportsLabel> sLabels { get; set; }
    }

    public class ExportsImage2
    {
        public string layerName { get; set; }
        public string itemIdentifier { get; set; }
        public string imageName { get; set; }
        public List<float> tint { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int zIndex { get; set; }
    }

    public class ExportsLabel
    {
        public string layerName { get; set; }
        public string itemIdentifier { get; set; }
        public int size { get; set; }
        public string text { get; set; }
        public string style { get; set; }
        public int zIndex { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public List<float> tint { get; set; }
    }
}