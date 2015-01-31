using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;
using OneLevel2D.Export;
using OneLevel2D.Model;
using OneLevel2D.TexturePacker;
using Layer = OneLevel2D.Model.CienLayer;

namespace OneLevel2D
{
    public partial class MainForm : Form
    {
        public const string ProgramName = "OneLevel2D";

        public const string AssetDirectory = @"\assets";
        public const string ImageDirectory = @"\image";

        public const string Overlap2DExtention = "pit";
        public const string Overlap2DImageDataDirectory = @"\assets\orig\images";
        public const string Overlap2DSceneDirectory = @"\scenes";

        public const string ProjectExtension = "cien";
        public const string SceneExtension = "dt";

        private readonly Packer TexturePacker = new Packer();
        private readonly Maker ModelMaker = new Maker();

        public MainForm()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            // TODO 접근하기 위해 설정. 
            State.SetBoard(blackboard);
            State.SetComponentListView(componentList);

            NewDocument("noname", 1920, 1080);


            // TODO 임시로 처리 해둔것. Project save to other directory를 구현하면 수정해야함.
            CienDocument.ProjectDirectory = Application.StartupPath;

            AddEvent();
        }

        private void AddEvent()
        {
            assetList.ItemDrag += assetList_ItemDrag;
            assetList.DragEnter += assetList_DragEnter;
            assetList.MouseDown += assetList_MouseDown;

            // TODO LayerList 수정

            blackboard.DragEnter += blackboard_DragEnter;
            blackboard.DragDrop += blackboard_DragDrop;
            blackboard.KeyDown += blackboard_KeyDown;
        }

        #region Command

        private void CutComponent()
        {
            if (!State.IsComponentSelected()) return;

            State.Copy();
            State.RemoveSelectedComponent();

            State.SelectedComponentAbandon();

            State.Board.Invalidate();

            ReloadComponentList();  // Troll, 나중에 확인.
        }

        private void CopyComponent()
        {
            if (!State.IsComponentSelected()) return;

            State.Copy();
        }

        private void PasteComponent()
        {
            if (State.CopiedComponentList == null) return;

            // TODO 기존의 zIndex 들을 검사해서 비어있으면 숫자들을 당겨줘야 한다.
            State.Paste();
        }

        private void UnDo()
        {
            if (State.Commands.Count == 0) return;

            State.ReverseLastCommand();

            State.Board.Invalidate();
        }

        private void ReDo()
        {
            if (State.RevertedCommands.Count == 0) return;

            State.ReverseLastReverse();

            State.Board.Invalidate();
        }

        #endregion

        #region New, Load, Save, Import

        private void NewDocument(string name, int width, int height)
        {
            State.Document = new CienDocument();
            State.Document.Init(name, width, height);

            State.Board.SetDocument(State.Document);
            State.Board.Invalidate();
            InitDocument();
        }

        private void LoadDocument(string dir)
        {
            string docstring = File.ReadAllText(dir);

            ParseDocument(docstring);
            InitDocument();
        }

