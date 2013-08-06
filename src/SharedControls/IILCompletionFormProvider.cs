using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ILNumerics.ILView {
    public interface IILCompletionFormProvider {
        // todo: abstract away listbox! replace with interface
        ListBox GetCompletionForm(); 

    }
}
