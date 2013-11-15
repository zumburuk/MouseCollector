using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace bugXML_Basics
{
    public interface iFileIO
    {
        bool LoadFromFile(string FilePath);
        bool Write2File(string FilePath);
    }

    /// <summary>
    /// This class is intended for simple manipulation of XML object for limited use suitable for our purpose
    /// For cascaded tags, you have to be creative... but remember the motto SMF, keep it simple
    /// That's why class is implemented shallow... going too deep? might be violating the spirit of SMF!
    /// </summary>
    public class cBaseXML: iFileIO
    {
        #region variables
        string blank_xml = @"<?xml version='1.0' encoding='utf-8'?>
           <XMLBase>
           </XMLBase>";
        XmlDocument xdoc;
        XmlNode rootnode;

        #endregion

        #region constructor
        public cBaseXML()
        {
            Reset();
        }


        public cBaseXML(string innerXML)
        {
            //Reset();
            //rootnode.InnerText = innerXML;
            Reset(innerXML);
        }

        #endregion

        #region properties

        #endregion

        #region methods

        /// <summary>
        /// Adds a new tag with data to root document
        /// If tag already exits only updates its data
        /// </summary>
        /// <param name="tag">tag to add</param>
        /// <param name="data">data that goes along with tag</param>
        /// <param name="UpdateIfExists">If TRUE, updates the current tag if it already exists,
        /// If FALSE, creates a new node independent of wheter the string exists or not</param>
        public void Add(string tag, string data, bool UpdateIfExists)
        {
            rootnode = xdoc.SelectSingleNode("XMLBase");
            XmlNode tagnode = rootnode.SelectSingleNode(tag);
            if (tagnode == null || !UpdateIfExists) // this is a new tag
            {
                tagnode = xdoc.CreateNode(XmlNodeType.Element, tag, null);
                tagnode.InnerText = data;
                rootnode.AppendChild(tagnode);
            }
            else
                tagnode.InnerXml = data;
        }

        /// <summary>
        /// removes given tag
        /// </summary>
        /// <param name="tag">name of the tag to be removed</param>
        public void Remove(string tag)
        {
            rootnode = xdoc.SelectSingleNode("XMLBase");
            XmlNode tagnode = rootnode.SelectSingleNode(tag);
            if (tagnode != null) // this is a new tag
                rootnode.RemoveChild(tagnode);
        }

        /// <summary>
        /// Returns the content of the node with the tag
        /// if tag does not exist returns empty string
        /// </summary>
        /// <param name="tag">node tag</param>
        /// <returns>node content</returns>
        public string NodeData(string tag)
        {
            string dt = "";
            XmlNode theNode = rootnode.SelectSingleNode(tag);
            if (theNode != null)
            {
                dt = theNode.InnerXml;
            }
            return dt;
        }

        /// <summary>
        /// Reset the content of the object to root object with no elements what so ever
        /// </summary>
        public void Reset()
        {
            xdoc = new XmlDocument();
            xdoc.LoadXml(blank_xml);
            rootnode = xdoc.SelectSingleNode("XMLBase");
        }

        /// <summary>
        /// Reset the content and set XMLBase content to inner_data
        /// For this to properly work, inner_data should have a proper XML tagged format
        /// </summary>
        /// <param name="inner_data"></param>
        public void Reset(string inner_data)
        {
            xdoc = new XmlDocument();
            string incoming = @"<?xml version='1.0' encoding='utf-8'?> 
            <XMLBase> "
            + @inner_data
            + @"</XMLBase>";
            xdoc.LoadXml(incoming);
            rootnode = xdoc.SelectSingleNode("XMLBase");
        }

        /// <summary>
        /// Returns the content of the root document
        /// </summary>
        /// <returns>Root document content i.e. XML and Root Document tags are excluded</returns>
        public override string ToString()
        {
            return rootnode.InnerXml;
        }

        #endregion

        #region iFileIO
        public bool LoadFromFile(string FilePath)
        {
            try
            {
                // try to load the file
                if (xdoc != null)
                {
                    xdoc.Load(FilePath);
                    rootnode = xdoc.SelectSingleNode("XMLBase");
                    return true;
                }
                else
                    return false;
            }
            catch { return false; }
        }

        public bool Write2File(string FilePath)
        {
            try 
            {
                // try to save the file
                if (xdoc != null)
                {
                    xdoc.Save(FilePath);
                    return true;
                }
                else
                    return false;
            }
            catch { return false; }
        }
        #endregion
    }


}
