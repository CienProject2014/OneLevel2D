using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;

namespace OneLevelJson
{
    class TexturePacker
    {
        private List<Rect> rects;
        private Log log= new Log();

        public void LoadAssets(List<Asset> assets)
        {
            if(rects == null)
                rects = new List<Rect>(assets.Count);

            foreach (var asset in assets)
            {
                rects.Add(new Rect()
                {
                    Width = asset.ImageSize.Width,
                    Height = asset.ImageSize.Height,
                    Name = asset.Name
                });
            }
        }

        public void Sort()
        {
            if (rects == null) return;

            //(a, b) => a.Width.CompareTo(b.Width); // width ascend
            //(a, b) => a.Height.CompareTo(b.Height);   // height ascend

            rects.Sort((a, b) => b.Height.CompareTo(a.Height)); // descend

            foreach (var rect in rects)
            {
                log.Write(rect.ToString());
            }
        }

        public void Pack()
        {
            Image img = new Bitmap("Asdf");
            img.Save("asdf", ImageFormat.Png);
        }
 
        class Rect
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name + ": " + Width + ", " + Height;
            }
        }

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
    }
}
