using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes.Antlr
{
    public class ShapVisitor : ShapPangBaseVisitor<object>
    {
        public ShapVisitor(Scenario scenario)
        {
            this.CurrentScenario = scenario;            
        }

        public override object VisitCompileUnit(ShapPangParser.CompileUnitContext context)
        {            
            VisitChildren(context);
            return base.VisitCompileUnit(context);
        }

        public override object VisitElement(ShapPangParser.ElementContext context)
        {
            CurrentScenario.Elements.Add(context.elementname.Text);
            VisitChildren(context);
            return base.VisitElement(context);
        }

        public override object VisitDeclaration(ShapPangParser.DeclarationContext context)
        {                       
            VisitChildren(context);
            return base.VisitDeclaration(context);
        }

        /// <summary>
        /// Represents the current scenario within which this visitor is performing it's operations.
        /// </summary>
        public Scenario CurrentScenario { get; set; }
    }
}