using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using OneLevel2D.Model;

namespace OneLevel2D.Export
{
    public class Maker
    {
        private ProjectModel project;
        private List<SceneModel> sceneModels;
        private const string ProjectFileName = "project.dt";
        private const string SceneFileName = "MainScene.dt";

        public void Initiate()
        {
            project = new ProjectModel();
            sceneModels = new List<SceneModel>();
        }

        public void Extract(CienDocument document)
        {
            ExtractProjectModel(document);

            ExtractSceneModel(document);
        }

        public void Make()
        {
            string projectString = JsonConvert.SerializeObject(project, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(CienDocument.ExportDirectory + @"\" + ProjectFileName, projectString);

            foreach (var sceneModel in sceneModels)
            {
                string sceneString = JsonConvert.SerializeObject(sceneModel, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
                File.WriteAllText(CienDocument.ExportDirectory + @"\scenes\" + sceneModel.sceneName + ".dt", sceneString);
            }
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
            foreach (var cienScene in document.Scenes)
            {
                SceneModel scene = new SceneModel();
                // composite
                scene.composite = new ExportComposite1
                {
                    layers = new List<ExportLayer>(),
                    sImages = new List<ExportsImage>(),
                    sComposites = new List<ExportsComposite>()
                };

                // layers
                foreach (var layer in cienScene.Layers)
                {
                    scene.composite.layers.Add(new ExportLayer
                    {
                        layerName = layer.Name,
                        isVisible = layer.IsVisible,
                        isLocked = layer.IsLocked
                    });
                }

                foreach (var component in cienScene.Components)
                {
                    // sImages
                    if (component is CienImage)
                    {
                        CienImage cienImage = (CienImage)component;
                        Point convertedLocation = CoordinateConverter.BoardToGame(cienImage.Location, cienImage.GetSize());
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
                        CienComposite cienComposite = (CienComposite)component;
                        Point convertedLocation = CoordinateConverter.BoardToGame(cienComposite.Location, cienComposite.GetSize());
                        scene.composite.sComposites.Add(new ExportsComposite
                        {
                            layerName = cienComposite.LayerName,
                            itemIdentifier = cienComposite.Id,
                            composite = new ExportComposite2
                            {
                                layers = new List<ExportLayer>(cienComposite.Layers.Count),
                                sImages = new List<ExportsImage2>(cienComposite.Composites.Count)
                            },
                            zIndex = cienComposite.ZIndex,
                            x = convertedLocation.X,
                            y = convertedLocation.Y,
                            tint = cienComposite.Tint
                        });

                        foreach (var layer in cienComposite.Layers)
                        {
                            scene.composite.sComposites.Last().composite.layers.Add(new ExportLayer
                            {
                                layerName = layer.Name,
                                isVisible = layer.IsVisible,
                                isLocked = layer.IsLocked
                            });
                        }

                        foreach (var image in cienComposite.Composites.FindAll(x => x is CienImage))
                        {
                            CienImage cienImage = (CienImage) image;
                            scene.composite.sComposites.Last().composite.sImages.Add(new ExportsImage2
                            {
                                layerName = cienImage.LayerName,
                                imageName = cienImage.ImageName.Split('.')[0],
                                tint = cienImage.Tint,
                                x = cienImage.Location.X,
                                y = cienImage.Location.Y
                            });
                        }
                    }
                }

                scene.ambientColor = new List<float> { 0.5f, 0.5f, 0.5f, 1 };

                scene.physicsPropertiesVO = new ExportPhysics();

                scene.sceneName = cienScene.Name;
                sceneModels.Add(scene);
            }
        }

        private Point ConvertLocation(CienDocument document, CienBaseComponent component)
        {
            Point translated = component.Location - Blackboard.LeftTopOffset;
            int newX = translated.X;
            int newY = document.Height - (translated.Y + component.GetSize().Height);
            return new Point(newX, newY);
        }
    }
}

