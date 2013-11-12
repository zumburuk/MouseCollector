using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using bugXML_Basics;

namespace bugMouseTest
{
    public interface iXML
    {
        string XML { get; set; }
    }
    
    /// <summary>
    /// Input methods preferred in test
    /// </summary>
    public enum InputDevices 
    {
        // if you change these later, also check the comment on most comprehensive constructor
        Mouse, 
        TouchPad 
    }

    /// <summary>
    /// tests are formed of tasks to be performed by the user
    /// a task data in XML and its template is accessible via
    /// readonly XML_template property
    /// you gotto create objects of this class to hold specific task results
    /// performed by the user
    /// 
    /// </summary>
    public class bugMouseTestData : iXML, iFileIO
    {
        public enum TagNames 
        { 
            User = 0,
            TaskID = 1,
            InputDevice = 2,
            TestScreenResolution = 3,
            MouseTrail = 4,
            TestCompleted = 5
        }

        #region declerations
        private string user_id, task_id;
        private Size test_screen;
        private InputDevices input_device;
        private string[] tags = {"user", "taskID", "input_device", "test_screen", "trail", "test_complete" };

        private cBaseXML trace_data;
        private List<TracePoint> points_collected;

        private bool test_completed;

        #endregion

        #region constructors

        /// <summary>
        /// default constructor for John Doe Fiddling, 
        /// assumes full HD screen and mouse input... if not appropriate, set it later
        /// either set the user and task IDs, screen resolution and input method later, 
        /// or use the overloaded constructors
        /// </summary>
        public bugMouseTestData() : this("John Doe", "Fiddling") {}

        /// <summary>
        /// Constructor: sets user and task ids at time of object creation
        /// assumes full HD screen and mouse input... if not appropriate, set it later
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <param name="TaskID">Task ID</param>
        public bugMouseTestData(string UserID, string TaskID) : this(UserID, TaskID, new Size(1920, 1080), InputDevices.Mouse) {}

