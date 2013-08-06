using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics.ILView {
    public class ILCompletionTextEntry : IILCompletionEntry {

        public string Text { get; set; }

        public void Draw(System.Windows.Forms.ListViewItem itemTarget) {
            itemTarget.SubItems[0].Text = Text; 
        }

        public override string ToString() {
            return Text; 
        }
    }
}
