namespace ILNumerics.ILView {
    partial class ILShellForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ilShell1 = new ILNumerics.ILView.ILShell();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Courier New", 10F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(361, 176);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 84);
            this.listBox1.TabIndex = 1;
            this.listBox1.Visible = false;
            // 
            // ilShell1
            // 
            this.ilShell1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilShell1.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ilShell1.Location = new System.Drawing.Point(0, 0);
            this.ilShell1.Name = "ilShell1";
            this.ilShell1.Size = new System.Drawing.Size(584, 361);
            this.ilShell1.TabIndex = 0;
            this.ilShell1.WordWrap = false;
            // 
            // ILShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.ControlBox = false;
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.ilShell1);
            this.Name = "ILShellForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "C# Interactive Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ILShellForm_FormClosing);
            this.Load += new System.EventHandler(this.ILShellForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ILShell ilShell1;
        private System.Windows.Forms.ListBox listBox1;
    }
}