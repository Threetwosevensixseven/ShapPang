using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes
{
    public class Element
    {        

        public Element(string elementName)
        {
            this.ElementName = elementName;
            this.Derivations = new List<Derivative>();
        }

        public List<Derivative> Derivations { get; set; }
        
        public string ElementName { get; set; }
    }
}
