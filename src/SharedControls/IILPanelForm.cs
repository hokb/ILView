using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILNumerics; 
using ILNumerics.Drawing; 

namespace ILNumerics.ILView {
    public interface IILPanelForm {
        ILPanel Panel { get; }
        string Text { get; set; }

        event EventHandler Closed;
        event EventHandler Load; 
    }
}
