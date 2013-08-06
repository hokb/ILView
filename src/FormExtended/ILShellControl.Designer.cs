namespace ILNumerics.ILView {
    partial class ILShellControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ilShell1 = new ILNumerics.ILShell();
            this.SuspendLayout();
            // 
            // ilShell1
            // 
            this.ilShell1.BackColor = System.Drawing.Color.Black;
            this.ilShell1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilShell1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ilShell1.ForeColor = System.Drawing.SystemColors.Menu;
            this.ilShell1.Location = new System.Drawing.Point(0, 0);
            this.ilShell1.Name = "ilShell1";
            this.ilShell1.Size = new System.Drawing.Size(284, 261);
            this.ilShell1.TabIndex = 0;
            this.ilShell1.CommandEntered += new System.EventHandler<ILNumerics.ILCommandEnteredEventArgs>(this.ilShell1_CommandEntered);
            // 
            // ILShellControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ilShell1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ILShellControl";
            this.ResumeLayout(false);

        }

        #endregion

        private ILShell ilShell1;

    }
}
