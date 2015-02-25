using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OneLevel2D.Model;

namespace OneLevel2D.TexturePacker
{
    public class Packer
    {
        /************************************************************************/
        /* Variables															*/
        /************************************************************************/
        private List<Asset> DrawableAssets { get; set; }
        private List<Rect> InputRects { get; set; }
        private List<Rect> OutputRects { get; set; }
        private int RealMaxWidth { get; set; }
        private int RealMaxHeight { get; set; }

        private const string PackImageName = "pack.png";
        private const string AtlasName = "pack.atlas";
        private const string FileFormat = "RGBA8888";
        private const string Filter = "Linear,Linear";
        private const int WidthLimit = 4096;
        private const int HeightLimit = 2048;

        //private Canvas canvas;
        public delegate void AlgorithmDelegate();
        public AlgorithmDelegate AlgorithmRun { get; private set; }

        public Packer()
        {
            //AlgorithmRun = MyAlogorithm;
            AlgorithmRun = MyAlogorithm3;
        }

        /************************************************************************/
        /*      																*/
        /************************************************************************/
        public void LoadAssets(List<Asset> assets)
        {
            DrawableAssets = assets.FindAll(x => x.Type != AssetType.Font);
            if (DrawableAssets == null || DrawableAssets.Count == 0)
            {
                MessageBox.Show(@"Packing할 asset이 없습니다.");
                return;
            }
            

            // Font가 아닌 모든 타입을 저장한다.
            if(InputRects == null)
                InputRects = new List<Rect>(DrawableAssets.Count);

            foreach (var asset in DrawableAssets)
            {
                InputRects.Add(new Rect
                {
                    Width = asset.ImageSize.Width,
                    Height = asset.ImageSize.Height,
                    Name = asset.GetNameWithExt()
                });
            }
        }

        public void Sort()
        {
            if (InputRects == null) return;

            //(a, b) => a.Width.CompareTo(b.Width) // width ascend
            //(a, b) => a.Height.CompareTo(b.Height)   // height ascend
            //(a, b) => b.Width.CompareTo(a.Width) // width descend
            //(a, b) => (a.Width*a.Height).CompareTo(b.Width*b.Height)
        }

        public void Pack()
        {
            Image img = new Bitmap("Asdf");
            img.Save("asdf", ImageFormat.Png);
        }

        public void RunPacking()
        {
            AlgorithmRun();
        }

