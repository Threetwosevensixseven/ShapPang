using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Helpers
{
    /// <summary>
    /// Just a little sugar to help prevent truncated tests from passing; add this
    /// exception to any new test and remove it when the test performs all the required
    /// tasks.
    /// </summary>
    public class TestIncompleteException : Exception
    {
        public TestIncompleteException() : base("This test is incomplete") { }
    }
}
