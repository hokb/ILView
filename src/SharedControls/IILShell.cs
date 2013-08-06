using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics.ILView {
    public interface IILShell {
        event EventHandler<ILCommandEnteredEventArgs> CommandEntered;
        event EventHandler<ILCompletionRequestEventArgs> CompletionRequested;
        //event EventHandler<EventArgs> CompletionsHide; 
        void WriteCommand(string text);
        void WriteError(string text);
        void WriteText(string text);
        void Init(object host); 
    }
}
