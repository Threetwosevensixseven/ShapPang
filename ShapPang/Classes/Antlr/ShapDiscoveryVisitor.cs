using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes.Antlr
{
    public class ShapDiscoveryVisitor : ShapPangBaseVisitor<object>
    {
        public ShapDiscoveryVisitor(ParsingContext context)
        {
            this.Context = context;
        }

        public override object VisitCompileUnit(ShapPangParser.CompileUnitContext context)
        {            
            return base.VisitCompileUnit(context);
        }

        public override object VisitElement(ShapPangParser.ElementContext context)
        {
            Element el = new Element(context.elementname.Text);
            Context.Scenario.AddElement(el);
            Context.ElementScope = el; 
            return base.VisitElement(context);            
        }

        public override object VisitDerivationdeclaration(ShapPangParser.DerivationdeclarationContext context)
        {
            Derivative der = new Derivative(context.ID().GetText(), context.description.Text.Substring(1, context.description.Text.Length-2), context.GetText(), Context.Scenario, Context.ElementScope);
            Context.Scenario.AddDerivation(der);
            Context.DerivationScope = der;
            object pendingReturn = base.VisitDerivationdeclaration(context);
            if (!der.Assignments.Contains(der.Name))
            {
                throw new Exception("A derivative must contain an assignment to itself in order to set it's return value");
            }
            return pendingReturn;
        }

        public override object VisitGivendeclaration(ShapPangParser.GivendeclarationContext context)
        {
            Given given = new Given(Context.ElementScope.ElementName + "." + context.ID().GetText(),0);
            Context.Scenario.AddGiven(given);
            if (context.description != null)
                given.Description = context.description.Text.Substring(1, context.description.Text.Length -2);
            return base.VisitGivendeclaration(context);
        }

        public override object VisitAssign(ShapPangParser.AssignContext context)
        {
            Context.DerivationScope.AddAssignment(context.ID().ToString());
            return base.VisitAssign(context);
        }

        public override object VisitExpressionReference(ShapPangParser.ExpressionReferenceContext context)
        {
                return base.VisitExpressionReference(context);          
        }

        internal ParsingContext Context { get; set; }
    }
}