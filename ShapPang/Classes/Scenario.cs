using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes
{
    /// <summary>
    /// A scenario defines the environment within which our calculations take place. A
    /// scenario consists of the "givens" which we associate, the calculation markup
    /// we intend to execute against the scenario, and the values of any derivatives/interim
    /// calculations which have been performed against the scenario.
    /// 
    /// Scenarios are persistable objects, and should be re-used by updating their
    /// associations when data changes.
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// The friendly name of this scenario.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the unique key for this instance of a secenario.        
        /// </summary>
        public Guid ID { get; }
    }
}
