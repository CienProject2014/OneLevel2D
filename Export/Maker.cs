using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OneLevelJson.ExportModel;
using OneLevelJson.Model;

namespace OneLevelJson.Export
{
    public class Maker
    {
        public Maker()
        {
            project = new ProjectModel();
            scene = new SceneModel();
        }

        public void Extract(Document document)
        {
            ExtractProjectModel(document);

            ExtractSceneModel(document);
        }

        public void Make()
        {
            string projectString = JsonConvert.SerializeObject(project);
            File.WriteAllText(Document.ExportDirectory + @"\" + ProjectFileName, projectString);


        }

        public void ExtractProjectModel(Document document)
        {
            ProjectModel.Scene scene = new ProjectModel.Scene()
            {
                ambientColor = new List<float>()
                {
                    0.5f, 0.5f, 0.5f, 1
                },
                physicsPropertiesV0 = new ProjectModel.Physics(),
                sceneName = "MainScene"
            };

            project.scenes = new List<ProjectModel.Scene>();
            project.scenes.Add(scene);

            project.originalResolution = new ProjectModel.Resolution()
            {
                width = document.Width,
                height = document.Height,
                name = "orig"
            };
        }

        public void ExtractSceneModel(Document document)
        {
            scene.composite = new Composite1()
            {

            };

            scene.ambientColor = new List<float>()
            {
                
            };

            scene.physcisPropertiesV0 = new Physics();

            scene.sceneName = "MainScene";
        }

        private readonly ProjectModel project;
        private SceneModel scene;
        private const string ProjectFileName = "project.dt";
    }
}
