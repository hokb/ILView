using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics.ILView {
    public class ILCompletionRequestEventArgs : EventArgs {

        public string Expression;
        public IEnumerable<IILCompletionEntry> CompletionResult;
        public string Prefix = "";
        public bool Success = false;
        public Point Location { get; set; }

        public ILCompletionRequestEventArgs(string expression, Point location) {
            Expression = expression;
            Location = location; 
            CompletionResult = null; 
        }
    }
}
