using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Helpers;
using ShapPang.Classes;

namespace UnitTests
{
    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void ParseEmptyElementInput()
        {
            string test = "Quote(\"A Quote Item\")\r\n{\r\n}";
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup(test);
            Assert.IsNotNull(scn.Elements.Find(t => t.ElementName == "Quote"));
        }

        [TestMethod]
        public void ParseBasicCalculation()
        {
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"Result\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            Assert.AreEqual("Basic", scn.Elements[0].ElementName, "Element must be detected as 'basic'");
            Assert.IsNotNull(scn.Givens.Find(t=> t.Key == "Basic.X"));
            Assert.IsNotNull(scn.Givens.Find(t => t.Key == "Basic.Y"));
            Assert.IsNotNull(scn.Elements[0].Derivations.Find(t => t.Name == "PROD"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ParseBrokenDerivativeDeclarationNoSelfAssignment()
        {
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"Result\")\r\n{\r\nFOO = X * Y\r\n}\r\n}");
            Assert.AreEqual("Basic", scn.Elements[0].ElementName, "Element must be detected as 'basic'");
            Assert.IsNotNull(scn.Givens.Find(t => t.Key == "Basic.X"));
            Assert.IsNotNull(scn.Givens.Find(t => t.Key == "Basic.Y"));            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ParseBorkedInput()
        {
            string test = "!}{}54236Quorte";
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup(test);
        }

        [TestMethod]
        public void ParseReadGivenDescription()
        {
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup("Basic()\r\n{\r\nX (\"A cartesian axis X\")\r\nY\r\nPROD(\"Result\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            Assert.AreEqual("A cartesian axis X", scn.Givens.Find(t=>t.Key == "Basic.X").Description);
        }
    }
}
