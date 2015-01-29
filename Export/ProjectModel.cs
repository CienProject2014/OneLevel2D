using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneLevel2D.Export
{
    public class ProjectModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ExportResolution> resolutions { get; set; }
        public List<ExportScene> scenes { get; set; }
        public ExportResolution originalResolution { get; set; }

        public class ExportScene
        {
            public List<float> ambientColor { get; set; }
            public ExportPhysics physicsPropertiesVO { get; set; }
            public string sceneName { get; set; }
        }

        public class ExportResolution
        {
            public int width { get; set; }
            public int height { get; set; }
            public string name { get; set; }
        }

        public class ExportPhysics
        {
        }
    }
}
