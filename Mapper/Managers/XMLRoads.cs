using Mapper.Containers;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Mapper.Managers
{
    public class XmlRoadHandler
    {
        public static XmlRoads LoadRoads()
        {
            XmlRoads roads = null;

            try
            {
                XmlSerializer _serialiser = new XmlSerializer(typeof(XmlRoads));
                TextReader _xmlReader = new StreamReader("roadDefinitions.xml");

                roads = _serialiser.Deserialize(_xmlReader) as XmlRoads;
            }
            catch
            {
                Debug.LogError("Cimtographer couldn't load in the roads XML file!");
            }

            return roads;
        }
    }

    [XmlRoot("RoadDefinitions", IsNullable = false)]
    public class XmlRoads
    {
        [XmlArray("Roads", IsNullable = false)]
        [XmlArrayItem("Road", IsNullable = false)]
        public XmlRoad[] roads = { new XmlRoad() };
    }
    
    public class XmlRoad
    {
        [XmlAttribute("Prefix")]
        public string prefix = "";

        [XmlAttribute("Postfix")]
        public string postfix = "";

        [XmlAttribute("LinkedOption")]
        public string linkedOption = "";

        [XmlAttribute("RoadType")]
        public string roadType = RoadContainer.Type.Unknown.ToString();

        [XmlAttribute("SearchLimit")]
        public string searchLimit = RoadContainer.Limit.None.ToString();

        [XmlArray("OSMTags", IsNullable = false)]
        [XmlArrayItem("Tag", IsNullable = false)]
        public XmlOSMTag[] tags = { new XmlOSMTag() };
    }

    public class XmlOSMTag
    {
        [XmlAttribute("Key")]
        public string key = "";

        [XmlAttribute("Value")]
        public string value = "";
    }
}
