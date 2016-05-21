using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes
{
    /// <summary>
    /// An element describes an object that encapsulates givens and derivations.
    /// </summary>
    public class Element
    {        
        public Element(string elementName)
        {
            this.ElementName = elementName;
            this.Derivations = new List<Derivative>();
        }

        /// <summary>
        /// The derivations associated with this element.
        /// </summary>
        public List<Derivative> Derivations { get; set; }
        
        /// <summary>
        /// The name of this element.
        /// </summary>
        public string ElementName { get; set; }
    }
}
