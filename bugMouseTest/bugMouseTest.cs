using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using bugXML_Basics;
using System.Windows.Forms;

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
        TouchPad,
        Other
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

        /// <summary>
        /// Sets or gets if the Test is completed
        /// </summary>
        public bool TestCompleted
        {
            get { return test_completed; }
            set { test_completed = value; }
        }

        public string FormattedContent
        {
            get
            {
                string res = "";
                res = "User: " + USER_ID + "\n";
                res += "Task: " + TASK_ID + "\n";
                res += "Input device: " + input_device.ToString() + "\n";
                res += "Test Screen: " + test_screen.Width.ToString() + "x" + test_screen.Height.ToString() + "\n";
                if (test_completed)
                    res += "Test completed with following data \n\n";
                else
                    res += "Test js NOT complete, but following partial data is available \n\n";
                if(points_collected != null)
                foreach (TracePoint tp in points_collected)
                    res += "\t" + tp.CursorLocation.X.ToString() + "," +
                        tp.CursorLocation.Y.ToString() + "\t @ " + tp.TimeStamp + "\n";

                return res;
            }
        }

        /// <summary>
        /// returns the list of currently logged points
        /// </summary>
        public List<TracePoint> LoggedPoints
        {
            get 
            {
                List<TracePoint> tpList = new List<TracePoint>();
                if (points_collected != null)
                    foreach (TracePoint tp in points_collected)
                        tpList.Add(tp.Clone());
                return tpList;
            }
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
        private cBaseXML toXML()
        {
            cBaseXML meXML = new cBaseXML();
            meXML.Add(tags[(int)TagNames.User], this.user_id, true);
            meXML.Add(tags[(int)TagNames.TaskID], this.task_id, true);
            meXML.Add(tags[(int)TagNames.InputDevice], this.input_device.ToString(), true);
            meXML.Add(tags[(int)TagNames.TestScreenResolution],
                this.test_screen.Width.ToString() + "," + this.test_screen.Height.ToString(), true);
            meXML.Add(tags[(int)TagNames.MouseTrail], ",", true); // add blank and replace this with points later
            cBaseXML trailXML = null;
            if (points_collected != null && points_collected.Count > 0)
            {
                trailXML = new cBaseXML();
                foreach (TracePoint trailpoint in points_collected)
                    trailXML.Add(TracePoint.TheTag, trailpoint.ToString(), false);
                meXML.Add(tags[(int)TagNames.MouseTrail], trailXML.ToString(), true);
            }
            meXML.Add(tags[(int)TagNames.TestCompleted], this.TestCompleted.ToString(), true);
            return meXML;
        }

        /// <summary>
        /// Loads from an string containing all tags representing 
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        private bool fromXML(string strXML)
        {
            try
            {
                cBaseXML inXML = new cBaseXML(strXML);
                // get the user info
                this.user_id = inXML.NodeData(tags[(int)TagNames.User]);
                // get task id
                this.task_id = inXML.NodeData(tags[(int)TagNames.TaskID]);
                // get input device
                this.input_device = GetDevice( inXML.NodeData(tags[(int)TagNames.InputDevice]));
                // get screen resolution
                this.test_screen = GetScreenSize( inXML.NodeData(tags[(int)TagNames.TestScreenResolution]));
                // get if test completed
                this.test_completed = Convert.ToBoolean(inXML.NodeData(tags[(int)TagNames.TestCompleted]));
                // finally get all the mouse trace points
                cBaseXML trcXML = new cBaseXML(inXML.NodeData(tags[(int)TagNames.MouseTrail]));
                string trc = "notyet";
                TracePoint tp;
                do
                {
                    // update trace
                    trc = trcXML.NodeData(TracePoint.TheTag);
                    if (trc != "")
                    {
                        tp = new TracePoint(trc);
                        this.AddTracePoint(tp);
                        trcXML.Remove(TracePoint.TheTag);
                    }
                } while (trc != "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts the input string to an Input Device type
        /// It has to be exatly given as in the InputDevices enumarated values
        /// </summary>
        /// <param name="devicename">Name of input device in string format - that is case sensitive</param>
        /// <returns></returns>
        private InputDevices GetDevice(string devicename)
        {
            InputDevices dev = InputDevices.Other;
            if (devicename == InputDevices.Mouse.ToString()) dev = InputDevices.Mouse;
            else if (devicename == InputDevices.TouchPad.ToString()) dev = InputDevices.TouchPad;
            return dev;
        }

        private Size GetScreenSize(string resolution)
        {
            Size res = new Size(0, 0);
            string[] valz = resolution.Split(',');
            try
            {
                res.Width = Convert.ToInt16(valz[0]);
                res.Height = Convert.ToInt16(valz[1]);
            }
            catch { }
            return res;
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

                return toXML().ToString();
            }
            set
            {
                fromXML(value);
            }
        }
        #endregion

        #region iFileIO
        /// <summary>
        /// loads a file from the given full file path
        /// </summary>
        /// <param name="FilePath">Full file path of the XML file</param>
        /// <returns>TRUE if load is successful, FALSE otherwise</returns>
        public bool LoadFromFile(string FilePath)
        {
            try
            {
                cBaseXML newTest = new cBaseXML();
                if (newTest.LoadFromFile(FilePath))
                {
                    string innerXML = newTest.ToString();
                    this.XML = innerXML;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            { 
                System.Diagnostics.Debug.WriteLine(ex.Message); 
                return false; 
            }
        }

        public bool Write2File(string FilePath)
        {
            return toXML().Write2File(FilePath);
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

        /// <summary>
        /// Sets the content of the object from a string in the form:
        /// x,y,time_stamp
        /// </summary>
        /// <param name="innerText">Inner Text of the XML tag representing this object</param>
        public TracePoint(string innerText)
        {
            SetFromInnerText(innerText);
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

        /// <summary>
        /// gets or sets the location of the cursor
        /// </summary>
        public Point CursorLocation
        {
            get { return new Point(cursor_point.X, cursor_point.Y); }
            set { cursor_point.X = value.X; cursor_point.Y = value.Y; }
        }

        /// <summary>
        /// gets or sets the time stamp of the Cursor Location captured
        /// </summary>
        public DateTime TimeStamp
        {
            get { return time_tag; }
            set { time_tag = value; }
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
                SetFromInnerText(traceXML.NodeData(TracePoint.thetag));
            }
            catch { }
        }

        private void SetFromInnerText(string innerText)
        {
            try
            {
                string[] valz = innerText.Split(',');
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

        // creates a clone of the current object
        public TracePoint Clone()
        {
            return new TracePoint(this.ToString());
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
            TargetSize= 6
        }

        public enum TaskShapes
        { 
            Rectangle = 0,
            Circle = 1,
            Other = 2
        }

        #region declerations
        private string[] tags = { "taskID", "shape", "backcolor", "bordercolor", "target_location", "cursor_location", "target_size" };
        string task_id;
        TaskShapes shape;
        Color back_color, border_color;
        Point target_location, cursor_location;
        Size target_size;
        
        #endregion

        #region constructors

        public bugMouseTaskDefinition() { }

        

        #endregion

        #region methods

        /// <summary>
        /// Converts a string in the form x,y to a point object
        /// </summary>
        /// <param name="pointXY">x,y in string form</param>
        /// <returns>point object set to x,y</returns>
        private Point GetPoint(string pointXY)
        {
            Point res = new Point(0, 0);
            string[] valz = pointXY.Split(',');
            try
            {
                res.X = Convert.ToInt16(valz[0]);
                res.Y = Convert.ToInt16(valz[1]);
            }
            catch { }
            return res;
        }

        /// <summary>
        /// Converts the input string in the form Width,Height into a size object
        /// </summary>
        /// <param name="strWH">string containing w,h</param>
        /// <returns>Size obtained from string, 0,0 if fails parsing</returns>
        private Size GetSize(string strWH)
        {
            Point pt = GetPoint(strWH);
            return new Size(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts a string expression into a TaskShape
        /// </summary>
        /// <param name="strShape">string containing Task Shape name</param>
        /// <returns>TaskShape obtained from string, if not a valid string 'other' is returned</returns>
        private TaskShapes GetShape(string strShape)
        {
            strShape = strShape.Trim();
            TaskShapes theShape = TaskShapes.Other;
            if (strShape == TaskShapes.Circle.ToString()) theShape = TaskShapes.Circle;
            else if (strShape == TaskShapes.Rectangle.ToString()) theShape = TaskShapes.Rectangle;
            return theShape;
        }

        /// <summary>
        /// Converts a color in R,G,B string form into a Color
        /// </summary>
        /// <param name="strColor">R,G,B string</param>
        /// <returns>Color in the string, if could not be converted, Color.transparent is returned</returns>
        private Color GetColor(string strColor)
        {
            Color res = new Color();
            res = Color.Transparent;
            string[] valz = strColor.Split(',');
            try
            {
                res = Color.FromArgb(
                    Convert.ToByte(valz[0]),
                    Convert.ToByte(valz[1]),
                    Convert.ToByte(valz[2]));
            }
            catch { }
            return res;
        }

        /// <summary>
        /// Creates a cBaseXML object based on the instance data
        /// </summary>
        /// <returns></returns>
        private cBaseXML toXML()
        {
            cBaseXML meXML = new cBaseXML();
            meXML.Add(tags[(int)TagNames.TaskID], this.task_id, true);
            meXML.Add(tags[(int)TagNames.Shape], this.shape.ToString(), true);
            meXML.Add(tags[(int)TagNames.BackColor], 
                this.back_color.R.ToString() + "," + this.back_color.G.ToString() + "," + this.back_color.B.ToString() , true);
            meXML.Add(tags[(int)TagNames.BorderColor],
                this.border_color.R.ToString() + "," + this.border_color.G.ToString() + "," + this.border_color.B.ToString(), true);
            meXML.Add(tags[(int)TagNames.TargetLocation],
                this.target_location.X.ToString() + "," + this.target_location.Y.ToString(), true);
            meXML.Add(tags[(int)TagNames.CursorLocation],
                this.cursor_location.X.ToString() + "," + this.cursor_location.Y.ToString(), true);          
            meXML.Add(tags[(int)TagNames.TargetSize],
                this.target_size.Width.ToString() + "," + this.target_size.Height.ToString(), true);
            return meXML;
        }

        /// <summary>
        /// Loads from an string containing all tags representing 
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        private bool fromXML(string strXML)
        {
            try
            {
                cBaseXML inXML = new cBaseXML(strXML);
                // get task info
                this.task_id = inXML.NodeData(tags[(int)TagNames.TaskID]);
                // get shape
                this.shape = (TaskShapes) GetShape(inXML.NodeData(tags[(int)TagNames.Shape]));
                // get back color
                this.back_color =  GetColor(inXML.NodeData(tags[(int)TagNames.BackColor]));
                // get border color
                this.border_color = GetColor(inXML.NodeData(tags[(int)TagNames.BorderColor]));
                // get target location
                this.target_location = GetPoint(inXML.NodeData(tags[(int)TagNames.TargetLocation]));
                // get cursor location
                this.cursor_location = GetPoint(inXML.NodeData(tags[(int)TagNames.CursorLocation]));
                // get cursor location
                this.target_size = GetSize(inXML.NodeData(tags[(int)TagNames.TargetSize]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// same as XML get
        /// </summary>
        /// <returns>the XML content of the test data with all tags except a root node!</returns>
        public override string ToString()
        {
            return this.XML;
        }

        #endregion

        #region properties
        public Rectangle TargetBoundingBox
        {
            get 
            {
                Rectangle scr = Screen.AllScreens[0].WorkingArea;
                /*/ following is for relative size and location
                Size tsize = new Size(scr.Width * target_size.Width / 100, scr.Height * target_size.Height / 100);
                Point tcenter = new Point(
                    scr.Width * target_location.X / 100 - tsize.Width/2, 
                    scr.Height * target_location.Y / 100 - tsize.Height/2);
                //*/
                //*/ absolute size and location
                Size tsize = new Size(target_size.Width, target_size.Height);
                Point tcenter = new Point(
                    target_location.X - tsize.Width/ 2, 
                    target_location.Y - tsize.Height / 2
                    );
                //*/
                return new Rectangle(tcenter, tsize);
            }
        }

        public Point CursorLocation
        {
            get
            {
                Rectangle scr = Screen.AllScreens[0].WorkingArea;
                return new Point(scr.Width * cursor_location.X / 100, scr.Height * cursor_location.Y / 100);
            }
        }

        public TaskShapes Shape { get { return shape; } }

        public Color BackColor { get { return back_color; } }

        public Color BorderColor { get { return border_color; } }
        #endregion

        #region iXML
        public string XML
        {
            get
            {
                return toXML().ToString();
            }
            set
            {
                fromXML(value);
            }
        }
        #endregion

        #region iFileIO
        /// <summary>
        /// loads a file from the given full file path
        /// </summary>
        /// <param name="FilePath">Full file path of the XML file</param>
        /// <returns>TRUE if load is successful, FALSE otherwise</returns>
        public bool LoadFromFile(string FilePath)
        {
            try
            {
                cBaseXML newTest = new cBaseXML();
                if (newTest.LoadFromFile(FilePath))
                {
                    string innerXML = newTest.ToString();
                    this.XML = innerXML;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Write2File(string FilePath)
        {
            return toXML().Write2File(FilePath);
        }
        #endregion
    }
}
