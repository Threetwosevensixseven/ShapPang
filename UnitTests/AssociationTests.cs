using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Helpers;
using ShapPang.Classes;

namespace UnitTests
{
    [TestClass]
    public class AssociationTests
    {
        [TestMethod]
        public void AssociateJSON()
        {
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"Result\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            scn.AssociateGivensFromJSON("{\r\n  \"Givens\": { \r\n \"Basic.X\": \"5\", \"Basic.Y\": \"5\"\r\n  }\r\n}");
            Assert.AreEqual("5", scn.Givens.Find(t => t.Key == "Basic.X").Value);
            Assert.AreEqual("5", scn.Givens.Find(t => t.Key == "Basic.Y").Value);
        }

        [TestMethod]
        public void AssociateXML()
        {
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"Result\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            scn.AssociateGivensFromXML("<Givens><Basic.X>5</Basic.X><Basic.Y>5</Basic.Y></Givens>");
            Assert.AreEqual(5, scn.Givens.Find(t => t.Key == "Basic.X").Value);
            Assert.AreEqual(5, scn.Givens.Find(t => t.Key == "Basic.Y").Value);
        }
    }
}
