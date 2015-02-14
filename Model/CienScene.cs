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
        public static int Number = 1;
        [JsonProperty]
        public string Name { get; set; }
        public List<CienBaseComponent> Components { get; set; }
        public List<CienLayer> Layers { get; set; }

        public CienScene()
        {
            // If we put some code here, then the Json Serializer make object twice
        }

        public void InitScene(Blackboard board)
        {
            Name = "Scene" + Number++;
            Components = new List<CienBaseComponent>();
            Layers = new List<CienLayer>(1) { new CienLayer(CienDocument.DefaultLayerName) };
            board.SetScene(this);
        }
    }
}
