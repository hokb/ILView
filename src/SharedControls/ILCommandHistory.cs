using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILNumerics {
    class ILCommandHistory {

        #region attributes
        
        #endregion

        #region properties
        List<string> Commands { get; set; }
        #endregion

        #region constructor
        public ILCommandHistory() {
            Commands = new List<string>(); 
        }
        #endregion

        #region public interface 
        public void Add(string command) {
            command = command.Trim();
            // only compare with latest command, prevent duplicates
            if (Commands.Count == 0 || Commands[Commands.Count - 1] != command) {
                Commands.Add(command); 
            }
        }
        public string this[int index] {
            get {
                if (index >= 0 && index < Commands.Count) {
                    return Commands[index]; 
                }
                return string.Empty; 
            }
        }
        public int Count {
            get { return Commands.Count; }
        }
        public string LastCommand { 
            get { return this[Count - 1]; } 
        }
        #endregion

    }
}
