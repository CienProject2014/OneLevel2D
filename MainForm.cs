using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using OneLevelJson.Export;
using OneLevelJson.Model;
using OneLevelJson.TexturePacker;
using Layer = OneLevelJson.Model.CienLayer;

namespace OneLevelJson
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            NewDocument("noname", 1920, 1080);

            // TODO 임시로 처리 해둔것. Project save to other directory를 구현하면 수정해야함.
            CienDocument.ProjectDirectory = Application.StartupPath;

            AddEvent();
        }

        private void InitDocument()
        {
            titleBarControl1.SetTitleName(CienDocument.Name + " - " + ProgramName);
            ReloadAssetList();
            ReloadComponentList();
            ReloadLayerList();

            ReloadBlackboard();

            CienComponent.Number = 0;

            // TODO 분리해주어야 좋을 Directory 설정. 이 부분을 어디서 사용할지 모르니까 쉽사리 분리를 하지 못하겠다.
            string projectPath = CienDocument.ProjectDirectory ?? Application.StartupPath;
            MakeDirectory(projectPath + @"\" + CienDocument.Name);
            MakeDirectory(projectPath + @"\" + CienDocument.Name + @"\" + AssetDirectory);
            MakeDirectory(projectPath + @"\" + CienDocument.Name + @"\" + AssetDirectory + ImageDirectory);
        }

        private void AddEvent()
        {
            assetList.SelectedIndexChanged += assetList_SelectedIndexChanged;
            assetList.ItemDrag += assetList_ItemDrag;
            assetList.DragOver += assetList_DragOver;
            assetList.DragEnter += assetList_DragEnter;

            componentList.MouseDown += componentList_MouseDown;
            componentList.SelectedIndexChanged += componentList_SelectedIndexChanged;

            layerList.SelectedIndexChanged += layerList_SelectedIndexChanged;
            layerList.MouseDown += layerList_MouseDown;
            layerList.ItemChecked += layerList_ItemChecked;

            blackboard.DragEnter += blackboard_DragEnter;
            blackboard.DragDrop += blackboard_DragDrop;
            blackboard.KeyDown += blackboard_KeyDown;
        }

        private void layerList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //MessageBox.Show(e.Item.Checked.ToString());

            if (State.Document == null) return;

            CienLayer selectedLayer = State.Document.Layers.Find(x => x.Name == e.Item.Text);
            if (selectedLayer != null) selectedLayer.SetVisible(e.Item.Checked);

            blackboard.Invalidate();
        }

        #region New, Load, Save, Import

        private void NewDocument(string name, int width, int height)
        {
            State.Document = new CienDocument(name, width, height);
            blackboard.SetDocument(State.Document);
            blackboard.Invalidate();
            InitDocument();
        }

        private void LoadDocument(string dir)
        {
            string docstring = File.ReadAllText(dir);

            ParseDocument(docstring);
            InitDocument();
        }

        private void SaveDocument(string filename)
        {
            string docjson = JsonConvert.SerializeObject(State.Document);
            File.WriteAllText(CienDocument.ProjectDirectory + @"\" + filename, docjson);
            // State.Document를 json으로 serialize해서 파일에 쓰기.
        }

        private void ParseDocument(string docstring)
        {
            // docstring을 deserialize해서 doc에 넣어주기.
            State.Document = JsonConvert.DeserializeObject<CienDocument>(docstring);
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

        private void AddComponent(ListView.SelectedListViewItemCollection items, Point location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string name = items[i].Text;
                Size offset = new Size(15 * i, 15 * i);
                Point transformedLocation = blackboard.PointTransform(location);
                State.Document.AddComponent(name, transformedLocation + offset);
                try
                {
                    componentList.Items.Add(new ListViewItem(State.Document.Components.Last().Id)
                    {
                        SubItems = { State.Document.Components.Last().ZIndex.ToString() }
                    });
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show(@"선택된 Layer가 없습니다: " + e.StackTrace);

                }
            }

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

        private void SortComponentList()
        {
            // descend => latter will be top
            State.Document.Components.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
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
            componentList.BeginUpdate();
            componentList.Items.Clear();

            SortComponentList();

            for (int i = State.Document.Components.Count - 1; i >= 0; i--)
            {
                var component = State.Document.Components[i];
                componentList.Items.Add(new ListViewItem(component.Id)
                {
                    SubItems = { component.ZIndex.ToString() },
                });
            }

            componentList.EndUpdate();

            blackboard.Invalidate();
        }

        private void ReloadLayerList()
        {
            layerList.BeginUpdate();
            layerList.Items.Clear();

            foreach (var layer in State.Document.Layers)
            {
                ListViewItem lvi = new ListViewItem(layer.Name)
                {
                    Checked = layer.IsVisible
                };

                layerList.Items.Add(lvi);
            }

            layerList.EndUpdate();
        }

        private void ReloadBlackboard()
        {
            blackboard.Invalidate();
        }

        #endregion

        /************************************************************************/
        /* Asset List															*/
        /************************************************************************/

        #region Asset List

        private void assetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (assetList.SelectedItems.Count != 0)
            {
                // TODO picbox를 임시로 삭제
                /*string selectedName = assetList.SelectedItems[0].Text;
                Asset selectedAsset = State.Document.Assets.Find(x => x.GetName() == selectedName);
                picBox.Image = MakeImageFrom(selectedAsset.GetNameWithExt());*/
            }
        }

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

        private void assetList_DragOver(object sender, DragEventArgs e)
        {
        }

        #endregion

        /************************************************************************/
        /* Component List														*/
        /************************************************************************/

        #region Componet List

        private void componentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (componentList.SelectedItems.Count != 0)
            {
                string selectedId = componentList.SelectedItems[0].Text;
                State.SelectComponent(State.Document.Components.Find(x => x.Id == selectedId));
            }
            blackboard.Invalidate();
        }

        private void componentList_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    ListViewHitTestInfo hitTestInfo = componentList.HitTest(e.X, e.Y);
                    if (hitTestInfo.Item == null) return;
                    if (componentList.SelectedIndices.Count > 1)
                    {
                        componentContextMenu.Items[0].Enabled = false;
                    }
                    componentContextMenu.Show(this, GetClickedToolPosition(componentList, e.Location));
                    break;
            }
        }

        private void componentRenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ContextRenameForm renameForm = new ContextRenameForm())
            {
                if (renameForm.ShowDialog() == DialogResult.OK)
                {
                    foreach (ListViewItem selectedItem in componentList.SelectedItems)
                    {
                        string selectedId = selectedItem.Text;
                        string newId = renameForm.Result;
                        State.Document.RenameComponent(selectedId, newId);
                    }
                }
            }

            ReloadComponentList();
        }

        private void conponentRemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (componentList.SelectedItems.Count > 0)
            {
                for (int i = componentList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem item = componentList.SelectedItems[i];
                    componentList.Items[item.Index].Remove();
                    State.Document.RemoveComponent(item.SubItems[0].Text);
                }
            }
            blackboard.Invalidate();
        }

        private void componentUpBtn_Click(object sender, EventArgs e)
        {
            if (!State.IsComponentSelected()) return;

            int newZIndex = State.Selected.Component.ZIndex + 1;
            if (newZIndex > State.Document.Components.Max(x => x.ZIndex)) return;

            /*State.Document.Components.Find(x => x.ZIndex == newZIndex).MoveDown();
            State.Selected.Component.MoveUp();*/

            State.Document.Components.Find(x => x.ZIndex == newZIndex).SetZindex(CienComponent.EmptyZindex);
            State.Selected.MoveUp();
            State.Document.Components.Find(x => x.ZIndex == CienComponent.EmptyZindex).SetZindex(newZIndex - 1);

            ReloadComponentList();
        }

        private void componentDownBtn_Click(object sender, EventArgs e)
        {
            if (!State.IsComponentSelected()) return;

            int newZIndex = State.Selected.Component.ZIndex - 1;
            if (newZIndex < 0) return;

            State.Document.Components.Find(x => x.ZIndex == newZIndex).SetZindex(CienComponent.EmptyZindex);
            State.Selected.MoveDown();
            State.Document.Components.Find(x => x.ZIndex == CienComponent.EmptyZindex).SetZindex(newZIndex + 1);

            ReloadComponentList();
        }

        #endregion

        /************************************************************************/
        /* Layer List															*/
        /************************************************************************/

        #region Layer List

        private void addLayer_Click(object sender, EventArgs e)
        {
            State.Document.Layers.Add(new Layer("layer" + State.Document.Layers.Count, true, false));
            ReloadLayerList();
        }

        private void deleteLayer_Click(object sender, EventArgs e)
        {
            var items = layerList.SelectedItems;
            if (items.Count == 0) return;

            for (int i = 0; i < items.Count; i++)
            {
                CienLayer selectedLayer = State.Document.Layers.Find(x => x.Name == items[i].Text);
                State.Document.Layers.Remove(selectedLayer);
            }

            ReloadLayerList();
        }

        private void layerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (layerList.SelectedItems.Count == 1)
            {
                State.SelectLayer(State.Document.Layers.Find(x => x.Name == layerList.SelectedItems[0].Text));
            }
        }

        private void layerList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hitTestInfo = layerList.HitTest(e.X, e.Y);
                if (hitTestInfo.Item == null) return;

                layerContextMenu.Show(this, GetClickedToolPosition(layerList, e.Location));
            }
        }

        private void layerRenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextRenameForm renameForm = new ContextRenameForm();
            if (renameForm.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in layerList.SelectedItems)
                {
                    string selectedId = selectedItem.Text;
                    string newId = renameForm.Result;
                    State.Document.RenameLayer(selectedId, newId);
                }
            }
            ReloadLayerList();
        }

        private void lockunlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (layerList.SelectedItems.Count == 1)
            {
                var selectedLayer = State.Document.Layers.Find(x => x.Name == layerList.SelectedItems[0].Text);
                selectedLayer.LockToggle();
            }
        }

        #endregion

        /************************************************************************/
        /* Blackboard															*/
        /************************************************************************/

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

            // TODO picBox 임시로 삭제
            //picBox.Image = MakeImageFrom(items[0].Text); // 미리보기 이미지 설정

            Point location = PointToClient(new Point(e.X, e.Y) - (Size)blackboard.Location);

            AddComponent(items, location);
            blackboard.Invalidate();
        }

        private void blackboard_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (State.IsComponentSelected())
                    {
                        State.Document.RemoveComponent(State.Selected.Component.Id);
                        State.SelectAbandon();
                        ReloadComponentList();
                    }
                    break;
                // case Arrow keys are handled at blackboard's OnKeyDown
            }
            blackboard.Invalidate();
        }

        #endregion

        /************************************************************************/
        /* Menu Strip															*/
        /************************************************************************/

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
                default:
                    MessageBox.Show(@"프로젝트 파일이 아닙니다!");
                    break;
            }
        }

        private void LoadOverlap2D(string filepath)
        {
            string dir = filepath.Substring(0, filepath.LastIndexOf(@"\", StringComparison.Ordinal));
            string imageDir = dir + Overlap2DImageDataDirectory;
            string sceneDir = dir + Overlap2DSceneDirectory;
            DirectoryInfo imagedi = new DirectoryInfo(imageDir);
            DirectoryInfo scenedi = new DirectoryInfo(sceneDir);
            FileInfo dtfi = new FileInfo(dir + @"\project.dt");

            /************************************************************************/
            /* Check Directories and Files											*/
            /************************************************************************/
            if (!imagedi.Exists || !imagedi.GetFiles().Any())
            {
                MessageBox.Show(@"해당 프로젝트에 Asset이 없습니다.");
                return;
            }
            if (!scenedi.Exists || !scenedi.GetFiles().Any())
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
            var imageFIs = imagedi.GetFiles();
            var imageStrings = new List<string>(imageFIs.Length);
            imageStrings.AddRange(imageFIs.Select(fileInfo => fileInfo.FullName));
            ImportAsset(imageStrings.ToArray());
            #endregion

            #region Load Scene
            var fIs = scenedi.GetFiles(SceneExtension);
            SceneModel[] sceneModels = new SceneModel[fIs.Length];
            for (int i = 0; i < fIs.Length; i++)
            {
                var fileInfo = fIs[i];
                string sceneString = File.ReadAllText(fileInfo.FullName);
                sceneModels[i] = JsonConvert.DeserializeObject<SceneModel>(sceneString);
            }
            var sceneModel = sceneModels[0];
            #endregion

            #region Load Layer
            State.Document.Layers.Clear();
            foreach (var exportLayer in sceneModel.composite.layers)
            {
                State.Document.Layers.Add(new CienLayer(exportLayer.layerName, exportLayer.isVisible, exportLayer.isLocked));
            }
            ReloadLayerList();
            #endregion

            #region Load sImages

            if (sceneModel.composite.sImages != null)
                foreach (var exportsImage in sceneModel.composite.sImages)
                {
                    var image = new CienImage(exportsImage.imageName + ".png",
                        exportsImage.itemIdentifier ?? "image" + CienComponent.Number++,
                        Point.Empty,
                        exportsImage.zIndex,
                        exportsImage.layerName);

                    Point convertedLocation = CoordinateConverter.GameToBoard(new Point((int)exportsImage.x, (int)exportsImage.y),
                        image.GetSize().Width, image.GetSize().Height);

                    image.SetLocation(convertedLocation);
                    State.Document.Components.Add(image);
                }

            #endregion

            #region Load sComposites

            if (sceneModel.composite.sComposites != null)
                foreach (var exportsComposite in sceneModel.composite.sComposites)
                {
                    if (exportsComposite.composite.sImages == null) continue;

                    var cienComposite = new CienComposite(
                        exportsComposite.composite.sImages[0].imageName,
                        exportsComposite.itemIdentifier ?? "composite" + CienComponent.Number++,
                        Point.Empty,
                        exportsComposite.layerName
                        );

                    Point convertedLocation =
                        CoordinateConverter.GameToBoard(new Point((int)exportsComposite.x, (int)exportsComposite.y),
                            cienComposite.GetSize().Width, cienComposite.GetSize().Height);
                    cienComposite.SetLocation(convertedLocation);   // cienComposite의 location을 정하고 하위 image를 추가해야 한다.

                    for (int i = 1; i < exportsComposite.composite.sImages.Count; i++)
                    {
                        var image = exportsComposite.composite.sImages[i];
                        cienComposite.AddImage(image.imageName, new Point((int)image.x, (int)image.y), image.layerName);
                    }

                    State.Document.Components.Add(cienComposite);
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
            }
        }

        #endregion

        /************************************************************************/
        /* DEBUG																*/
        /************************************************************************/

        /************************************************************************/
        /* Variables															*/
        /************************************************************************/
        //private CienDocument _document;
        private readonly Packer TexturePacker = new Packer();
        private readonly Maker ModelMaker = new Maker();
        public const string ProgramName = "OneLevel2D";
        public const string ProjectExtension = "cien";
        public const string AssetDirectory = @"\assets";
        public const string ImageDirectory = @"\image";

        public const string Overlap2DExtention = "pit";
        public const string Overlap2DImageDataDirectory = @"\assets\orig\images";
        public const string Overlap2DSceneDirectory = @"\scenes";

        public const string SceneExtension = "*.dt";

        private void tESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*State.SelectComponent(new CienImage("asdf", "sdf", Point.Empty, 0));
            MessageBox.Show(State.Selected.Component.Id);*/
        }

    }
}