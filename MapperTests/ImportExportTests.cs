using Mapper.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Serialization;

namespace MapperTests
{
    [TestClass()]
    public class ImportExportTests
    {
        [TestMethod()]
        public void RoadExport()
        {
            XmlSerializer _serialiser = new XmlSerializer(typeof(XmlRoads));
            TextWriter _xmlWriter = new StreamWriter("exportedXML.xml");
            
            _serialiser.Serialize(_xmlWriter, new XmlRoads());
        }

        [TestMethod()]
        public void RoadImport()
        {
            XmlSerializer _serialiser = new XmlSerializer(typeof(XmlRoads));
            TextReader _xmlReader = new StreamReader("importXML.xml");

            XmlRoads roads = _serialiser.Deserialize(_xmlReader) as XmlRoads;
        }
    }
}
