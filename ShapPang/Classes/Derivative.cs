using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes
{
    /// <summary>
    /// A derivative is the output of any potential calculation within an element.
    /// </summary>
    public class Derivative
    {
        /// <summary>
        /// This is a list of all the assigments which take place while calculating this derived value.
        /// The list must contain at least one assigment to the parent value;
        /// </summary>
        public List<string> Assignments { get; set; }

        /// <summary>
        /// Constructs a derivative with the provided name
        /// </summary>
        /// <param name="derivativeName">The name to give to this derivation</param>
        public Derivative(string derivativeName)
        {
            this.Name = derivativeName;
            Assignments = new List<string>();
        }

        public string Name { get; set; }
    }
}
