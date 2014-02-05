namespace MouseCollector
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtBoxDebug = new System.Windows.Forms.RichTextBox();
            this.lBoxScenarios = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // rtBoxDebug
            // 
            this.rtBoxDebug.Location = new System.Drawing.Point(14, 13);
            this.rtBoxDebug.Name = "rtBoxDebug";
            this.rtBoxDebug.Size = new System.Drawing.Size(332, 406);
            this.rtBoxDebug.TabIndex = 0;
            this.rtBoxDebug.Text = "";
            // 
            // lBoxScenarios
            // 
            this.lBoxScenarios.FormattingEnabled = true;
            this.lBoxScenarios.Location = new System.Drawing.Point(352, 13);
            this.lBoxScenarios.Name = "lBoxScenarios";
            this.lBoxScenarios.Size = new System.Drawing.Size(192, 394);
            this.lBoxScenarios.TabIndex = 1;
            this.lBoxScenarios.SelectedIndexChanged += new System.EventHandler(this.lBoxScenarios_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 580);
            this.Controls.Add(this.lBoxScenarios);
            this.Controls.Add(this.rtBoxDebug);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtBoxDebug;
        private System.Windows.Forms.ListBox lBoxScenarios;
    }
}

