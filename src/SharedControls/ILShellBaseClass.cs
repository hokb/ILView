using ILNumerics.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics.ILView {
    /// <summary>
    /// This class provides the static class context for all expressions / classes in the interactive shell. Extend this class with new commands available in the shell!
    /// </summary>
    public class ILShellBaseClass : ILMath {

        public static string help {
            get {
                return @"
ILNumerics Interactive C# Shell
Version: " + version + ", Date: " + DateTime.Now.ToString() + @"
__________________________________________________________
Help overview: 

General shell handling. Enter expressions at the command prompt. 
Evaluate by pressing 'Enter'. Text output is displayed immediately:
> 1 + 2 [Enter]
3

Common variable declaration rules apply. All static functions from 
ILNumerics.ILMath are directly accessible: 
> ILArray<double> A = rand(3,2); 

Variables are retained between command invocations: 
 > (A + 3) * 2
<Double> [3,2]
   7,25088    7,02224 
   7,17780    7,03576 
   6,06555    7,97830
 
If an object of type ILNumerics.Drawing.ILScene is returned by an 
expression, it replaces the current scene in the panel of the main window:
> new ILScene { Camera = { new ILSphere() } }
(... replaces existing visual with new scene)

The panel is accessed by 'Panel': 
> Panel.BackColor = Color.Gray; 
> Panel.Scene.First<ILTriangles>().Color = Color.White; 
(... colors the background and the fill of the shphere )

The shell accepts all valid C# constructs. Lambda expressions are compiled on 
the fly: 
> Func<double,double> cubic = d => d * d * d; 
> cubic(3)
27

Multiple lines are spanned with [Shift]+[Enter]: 
> class myPoints : ILPoints { 
    public myPoints() {
        Positions = ILMath.tosingle(ILMath.rand(3,100));
        Color = System.Drawing.Color.Red; 
    }
}
Panel.Scene.Camera.Add(new myPoints())
(... adds 100 random red points to the current scene)

Misc: 
____________________________________________________________
* Errors are reported in red with line and column number and description. 
* History of commands: [CURSOR UP] and [CURSOR DOWN]
* version - displays versions: [OS]/[ILView]/[ILNumerics]
* loadscene(source) - loads a scene and replaces existing scene
                      valid sources: ILC# code: loadscene(""if920ce"") 
* help - displays this help text

Read the full documentation on ILNumerics at: 
http://ilnumerics.net/docu.html
";
            }
        }

        private static string getVersion() {
            try { 
                var versionString = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                
                return versionString; 
            } catch (Exception exc) { //todo: be more specific here! 
                return "(unknown)"; 
            }
        }
        /// <summary>
        /// The panel currently selected as target for interactive scene drawing
        /// </summary>
        public static ILPanel Panel { get; set; }

        public static ILView View { get; set; }

        public static string version { 
            get {
                try {
                    var ilviewVers = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                    var ilnumerics = typeof(ILMath).Assembly.GetName().Version.ToString();

                    var ret = String.Format("ILView: {0} - ILNumerics: {1} - OS: {2}", ilviewVers, ilnumerics, Environment.OSVersion.VersionString);
                    return ret; 
                } catch (Exception exc) { //todo: be more specific here! 
                    return "(unknown)";
                }
            } 
        }
        /// <summary>
        /// Load a new scene from ILC#, URI to C# source code or by C# code
        /// </summary>
        /// <param name="source">source to create scene from</param>
        public static void loadscene(string source) {
            if (View != null) {
                View.LoadSzene(source); 
            }
        }

    }
}
