//using StarFruit.CLI;
//using System.Windows.Forms;

//namespace System.CommandLine.StarFruit.WinForms
//{
//    static class Program
//    {
//        /// <summary>
//        ///  The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main(string[] args)
//        {
//            SetupWinForms();
//            var reflectionParser  = ReflectionParser<TemplateCli>.GetReflectionParser ();
//            var command = reflectionParser.GetCommand();
//            var form = new Form1();
//            var gui = new GuiForm(form);
//            gui.Configure(command);
//            Application.Run(form);
//        }

//        private static void SetupWinForms()
//        {
//            Application.SetHighDpiMode(HighDpiMode.SystemAware);
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//        }
//    }
//}
