namespace GabrielForm.Components
{
    partial class FotoUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.circleBox1 = new GabrielForm.CircleBox();
            ((System.ComponentModel.ISupportInitialize)(this.circleBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // circleBox1
            // 
            this.circleBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.circleBox1.Location = new System.Drawing.Point(0, 0);
            this.circleBox1.Name = "circleBox1";
            this.circleBox1.Size = new System.Drawing.Size(60, 60);
            this.circleBox1.TabIndex = 1;
            this.circleBox1.TabStop = false;
            // 
            // FotoUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.circleBox1);
            this.Name = "FotoUserControl";
            this.Size = new System.Drawing.Size(60, 60);
            ((System.ComponentModel.ISupportInitialize)(this.circleBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CircleBox circleBox1;
    }
}
