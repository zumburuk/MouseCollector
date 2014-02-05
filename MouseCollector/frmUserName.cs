using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MouseCollector
{
    public partial class frmUserName : Form
    {
        public delegate void delUsernameEntered(string UserName);
        public event delUsernameEntered UserNameEntered;

        public frmUserName()
        {
            InitializeComponent();
        }

        private void tBoxUserName_Click(object sender, EventArgs e)
        {
            tBoxUserName.SelectAll();
        }

        private void tBoxUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.Close(); // we are done close the form
        }

        private void frmUserName_FormClosing(object sender, FormClosingEventArgs e)
        {
            // just send whatever value is in the textbox as the user name
            // there is a flaw in this logic since user can close the window through the GUI
            // this is not hanled yet
            if (UserNameEntered != null)
                UserNameEntered(tBoxUserName.Text); 

        }


    }
}
