using Antlr4.Runtime;
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
    public class Derivative:IValue
    {
        private ShapPangLexer lexer;
        private AntlrInputStream input;
        private CommonTokenStream tokens;
        private ShapPangParser parser;
        private ShapExecutionVisitor visitor;
        /// <summary>
        /// This is a list of all the assigments which take place while calculating this derived value.
        /// The list must contain at least one assigment to the parent value;
        /// </summary>
        public List<string> Assignments { get; set; }

        /// <summary>
        /// Constructs a derivative with the provided name
        /// </summary>
        /// <param name="derivativeName">The name to give to this derivation</param>
        public Derivative(string derivativeName, string description, string payload, Scenario OwningScenario, Element element)
        {
            this.Name = derivativeName;
            this.Payload = payload;
            this.Description = description;
            Assignments = new List<string>();
            this.Element = element;
            this.Scenario = OwningScenario;
            Calculated = false;
        }

        /// <summary>
        /// The name of this derivation
        /// </summary>
        public string Name { get; set; }        

        /// <summary>
        /// The markup payload which this derivation executes in order to determine its value.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// The human readable description of this derivation.
        /// </summary>
        public string Description { get; set; }

        internal void CalculateDerivative()
        {
            
            input = new AntlrInputStream(this.Payload);
            lexer = new ShapPangLexer(input);
            tokens = new CommonTokenStream(lexer);
            parser = new ShapPangParser(tokens);
            visitor = new ShapExecutionVisitor(this);
            parser.AddErrorListener(new ShapPangErrorListener());
            ShapPangParser.DerivationdeclarationContext context = parser.derivationdeclaration();
            visitor.VisitDerivationdeclaration(context);
            Calculated = true;
        }


        public Scenario Scenario { get; set; }

        public bool Calculated { get; set; }

        public Element Element { get; set; }
    }
}
