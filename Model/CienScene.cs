using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienScene
    {
        [JsonProperty]
        public static string Name { get; set; }
        public List<CienComponent> Components { get; set; }
        public List<CienLayer> Layers { get; set; }

        public CienScene() { }

        public void InitScene()
        {
            Name = "Scene";
            Components = new List<CienComponent>();
            Layers = new List<CienLayer>(1) { new CienLayer(CienDocument.DefaultLayerName) };
        }
    }
}