        public void MakePackImage(string dir)
        {
            if (OutputRects == null || OutputRects.Count == 0) return;

            Bitmap pack = new Bitmap(RealMaxWidth, RealMaxHeight);
            using (Graphics gfx = Graphics.FromImage(pack))
            {
                foreach (var outputRect in OutputRects)
                {
                    Asset match = DrawableAssets.Find(t => t.GetNameWithExt() == outputRect.Name);
                    gfx.DrawImage((Image)match.Data, new Rectangle(outputRect.Position, new Size(outputRect.Width, outputRect.Height)));
                }
            }
            pack.Save(dir + @"\"+PackImageName, ImageFormat.Png);
        }

        public void MakeAtlas(string dir)
        {
            if (OutputRects == null || OutputRects.Count == 0) return;

            string text = "";

            text = text
                   + PackImageName + "\n"
                   + "size: " + RealMaxWidth + "," + RealMaxHeight + "\n"
                   + "format: " + FileFormat + "\n"
                   + "filter: " + Filter + "\n"
                   + "repeat: " + "none" + "\n";
            
            foreach (var outputRect in OutputRects)
            {
                text = text
                       + outputRect.Name.Split('.')[0] + "\n"
                       + "  " + "rotate: " + "false" + "\n"
                       + "  " + "xy: " + outputRect.Position.X + ", " + outputRect.Position.Y + "\n"
                       + "  " + "size: " + outputRect.Width + ", " + outputRect.Height + "\n"
                       + "  " + "orig: " + outputRect.Width + ", " + outputRect.Height + "\n"
                       + "  " + "offset: " + 0 + ", " + 0 + "\n"
                       + "  " + "index: " + -1 + "\n";
            }

            File.WriteAllText(dir + @"\" + AtlasName, text);
        }

        /************************************************************************/
        /* Algorithms															*/
        /************************************************************************/
        public void MyAlogorithm()
        {
            if (InputRects == null || InputRects.Count == 0)
            {
                MessageBox.Show(@"Texture Packing할 Rectangle이 없습니다.");
                return;
            }
            if (OutputRects == null)
                OutputRects = new List<Rect>(InputRects.Count);
            
            InputRects[0].Position = new Point(0, 0);
            OutputRects.Add(InputRects[0]);

            for (int i = 1; i < InputRects.Count; i++)
            {
                // TODO 이미지 가로로 쌓기
                //InputRects[i].Position = InputRects[i - 1].Position + new Size(0, InputRects[i-1].Height);
                InputRects[i].Position = InputRects[i - 1].Position + new Size(InputRects[i - 1].Width, 0);
                OutputRects.Add(InputRects[i]);
            }

            // TODO 이미지 가로로 쌓기
            /*RealMaxWidth = OutputRects.Max(t => t.Width);
            RealMaxHeight = OutputRects.Sum(t => t.Height);*/
            RealMaxWidth = OutputRects.Sum(t => t.Width);
            RealMaxHeight = OutputRects.Max(t => t.Height);
        }


        public void MyAlogorithm3()
        {
            if (InputRects == null || InputRects.Count == 0)
            {
                MessageBox.Show(@"Texture Packing할 Rectangle이 없습니다.");
                return;
            }
            if (OutputRects == null)
                OutputRects = new List<Rect>(InputRects.Count);

            InputRects.Sort((a, b) => b.Height.CompareTo(a.Height));

            var rectsLineList = new List<List<Rect>>();
            for (var i = 0; i < InputRects.Count; i++)
            {
                if (InputRects[i].Width > WidthLimit) continue;

                if (rectsLineList.Count == 0)
                {
                    InputRects[i].SetPosition(new Point(0, 0));
                }
                else
                {
                    var top = rectsLineList.Last().First();
                    InputRects[i].SetPosition(top.LeftBottom());
                }

                var rectsLine = new List<Rect> {InputRects[i]};

                while (i < InputRects.Count-1)
                {
                    i++;
                    InputRects[i].SetPosition(InputRects[i - 1].TopRight());
                    rectsLine.Add(InputRects[i]);

                    if (rectsLine.Sum(x => x.Width) >= WidthLimit)
                    {
                        i--;
                        rectsLine.Remove(rectsLine.Last());
                        break;
                    }
                }

                rectsLineList.Add(rectsLine);
            }

            foreach (var rectsLine in rectsLineList)
            {
                OutputRects.AddRange(rectsLine);
            }

            Debug.Print("Packed rects: " + OutputRects.Count + "/" + InputRects.Count);

            RealMaxWidth = rectsLineList.Max(x => x.Sum(y => y.Width));
            RealMaxHeight = rectsLineList.Sum(x => x.Max(y => y.Height));
        }

        #region 미구현 알고리즘1
        /*public void First()
        {
            // TODO Rect를 가로로 나열하기, 최고 높이, 넓이 합 결정하기
            int widthSum = 0;
            int areaSum = 0;
            for (int i = 0; i < _rects.Count; i++)
            {
                _rects[i].Position = new Point(widthSum, 0);
                widthSum += _rects[i].Width;
                areaSum += _rects[i].Width*_rects[i].Height;
            }
            //widthSum += _rects.Last().Width;

            _totalWidth = widthSum;
            _maximumHeight = _rects[0].Height;
            _totalArea = areaSum;

            // TODO 첫번째 Best 넓이 정함.
            int width2N = FindSmallest2N(_totalWidth);
            int height2N = FindSmallest2N(_maximumHeight);

            bestSizes.Add(new Size(width2N, height2N));
            ratios.Add((double)_totalArea/(width2N*height2N));
        }

        public void Second()
        {
            // TODO 가장 오른쪽의 사각형들 부터 왼쪽으로 채워 넣는다.
            int N = _rects.Count;
            for (int i = N; i >= 2; i--)
            {
                for (int j = 0; j < N; j++)
                {
                    if (_rects[i].Height < _rects[j].Height - _rects[j + 1].Height)
                    {
                        _rects[i].Position = _rects[j+1].Position + Size(0, _rects[j+1].Height);
                        break;
                    }
                }
                
            }

        }

        public void Third()
        {
            // TODO
        }*/
        #endregion

        #region 미구현 알고리즘2

        #endregion

        #region 수학적 도구들
        public static int FindSmallest2N(int number)
        {
            int result;

            if (number % 2 == 1) number++;

            while (!CheckTwoMultiple(number))
            {
                number += 2;
            }

            result = number;

            return result;
        }

        public static bool CheckTwoMultiple(int number)
        {
            bool result = false;
            int checker = 0;
            while (number > 2)
            {
                if (number % 2 == 1)
                {
                    checker++;
                    break;
                }
                number = number / 2;
            }
            if (checker == 0) result = true;

            return result;
        }
        #endregion

        #region Model
        public class Rect
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public Point Position { get; set; }
            public string Name { get; set; }
            private bool Set;

            public void SetPosition(Point position)
            {
                Position = position;
                Set = true;
            }

            public bool IsSet(){return Set;}

            public override string ToString()
            {
                return Name + ": " + Width + ", " + Height;
            }

            public Point TopRight()
            {
                return new Point(Position.X+Width, Position.Y);
            }

            public Point LeftBottom()
            {
                return new Point(Position.X, Position.Y+Height);
            }

            public Point RightBottom()
            {
                return new Point(Position.X+Width, Position.Y+Height);
            }

        }

        public class RectFloor
        {
            private List<Rect> Rects { get; set; }

            public RectFloor()
            {
                Rects = new List<Rect>();
            }

            public RectFloor(List<Rect> rects)
            {
                Rects = rects;
            }

            public void Add(Rect rect)
            {
                Rects.Add(rect);
            }

            public int Count()
            {
                return Rects.Count;
            }

            public Rect First()
            {
                return Rects.First();
            }
        }
        #endregion

        #region 캔버스
        /*private class Canvas
        {
            public int Width { get; set; }
            public int Height { get; set; }
            private Size _size;
            //public Dictionary<Rect, Point> Rects { get; set; }  // sorted

            public Canvas() : this(16, 16) { }
            public Canvas(int width, int height)
            {
                //Rects = new Dictionary<Rect, Point>();
                Width = width;
                Height = height;
                _size = new Size(width, height);
            }

            public void AddRect(Rect rect)
            {
                _rects.Add(rect);
                /*int rectAreaSum = _rects.Sum(t => t.Width*t.Height);
                int rectsWidthSum = _rects.Sum(t => t.Width);
                int rectsHeightSum = _rects.Sum(t => t.Height);
                if (Width*Height < rectAreaSum)
                {
                    (((Width - rectsWidthSum) - (Height - rectsHeightSum)) >= 0) ? Width : Height;
                }#1#
                int realMaxWidth = _rects.Max(t => t.Position.X + t.Width);
                int realMaxHeight = _rects.Max(t => t.Position.Y + t.Height);

                if (realMaxWidth > Width) Width *= 2;
                if (realMaxHeight > Height) Height *= 2;
            }

            private List<Rect> _rects;
        }*/
        #endregion

        #region 생성자와 기타 모델
/*        public TexturePacker(Document document)
        {
            this.document = document;
            name = "pack.png";
            imageSize = Size.Empty;
            format = "RGBA8888";
            filter = "Linear, Linear";
            repeat = "none";
            textures = new List<TextureModel>();
        }

        private void MakePack()
        {

        }

        private void MakeTextureModel()
        {
            foreach (var asset in document.Assets)
            {
                TextureModel texture = new TextureModel();
                texture.name = asset.Name;
                texture.rotate = false;
                
            }
        }

        private Document document;
        private string name;
        private Size imageSize;
        private string format;
        private string filter;
        private string repeat;
        private List<TextureModel> textures;

        class TextureModel
        {
            public string name;
            public bool rotate;
            public int x, y;
            public Size size;
            public Size orig;
            public Size offset;
        }*/
        #endregion


        // For algorithm1
/*        private int _maximumHeight;
        private int _totalWidth;
        private int _totalArea;
        private List<Size> bestSizes;
        private List<double> ratios;*/
    }
}
