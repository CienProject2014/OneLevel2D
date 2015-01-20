using System;
using System.Windows.Forms;

namespace OneLevelJson
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form = new MainForm();
            Application.Run(Form);
        }

        public static MainForm Form;

    }
}
