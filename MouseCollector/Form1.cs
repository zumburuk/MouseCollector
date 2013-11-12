using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using bugXML_Basics;
using bugMouseTest;

namespace MouseCollector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //*/
            bugMouseTestData bm = new bugMouseTestData();
            rtBoxDebug.Text = bm.XML_template;

            bugMouseTestData mTest = new bugMouseTestData("bug", "dummy", 
                new Size(Screen.AllScreens[0].WorkingArea.Width, Screen.AllScreens[0].WorkingArea.Height), 
                InputDevices.TouchPad);
            Random r = new Random();
            for (int i = 0; i < 5; i++)
                mTest.AddTracePoint(new Point(r.Next(0, 1500), r.Next(0, 1500)), DateTime.Now);

            rtBoxDebug.Text = mTest.ToString();
            if (mTest.Write2File(Application.StartupPath + "//test.xml"))
                this.Text = "Fine";
            else
                this.Text = "Failed to create XML file";

            //*/

            /*/
            // let's test the basic XML just in case
            cBaseXML nxml = new cBaseXML();
            cBaseXML nxmlinner = new cBaseXML();
            nxml.Add("user", "bugra", true);
            nxml.Add("trace", ",", true);

            nxmlinner.Add("point", "3,4", false);
            nxmlinner.Add("point", "12,42", false);
            nxmlinner.Add("point", "1,5", false);
            nxml.Add("trace", nxmlinner.ToString(), true);

            rtBoxDebug.Text = nxml.ToString();

            //*/
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = e.X.ToString() + ":" + e.Y.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
