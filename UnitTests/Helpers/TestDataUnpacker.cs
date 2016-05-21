using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UnitTests.Helpers
{
    public class TestDataUnpacker
    {
        private FileStream testDataFileStream;
        private XmlDocument xmlDoc;

        public TestDataUnpacker()
        {
            testDataFileStream = new FileStream("TestData\\TestData.xml", FileMode.Open);
            xmlDoc = new XmlDocument();
            xmlDoc.Load(testDataFileStream);
            testDataFileStream.Close();
            testDataFileStream.Dispose();
        }

        public TestData GetTestData(string TestName)
        {
            XmlNode node = xmlDoc.SelectSingleNode("//Test[@testname='" + TestName+"']");
            if (node == null)
                throw new ArgumentException("Selected test data does not exist");
            string markup = node.SelectSingleNode("Markup").FirstChild.Value;
            string XML = node.SelectSingleNode("XML").InnerXml;
            string JSON = node.SelectSingleNode("JSON").FirstChild.Value;
            string description = node.SelectSingleNode("Description").FirstChild.Value;
            TestData testdata = new TestData(markup, XML, JSON, description);
            return testdata;
        }
    }
}
