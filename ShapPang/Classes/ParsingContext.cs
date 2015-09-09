using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapPang.Classes
{
    public class ParsingContext
    {
        public Element ElementScope { get; set; }

        public Scenario Scenario { get; set; }

        public Derivative DerivationScope { get; set; }
    }
}
