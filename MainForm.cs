using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OneLevelJson
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            _document = new Document();

            InitDocument();

            AddEvent();
        }

        private void InitDocument()
        {
            // TODO blackboard가 사용하는 document를 설정해주고 blackboard를 Refresh 해줘야 한다.

            blackboard.PresentDocument = _document;
            ReloadAssetList();
            ReloadComponentList();
            blackboard.Invalidate();

            // TODO 분리해주어야 좋을 Directory 설정. 이 부분을 어디서 사용할지 모르니까 쉽사리 분리를 하지 못하겠다.
            string projectPath = Document.SaveDirectory ?? Application.StartupPath;
            MakeDirectory(projectPath + AssetDirectory);
            MakeDirectory(projectPath + ImageDataDirectory);
        }

        private void NewDocument(string name, int width, int height)
        {
            _document = new Document(name, width, height);
            InitDocument();
        }

        private void LoadDocument(string dir)
        {
            string extension = dir.Split('.').Last();
            if (extension != ProjectExtension)
            {
                MessageBox.Show(@"프로젝트 파일이 아닙니다!");
                return;
            }

            string docstring = File.ReadAllText(dir);
            ParseDocument(docstring);

            InitDocument();
        }

        /*private void SaveDocument(string dir)
        {
            string docjson = JsonConvert.SerializeObject(_document);
            File.WriteAllText(dir, docjson);
            // _document를 json으로 serialize해서 파일에 쓰기.
        }*/

        private void SaveDocument(string filename)
        {
            string docjson = JsonConvert.SerializeObject(_document);
            File.WriteAllText(Document.SaveDirectory + @"\" + filename, docjson);
            // _document를 json으로 serialize해서 파일에 쓰기.
        }

        private void ParseDocument(string docstring)
        {
            // docstring을 deserialize해서 doc에 넣어주기.
            _document = JsonConvert.DeserializeObject<Document>(docstring);
        }


        private void ImportAsset(string[] files)
        {
            // 1. 이미지들을 실행파일이 있는 프로젝트로 복사한 후,

            foreach (var file in files)
            {
                string projectDirectory = Document.SaveDirectory ?? Application.StartupPath;
                try
                {
                    File.Copy(file, projectDirectory + ImageDataDirectory + @"\" + file.Split('\\').Last());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Asset newAsset = MakeAssetFrom(file);
                _document.AddAsset(newAsset);
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

        private void ReloadAssetList()
        {
            // 1. assetList를 비운다.
            assetList.BeginUpdate();
            assetList.Clear();
            assetImageList.Images.Clear();

            // 2. Document.Assets의 목록대로 assetList를 만든다.
            int listCounter = 0;
            foreach (var asset in _document.Assets)
            {
                // 2-1. ListViewItem을 만든다.
                ListViewItem lvi = new ListViewItem(asset.Name)
                {
                    ImageIndex = listCounter++
                };
                // 2-2. 이미지를 ImageList에 추가한다.
                assetImageList.Images.Add(MakeImageFrom(asset.Name));

                // 2-3. ListView에 추가한다.
                assetList.Items.Add(lvi);
            }
            assetList.EndUpdate();
        }

        private void AddComponent(ListView.SelectedListViewItemCollection items, Point location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string name = items[i].Text;
                Size offset = new Size(15 * i, 15 * i);
                _document.AddComponent(name, location + offset);
            }

            ReloadComponentList();
        }

        private void ReloadComponentList()
        {
            componentList.BeginUpdate();
            componentList.Clear();
            componentImageList.Images.Clear();

            int listCounter = 0;
            foreach (var component in _document.Components)
            {
                ListViewItem lvi = new ListViewItem(component.Id)
                {
                    ImageIndex = listCounter++
                };
                componentImageList.Images.Add(MakeImageFrom(component.ParentAsset.Name));

                componentList.Items.Add(lvi);
            }
            componentList.EndUpdate();
        }

        private Image MakeImageFrom(string file)
        {
            string projectDirectory = Document.SaveDirectory ?? Application.StartupPath;
            return Image.FromFile(projectDirectory + ImageDataDirectory + @"\" + file);
        }

        public void MakeDirectory(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists) Directory.CreateDirectory(dir);
        }

        /************************************************************************/
        /* Event Callback                                                       */
        /************************************************************************/
        private void AddEvent()
        {
            assetList.ItemActivate += assetList_ItemActivate;
            assetList.ItemDrag += assetList_ItemDrag;
            assetList.DragOver += assetList_DragOver;
            assetList.DragEnter += assetList_DragEnter;

            componentList.SelectedIndexChanged += componentList_SelectedIndexChanged;
            componentList.MouseDown += componentList_MouseDown;

            blackboard.DragEnter += blackboard_DragEnter;
            blackboard.DragDrop += blackboard_DragDrop;
            blackboard.KeyDown += blackboard_KeyDown;
        }


        /************************************************************************/
        /* Asset List															*/
        /************************************************************************/
        private void assetList_ItemActivate(object sender, EventArgs e)
        {
            MessageBox.Show(@"File Information");
        }

        private void assetList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _log.Write("DRAG START");
            //            assetList.DoDragDrop(e.Item, DragDropEffects.Move); // start dragging
            assetList.DoDragDrop(assetList.SelectedItems, DragDropEffects.Move); // start dragging

            // the code below will run after the end of dragging
            _log.Write("DRAG END");
        }

        private void assetList_DragEnter(object sender, DragEventArgs e)
        {
            _log.Write("DRAG ENTER");
            e.Effect = e.AllowedEffect;
        }

        private void assetList_DragOver(object sender, DragEventArgs e)
        {
            _log.Write(e.X + " " + e.Y);
        }

        /************************************************************************/
        /* Component List														*/
        /************************************************************************/
        private void componentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (componentList.SelectedItems.Count != 0)
            {
                string selectedId = componentList.SelectedItems[0].Text;
                Component selectedComponent = _document.Components.Find(x => x.Id == selectedId);
                picBox.Image = MakeImageFrom(selectedComponent.ParentAsset.Name);
            }
        }

        private void componentList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hitTestInfo = componentList.HitTest(e.X, e.Y);
                if (hitTestInfo.Item == null) return;

                if (componentList.SelectedIndices.Count > 1)
                {
                    componentMenuStrip.Items[0].Enabled = false;
                }
                componentMenuStrip.Show(this, componentList.Location + (Size)e.Location);
            }
        }

        /************************************************************************/
        /* Blackboard															*/
        /************************************************************************/
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
            picBox.Image = MakeImageFrom(items[0].Text); // 미리보기 이미지 설정

            Point location = blackboard.PointToClient(new Point(e.X, e.Y));

            AddComponent(items, location);
            blackboard.Invalidate();
        }

        private void blackboard_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (blackboard.SelectedComponent != null)
                    {
                        _document.RemoveComponent(blackboard.SelectedComponent.Id);
                        blackboard.RemoveSelected();
                    }
                    break;
            }
            blackboard.Invalidate();
        }

        /************************************************************************/
        /* Menu Strip															*/
        /************************************************************************/
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
            switch (openProjectDialog.ShowDialog())
            {
                case DialogResult.OK:
                    LoadDocument(openProjectDialog.FileNames[0]); // for debug
                    break;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuNewForm newForm = new MenuNewForm();
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
            switch (saveFolderBrowserDialog.ShowDialog())
            {
                case DialogResult.OK:
                    if (Document.SaveDirectory == null)
                    {
                        string newDir = saveFolderBrowserDialog.SelectedPath + @"\" + _document.Name;
                        MakeDirectory(newDir);
                        Document.SaveDirectory = newDir;
                        Directory.Move(Application.StartupPath + AssetDirectory, Document.SaveDirectory + AssetDirectory);
                        InitDocument();
                    }

                    SaveDocument(_document.Name + "." + ProjectExtension);

                    break;
            }

        }

        private void jsonExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO Model로 Export하는거 만들어야지.
            switch (exportFolderBrowser.ShowDialog())
            {
                case DialogResult.OK:
                    if (Document.ExportDirectory == null)
                    {
                        string exportDir = exportFolderBrowser.SelectedPath;
                        Document.ExportDirectory = exportDir;
                    }

                    Export(Document.ExportDirectory);

                    break;
            }
        }

        private void Export(string exportDir)
        {
            string imagePackDir = exportDir + @"\assets\orig";
            MakeDirectory(imagePackDir);

            texturePacker = new TexturePacker.Packer();
            texturePacker.LoadAssets(_document.Assets);
            texturePacker.RunPacking();
            texturePacker.MakePackImage(imagePackDir);
            texturePacker.MakeAtlas(imagePackDir);

            #region 모델 관련 작업
            /*            /***********************************************************************#1#
            /* Project Model														#1#
            /***********************************************************************#1#
            ProjectModel project = new ProjectModel();
            project.scenes = new List<ProjectModel.Scene>();

            ProjectModel.Scene scene = new ProjectModel.Scene()
            {
                ambientColor = { 0.5f, 0.5f, 0.5f, 1 },
                sceneName = _document.Name
            };
            ProjectModel.Resolution orig = new ProjectModel.Resolution()
            {
                width = _document.Width,
                height = _document.Height,
                name = "orig"
            };
            project.scenes.Add(scene);
            project.originalResolution = orig;

            /***********************************************************************#1#
            /* Scene Model														    #1#
            /***********************************************************************#1#
            SceneModel sceneModel = new SceneModel();
            sceneModel.composite = new Composite1();
            sceneModel.ambientColor = new List<float>();
            sceneModel.sceneName = _document.Name;

            sceneModel.composite.layers = new List<Layer>();
            sceneModel.composite.sComposites = new sComposite();

            sceneModel.composite.sComposites.layerName = "";*/
            #endregion
        }

        /************************************************************************/
        /* Component List Menu Strip											*/
        /************************************************************************/
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuRenameForm renameForm = new ContextMenuRenameForm();
            if (renameForm.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in componentList.SelectedItems)
                {
                    string selectedId = selectedItem.Text;
                    string newId = renameForm.Result;
                    _document.ReNameComponent(selectedId, newId);
                }
            }
            ReloadComponentList();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (componentList.SelectedItems.Count > 0)
            {
                for (int i = componentList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem item = componentList.SelectedItems[i];
                    componentList.Items[item.Index].Remove();
                    _document.RemoveComponent(item.SubItems[0].Text);
                }
            }
            blackboard.Invalidate();
        }

        /************************************************************************/
        /* DEBUG																*/
        /************************************************************************/

        /************************************************************************/
        /* Variables															*/
        /************************************************************************/
        private readonly Log _log = new Log();
        private Document _document;
        private TexturePacker.Packer texturePacker;
        private const string ProjectExtension = "dt";
        public static readonly string AssetDirectory = @"\assets";
        public static readonly string ImageDataDirectory = @"\assets\image";
    }
}