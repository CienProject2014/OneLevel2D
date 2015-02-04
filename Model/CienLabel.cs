using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienLabel : CienBaseComponent
    {
        /* Variables ************************************************************/
        // should be public for Serialization/Deserialization
        public string Text { get; set; }
        public string Style { get; set; }
        public int FontSize { get; set; }
        public int Align { get; set; }
        [JsonIgnore]
        public Font FontData { get; set; }
        /************************************************************************/

        public CienLabel(string text, int fontSize, string style, List<float> tint, string id, int zIndex,
            string layerName = CienDocument.DefaultLayerName)
        {
            
            Text = text;
            FontSize = fontSize;
            Style = style;
            Align = 0;

            // make FontData
            LoadFont();

            Id = id;
            Location = new Point(0, 0);
            ZIndex = zIndex;
            LayerName = layerName;
            Tint = new List<float>(tint);
        }

        public override string ToString()
        {
            return Id + " " + Text + " " + FontSize;
        }

        public override object Clone()
        {
            return null;
        }

        public override Size GetSize()
        {
            return TextRenderer.MeasureText(Text, FontData);
        }

        private void LoadFont()
        {
            var fileName = MainForm.ProjectDirectory + @"\" + CienDocument.Name + MainForm.FontDirectory + @"\" +
                           Style + ".ttf";
            var myFonts = new PrivateFontCollection();

            if (File.Exists(fileName))
            {
            }
            else
            {
                MessageBox.Show(@"파일이 없습니다. ");
                return;
            }

            FontStyle fontStyle;
            Enum.TryParse(Style, out fontStyle);
            try
            {
                myFonts.AddFontFile(fileName);//we add the full path of the ttf file
                FontData = new Font(myFonts.Families[0], FontSize, FontStyle.Regular);            // TODO 현재는 Regular 스타일만 쓸 수 있음.
            }
            catch (IOException e)
            {
                MessageBox.Show(@"파일을 읽을 수 없습니다. " + e.Message);
            }
        }

        public override Image GetImage()
        {
            Size pixelSize = GetSize();
            Color color = TintToColor();
            
            Bitmap bitmap = new Bitmap(pixelSize.Width, pixelSize.Height);
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.DrawString(Text, FontData, new SolidBrush(color), new PointF(0,0));
            }

            return bitmap;
        }

        private Color TintToColor()
        {
            var r = (int)(Tint[0] * 255);
            var g = (int)(Tint[1] * 255);
            var b = (int)(Tint[2] * 255);
            var a = (int)(Tint[3] * 255);
            return Color.FromArgb(r, g, b, a);
        }
    }
}
