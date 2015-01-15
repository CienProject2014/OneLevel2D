/************************************************************************/
/*                              [ALGORITHM]
 * 1.  Sort the rectangles by height, greatest height first.
 * 2.  Start off with an enclosing rectangle that is as high as the highest 
 *     rectangle, and that has unlimited width.
 * 3.  Place the rectangles in the enclosing rectangle one by one, starting 
 *     with the highest rectangle and ending with the lowest rectangle. 
 *     Put each rectangle as far left as possible. If there are several 
 *     left most locations, use the highest one.
 * 4.  Make the width of the enclosing rectangle equal to the total width 
 *     taken by the rectangles. That is, move the right edge of the 
 *     enclosing rectangle to left until it touches the right edge of the 
 *     right most rectangle. That way, the enclosing rectangle is 
 *     no wider than needed.
 * 5.  Did you manage to place all rectangles in the enclosing rectangle? 
 *     In that case:
 *       - If the enclosing rectangle you've got now is the smallest 
 *         "successful" enclosing rectangle so far, store this enclosing 
 *         rectangle as the best enclosing rectangle so far.
 *       - It's time to try a smaller enclosing rectangle - decrease the 
 *         width of the enclosing rectangle by one.
 * 6.  Note that reducing the width and increasing the height means 
 *     in effect that we're testing the range of enclosing rectangles 
 *     from low and wide to high and narrow.
 * 7.  If the total area (width x height) of the enclosing rectangle you
 *     have now is smaller than the total area of all the rectangles 
 *     you're going to try to place inside it, then this is obviously not 
 *     a viable enclosing rectangle. Increase the height by one until you 
 *     get a viable enclosing rectangle. Then go to the next step.
 * 8.  If the enclosing rectangle you've got now is bigger than the best 
 *     enclosing rectangle so far, there is no point in testing this 
 *     enclosing rectangle. Decrease the width by one and go back to 
 *     step 7 to make sure it is now not too small. Otherwise go to 
 *     the next step.
 * 9.  If your new enclosing rectangle is narrower than the widest 
 *     rectangle, you can stop now, and report the best enclosing rectangle 
 *     you found so far. This is because the algorithm never increases the 
 *     width of the enclosing rectangle, and if the widest rectangle won't 
 *     fit, there is obviously no point in testing the enclosing rectangle.
 * 10. Now that your new enclosing rectangle is neither too small nor too 
 *     big, go back to step 3 to see if you can place all rectangles 
 *     inside it.                                                       */
/************************************************************************/

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace OneLevelJson.TexturePacker
{
    class Packer
    {
        public Packer()
        {
            AlgorithmRun = MyAlogorithm;
        }

        /************************************************************************/
        /*      																*/
        /************************************************************************/
        public void LoadAssets(List<Asset> assets)
        {
            Assets = assets;
            if(InputRects == null)
                InputRects = new List<Rect>(assets.Count);

            foreach (var asset in assets)
            {
                InputRects.Add(new Rect
                {
                    Width = asset.ImageSize.Width,
                    Height = asset.ImageSize.Height,
                    Name = asset.Name
                });
            }
        }

        public void Sort()
        {
            if (InputRects == null) return;

            //(a, b) => a.Width.CompareTo(b.Width); // width ascend
            //(a, b) => a.Height.CompareTo(b.Height);   // height ascend

            InputRects.Sort((a, b) => b.Height.CompareTo(a.Height)); // descend

            foreach (var rect in InputRects)
            {
                _log.Write(rect.ToString());
            }
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
            Bitmap pack = new Bitmap(RealMaxWidth, RealMaxHeight);
            using (Graphics gfx = Graphics.FromImage(pack))
            {
                foreach (var outputRect in OutputRects)
                {
                    Asset match = Assets.Find(t => t.Name == outputRect.Name);
                    gfx.DrawImage((Image)match.Data, new Rectangle(outputRect.Position, new Size(outputRect.Width, outputRect.Height)));
                }
            }
            pack.Save(dir + @"\"+Filename, ImageFormat.Png);
        }

        public void MakeAtlas(string dir)
        {
            if (OutputRects == null) return;

            string text = "";

            text = text
                   + Filename + "\n"
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
                       + "  " + "offset" + 0 + ", " + 0 + "\n"
                       + "  " + "index: " + -1 + "\n";
            }

            File.WriteAllText(dir, text);
        }

        /************************************************************************/
        /* Algorithms															*/
        /************************************************************************/
        public void MyAlogorithm()
        {
            if (OutputRects == null)
                OutputRects = new List<Rect>(InputRects.Count);

            InputRects[0].Position = new Point(0, 0);
            OutputRects.Add(InputRects[0]);

            for (int i = 1; i < InputRects.Count; i++)
            {
                InputRects[i].Position = InputRects[i - 1].Position + new Size(0, InputRects[i-1].Height);
                OutputRects.Add(InputRects[i]);
            }

            RealMaxWidth = OutputRects.Max(t => t.Width);
            RealMaxHeight = OutputRects.Sum(t => t.Height);
        }

        #region 미구현 알고리즘
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

        /************************************************************************/
        /* Tools																*/
        /************************************************************************/
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


        /************************************************************************/
        /* Model																*/
        /************************************************************************/
        public class Rect
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public Point Position { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name + ": " + Width + ", " + Height;
            }

        }

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

        /************************************************************************/
        /* Variables															*/
        /************************************************************************/
        public List<Asset> Assets { get; private set; }
        public List<Rect> InputRects { get; private set; }
        public List<Rect> OutputRects { get; private set; }
        public int RealMaxWidth { get; private set; }
        public int RealMaxHeight { get; private set; }

        private const string Filename = "pack.png";
        private const string FileFormat = "RGBA8888";
        private const string Filter = "Linear,Linear";

        //private Canvas canvas;
        public delegate void AlgorithmDelegate();
        public AlgorithmDelegate AlgorithmRun { get; private set; }


        // For algorithm1
/*        private int _maximumHeight;
        private int _totalWidth;
        private int _totalArea;
        private List<Size> bestSizes;
        private List<double> ratios;*/

        private readonly Log _log = new Log();

    }
}
