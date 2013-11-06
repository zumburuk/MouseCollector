using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bugMouseTest
{
    /// <summary>
    /// tests are formed of tasks to be performed by the user
    /// a task data in XML and its template is accessible via
    /// readonly XML_template property
    /// you gotto create objects of this class to hold specific task results
    /// performed by the user
    /// 
    /// </summary>
    public class bugMouseTestData
    {

        #region declerations
        private string user_id, task_id;

        #endregion

        #region constructors

        public bugMouseTestData()
        { 
            
        }

        #endregion

        #region properties

        /// <summary>
        /// Sets or gets the ID of the test subject
        /// </summary>
        public string USER_ID
        {
            get { return user_id; }
            set { user_id = value; }
        }


        /// <summary>
        /// Gest or sets the ID of the taks
        /// Task IDs should be defined in Task Definition objects 
        /// i.e. objects of bugMouseTaskDefinition class
        /// </summary>
        public string TASK_ID
        {
            get { return task_id; }
            set { task_id = value; }
        }

        /// <summary>
        /// This readonly property summurizes the XML structure of the information
        /// contained in a bugMouseTestData object
        /// </summary>
        public string XML_template
        {
            get {
                return "<task_data>\n" + 
                "<user> user_email or ID </user>\n" +
                "<taskID>TASK ID </taskID>\n" +
                "<input_device> Mouse or Touchpad etc. </input_device>\n" +
                "<test_screen> w,h in pixels </test_screen>\n" +
                "<trail>\n" +
                "    <cursor> x,y </cursor>\n" +
                "    <time_stamp> now.date.toString() </time_stamp>\n" +
                "</trail>\n" +
                "<test_complete> YES or NO, it is not YES in case test is interrupted </test_complete>\n" +
                "</task_data>\n";
            }
        }

        #endregion
    }

    public class bugMouseTaskDefinition
    { 
    
    }
}
