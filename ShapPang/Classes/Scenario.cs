using Antlr4.Runtime;
using ShapPang.Classes.Antlr;
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
        private string name;
        private ShapPangParser parser;
        private CommonTokenStream tokens;
        private ShapPangLexer lexer;
        private AntlrInputStream input;
        private ShapVisitor visitor;

        public void InstallMarkup(string markup)
        {
            input = new AntlrInputStream(markup);
            lexer = new ShapPangLexer(input);
            tokens = new CommonTokenStream(lexer);
            parser = new ShapPangParser(tokens);
            visitor = new ShapVisitor(this);
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
            Elements = new List<string>();
        }
        /// <summary>
        /// The friendly name of this scenario.
        /// </summary>
        public string Name { get {return name;} }

        /// <summary>
        /// This is the unique key for this instance of a secenario.        
        /// </summary>
        public Guid ID { get; set; }


        public List<string> Elements { get; set; }
    }
}
