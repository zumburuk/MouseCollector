namespace MouseCollector
{
    partial class frmUserName
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
            this.tBoxUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tBoxUserName
            // 
            this.tBoxUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tBoxUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxUserName.Location = new System.Drawing.Point(12, 12);
            this.tBoxUserName.Name = "tBoxUserName";
            this.tBoxUserName.Size = new System.Drawing.Size(588, 26);
            this.tBoxUserName.TabIndex = 0;
            this.tBoxUserName.Text = "John Doe";
            this.tBoxUserName.Click += new System.EventHandler(this.tBoxUserName_Click);
            this.tBoxUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBoxUserName_KeyDown);
            // 
            // frmUserName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 55);
            this.Controls.Add(this.tBoxUserName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUserName";
            this.Text = "Please enter a user name and press ENTER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUserName_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tBoxUserName;
    }
}