namespace ILNumerics.ILView {
    partial class ILMainFormSimple {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ILMainFormSimple));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripCmbSource = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnConsoleVisible = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSVG = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPNG = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCmbSource,
            this.toolStripBtnLoad,
            this.toolStripSeparator1,
            this.toolStripBtnConsoleVisible,
            this.toolStripSeparator2,
            this.toolStripButtonSVG,
            this.toolStripButtonPNG});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(786, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripCmbSource
            // 
            this.toolStripCmbSource.Name = "toolStripCmbSource";
            this.toolStripCmbSource.Size = new System.Drawing.Size(121, 25);
            this.toolStripCmbSource.Text = "(enter example source URI or ILC# here or select from list...)";
            this.toolStripCmbSource.ToolTipText = "enter example source URI or ILC# here or select from list...";
            this.toolStripCmbSource.TextChanged += new System.EventHandler(this.toolStripCmbSource_TextChanged);
            // 
            // toolStripBtnLoad
            // 
            this.toolStripBtnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnLoad.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnLoad.Image")));
            this.toolStripBtnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnLoad.Name = "toolStripBtnLoad";
            this.toolStripBtnLoad.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtnLoad.Text = "toolStripButton1";
            this.toolStripBtnLoad.ToolTipText = "Reload scene from ILC# or URI";
            this.toolStripBtnLoad.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripBtnConsoleVisible
            // 
            this.toolStripBtnConsoleVisible.CheckOnClick = true;
            this.toolStripBtnConsoleVisible.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnConsoleVisible.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnConsoleVisible.Image")));
            this.toolStripBtnConsoleVisible.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnConsoleVisible.Name = "toolStripBtnConsoleVisible";
            this.toolStripBtnConsoleVisible.Size = new System.Drawing.Size(72, 22);
            this.toolStripBtnConsoleVisible.Text = "C# Console";
            this.toolStripBtnConsoleVisible.ToolTipText = "Toogles vsibility for the C# interactive shell window";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSVG
            // 
            this.toolStripButtonSVG.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSVG.Image")));
            this.toolStripButtonSVG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSVG.Name = "toolStripButtonSVG";
            this.toolStripButtonSVG.Size = new System.Drawing.Size(48, 22);
            this.toolStripButtonSVG.Text = "SVG";
            this.toolStripButtonSVG.ToolTipText = "Export current scene as SVG";
            this.toolStripButtonSVG.Click += new System.EventHandler(this.toolStripButtonSVG_Click);
            // 
            // toolStripButtonPNG
            // 
            this.toolStripButtonPNG.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPNG.Image")));
            this.toolStripButtonPNG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPNG.Name = "toolStripButtonPNG";
            this.toolStripButtonPNG.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonPNG.Text = "PNG";
            this.toolStripButtonPNG.ToolTipText = "Export current scene as PNG";
            this.toolStripButtonPNG.Click += new System.EventHandler(this.toolStripButtonPNG_Click);
            // 
            // ILMainFormSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 415);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ILMainFormSimple";
            this.Text = "ILNumerics Example Viewer";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripCmbSource;
        private System.Windows.Forms.ToolStripButton toolStripBtnLoad;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSVG;
        private System.Windows.Forms.ToolStripButton toolStripButtonPNG;
        private System.Windows.Forms.ToolStripButton toolStripBtnConsoleVisible;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;


    }
}