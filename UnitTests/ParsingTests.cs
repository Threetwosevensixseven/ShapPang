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
        public void ParseInput()
        {
            string test = "Quote(\"A Quote Item\")\r\n{	}";
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup(test);
            if (!scn.Elements.Contains("Quote"))
                throw new Exception("Element not detected correctly");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ParseBorkedInput()
        {
            string test = "!}{}54236Quorte";
            Scenario scn = new Scenario("Test");
            scn.InstallMarkup(test);
        }
    }
}
