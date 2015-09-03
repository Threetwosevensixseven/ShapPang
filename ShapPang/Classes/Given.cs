using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes
{
    /// <summary>
    /// A "Given" is a specific fact we know about a scenario before we attempt
    /// any derivations using a ShapPang markup. Givens may represent nested collections or
    /// simple name/value pairs.
    /// </summary>
    public class Given:IValue
    {

        public Given(string key, decimal value)
        {
            // TODO: Complete member initialization
            this.Key = key;
            this.Value = value;
        }
        public string Key { get; set; }

        public string Description { get; set; }
    }
}
