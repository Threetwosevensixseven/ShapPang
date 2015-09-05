using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapPang.Classes;

namespace UnitTests
{
    [TestClass]
    public class DescriptionTests
    {
        [TestMethod]
        public void BasicXTimesYIsSelfDescribing()
        {
            Scenario testScenario = new Scenario("X Times Y");
            testScenario.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"whose result is found by\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Basic.X>5</Basic.X><Basic.Y>5</Basic.Y></Givens>");
            string description = "";
            testScenario.CalculateDerivative("Basic.PROD", out description);
            Assert.AreEqual("Basic contains a PROD, whose result is found by the multiplication of the given Basic.X (5) and the given Basic.Y (5) yielding a value of 25", description);
        }

        [TestMethod]
        public void BasicXDividedByYIsSelfDescribing()
        {
            Scenario testScenario = new Scenario("X / Y");
            testScenario.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"whose result is found by\")\r\n{\r\nPROD = X \\ Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Basic.X>10</Basic.X><Basic.Y>2</Basic.Y></Givens>");
            string description = "";
            testScenario.CalculateDerivative("Basic.PROD", out description);
            Assert.AreEqual("Basic contains a PROD, whose result is found by the division of the given Basic.X (10) by the given Basic.Y (2) yielding a value of 5", description);
        }

        [TestMethod]
        public void BasicXMinusYIsSelfDescribing()
        {
            Scenario testScenario = new Scenario("X Minus Y");
            testScenario.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"whose result is found by\")\r\n{\r\nPROD = X - Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Basic.X>5</Basic.X><Basic.Y>5</Basic.Y></Givens>");
            string description = "";
            testScenario.CalculateDerivative("Basic.PROD", out description);
            Assert.AreEqual("Basic contains a PROD, whose result is found by the subtraction of the given Basic.Y (5) from the given Basic.X (5) yielding a value of 0", description);
        }

        [TestMethod]
        public void BasicXAddYIsSelfDescribing()
        {
            Scenario testScenario = new Scenario("X Add Y");
            testScenario.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"whose result is found by\")\r\n{\r\nPROD = X + Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Basic.X>5</Basic.X><Basic.Y>5</Basic.Y></Givens>");
            string description = "";
            testScenario.CalculateDerivative("Basic.PROD", out description);
            Assert.AreEqual("Basic contains a PROD, whose result is found by the addition of the given Basic.Y (5) to the given Basic.X (5) yielding a value of 10", description);
        }
    }
}
