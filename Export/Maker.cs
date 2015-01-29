using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OneLevel2D.Model;

namespace OneLevel2D.Export
{
    public class Maker
    {
        public void Initiate()
        {
            project = new ProjectModel();
            scene = new SceneModel();
        }

        public void Extract(CienDocument document)
        {
            ExtractProjectModel(document);

            ExtractSceneModel(document);
        }

        public void Make()
        {
            string projectString = JsonConvert.SerializeObject(project);
            File.WriteAllText(CienDocument.ExportDirectory + @"\" + ProjectFileName, projectString);

            string sceneString = JsonConvert.SerializeObject(scene);
            File.WriteAllText(CienDocument.ExportDirectory + @"\scenes\" + SceneFileName, sceneString);
        }

        public void ExtractProjectModel(CienDocument document)
        {
            ProjectModel.ExportScene scene = new ProjectModel.ExportScene
            {
                ambientColor = new List<float>
                {
                    0.5f, 0.5f, 0.5f, 1
                },
                physicsPropertiesVO = new ProjectModel.ExportPhysics(),
                sceneName = "MainScene"
            };

            project.scenes = new List<ProjectModel.ExportScene>();
            project.scenes.Add(scene);

            project.originalResolution = new ProjectModel.ExportResolution
            {
                width = document.Width,
                height = document.Height,
                name = "orig"
            };
        }

        public void ExtractSceneModel(CienDocument document)
        {
            // composite
            scene.composite = new ExportComposite1
            {
                layers = new List<ExportLayer>(),
                sImages = new List<ExportsImage>(),
                sComposites = new List<ExportsComposite>()
            };

            // layers
            foreach (var layer in document.Layers)
            {
                scene.composite.layers.Add(new ExportLayer
                {
                    layerName = layer.Name,
                    isVisible = layer.IsVisible,
                    isLocked = layer.IsLocked
                });
            }

            foreach (var component in document.Components)
            {
                // sImages
                if (component is CienImage)
                {
                    CienImage cienImage = (CienImage) component;
                    Point convertedLocation = CoordinateConverter.BoardToGame(cienImage.Location, cienImage.GetSize().Width, cienImage.GetSize().Height);
                    //Point convertedLocation = ConvertLocation(document, cienImage);
                    scene.composite.sImages.Add(new ExportsImage
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
                    scene.composite.sComposites.Add(new ExportsComposite
                    {
                        layerName = cienComposite.LayerName,
                        itemIdentifier = cienComposite.Id,
                        composite = new ExportComposite2
                        {
                            layers = new List<ExportLayer>(cienComposite.composite.Layers.Count),
                            sImages = new List<ExportsImage2>(cienComposite.composite.Images.Count)
                        },
                        zIndex = cienComposite.ZIndex,
                        x = cienComposite.Location.X,
                        y = cienComposite.Location.Y,
                        tint = cienComposite.Tint
                    });

                    foreach (var layer in cienComposite.composite.Layers)
                    {
                        scene.composite.sComposites.Last().composite.layers.Add(new ExportLayer
                        {
                            layerName =  layer.Name,
                            isVisible = layer.IsVisible,
                            isLocked = layer.IsLocked
                        });
                    }

                    foreach (var image in cienComposite.composite.Images)
                    {
                        scene.composite.sComposites.Last().composite.sImages.Add(new ExportsImage2
                        {
                            layerName = image.LayerName,
                            imageName = image.ImageName.Split('.')[0],
                            tint = image.Tint,
                            x = image.X,
                            y = image.Y
                        });
                    }
                }
            }

            scene.ambientColor = new List<float>
            {
                0.5f, 0.5f, 0.5f, 1
            };

            scene.physicsPropertiesVO = new ExportPhysics();

            scene.sceneName = "MainScene";
        }

        private Point ConvertLocation(CienDocument document, CienComponent component)
        {
            Point translated = component.Location - Blackboard.LeftTopOffset;
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

