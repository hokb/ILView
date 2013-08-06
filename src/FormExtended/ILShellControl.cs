using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ILNumerics.ILView {
    public partial class ILShellControl : DockContent, IILShellControl {
        
        #region attributes

        #endregion

        #region event handlers
        public event EventHandler<ILCommandEnteredEventArgs> CommandEntered;
        protected void OnCommandEntered(string expression) {
            if (CommandEntered != null) {
                CommandEntered(this, new ILCommandEnteredEventArgs(expression));
            }
        }
        protected void OnCommandEntered(ILCommandEnteredEventArgs args) {
            if (CommandEntered != null) {
                CommandEntered(this, args);
            }
        }
        #endregion

        #region constructors

        public ILShellControl() {
            InitializeComponent();
        }
        #endregion

        #region public interface 
        public void WriteText(string text) {
            ilShell1.WriteText(text); 
        }
        public void WriteCommand(string text) {
            ilShell1.WriteCommand(text); 

        }
        public void WriteError(string text) {
            ilShell1.WriteError(text); 
        }
        public void Init(object host) {

        }
        #endregion

        #region private helpers
        private void ilShell1_CommandEntered(object sender, ILCommandEnteredEventArgs e) {
            OnCommandEntered(e);
        }
        #endregion
    }
}
