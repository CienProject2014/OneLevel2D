using System.Collections.Generic;

namespace OneLevelJson
{
    public class ProjectModel
    {
        public List<Resolution> resolutions { get; set; }
        public List<Scene> scenes { get; set; }
        public Resolution originalResolution { get; set; }

        public class Scene
        {
            public List<float> ambientColor { get; set; }
            public Physics physicsPropertiesV0 { get; set; }
            public string sceneName { get; set; }
        }

        public class Resolution
        {
            public int width { get; set; }
            public int height { get; set; }
            public string name { get; set; }
        }

        public class Physics
        {
        }
    }
}
