using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Helpers
{
    public class TestData
    {
        public string Markup {get;set;}
        public string XML { get; set; }
        public string JSON { get; set; }
        public string Description { get; set; }

        public TestData(string markup, string XML, string JSON, string Description)
        {
            this.Markup = markup;
            this.XML = XML;
            this.JSON = JSON;
            this.Description = Description;
        }
    }
}
