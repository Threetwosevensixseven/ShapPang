using Antlr4.Runtime;
using ShapPang.Classes.Antlr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        private string name;
        private ShapPangParser parser;
        private CommonTokenStream tokens;
        private ShapPangLexer lexer;
        private AntlrInputStream input;
        private ShapPangBaseVisitor<object> visitor;

        /// <summary>
        /// Installs this scenario's markup. This will inflate the list of givens, elements and available
        /// derivatives.
        /// </summary>
        /// <param name="markup">The calculation markup to be used in this scenario.</param>
        public void InstallMarkup(string markup)
        {
            Markup = markup;
            input = new AntlrInputStream(markup);
            lexer = new ShapPangLexer(input);
            tokens = new CommonTokenStream(lexer);
            parser = new ShapPangParser(tokens);
            visitor = new ShapDiscoveryVisitor(this);
            parser.AddErrorListener(new ShapPangErrorListener());
            ShapPangParser.CompileUnitContext context = parser.compileUnit();            
            if (parser.NumberOfSyntaxErrors != 0)
                throw new Exception("Parsing failed");
            visitor.VisitCompileUnit(context);                 
        }

        /// <summary>
        /// This generates a new blank scenario with the name provided. Attempting to execute this scenario will
        /// result in an exception.
        /// </summary>
        /// <param name="Name">The name for this scenario</param>
        public Scenario(string Name)
        {
            name = Name;
            ID = Guid.NewGuid();
            Elements = new List<Element>();
            Givens = new List<Given>();
        }
        /// <summary>
        /// The friendly name of this scenario.
        /// </summary>
        public string Name { get {return name;} }

        /// <summary>
        /// This is the unique key for this instance of a secenario.        
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Represents the calculation elements that a scenario can contain. For example, a scenario
        /// could contain elements that represent a quote, quote lines and product items.
        /// </summary>
        public List<Element> Elements { get; set; }

        /// <summary>
        /// Represents the list of "givens" or facts that we know about a scenario.
        /// </summary>
        public List<Given> Givens { get; set; }

        /// <summary>
        /// This method accepts an XML formatted string and associates it with givens in the current scenario.
        /// </summary>
        /// <param name="XML">An XML formatted string</param>
        public void AssociateGivensFromXML(string XML)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XML);
            foreach (Given given in Givens)
            {
                XmlNode node = xml.SelectSingleNode("//" + given.Key);
                if (node == null)
                    continue;
                given.Value = decimal.Parse(node.FirstChild.Value.ToString());
            }
        }

        public decimal CalculateDerivative(string derivativeReference)
        {
            string dummy;
            return CalculateDerivative(derivativeReference, out dummy);
        }

        /// <summary>
        /// The markup which has been associated with this scenario.
        /// </summary>
        public string Markup { get; set; }

        /// <summary>
        /// This method associates an external source of givens (provided as a JSON formatted string).
        /// </summary>
        /// <param name="json">A JSON formatted string</param>
        public void AssociateGivensFromJSON(string json)
        {            
        }

        internal IValue ResolveReference(string reference)
        {
            IValue val = Givens.Find(t => t.Key == CurrentElement.ElementName + "." + reference);    
            if (val == null)
            {
                val = Givens.Find(t => t.Key == reference);
            }
            if (val == null)
            {            
                string[] references = reference.Split('.');
                Element el = Elements.Find(t => t.ElementName == references[0]);
                if (el == null)
                    throw new ArgumentException("Unable to reference expression element");
                val = el.Derivations.Find(t => t.Name == references[1]);
            }
            return val;
        }

        public Element CurrentElement { get; set; }

        public Derivative CurrentDerivation { get; set; }

        public decimal CalculateDerivative(string derivativeReference, out string description)
        {
            CurrentlyBuildingExplanation = "";
            string[] references = derivativeReference.Split('.');
            Element el = this.Elements.Find(t => t.ElementName == references[0]);
            if (el == null)
                throw new ArgumentException("Provided element reference does not exist in this scenario.");
            Derivative der = el.Derivations.Find(t => t.Name == references[1]);
            if (der == null)
                throw new ArgumentException("Provided derivative reference does not exist in the discovered element");
            CurrentlyBuildingExplanation += el.ElementName + " contains a " + der.Name + ", " + der.Description; 
            input = new AntlrInputStream(der.Payload);
            lexer = new ShapPangLexer(input);
            tokens = new CommonTokenStream(lexer);
            parser = new ShapPangParser(tokens);
            visitor = new ShapExecutionVisitor(this);
            parser.AddErrorListener(new ShapPangErrorListener());
            ShapPangParser.DerivationdeclarationContext context = parser.derivationdeclaration();
            this.CurrentElement = el;
            visitor.VisitDerivationdeclaration(context);
            description = CurrentlyBuildingExplanation += " yielding a value of " + CurrentDerivation.Value.ToString();
            return CurrentDerivation.Value;
        }

        public string CurrentlyBuildingExplanation { get; set; }
    }
}