        private void InitDocument()
        {
            titleBarControl1.SetTitleName(CienDocument.Name + " - " + ProgramName);

            ReloadAssetList();
            ReloadComponentList();
            ReloadLayerList();

            State.Board.Invalidate();

            CienComponent.Number = 0;

            // TODO 분리해주어야 좋을 Directory 설정. 이 부분을 어디서 사용할지 모르니까 쉽사리 분리를 하지 못하겠다.
            string projectPath = CienDocument.ProjectDirectory ?? Application.StartupPath;
            MakeDirectory(projectPath + @"\" + CienDocument.Name);
            MakeDirectory(projectPath + @"\" + CienDocument.Name + @"\" + AssetDirectory);
            MakeDirectory(projectPath + @"\" + CienDocument.Name + @"\" + AssetDirectory + ImageDirectory);
        }

        private void SaveDocument(string filename)
        {
            string docjson = JsonConvert.SerializeObject(State.Document, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            File.WriteAllText(CienDocument.ProjectDirectory + @"\" + filename, docjson);
        }

        private void ParseDocument(string docstring)
        {
            var newDoc = JsonConvert.DeserializeObject<CienDocument>(docstring, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            State.Document = newDoc;
        }

        private void ImportAsset(string[] files)
        {
            // 1. 이미지들을 실행파일이 있는 프로젝트로 복사한 후,
            foreach (var file in files)
            {
                string projectDirectory = CienDocument.ProjectDirectory ?? Application.StartupPath;

                try
                {
                    File.Copy(file, projectDirectory + @"\" + CienDocument.Name + @"\"
                                    + AssetDirectory + ImageDirectory + @"\" + file.Split('\\').Last());

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Asset newAsset = MakeAssetFrom(file);
                State.Document.AddAsset(newAsset);
            }

            // 3. 현재 AssetList를 다시 로드한다.
            ReloadAssetList();
        }

        private Asset MakeAssetFrom(string file)
        {
            AssetType type;

            string extension = file.Split('.')[0];
            string name = file.Split('\\').Last();
            switch (extension)
            {
                case "png":
                case "PNG":
                case "jpg":
                case "JPG":
                    type = AssetType.Image;
                    break;
                default:
                    type = AssetType.Image;
                    break;
            }
            return new Asset(type, name);
        }

        private void Export(string exportDir)
        {
            string imagePackDir = exportDir + @"\orig";
            MakeDirectory(imagePackDir);

            // 1. TexturePacker로 기본적인 이미지를 만든다.
            TexturePacker.LoadAssets(State.Document.Assets);
            TexturePacker.RunPacking();
            TexturePacker.MakePackImage(imagePackDir);
            TexturePacker.MakeAtlas(imagePackDir);

            // 2. project.dt, scene.dt를 만든다.
            ModelMaker.Initiate();
            ModelMaker.Extract(State.Document);
            ModelMaker.Make();
        }

        #endregion

        #region AddComponent, MakeImageFrom, MakeDirectory, CheckExt, GetClickedToolPostion, Sort

        private void AddNewComponent(ListView.SelectedListViewItemCollection items, Point location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string name = items[i].Text;
                Size offset = new Size(15 * i, 15 * i);
                Point transformedLocation = State.Board.PointTransform(location);
                State.Document.NewComponent(name, transformedLocation + offset);
                // TODO 중복되는 경우를 해결할 필요가 있다.
                State.ComponentView.AddComponent(State.Document.CurrentScene.Components.Last());
            }

            State.Document.SortComponentsAscending();

            ReloadComponentList();
        }

        private Image MakeImageFrom(string imageName)
        {
            string projectDirectory = CienDocument.ProjectDirectory ?? Application.StartupPath;
            string newImagename = imageName;

            if (!CheckExt(imageName))
            {
                Asset asset = State.Document.Assets.Find(x => x.GetName() == imageName);
                newImagename = asset.GetNameWithExt();
            }

            return Image.FromFile(projectDirectory + @"\" + CienDocument.Name + @"\"
                                  + AssetDirectory + ImageDirectory + @"\" + newImagename);
        }

        public void MakeDirectory(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists) Directory.CreateDirectory(dir);
        }

        public bool CheckExt(string name)
        {
            return name.Split('.').Length > 1;
        }

        private Point GetClickedToolPosition(Control tool, Point clickedPosition)
        {
            // clickedPostion : 부모 container의 좌상단을 기준으로 커서의 위치
            // 
            Point location = new Point();
            if (toolContainer1.Contains(tool))
            {
                if (toolContainer1.Panel1.Contains(tool))
                {
                    location = splitContainer.Panel1.Location
                               + (Size)toolContainer1.Panel1.Location;
                }
                else if (toolContainer1.Panel2.Contains(tool))
                {
                    location = splitContainer.Panel1.Location
                               + (Size)toolContainer1.Panel2.Location;
                }
            }
            else if (toolContainer2.Contains(tool))
            {
                if (toolContainer2.Panel1.Contains(tool))
                {
                    location = splitContainer.Panel2.Location
                               + (Size)toolContainer2.Panel1.Location;
                }
                else if (toolContainer2.Panel2.Contains(tool))
                {
                    location = splitContainer.Panel2.Location
                               + (Size)toolContainer2.Panel2.Location;
                }
            }
            return splitContainer.Location + (Size)location + (Size)clickedPosition;
        }

        #endregion

        #region Reload Asset, Component, Layer, Blackboard

        private void ReloadAssetList()
        {
            // 1. assetList를 비운다.
            assetList.BeginUpdate();
            assetList.Items.Clear();
            assetImageList.Images.Clear();

            // 2. Document.Assets의 목록대로 assetList를 만든다.
            int listCounter = 0;
            foreach (var asset in State.Document.Assets)
            {
                // 2-1. ListViewItem을 만든다.
                ListViewItem lvi = new ListViewItem(asset.GetName())
                {
                    ImageIndex = listCounter++
                };
                // 2-2. 이미지를 ImageList에 추가한다.
                assetImageList.Images.Add(MakeImageFrom(asset.GetNameWithExt()));

                // 2-3. ListView에 추가한다.
                assetList.Items.Add(lvi);
            }
            assetList.EndUpdate();
        }

        private void ReloadComponentList()
        {
            State.ComponentView.Clear();

            State.Document.SortComponentsAscending();
            for (int i = State.Document.CurrentScene.Components.Count - 1; i >= 0; i--)
            {
                var component = State.Document.CurrentScene.Components[i];
                State.ComponentView.AddComponent(component);
            }

            State.Board.Invalidate();
        }

        private void ReloadLayerList()
        {
            layerList.Clear();
            foreach (var layer in State.Document.CurrentScene.Layers)
            {
                layerList.AddLayer(layer);
            }
        }

        #endregion

        #region Asset List

        private void assetList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //            assetList.DoDragDrop(e.Item, DragDropEffects.Move); // start dragging
            assetList.DoDragDrop(assetList.SelectedItems, DragDropEffects.Move); // start dragging

            // the code below will run after the end of dragging
        }

        private void assetList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void assetRemoveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (assetList.SelectedItems.Count > 0)
            {
                for (int i = assetList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem item = assetList.SelectedItems[i];
                    assetList.Items[item.Index].Remove();

                    State.Document.RemoveAsset(item.Text);
                    // TODO 이미지 확장자를 유동적으로 
                    var imageDir = CienDocument.ProjectDirectory + @"\" + CienDocument.Name + @"\"
                                   + AssetDirectory + ImageDirectory + @"\" + item.Text + ".png";
                    if (File.Exists(imageDir))
                    {
                        try
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            File.Delete(imageDir);
                        }
                        catch (IOException exception)
                        {
                            MessageBox.Show(exception.Message);
                        }       
                    }
                }
            }
            ReloadComponentList();
            State.Board.Invalidate();
        }

        void assetList_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    ListViewHitTestInfo hitTestInfo = assetList.HitTest(e.X, e.Y);
                    if (hitTestInfo.Item == null) return;
                    assetContextMenu.Show(this, GetClickedToolPosition(assetList, e.Location));
                    break;
            }
        }

        #endregion

        #region Blackboard

        private void blackboard_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void blackboard_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection))) return;

            var items =
                e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)) as
                    ListView.SelectedListViewItemCollection;

            if (items == null) return;

            Point location = PointToClient(new Point(e.X, e.Y) - (Size)State.Board.Location);

            AddNewComponent(items, location);
            State.Board.Invalidate();
        }

        private void blackboard_KeyDown(object sender, KeyEventArgs e)
        {
            int dx = 0, dy = 0;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    dx -= 1;
                    break;
                case Keys.Right:
                    dx += 1;
                    break;
                case Keys.Up:
                    dy -= 1;
                    break;
                case Keys.Down:
                    dy += 1;
                    break;
                case Keys.Delete:
                    if (State.IsComponentSelected())
                    {
                        State.Document.RemoveComponent(State.Selected.Component.Id);
                        State.SelectedComponentAbandon();
                        ReloadComponentList();
                    }
                    break;
            }
            if (e.Shift)
            {
                dx *= 10;
                dy *= 10;
            }
            if (State.IsComponentSelected()) State.Selected.Move(new Point(dx, dy));


            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        CutComponent();
                        break;
                    case Keys.C:
                        CopyComponent();
                        break;
                    case Keys.V:
                        PasteComponent();
                        break;
                    case Keys.Z:
                        UnDo();
                        break;
                    case Keys.Y:
                        ReDo();
                        break;
                }
            }
            State.Board.Invalidate();
        }

        #endregion

        #region Menu Strip

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (imageImportDialog.ShowDialog())
            {
                case DialogResult.OK:
                    ImportAsset(imageImportDialog.FileNames); // for debug
                    break;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDialog.ShowDialog() != DialogResult.OK) return;
            string filepath = openProjectDialog.FileNames[0];
            string extension = filepath.Split('.').Last();

            switch (extension)
            {
                case ProjectExtension:
                    LoadDocument(filepath);
                    break;
                case Overlap2DExtention:
                    LoadOverlap2D(filepath);
                    break;
                case SceneExtension:
                    MessageBox.Show(@"단일 Scene을 수정하는 기능은 아직 구현되지 않았습니다.");
                    break;
                default:
                    MessageBox.Show(@"지원하는 파일이 아닙니다!");
                    break;
            }
        }

        private void LoadOverlap2D(string filepath)
        {
            string dir = filepath.Substring(0, filepath.LastIndexOf(@"\", StringComparison.Ordinal));
            string imageDir = dir + Overlap2DImageDataDirectory;
            string sceneDir = dir + Overlap2DSceneDirectory;
            DirectoryInfo imageDi = new DirectoryInfo(imageDir);
            DirectoryInfo sceneDi = new DirectoryInfo(sceneDir);
            FileInfo dtfi = new FileInfo(dir + @"\project.dt");

            /************************************************************************/
            /* Check Directories and Files											*/
            /************************************************************************/
            if (!imageDi.Exists || !imageDi.GetFiles().Any())
            {
                MessageBox.Show(@"해당 프로젝트에 Asset이 없습니다.");
                return;
            }
            if (!sceneDi.Exists || !sceneDi.GetFiles().Any())
            {
                MessageBox.Show(@"해당 프로젝트에 Scene이 없습니다.");
                return;
            }
            if (!dtfi.Exists)
            {
                MessageBox.Show(@"해당 프로젝트에 project.dt 파일이 없습니다.");
                return;
            }

            /************************************************************************/
            /* Make New Document													*/
            /************************************************************************/
            // project.pit
            // TODO Overlap2D 프로젝트 이름을, 나중에 scene이 여러개가 되면 사용한다.
            string pitProjectString = File.ReadAllText(filepath);
            Overlap2DProject projectName = JsonConvert.DeserializeObject<Overlap2DProject>(pitProjectString);

            // project.dt
            string dtProjectString = File.ReadAllText(dtfi.FullName);
            ProjectModel projectModel = JsonConvert.DeserializeObject<ProjectModel>(dtProjectString);

            NewDocument(projectModel.scenes.Last().sceneName,
                projectModel.originalResolution.width,
                projectModel.originalResolution.height);

            /************************************************************************/
            /* Load Assets, Layers and Components									*/
            /************************************************************************/
            #region Load Asset
            var imageFIs = imageDi.GetFiles();
            var imageStrings = new List<string>(imageFIs.Length);
            imageStrings.AddRange(imageFIs.Select(fileInfo => fileInfo.FullName));
            ImportAsset(imageStrings.ToArray());
            #endregion

            #region Load Scene
            var fIs = sceneDi.GetFiles("*."+SceneExtension);
            SceneModel[] sceneModels = new SceneModel[fIs.Length];
            for (int i = 0; i < fIs.Length; i++)
            {
                var fileInfo = fIs[i];
                string sceneString = File.ReadAllText(fileInfo.FullName);
                sceneModels[i] = JsonConvert.DeserializeObject<SceneModel>(sceneString);
            }
            // TODO 여러개의 Scene을 지원하지 않음
            var sceneModel = sceneModels[0];
            #endregion

            #region Load Layer
            State.Document.CurrentScene.Layers.Clear();
            foreach (var exportLayer in sceneModel.composite.layers)
            {
                State.Document.CurrentScene.Layers.Add(new CienLayer(exportLayer.layerName, exportLayer.isVisible, exportLayer.isLocked));
            }
            ReloadLayerList();
            #endregion

            #region Load sImages

            if (sceneModel.composite.sImages != null)
                foreach (var exportsImage in sceneModel.composite.sImages)
                {
                    var image = new CienImage(exportsImage.imageName + ".png",
                        exportsImage.itemIdentifier ?? "image" + CienComponent.Number,
                        Point.Empty,
                        exportsImage.zIndex,
                        exportsImage.layerName);

                    Point convertedLocation = CoordinateConverter.GameToBoard(new Point((int)exportsImage.x, (int)exportsImage.y),
                        image.GetSize().Width, image.GetSize().Height);

                    image.SetLocation(convertedLocation);
                    State.Document.AddComponent(image);
                }

            #endregion

            #region Load sComposites

            if (sceneModel.composite.sComposites != null)
                foreach (var exportsComposite in sceneModel.composite.sComposites)
                {
                    if (exportsComposite.composite.sImages == null) continue;

                    var cienComposite = new CienComposite(
                        exportsComposite.composite.sImages[0].imageName + ".png",
                        exportsComposite.itemIdentifier ?? "composite" + CienComponent.Number,
                        Point.Empty,
                        exportsComposite.zIndex,
                        exportsComposite.layerName
                        );

                    Point convertedLocation =
                        CoordinateConverter.GameToBoard(new Point((int)exportsComposite.x, (int)exportsComposite.y),
                            cienComposite.GetSize().Width, cienComposite.GetSize().Height);
                    cienComposite.SetLocation(convertedLocation);   // cienComposite의 location을 정하고 하위 image를 추가해야 한다.

                    for (int i = 1; i < exportsComposite.composite.sImages.Count; i++)
                    {
                        var image = exportsComposite.composite.sImages[i];
                        // TODO Image를 로드할때는 composite크기 기준으로 좌상단 원점 좌표계로 바꿔야 한다.
                        //CoordinateConverter.CompositeToBoard(new Point((int) image.x, (int) image.y), exportsComposite);
                        cienComposite.AddImage(image.imageName, new Point((int)image.x, (int)image.y), image.layerName);
                    }

                    State.Document.AddComponent(cienComposite);
                }

            #endregion

            ReloadComponentList();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewForm newForm = new NewForm();
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                string name = newForm.Name;
                int width = newForm.Width;
                int height = newForm.Height;
                NewDocument(name, width, height);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO 일단 프로젝트 저장(Save)은 프로그램이 있는 위치에 하도록 하자.
            /*if (Document.SaveDirectory == null && saveFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string newDir = selectedPath + @"\" + Document.Name;
                MakeDirectory(newDir);
                Document.SaveDirectory = newDir;
                Directory.Move(Application.StartupPath + AssetDirectory, Document.SaveDirectory + AssetDirectory);
            }*/

            CienDocument.ProjectDirectory = Application.StartupPath;
            InitDocument();
            SaveDocument(CienDocument.Name + "." + ProjectExtension);

            MessageBox.Show(CienDocument.Name + @" 프로젝트가 저장되었습니다.");
        }

        private void jsonExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CienDocument.ExportDirectory == null && exportFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                CienDocument.ExportDirectory = exportFolderBrowser.SelectedPath;
                MakeDirectory(CienDocument.ExportDirectory + @"\scenes");
                Export(CienDocument.ExportDirectory);

                MessageBox.Show("Export COMPLETE!");
            }
        }

        #endregion

        /************************************************************************/
        /* DEBUG																*/
        /************************************************************************/

        private void tESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*State.SelectComponent(new CienImage("asdf", "sdf", Point.Empty, 0));
            MessageBox.Show(State.Selected.Component.Id);*/
        }

    }
}