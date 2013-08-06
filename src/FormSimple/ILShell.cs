using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ILNumerics.ILView {

    public partial class ILShell : RichTextBox, IILShell {

        /* features to be supported: 
         * + Entering commands over single & multiple lines (+)
         * + Copy & Paste commands (multiple lines) (+)
         * + Edit current command (+)
         * + Code completion (on '.' key, 'tab' key)
         * + Syntax highlighting 
         * + Error reporting & highlighting 
         * + Text result reporting (+)
         * + History (+)
         * + Custom commands (Compile, Scene, Example, Tutor, ... )
         */

        /* DISCLAIMER: This code is derived from and uses parts of the following projects: 
         * + ShellControl - A console emulation control; By S. Senthil Kumar, 26 Feb 2005 
         * http://www.codeproject.com/Articles/9621/ShellControl-A-console-emulation-control?msg=2063648
         * It has further been inspired by and copies major parts of the concept of: 
         * + Mono.CSharp.GUI.Shell; Copyright (C) 2006-2008 Novell, Inc., (CSharpShell.cs)
         *        Written by Aaron Bockover <aaron@abock.org>.
         *                   Miguel de Icaza (miguel@gnome.org). 
         */                   

        #region attributes 
        private static readonly string Prompt = "> ";
        private static readonly string InitMessage = @"ILNumerics C# Interactive Console

" + ILShellBaseClass.version + @" 
Type 'help' for instructions! 
-------------------------------------------------------------------------------------
"; 
        private int m_cmdLineStart = 0;
        private ILCommandHistory m_history;
        private int m_curHistoryIdx;
        private StringBuilder m_sbuilder; 
        #endregion

        #region event handlers 
        public event EventHandler<ILCommandEnteredEventArgs> CommandEntered;
        protected void OnCommandEntered(string expression) {
            if (CommandEntered != null) {
                CommandEntered(this, new ILCommandEnteredEventArgs(expression));
            }
        }
        public event EventHandler<ILCompletionRequestEventArgs> CompletionRequested;
        protected void OnCompletionRequested(string expression, Point location) {
            if (CompletionRequested != null && !String.IsNullOrEmpty(expression)) {
                var args = new ILCompletionRequestEventArgs(expression, location); 
                CompletionRequested(this, args);
                if (args.CompletionResult != null && args.CompletionResult.Count() > 0) {
                    ShowCompletions(args);
                } else {
                    HideCompletions();
                }

            }
        }

        
        #endregion

        #region constructors
        public ILShell() {
            InitializeComponent();
            Text = "";
            WordWrap = false; 
            m_history = new ILCommandHistory();
            m_sbuilder = new StringBuilder();
            m_cmdLineStart = -1;
            WriteText(InitMessage); 
        }
        #endregion

        #region public interface
        public void WriteText(string text) {
            // todo: styling
            base.AppendText(text);
            base.ScrollToCaret(); 
        }
        public void WriteCommand(string text) {
            if (InvokeRequired) {
                Invoke((Action<string>)WriteCommand, text); 
            } else {
                StartCommand();
                // todo: syntax highlighting
                AppendCommand(text);
                EndCommand();
            }
        }
        public void StartCommand() {
            // always starts new line 
            // always writes prompt as first line start 
            base.AppendText(Environment.NewLine + Prompt);
            m_cmdLineStart = CmdLineEnd;
            var oldStart = SelectionStart; 
            SelectionStart = oldStart - Prompt.Length; 
            SelectionLength = Prompt.Length; 
            SelectionColor = Color.Black; 
            SelectionLength = 0; 
            SelectionStart = oldStart; 
        }
        public void AppendCommand(string text) {
            // todo: syntax highlighting
            base.AppendText(text);
        }
        public void EndCommand() {
            if (m_cmdLineStart <= CmdLineEnd) {
                var cmd = CurrentCommand;
                m_cmdLineStart = -1; 
                if (!String.IsNullOrEmpty(cmd)) {
                    OnCommandEntered(cmd);
                    m_history.Add(cmd);
                    m_curHistoryIdx = m_history.Count - 1; 
                }
            }
            HideCompletions(); 
        }
        public void WriteError(string text) {
            // todo: error styling
            AppendText(Environment.NewLine);
            text = text.TrimEnd('\r','\n',' ','\t'); 
            int start = SelectionStart;
            text = "Error: " + text; 
            base.AppendText(text);
            
            SelectionStart = start;
			SelectionLength = text.Length;// - Environment.NewLine.Length; 
            SelectionColor = Color.Red;
            HideCompletions(); 
        }

        public string CurrentCommand {
            get {
                if (m_cmdLineStart < 0) return string.Empty;
                m_sbuilder.Clear();
                m_sbuilder.Append(removePrompt(Lines[m_cmdLineStart])); 
                for (int i = m_cmdLineStart + 1; i <= CmdLineEnd; i++) {
                    m_sbuilder.Append(Environment.NewLine + Lines[i]); 
                }
                return m_sbuilder.ToString(); 
            }
        }
        public override string Text {
            get { return base.Text; }
            set {
                if (!String.IsNullOrEmpty(value)) {
                    // convert line endings to this platform new lines 
                    // (not very efficient ... )
                    value = value.Replace("\r\n", "\n"); 
                    var lines = value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); 
                    value = String.Join(Environment.NewLine, lines);
                    base.Text = value;
                }
            }
        }
        public int CursorLinePosition {
            get {
                return GetLineFromCharIndex(SelectionStart);
            }
        }
        public int CursorColumnPosition {
            get {
                var firstIdx = GetFirstCharIndexFromLine(CursorLinePosition);
                return SelectionStart - firstIdx;
            }
        }
        public ListBox CompletionBox {
            get {
                if (Parent != null && Parent is IILCompletionFormProvider) {
                    return (Parent as IILCompletionFormProvider).GetCompletionForm(); 
                }
                return null; 
            }
        }
        #endregion

        #region private helpers
        private void HideCompletions() {
            var box = CompletionBox;
            if (box != null) {
                box.Visible = false;
            }
        }

        public void ShowCompletions(ILCompletionRequestEventArgs args) {
            var box = CompletionBox;
            if (box != null) {
                IEnumerable<IILCompletionEntry> items = args.CompletionResult;
                Point location = args.Location;
                box.Items.Clear();
                foreach (var item in items) {
                    box.Items.Add(args.Prefix + item.ToString());
                }
                box.Location = sizeAndPositionCompletionWindow(box, location);
                box.SelectedIndex = 0;
                box.MouseClick -= box_MouseClick;
                box.MouseClick += box_MouseClick;
                box.Show(); 
                Focus();
            }
        }

        void box_MouseClick(object sender, MouseEventArgs e) {
            ReplaceWord(CompletionBox.SelectedItem.ToString());
            HideCompletions();
        }
        private Point sizeAndPositionCompletionWindow(ListBox box, Point location) {
            // resize
            int numberRows = Math.Min(box.Items.Count, 8) + 1;
            box.Height = box.ItemHeight * numberRows;

            // optimal: right under the caret
            int x = location.X, y = location.Y + Font.Height;
            if (x + box.Width > ClientSize.Width) {
                x = ClientSize.Width - box.Width;
            }
            if (y + box.Height > ClientSize.Height) {
                y = location.Y - box.Height;
            }
            return new Point(x, y);
        }
        private string removePrompt(string p) {
            if (p.StartsWith(Prompt)) {
                return p.Substring(Prompt.Length);
            }
            return p;
        }
        private int CmdLineEnd { get { return Lines.Length - 1; } }
        protected override void OnKeyPress (KeyPressEventArgs e)
		{
			// regular keys -> ensure "in command state"
			if (m_cmdLineStart < 0) {
				StartCommand (); 
			}
			if (e.KeyChar == (char)13) {
				e.Handled = true; 
			}
			base.OnKeyPress(e);
        }
        protected override void OnTextChanged(EventArgs e) {
            base.OnTextChanged(e);
            // handle completions 
            Point location = GetPositionFromCharIndex(SelectionStart);
            OnCompletionRequested(CurrentCommand, location); 
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            KeyDownHandler(e); 
            base.OnKeyDown(e); 
        }

		private void KeyDownHandler(KeyEventArgs e) {
            // handle backspace 
            if (e.KeyCode == Keys.Back) { // || e.KeyCode == Keys.Delete)
                e.Handled = !handleBackKey();
				e.SuppressKeyPress = e.Handled; 
                return; 
            }
            // handle Shift + Enter (multiple command lines 
            if (e.KeyCode == Keys.Enter) {
                if (e.Shift) {
                    e.Handled = true;
                    //e.SuppressKeyPress = true; 
                    AppendCommand(Environment.NewLine);
                    return;
                } else {
                    if (CompletionBox != null && CompletionBox.Visible == true) {
                        ReplaceWord(CompletionBox.SelectedItem.ToString());
                        HideCompletions();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        return;
                    } else {
                        e.Handled = true;
                        //e.SuppressKeyPress = true; 
                        EndCommand();
                        return;
                    }
                }
            } else if (e.KeyCode == Keys.Tab && CompletionBox != null && CompletionBox.Visible == true) {
                ReplaceWord(CompletionBox.SelectedItem.ToString());
                HideCompletions();
                e.Handled = true;
                e.SuppressKeyPress = true;
                return; 
            }

            // Prevent caret from moving before the prompt
            if (e.KeyCode == Keys.Left && IsCaretJustBeforePrompt()) {
                e.Handled = true;
                return; 
            } else if (e.KeyCode == Keys.Down) {
                if (CompletionBox != null && CompletionBox.Visible) {
                    if (CompletionBox.SelectedIndex < CompletionBox.Items.Count - 1) {
                        CompletionBox.SelectedIndex++; 
                    }
                    e.Handled = true;
                } else {
                    HandleHistory(ref m_curHistoryIdx);
                    m_curHistoryIdx++;
                    e.Handled = true;
                }
                return;
            } else if (e.KeyCode == Keys.Up) {
                if (CompletionBox != null && CompletionBox.Visible) {
                    if (CompletionBox.SelectedIndex > 0 && CompletionBox.Items.Count > 0) {
                        CompletionBox.SelectedIndex--;
                    }
                    e.Handled = true; 
                } else {
                    HandleHistory(ref m_curHistoryIdx);
                    m_curHistoryIdx--;
                    e.Handled = true;
                }
                return;
            } else if (e.KeyCode == Keys.Right) {
                return;
            } else if (e.KeyCode == Keys.Home) {
                int currentLineIdx = m_cmdLineStart >= 0 ? m_cmdLineStart : GetLineFromCharIndex(SelectionStart);
                int startCmd = GetFirstCharIndexFromLine(currentLineIdx);
                if (Lines[currentLineIdx].StartsWith(Prompt))
                    startCmd += Prompt.Length; 
                //int endSelect = SelectionStart;
                SelectionStart = startCmd;
                //SelectionLength = endSelect - startCmd; 
                e.Handled = true;
                return;
            } else if (e.Control) {
                if (e.KeyValue == 17) {
                    // Control only 
                    e.Handled = true;
                    return;
                } else if (e.KeyCode == Keys.C) {
                    // copy should not move the carret
                    return; 
                }
            } 
            // handle all common user input, including PASTE event (by keyboard only)

            // If the caret is anywhere else, set it back when a key is pressed.
            if (!IsCaretAtWritablePosition()) {
                MoveCaretToEndOfText();
            }
        }

        protected override bool IsInputKey(Keys keyData) {
            if (keyData == Keys.Tab || keyData == (Keys.Shift | Keys.Tab)) return true;
            return base.IsInputKey(keyData);
        }
        private void ReplaceWord(string word) {
            int start, end = start = SelectionStart; 
            int cmdStart = GetCurrentCommandStartCharIdx(); 
            if (cmdStart < 0) return; 
            string txt = Text;
            //if (start >= txt.Length) start = txt.Length - 1; 
            while (start > cmdStart && (char.IsLetterOrDigit(txt[start-1]) || '_' == txt[start-1])) {
                start--; 
            }
            SelectionStart = start;
            SelectionLength = end - start + 1;
            SelectedText = word;
            
        }

        private void HandleHistory (ref int p)
		{
			// simplyfied history variant: do not recognize partial texts manually entered by user
			if (m_history.Count == 0) return; 
			if (p < 0) 
				p = 0; 
			if (p >= m_history.Count) {
				p = m_history.Count - 1; 
			}
			// replace full command with history entry 
			if (m_cmdLineStart < 0) StartCommand(); 
			int charIdxStart = GetCurrentCommandStartCharIdx(); 
			int charIdxEnd = Text.Length-1; 
			if (charIdxEnd < charIdxStart) charIdxEnd = charIdxStart; 
			base.SelectionStart = charIdxStart; 
			base.SelectionLength = charIdxEnd - charIdxStart + 1; 
			base.SelectedText = m_history[p]; 
        }

		int GetCurrentCommandStartCharIdx ()
		{
			if (m_cmdLineStart < 0)
				return 0; 
			int startCharIdx = GetFirstCharIndexFromLine(m_cmdLineStart); 
			return startCharIdx + Prompt.Length; 
		}

        private bool handleBackKey() {
            var c = CursorColumnPosition;
            if (CursorLinePosition == m_cmdLineStart) {
                return c > Prompt.Length;
            } else {
                return true; 
            }
        }
        // Overridden to protect against deletion of contents
        // cutting the text and deleting it from the context menu
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case 0x0302: //WM_PASTE
                case 0x0300: //WM_CUT
                case 0x000C: //WM_SETTEXT
                    if (!IsCaretAtWritablePosition())
                        MoveCaretToEndOfText();
                    break;
                case 0x0303: //WM_CLEAR
                    return;
            }
            base.WndProc(ref m);
        }
        private bool IsTerminatorKey(Keys key) {
            return key == Keys.Enter;
        }
        private bool IsTerminatorKey(char keyChar) {
            return ((int)keyChar) == 13;
        }

        private bool IsCaretJustBeforePrompt() {
            return IsCaretInCmd() && 
				(CursorColumnPosition == Prompt.Length && CursorLinePosition == m_cmdLineStart);
        }

        private void MoveCaretToEndOfText() {
            this.SelectionStart = this.TextLength;
            this.ScrollToCaret();
        }
        private bool IsCaretAtWritablePosition() {
            return IsCaretInCmd() && CursorColumnPosition >= Prompt.Length;
        }
        private bool IsCaretInCmd() {
            int clp = CursorLinePosition;
            if (clp > m_cmdLineStart)
                return true;
            else if (clp == m_cmdLineStart) {
                return CursorColumnPosition >= Prompt.Length;
            } 
            return false; 
        }
        private string GetCurrentLine() {
            int lineIdx = GetLineFromCharIndex(SelectionStart);
            if (lineIdx >= 0 && lineIdx < Lines.Length && Lines.Length > 0)
                return Lines[lineIdx];
            else
                return "";
        }

        protected void SyntaxHighlightCmd(int start, int length) {
            // todo
        }
        #endregion

        public void Init(object host) {
            
        }
    }

}
