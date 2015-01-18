using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OneLevelJson.Model;

namespace OneLevelJson.Export
{
    public class Maker
    {
        public void Initiate()
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

            string sceneString = JsonConvert.SerializeObject(scene);
            File.WriteAllText(Document.ExportDirectory + @"\scenes\" + SceneFileName, sceneString);
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
            // composite
            scene.composite = new Composite1()
            {
                layers = new List<Layer>(),
                sImages = new List<sImage>(),
                sComposites = new List<sComposite>()
            };

            // layers
            foreach (var layer in document.Layers)
            {
                scene.composite.layers.Add(new Layer()
                {
                    layerName = layer.Name
                });
            }

            foreach (var component in document.Components)
            {
                // sImages
                if (component is CienImage)
                {
                    CienImage cienImage = (CienImage) component;
                    Point convertedLocation = ConvertLoation(document, cienImage);
                    scene.composite.sImages.Add(new sImage()
                    {
                        layerName = cienImage.LayerName,
                        itemIdentifier = cienImage.Id,
                        imageName = cienImage.ImageName.Split('.')[0],
                        zIndex = cienImage.ZIndex,
                        x = convertedLocation.X,
                        y = convertedLocation.Y,
                        /*x = cienImage.Location.X,
                        y = cienImage.Location.Y,*/
                        tint = cienImage.Tint
                    });
                     
                }
                // sComposites
                else if (component is CienComposite)
                {
                    CienComposite cienComposite = (CienComposite) component;
                    scene.composite.sComposites.Add(new sComposite()
                    {
                        layerName = cienComposite.LayerName,
                        itemIdentifier = cienComposite.Id,
                        composite = new Composite2()
                        {
                            layers = new List<Layer>(cienComposite.composite.Layers.Count),
                            sImages = new List<sImage2>(cienComposite.composite.Images.Count),
                        },
                        zIndex = cienComposite.ZIndex,
                        x = cienComposite.Location.X,
                        y = cienComposite.Location.Y,
                        tint = cienComposite.Tint
                    });

                    foreach (var layer in cienComposite.composite.Layers)
                    {
                        scene.composite.sComposites.Last().composite.layers.Add(new Layer()
                        {
                            layerName =  layer.Name
                        });
                    }

                    foreach (var image in cienComposite.composite.Images)
                    {
                        scene.composite.sComposites.Last().composite.sImages.Add(new sImage2()
                        {
                            layerName = image.LayerName,
                            imageName = image.ImageName.Split('.')[0],
                            tint = image.Tint
                        });
                    }
                }
            }

            scene.ambientColor = new List<float>()
            {
                0.5f, 0.5f, 0.5f, 1
            };

            scene.physcisPropertiesV0 = new Physics();

            scene.sceneName = "MainScene";
        }

        private Point ConvertLoation(Document document, Component component)
        {
            Point translated = component.Location - (Size) Blackboard.LeftTopPoint;
            int newX = translated.X;
            int newY = document.Height - (translated.Y + component.GetSize().Height);
            return new Point(newX, newY);
        }

        private ProjectModel project;
        private SceneModel scene;
        private const string ProjectFileName = "project.dt";
        private const string SceneFileName = "MainScene.dt";
    }
}
