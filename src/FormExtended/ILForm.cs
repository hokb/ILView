using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;
using Mono.CSharp;
using System.IO;
using ICSharpCode.TextEditor.Document;

namespace ILNumerics.ILView {
    partial class ILForm : Form {

        #region attributes
        private string m_source;
        public static string ilc_requestURL = @"http://ilnumerics.net/ilcf/{0}/{1}/in.txt";
        private static string defaultSourceTextBoxText = "(enter source URI/ ILC# here or choose from list ...)"; 
        private ILShellControl m_shell;
        private Action m_cleanUpExample;
        private Evaluator m_evaluator;
        private StringBuilder m_errorStream;
        #endregion

        #region properties

        public Evaluator Evaluator {
            get { 
                if (m_evaluator == null) 
                    resetEvaluator(); 
                return m_evaluator; 
            }
        }
        public StringBuilder ErrorStream {
            get {
                if (m_errorStream == null) {
                    m_errorStream = new StringBuilder(); 
                }
                return m_errorStream; 
            }
        }


        /// <summary>
        /// The source for the initial state of the viewer
        /// </summary>
        /// <remarks><para>Allowed values are: a uri string, C# code with a valid szene definition, empty string.
        /// </para>
        /// <list type="bullet">
        /// <item>URI: the uri is expected to reference a file location with a valid scene definition (C# code).</item>
        /// <item>C# Code: a valid C# code snippet creating a ILScene. Valid snippets are syntactically correct and end 
        /// with a statement, returning the scene object. One example of a valid C# code snippet which creates a simple 
        /// scene with a sphere in 3D: 
        /// <code>var scene = new ILScene { Camera = { new ILSphere() } }; 
        /// scene;</code>
        /// Note that the following namespaces are automatically included, hence no <c>using</c> statements are necessary for classes in them: 
        /// <c>System; System.Drawing; System.Collections.Generic; System.Linq; ILNumerics; ILNumerics.Drawing; ILNumerics.Drawing.Plotting;</c>
        /// </item>
        /// <item>Empty String: the ILNumerics example will be created. This shows a scene with three subpplots 
        /// with several static and dynamic features and plots.</item>
        /// </list></remarks>
        public string Source {
            get { return m_source; }
            set { 
                m_source = value; 
                LoadSzene(m_source); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ILPanel Panel {
            get { 
                //var doc = dockPanel1.Documents.ElementAt(0) as ILMainContent; 
                //if (doc != null) 
                //    return doc.Panel;
                return null; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ILShellControl Shell {
            get { return m_shell; } 
        }
        #endregion

        #region constructors
        public ILForm() {
            InitializeComponent();
            // setup source combo box
            // fill couple of interesting sources
            cmbSources1.Items.Add("id0b426");
            cmbSources1.Items.Add("if920ce");
            cmbSources1.Items.Add("id8c59a");
            cmbSources1.Items.Add("icd3109");
            cmbSources1.Items.Add("i3748c4");
            cmbSources1.Items.Add("ib5b26c");
            cmbSources1.Items.Add("i8951b3");
            cmbSources1.Items.Add("i794c2c");
            cmbSources1.Items.Add("i461339");
            cmbSources1.Items.Add("i9b1f80");
            cmbSources1.Items.Add("ia3fcd0");
            cmbSources1.Items.Add("i57d081");
            cmbSources1.Items.Add("i77c8ac");
            cmbSources1.Items.Add("i9481ea");
            cmbSources1.Items.Add("i5a1dc1");
            cmbSources1.Items.Add("i24bd56");
            cmbSources1.Items.Add("example"); 

            ILMain mainContent = new ILMainContent(); 
            mainContent.Show(dockPanel1, dockState: DockState.Document); 
            m_shell = new ILShellControl();
            Shell.Show(dockPanel1, dockState: DockState.DockTopAutoHide);
            Shell.CommandEntered += shell_Evaluate;
            Shell.Text = "C# Interactive Console";

            //Panel.Driver = RendererTypes.GDI;

        }

        void shell_Evaluate(object sender, ILCommandEnteredEventArgs e) {
            Evaluate(e.Command); 
        }
        #endregion
        private void resetEvaluator() {
            m_errorStream = new StringBuilder();
            //StreamReader sr = 
            TextWriter tw = new StringWriter(m_errorStream);
            var ctx = new Mono.CSharp.CompilerContext(
                new Mono.CSharp.CompilerSettings() {
                    AssemblyReferences = new List<string>() { 
                            typeof(ILMath).Assembly.FullName, 
                            typeof(System.Drawing.PointF).Assembly.FullName,
                            typeof(System.Linq.Queryable).Assembly.FullName
                        },
                    //Unsafe = true
                }, new StreamReportPrinter(tw));

            var eval = new Mono.CSharp.Evaluator(ctx);
            // reset line colors (thread safe)
            ILNumerics.Drawing.Plotting.ILLinePlot.NextColors = new ILColorEnumerator();

            string m_head = @"
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq; 
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;";

            eval.Run(m_head);
            m_evaluator = eval; 
        }

        #region public interface 
        public void Evaluate(string expression) {
            try {
                object result; 
                bool result_set;
                ErrorStream.Clear(); 
                string ret = this.Evaluator.Evaluate(expression, out result, out result_set);
                if (result_set) {
                    if (result is ILScene) {
                        Panel.Scene = (result as ILScene); 
                    } else {
                        m_shell.WriteText(Environment.NewLine);
                        m_shell.WriteText(result.ToString());
                    }
                } else if (ErrorStream.Length > 0) {
                    m_shell.WriteError(ErrorStream.ToString()); 
                }
                Panel.Configure();
                Panel.Refresh();
            } catch (Exception exc) {
                m_shell.WriteError(exc.ToString()); 
            }
        }

        public static bool URLFromILCode(string ilcode, out string url) {
            url = "";
            // match whole words only
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^i([a-z0-9]{2})([a-z0-9$]+)$");
            var matches = reg.Match(ilcode);
            url = "";
            if (matches.Success && matches.Groups != null && matches.Groups.Count > 2) {
                url = String.Format(ilc_requestURL, matches.Groups[1].Value, matches.Groups[2]);
                return true;
            }
            return false; 
        }

        public void LoadSzene(string source) {
            Uri uri; 
            string expression = ""; 
            // is ilc code? 
            string url = "";
            if (URLFromILCode(source, out url)) {
                source = url;
            }
            // ... for example
            try {
                if (source.ToLower() == "example") {
                    Trace.WriteLine("Showing ILNumerics Example ...");
                    SetExampleScene(Panel);
                    Panel.Configure(); 
                    // show selected driver and framerate in forms title bar
                    Panel.EndRenderFrame -= registerShowFrameInfo;
                    Panel.EndRenderFrame += registerShowFrameInfo;
                    // we have a dynamic scene, start the clockx
                    Panel.Clock.Running = true;
                    cmbSources1.Text = defaultSourceTextBoxText;
                    return; 
                }
            } catch (Exception exc) {
                Trace.WriteLine("Failure:");
                Trace.WriteLine(exc.ToString());
            }

            #region is URI?
            try {
                uri = new Uri(source);
                if (uri.IsFile) {
                    Trace.WriteLine("Loading example instance from: " + uri.AbsolutePath);
                    expression = System.IO.File.ReadAllText(uri.AbsolutePath);
                } else {
                    Trace.WriteLine("Fetching example instance from: " + uri);
                    var web = new System.Net.WebClient();
                    expression = web.DownloadString(uri);
                }
            } catch (ArgumentNullException) {
                expression = source; 
            } catch (UriFormatException) {
                expression = source; 
            } catch (System.Net.WebException wexc) {
                MessageBox.Show(wexc.ToString());
            }
            #endregion

            #region is C# Code?
            try {
                Trace.WriteLine("Evaluating Expression: ");
                Trace.WriteLine(expression);
                if (m_cleanUpExample != null) {
                    m_cleanUpExample();
                    m_cleanUpExample = null; 
                }

                //Evaluate(expression);
                if (!string.IsNullOrEmpty(source) && source != expression) {
                    cmbSources1.Text = source; 
                } else { 
                    cmbSources1.Text = defaultSourceTextBoxText;    
                }
                Shell.WriteCommand(expression); 
                return; 
            } catch (Exception exc) {
                Trace.WriteLine("Failure: ");
                Trace.WriteLine(exc.ToString());
            }
            #endregion
        }
        #endregion

        #region private helpers

        private void registerShowFrameInfo(object sender, ILRenderEventArgs args) {
            string txt = "ILView Example --- Driver: " + Panel.Driver.ToString();
            if (Panel.Driver == RendererTypes.OpenGL && Settings.OpenGL31_FIX_GL_CLIPVERTEX) {
                txt += " (Fix)";
            }
            txt += "  FPS: " + Panel.FPS.ToString();
            if (Text != txt) {
                Text = txt; // show in title + log
                System.Diagnostics.Trace.WriteLine("Render Frame:" + Text);
            }
        }

        private void SetExampleScene(ILPanel panel) {
            ILScene scene = new ILScene();
            try {
                ILLabel.DefaultFont = new System.Drawing.Font("Helvetica", 8);
                //ilPanel1.Driver = RendererTypes.GDI; 

                #region upper left plot
                // prepare some data 
                ILArray<float> P = 1,
                    x = ILMath.linspace<float>(-2, 2, 40),
                    y = ILMath.linspace<float>(2, -2, 40);

                ILArray<float> F = ILMath.meshgrid(x, y, P);
                // a simple RBF
                ILArray<float> Z = ILMath.exp(-(1.2f * F * F + P * P));
                // surface expects a single matrix
                Z[":;:;2"] = F; Z[":;:;1"] = P;

                // add a plot cube 
                var pc = scene.Add(new ILPlotCube {
                    // shrink viewport to upper left quadrant
                    ScreenRect = new RectangleF(0.05f, 0, 0.4f, 0.5f),
                    // 3D rotation
                    TwoDMode = false,
                    Children = { 
                        // add surface 
                        new ILSurface(Z) {
                            // disable mouse hover marking
                            Fill = { Markable = false },
                            Wireframe = { Markable = false },
                            // make it shiny
                            UseLighting = true,
                            Children = { new ILColorbar() }
                        }, 
                        //ILLinePlot.CreateXPlots(Z["1:10;:;0"], markers: new List<MarkerStyle>() { 
                        //    MarkerStyle.None,MarkerStyle.None,MarkerStyle.None,MarkerStyle.None,MarkerStyle.Circle, MarkerStyle.Cross, MarkerStyle.Plus, MarkerStyle.TriangleDown }),
                        //new ILLegend("hi","n","ku","zs","le", "blalblalblalblalb\\color{red} hier gehts rot")
                    },
                    Rotation = Matrix4.Rotation(new Vector3(1.1f, -0.4f, -0.69f), 1.3f)
                });

                #endregion

                #region top right plot
                // create a gear shape 
                var gear = new ILGear(toothCount: 30, inR: 0.5f, outR: 0.9f) {
                    Fill = { Markable = false, Color = Color.DarkGreen }
                };
                // group with right clipping plane
                var clipgroup = new ILGroup() {
                    Clipping = new ILClipParams() {
                        Plane0 = new Vector4(1, 0, 0, 0)
                    },
                    Children = {
                            // a camera holding the (right) clipped gear
                            new ILCamera() {
                                // shrink viewport to upper top quadrant
                                ScreenRect = new RectangleF(0.5f,0,0.5f,0.5f),
                                // populate interactive changes back to the global scene
                                IsGlobal = true,
                                // adds the gear to the camera
                                Children = { gear },
                                Position = new Vector3(0,0,-15)
                            }
                        }
                };
                // setup the scene 
                var gearGroup = scene.Add(new ILGroup {
                    clipgroup, clipgroup // <- second time: group is cloned
                });

                gearGroup.First<ILCamera>().Parent.Clipping = new ILClipParams() {
                    Plane0 = new Vector4(-1, 0, 0, 0)
                };
                // make the left side transparent green
                gearGroup.First<ILTriangles>().Color = Color.FromArgb(100, Color.Green);

                // synchronize both cameras; source: left side 
                gearGroup.First<ILCamera>().PropertyChanged += (s, arg) => {
                    gearGroup.Find<ILCamera>().ElementAt(1).CopyFrom(s as ILCamera, false);
                };
                #endregion

                #region left bottom plot
                // start value
                int nrBalls = 10; bool addBalls = true;
                var balls = new ILPoints("balls") {
                    Positions = ILMath.tosingle(ILMath.randn(3, nrBalls)),
                    Colors = ILMath.tosingle(ILMath.rand(3, nrBalls)),
                    Color = null,
                    Markable = false
                };
                var leftBottomCam = scene.Add(new ILCamera {
                    ScreenRect = new RectangleF(0, 0.5f, 0.5f, 0.5f),
                    Projection = Projection.Perspective,
                    Children = { balls }
                });
                // funny label
                string harmony = @"\color{red}H\color{blue}a\color{green}r\color{yellow}m\color{magenta}o\color{cyan}n\color{black}y\reset
";
                var ballsLabel = scene.Add(new ILLabel(tag: "harmony") {
                    Text = harmony,
                    Fringe = { Color = Color.FromArgb(240, 240, 240) },
                    Position = new Vector3(-0.75f, -0.25f, 0)
                });
                long oldFPS = 1;
                PointF currentMousePos = new PointF();
                // setup the swarm. Start with a few balls, increase number 
                // until framerate drops below 60 fps. 
                ILArray<float> velocity = ILMath.tosingle(ILMath.randn(3, nrBalls));
                EventHandler<ILRenderEventArgs> updateBallsRenderFrame = (s, arg) => {
                    // transform viewport coords into 3d scene coords
                    Vector3 mousePos = new Vector3(currentMousePos.X * 2 - 1,
                                                    currentMousePos.Y * -2 + 1, 0);
                    // framerate dropped? -> stop adding balls
                    if (panel.FPS < oldFPS && panel.FPS < 60) addBalls = false;
                    oldFPS = panel.FPS;
                    Computation.UpdateBalls(mousePos, balls, velocity, addBalls);
                    // balls buffers have been changed -> must call configure() to publish
                    balls.Configure();
                    // update balls label
                    ballsLabel.Text = harmony + "(" + balls.Positions.DataCount.ToString() + " balls)";
                };

                // saving the mouse position in MouseMove is easier for 
                // transforming the coordinates into the viewport
                leftBottomCam.MouseMove += (s, arg) => {
                    // save the mouse position 
                    currentMousePos = arg.LocationF;
                };
                panel.BeginRenderFrame += updateBallsRenderFrame;
                m_cleanUpExample = () => {
                    leftBottomCam.MouseMove -= (s, arg) => {
                        // save the mouse position 
                        currentMousePos = arg.LocationF;
                    };
                    panel.BeginRenderFrame -= updateBallsRenderFrame; 
                };
                #endregion

                panel.Scene = scene; 

            } catch (Exception exc) {
                System.Diagnostics.Trace.WriteLine("ILPanel_Load Error:");
                System.Diagnostics.Trace.WriteLine("====================");
                System.Diagnostics.Trace.WriteLine(exc.ToString());
                MessageBox.Show(exc.ToString());
            }
        }

        private class Computation : ILMath {
            public static void UpdateBalls(Vector3 center, ILPoints balls, ILOutArray<float> velocity, bool addBalls) {
                using (ILScope.Enter()) {
                    ILArray<float> position = balls.Positions.Storage;
                    ILArray<float> colors = balls.Colors.Storage;
                    if (addBalls) {
                        // increase number of balls (this is very inefficient!)
                        position[full, r(end + 1, end + 10)] = tosingle(randn(3, 10));
                        colors[full, r(end + 1, end + 10)] = tosingle(rand(4, 10));
                        velocity[full, r(end + 1, end + 10)] = tosingle(randn(3, 10));
                    }
                    ILArray<float> d = array<float>(center.X, center.Y, center.Z);
                    ILArray<float> off = position * 0.1f;
                    ILArray<float> dist = sqrt(sum(position * position));
                    ILArray<int> where = find(dist < 0.2f);
                    velocity[full, where] = velocity[full, where]
                                                + tosingle(rand(3, where.Length)) * 0.2f;
                    dist.a = position - d;
                    off.a = off + dist * -0.02f / sqrt(sum(dist * dist));
                    velocity.a = velocity * 0.95f - off;
                    balls.Positions.Update(position + velocity * 0.12f);
                    ILArray<float> Zs = abs(position[end, full]);

                    colors[end, full] = Zs / maxall(Zs) * 0.7f + 0.3f;
                    balls.Colors.Update(colors);
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                Source = cmbSources1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                Source = cmbSources1.Text;
            } catch (Exception exc) {
                Trace.WriteLine("Error selecting new scene: ");
                Trace.WriteLine(exc.ToString()); 
                MessageBox.Show(exc.ToString()); 
            }
        }
        #endregion
    }
}
