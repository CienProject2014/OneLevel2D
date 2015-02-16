using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienLabel : CienBaseComponent
    {
        /* Variables ************************************************************/
        // should be public for Serialization/Deserialization
        public string Text { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public int Align { get; set; }
        [JsonIgnore]
        public Font FontData { get; set; }
        /************************************************************************/

        public CienLabel(string text, int fontSize, string fontName, List<float> tint, string id, int zIndex,
            string layerName = CienDocument.DefaultLayerName)
        {
            
            Text = text;
            FontSize = fontSize;
            FontName = fontName;
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
                           FontName + ".ttf";
            var myFonts = new PrivateFontCollection();

            if (!File.Exists(fileName))
            {
                MessageBox.Show(@"파일이 없습니다. ");
                return;
            }

            myFonts.AddFontFile(fileName); //we add the full path of the ttf file

            if (GetFont(myFonts.Families[0], FontStyle.Regular) != null)
            {
                FontData = GetFont(myFonts.Families[0], FontStyle.Regular);
            }
            else if (GetFont(myFonts.Families[0], FontStyle.Bold) != null)
            {
                FontData = GetFont(myFonts.Families[0], FontStyle.Bold);
            }
            else if (GetFont(myFonts.Families[0], FontStyle.Italic) != null)
            {
                FontData = GetFont(myFonts.Families[0], FontStyle.Italic);
            }
            else if (GetFont(myFonts.Families[0], FontStyle.Bold | FontStyle.Italic) != null)
            {
                FontData = GetFont(myFonts.Families[0], FontStyle.Bold | FontStyle.Italic);
            }
            else
            {
                MessageBox.Show(@"Regular, Bold, Italic이 존재하지 않는 폰트입니다.");
            }

            myFonts.Dispose();
        }

        private Font GetFont(FontFamily fontFamily, FontStyle fontStyle)
        {
            Font font;

            try
            {
                font = new Font(fontFamily, FontSize, fontStyle);
            }
            catch
            {
                font = null;
            }

            return font;
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
            return Color.FromArgb(a, r, g, b);
        }
    }
}