        /// <summary>
        /// Most comprehensive constructor, sets user and task IDs along with screen size and input method
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <param name="TaskID">Task ID</param>
        /// <param name="scrSize">Screen size i.e. width and height in pixels</param>
        /// <param name="inpMethod">Input method: i.e. mouse or touchpad</param>
        public bugMouseTestData(string UserID, string TaskID, Size scrSize, InputDevices inpMethod)
        {
            test_screen = new Size(scrSize.Width, scrSize.Height);
            input_device = inpMethod;
            this.USER_ID = UserID;
            this.TASK_ID = TaskID;

            test_completed = false;
            
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

        public bool TestCompleted
        {
            get { return test_completed; }
            set { test_completed = value; }
        }

        #endregion

        #region methods
        public void AddTracePoint(Point cursorP, DateTime capturetime)
        {
            AddTracePoint(new TracePoint(cursorP, capturetime));
        }

        public void AddTracePoint(TracePoint tp)
        {
            if (points_collected == null) points_collected = new List<TracePoint>(); // first point is to be added  
            points_collected.Add(tp);
        }

        /// <summary>
        /// same as XML get
        /// </summary>
        /// <returns>the XML content of the test data with all tags except a root node!</returns>
        public override string ToString()
        {
            return this.XML;
        }

        /// <summary>
        /// Creates a cBaseXML object based on the instance data
        /// </summary>
        /// <returns></returns>
        private cBaseXML inXML()
        {
            cBaseXML meXML = new cBaseXML();
            meXML.Add(tags[(int)TagNames.User], this.user_id, true);
            meXML.Add(tags[(int)TagNames.TaskID], this.task_id, true);
            meXML.Add(tags[(int)TagNames.InputDevice], this.input_device.ToString(), true);
            meXML.Add(tags[(int)TagNames.TestScreenResolution],
                this.test_screen.Width.ToString() + "," + this.test_screen.Height.ToString(), true);
            meXML.Add(tags[(int)TagNames.MouseTrail], ",", true);
            cBaseXML trailXML = null;
            if (points_collected.Count > 0) trailXML = new cBaseXML();
            foreach (TracePoint trailpoint in points_collected)
                trailXML.Add(TracePoint.TheTag, trailpoint.ToString(), false);
            meXML.Add(tags[(int)TagNames.MouseTrail], trailXML.ToString(), true);
            return meXML;
        }

        #endregion

        #region iXML
        /// <summary>
        /// returns the XML content of the test data with all tags except a root node!
        /// </summary>
        public string XML
        {
            get
            {
                // create a new cBaseXML object

                return inXML().ToString();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region iFileIO
        public bool LoadFromFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public bool Write2File(string FilePath)
        {
            return inXML().Write2File(FilePath);
        }
        #endregion
    }

    /// <summary>
    /// This class is meant to support bugMouseTestData
    /// Each instance contains x,y,time_stamp
    /// Test data contains a list of TracePoint objects
    /// </summary>
    public class TracePoint : iXML
    {
        public enum TagNames{CursorCoordinates = 0, TimeStamp = 1}

        #region declerations
        Point cursor_point;
        DateTime time_tag;
        public const string thetag = "point_data" ;
        
        #endregion

        #region constructors
        public TracePoint() { }

        public TracePoint(Point cursorP, DateTime timeTag)
        {
            cursor_point = new Point( cursorP.X, cursorP.Y);
            time_tag = timeTag;
        }

        public TracePoint(cBaseXML traceXML)
        {
            SetFromXML(traceXML);
        }

        #endregion

        #region properties

        public static string TheTag { get { return thetag; } }

        public cBaseXML XMLp
        {
            get 
            {
                cBaseXML traceXML = new cBaseXML();
                string innerData = cursor_point.X.ToString() + "," + cursor_point.Y.ToString() + "," + time_tag.ToString();
                traceXML.Add(thetag, innerData, true);
                return traceXML;
            }
            set { SetFromXML(value); }
        }

        #endregion

        #region methods
        /// <summary>
        /// sets the content of the object from a cBaseXL object
        /// if object contains more than one trace data first one will be used for instantitaion
        /// </summary>
        /// <param name="traceXML">cbaseXML object contaning one trace point data</param>
        private void SetFromXML(cBaseXML traceXML)
        {
            try
            {
                string[] valz = traceXML.NodeData(TracePoint.thetag).Split(',');
                int resx = 0, resy = 0; // if both are valid set the local
                if (int.TryParse(valz[0], out resx) && int.TryParse(valz[1], out resy))
                    cursor_point = new Point(resx, resy);
                time_tag = Convert.ToDateTime(valz[2]);
            }
            catch { }
        }

        /// <summary>
        /// returns the XML data without theTag
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return cursor_point.X.ToString() + "," + cursor_point.Y.ToString() + "," + time_tag.ToString(); ;
        }

        #endregion

        public string XML
        {
            get
            {
                return "<" + thetag + ">" + cursor_point.X.ToString() + "," + cursor_point.Y.ToString()
                    + "," + time_tag.ToString() + "</" + thetag + ">";
            }
            set
            {
                cBaseXML traceXML = new cBaseXML(value);
            }
        }
    }

    public class bugMouseTaskDefinition: iXML, iFileIO
    {
        public enum TagNames
        {
            TaskID = 0,
            Shape = 1,
            BackColor = 2,
            BorderColor = 3,
            TargetLocation = 4,
            CursorLocation = 5,
            Size= 6,
            Cursor = 7
        }

        public enum TaskShapes
        { 
            Rectangle = 0,
            Circle = 1
        }

        #region declerations
        private string[] tags = { "taskID", "shape", "backcolor", "bordercolor", "target_location", "cursor_location", "size", "cursor" };

        #endregion

        #region constructors

        public bugMouseTaskDefinition() { }

        

        #endregion

        #region methods

        #endregion

        #region iXML
        public string XML
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region iFileIO
        public bool LoadFromFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public bool Write2File(string FilePath)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
