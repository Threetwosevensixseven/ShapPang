using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapPang.Classes;
using UnitTests.Helpers;

namespace UnitTests
{
    [TestClass]
    public class CalculationTests
    {
        TestDataUnpacker unpack = new TestDataUnpacker();

        [TestMethod]
        public void IndirectionTest()
        {
            Scenario testScenario = new Scenario("IndirectionTest");
            TestData data = unpack.GetTestData("IndirectionTest");
            testScenario.InstallMarkup(data.Markup);
            testScenario.AssociateGivensFromXML(data.XML);
            Assert.AreEqual(25, testScenario.CalculateDerivative("Other.FOO"));            
        }

        [TestMethod]
        public void BasicXTimesY()
        {
            Scenario testScenario = new Scenario("X Times Y");
            TestData data = unpack.GetTestData("BasicXTimesY");
            testScenario.InstallMarkup(data.Markup);
            testScenario.AssociateGivensFromXML(data.XML);
            Assert.AreEqual(25, testScenario.CalculateDerivative("Basic.PROD"));
        }

        [TestMethod]
        public void BasicXDividedByY()
        {
            Scenario testScenario = new Scenario("X Divided By Y");
            TestData data = unpack.GetTestData("BasicXDividedByY");
            testScenario.InstallMarkup(data.Markup);
            testScenario.AssociateGivensFromXML(data.XML);
            Assert.AreEqual(5, testScenario.CalculateDerivative("Basic.PROD"));
        }

        [TestMethod]
        public void BasicXMinusY()
        {
            Scenario testScenario = new Scenario("X Subtract Y");
            TestData data = unpack.GetTestData("BasicXMinusY");
            testScenario.InstallMarkup(data.Markup);
            testScenario.AssociateGivensFromXML(data.XML);
            Assert.AreEqual(0, testScenario.CalculateDerivative("Basic.PROD"));
        }

        [TestMethod]
        public void BasicXAddY()
        {
            Scenario testScenario = new Scenario("X Add Y");
            TestData data = unpack.GetTestData("BasicXAddY");
            testScenario.InstallMarkup(data.Markup);
            testScenario.AssociateGivensFromXML(data.XML);
            Assert.AreEqual(10, testScenario.CalculateDerivative("Basic.PROD"));
        }

        [TestMethod]
        public void TestGivenReferenceInSeperateElement()
        {
            Scenario testScenario = new Scenario("X Times Y");
            testScenario.InstallMarkup("Far()\r\n{\r\nX\r\n}\r\nBasic()\r\n{\r\nY\r\nPROD(\"whose result is found by\")\r\n{\r\nPROD = Far.X * Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Far.X>5</Far.X><Basic.Y>5</Basic.Y></Givens>");
            Assert.AreEqual(25, testScenario.CalculateDerivative("Basic.PROD"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionIfDerivativeNotFound()
        {
            Scenario testScenario = new Scenario("X Times Y");
            TestData data = unpack.GetTestData("BasicXTimesY");
            testScenario.InstallMarkup(data.Markup);
            testScenario.AssociateGivensFromXML(data.XML);
            testScenario.CalculateDerivative("Foo.PROD");
        }
    }
}
