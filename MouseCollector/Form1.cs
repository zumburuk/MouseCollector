using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using bugXML_Basics;
using bugMouseTest;

namespace MouseCollector
{
    public partial class Form1 : Form
    {
        #region declerations
        string user_name = "John Doe"; // no name given yet
        List<string> TestScenarios; // will hold the list of scenario names
        Dictionary<string, string> TestScenarioFileLocations; // will hold the location of scenario files
        string default_test = "default_test.txt"; // name of the file that contains the test scenarios. This file is expected to be in the application folder
        string scenarios_folder = "scenarios"; // contains scenario files defined in default_test
        string test_results_folder = "test_results"; // contains user test results for each scenario defined in default_test
        bugMouseTaskDefinition current_scenario;

        bool test_started = false; // used to check if test started
        bool test_ready = false; // used to check if test is loaded and ready to start

        Rectangle target_boundingbox;
        #endregion

        #region constructors
        public Form1()
        {
            InitializeComponent();
            
            this.WindowState = FormWindowState.Maximized;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            this.Cursor = Cursors.Cross;

            #region load test scenarios
            TestScenarios = new List<string>();
            TestScenarioFileLocations = new Dictionary<string, string>();
            // open the default test file

            // Read the test file and load scenarios line by line.
            // first update folders based on current application folder
            default_test = Application.StartupPath + "\\" + default_test;
            scenarios_folder = Application.StartupPath + "\\" + scenarios_folder;
            test_results_folder = Application.StartupPath + "\\" + test_results_folder;
            string line = ""; // will read lines from test definition file
            
            if (File.Exists(default_test))
            {
                if (Directory.Exists(scenarios_folder))
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(default_test);
                    while ((line = file.ReadLine()) != null )
                    {
                        if (line.Trim() != "")
                        {
                            lBoxScenarios.Items.Add(line);
                            TestScenarioFileLocations.Add(line, scenarios_folder + "\\" + line);
                            TestScenarios.Add(line);
                        }
                    }
                    file.Close();
                }
                else
                {
                    MessageBox.Show("Test scenarios folder does not exist. \nProgram will terminate", "File does not exist");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Test definition file does not exist. \nProgram will terminate", "File does not exist");
                this.Close();
            }
            #endregion

            #region temporary test codes - erease before relase
            /*/
            bugMouseTestData bm = new bugMouseTestData();
            rtBoxDebug.Text = bm.XML_template;

            bugMouseTestData mTest = new bugMouseTestData();

            bugMouseTaskDefinition taskdef = new bugMouseTaskDefinition();
            //*/
            /*/
            mTest = new bugMouseTestData("bug", "dummy", 
                new Size(Screen.AllScreens[0].WorkingArea.Width, Screen.AllScreens[0].WorkingArea.Height), 
                InputDevices.TouchPad);
            Random r = new Random();
            for (int i = 0; i < 5; i++)
                mTest.AddTracePoint(new Point(r.Next(0, 1500), r.Next(0, 1500)), DateTime.Now);
            if (mTest.Write2File(Application.StartupPath + "\\test.xml"))
                rtBoxDebug.Text = mTest.ToString();
            else
                rtBoxDebug.Text = "Failed to write XML file";
             
            //*/
            
            /*/
            if (mTest.LoadFromFile(Application.StartupPath + "\\test.xml"))
            {
                // print user
                rtBoxDebug.Text = mTest.FormattedContent;
                
            }
            else
                rtBoxDebug.Text = "Failed to read XML file";
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

            /*/
            if (taskdef.LoadFromFile(Application.StartupPath + "\\scenarios.xml"))
                rtBoxDebug.Text = taskdef.ToString();
            else
                rtBoxDebug.Text = "Failed to read XML File";
            if (taskdef.Write2File(Application.StartupPath + "\\scenario_written.xml"))
                rtBoxDebug.Text = taskdef.ToString();
            else
                rtBoxDebug.Text = "Failed to write XML File";
            //*/
            #endregion

        }
        #endregion

        #region event handlers
        private void UserNameSet(string UserName) { user_name = UserName; rtBoxDebug.Text = UserName; this.Show(); }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = e.X.ToString() + ":" + e.Y.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close(); // comment this line out before release!!!
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // get the username
            frmUserName userNameForm = new frmUserName();
            userNameForm.UserNameEntered += new frmUserName.delUsernameEntered(UserNameSet);
            userNameForm.Show();
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.Arrow; // just to be self complete
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (current_scenario != null)
            {
                Graphics g = e.Graphics;
                Pen borderPen = new Pen(current_scenario.BorderColor);
                Pen backPen = new Pen(current_scenario.BackColor);
                if (current_scenario.Shape == bugMouseTaskDefinition.TaskShapes.Circle) // draw circle
                {
                    g.FillEllipse(backPen.Brush, current_scenario.TargetBoundingBox);
                    g.DrawEllipse(borderPen, current_scenario.TargetBoundingBox);
                }
                else // currently rectangel and everything else is handled here as a rectangle
                {
                    g.FillRectangle(backPen.Brush, current_scenario.TargetBoundingBox);
                    g.DrawRectangle(borderPen, current_scenario.TargetBoundingBox);
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Loads the scenario given by the file name
        /// </summary>
        /// <param name="current_scenario_file">Contains the file name of a test scenario. This is not the full path, just the file name</param>
        private void LoadScenario(string current_scenario_file)
        { 
            // now that file existance have been checked out in the constructor, we will assume that the files are still there
            // still being carefull wont kill
            try
            {
                current_scenario = new bugMouseTaskDefinition();
                current_scenario.LoadFromFile(TestScenarioFileLocations[current_scenario_file]);
            }
            catch 
            {
                MessageBox.Show("Scenario could not be loaded", current_scenario_file + " could not be loaded. \nProgram will terminate");
                this.Close();
            }
        }

        /// <summary>
        /// Displays the scenario last loaded by LoadScenario
        /// </summary>
        private void DisplayCurrentScenario()
        {
            test_ready = true;
            //Cursor.Hide();
            // force draw target by paint
            this.Invalidate();
        }

        #endregion

        /// <summary>
        /// delete this function at the end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lBoxScenarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadScenario(lBoxScenarios.SelectedItem.ToString());
            DisplayCurrentScenario();
            rtBoxDebug.Text = current_scenario.ToString();
            /*/
            bugMouseTaskDefinition tdef = new bugMouseTaskDefinition();
            string key = lBoxScenarios.SelectedItem.ToString();
            if(TestScenarioFileLocations.ContainsKey(key))
                if(tdef.LoadFromFile(TestScenarioFileLocations[key]))
                    rtBoxDebug.Text = tdef.ToString();
            //*/
        }


    }
}
