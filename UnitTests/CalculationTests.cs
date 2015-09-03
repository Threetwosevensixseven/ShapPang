﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapPang.Classes;
using UnitTests.Helpers;

namespace UnitTests
{
    [TestClass]
    public class CalculationTests
    {
        [TestMethod]
        public void BasicXTimesY()
        {
            Scenario testScenario = new Scenario("X Times Y");
            testScenario.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"Result\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Basic.X>5</Basic.X><Basic.Y>5</Basic.Y></Givens>");
            Assert.AreEqual(25, testScenario.CalculateDerivative("Basic.PROD"));            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionIfDerivativeNotFound()
        {
            Scenario testScenario = new Scenario("X Times Y");
            testScenario.InstallMarkup("Basic()\r\n{\r\nX\r\nY\r\nPROD(\"Result\")\r\n{\r\nPROD = X * Y\r\n}\r\n}");
            testScenario.AssociateGivensFromXML("<Givens><Basic.X>5</Basic.X><Basic.Y>5</Basic.Y></Givens>");
            testScenario.CalculateDerivative("Foo.PROD");
        }
    }
}
