using Antlr4.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShapPang.Classes.Antlr;
using System;
using System.Collections.Generic;
using System.IO;
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
        private ParsingContext parseContext = new ParsingContext();

        private Stack<ParsingContext> scenarioStack = new Stack<ParsingContext>();
        /// <summary>
        /// Installs this scenario's markup. This will inflate the list of givens, elements and available
        /// derivatives.
        /// </summary>
        /// <param name="markup">The calculation markup to be used in this scenario.</param>
        public void InstallMarkup(string markup)
        {
            parseContext.Scenario = this;
            Markup = markup;
            input = new AntlrInputStream(markup);
            lexer = new ShapPangLexer(input);
            tokens = new CommonTokenStream(lexer);
            parser = new ShapPangParser(tokens);
            visitor = new ShapDiscoveryVisitor(parseContext);
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
            elements = new List<Element>();
            givens = new List<Given>();
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
        public List<Element> elements = new List<Element>();

        public List<Element> Elements { get { return elements; } }

        /// <summary>
        /// Represents the list of "givens" or facts that we know about a scenario.
        /// </summary>
        public List<Given> givens = new List<Given>();

        public List<Given> Givens { get { return givens; } }

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
            JObject j = JObject.Parse(json);
            JsonReader reader = j.CreateReader();
            
        }

        /// <summary>
        /// Resolves a provided reference of forms "X" or "X.Y" and passes back the first given or 
        /// derivation which matches.
        /// </summary>
        /// <param name="reference">The string reference for the given or derivative</param>
        /// <returns>An IValue representing either the given or derivation the reference points to.</returns>
        internal IValue ResolveReference(string reference, string ElementScope)
        {
            //Find near givens with a full reference first.
            IValue val = Givens.Find(t => t.Key == ElementScope + "." + reference);    
            if (val == null)
            {
                //Ok, try near givens with a partial reference
                val = Givens.Find(t => t.Key == reference);
            }
            //Ok, givens exhausted; find me a derivation with that reference.
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

        /// <summary>
        /// This method takes a reference to a derivative and forces a calculation of it's value.
        /// </summary>
        /// <param name="derivativeReference">A reference in the form "X.Y"</param>
        /// <param name="description">A description string to be filled with the derivations method.</param>
        /// <returns>The decimal value of the calculation.</returns>
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
            der.CalculateDerivative();          
            description = CurrentlyBuildingExplanation += " yielding a value of " + der.Value.ToString();
            return der.Value;
        }

        public string CurrentlyBuildingExplanation { get; set; }

        internal void AddElement(Element el)
        {
            this.Elements.Add(el);
        }

        internal void AddDerivation(Derivative der)
        {
            parseContext.ElementScope.Derivations.Add(der);
        }

        internal void AddGiven(Given given)
        {
            Givens.Add(given);
        }
    }
}
