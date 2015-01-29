using System;
using System.IO;
using System.Windows.Forms;

namespace OneLevel2D
{
    public class Log
    {
        readonly string _filePath;

        public Log()
        {
            _filePath = @"C:\Users\HAJIN\Documents\Visual Studio 2013\Projects\OneLevel2D\bin\Debug\Logs\" + "[" + DateTime.Now.ToString("HH-mm-ss") + "]" + ".log";

            try
            {
                var fi = new FileInfo(_filePath);

                if (fi.Exists) return;
                using (StreamWriter sw = new StreamWriter(_filePath))
                {
                    sw.WriteLine("LOG START");
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public string GetDateTime()
        {
            DateTime nowDate = DateTime.Now;
            return nowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + nowDate.Millisecond.ToString("000");
        }

        public void Write(string str)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_filePath))
                {
                    string temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                    sw.WriteLine(temp);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
