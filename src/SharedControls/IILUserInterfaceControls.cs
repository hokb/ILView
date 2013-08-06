using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics.ILView {

    public class SourceChangedEventArgs : EventArgs {
        public string Source;
    }
    public class ShellVisibleChangedEventArgs : EventArgs {
        public bool Visible;
    }
    
    public interface IILUserInterfaceControls {

        event EventHandler<ShellVisibleChangedEventArgs> ShellVisibleChanged;
        event EventHandler ExportSVG;
        event EventHandler ExportPNG;
        event EventHandler<SourceChangedEventArgs> SourceChanged; 

        // todo: add more UI control abstractions here if needed ....
    }
}
