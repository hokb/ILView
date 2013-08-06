using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using Mono;
using Mono.CSharp;

namespace ILNumerics.ILView {

    class Program {
        private static string ilc_instanceRegexp = @"_i([a-z0-9]{2})([a-z0-9$]+)\.exe";
        private static string ErrorlogName = "ILNumerics_Errorlog.txt"; 
        private static string[] s_commandLineArgs; 
        public static string ExeName {
            get {
                try {
                    return (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                } catch (System.Security.SecurityException) {
                    return String.Empty;
                }
            }
        }
        
        [STAThread]
        static void Main(string[] args) {
            s_commandLineArgs = args; 
            initializeLogging(); 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // for now, we create the most simple version of IILPanelForm and IILShellControl and instantiate the ILView with them
            var mainForm = new ILMainFormSimple(); 
            ILView view = new ILView(mainForm, new ILShellForm(), mainForm);
            view_Load(view,EventArgs.Empty);
            Application.Run(view);
        }

        static void view_Load(object sender, EventArgs e) {
            string url;
            string exePath;
            ILView view = sender as ILView; 
            // from command parameters ? 

#if DEBUG
//            s_commandLineArgs = new string[] { @"var scene = new ILScene() {
//    Camera = { 
//        new ILSphere() 
//    }
//};
//scene;" 
//            }; 
#endif

            if (s_commandLineArgs != null && s_commandLineArgs.Length > 0 && !String.IsNullOrEmpty(s_commandLineArgs[0])) {
                try {
                    string expression = s_commandLineArgs[0];
                    // evaluate expression
                    Console.WriteLine("Evaluating Expression from Command Line Input: ");
                    Console.WriteLine(expression);
                    view.Source = expression;
                    return; 
                } catch (Exception exc) {
                    Trace.WriteLine(exc.ToString());
                    MessageBox.Show(exc.ToString());
                }
            }
            // from exe name ? 
            try {
                if (!String.IsNullOrEmpty(ExeName) && urlFromExeName(ExeName, out url)) {
                    view.Source = url;
                    return;
                }
            } catch (System.Security.SecurityException exc) { }
            view.Source = "example";  // shows the common example
        }

        private static void initializeLogging() {
            Trace.Listeners.Add(new ConsoleTraceListener()); 
            // register new trace listener
            if (System.IO.File.Exists(ErrorlogName)) System.IO.File.Delete(ErrorlogName);
            Trace.Listeners.Add(new TextWriterTraceListener(ErrorlogName, "ILNumerics"));
            AppDomain.CurrentDomain.ProcessExit += (s, arg) => { Trace.Close(); };
            Trace.WriteLine("Starting Trace at " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            Trace.WriteLine("======================================================");
            Trace.WriteLine("");
        }

        private static bool urlFromExeName(string exename, out string url) {
#if DEBUG
            //exename = "ilviewer_ibad83e.exe";
#endif
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"_i([a-z0-9]{2})([a-z0-9$]+).exe");
            var matches = reg.Match(exename);
            url = "";
            if (matches.Success && matches.Groups != null && matches.Groups.Count > 2) {
                url = String.Format(ILView.ilc_requestURL, matches.Groups[1].Value, matches.Groups[2]);
                return true;
            }
            return false;
        }

    }

}
