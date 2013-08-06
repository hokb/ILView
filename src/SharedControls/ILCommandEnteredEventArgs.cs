using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics {

    public class ILCommandEnteredEventArgs : EventArgs {
        /// <summary>
        /// The command text which has been entered into the shell. This may consist out of multiple lines.
        /// </summary>
        public string Command { get; internal set; }

        public ILCommandEnteredEventArgs(string command) {
            Command = command; 
        }
    }

}
