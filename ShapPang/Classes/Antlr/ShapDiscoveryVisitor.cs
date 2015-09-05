using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapPang.Classes.Antlr
{
    public class ShapDiscoveryVisitor : ShapPangBaseVisitor<object>
    {
        public ShapDiscoveryVisitor(Scenario scenario)
        {
            this.CurrentScenario = scenario;            
        }

        public override object VisitCompileUnit(ShapPangParser.CompileUnitContext context)
        {            
            return base.VisitCompileUnit(context);
        }

        public override object VisitElement(ShapPangParser.ElementContext context)
        {
            CurrentScenario.CurrentElement = new Element(context.elementname.Text);
            CurrentScenario.Elements.Add(CurrentScenario.CurrentElement);                       
            return base.VisitElement(context);            
        }

        public override object VisitDerivationdeclaration(ShapPangParser.DerivationdeclarationContext context)
        {
            CurrentScenario.CurrentDerivation = new Derivative(context.ID().GetText(), context.description.Text.Substring(1, context.description.Text.Length-2), context.GetText());
            CurrentScenario.CurrentElement.Derivations.Add(CurrentScenario.CurrentDerivation);
            object pendingReturn = base.VisitDerivationdeclaration(context);
            if (!CurrentScenario.CurrentDerivation.Assignments.Contains(CurrentScenario.CurrentDerivation.Name))
            {
                throw new Exception("A derivative must contain an assignment to itself in order to set it's return value");
            }
            return pendingReturn;
        }

        public override object VisitGivendeclaration(ShapPangParser.GivendeclarationContext context)
        {
            Given given = new Given(CurrentScenario.CurrentElement.ElementName + "." + context.ID().GetText(),0);
            CurrentScenario.Givens.Add(given);
            if (context.description != null)
                given.Description = context.description.Text.Substring(1, context.description.Text.Length -2);
            return base.VisitGivendeclaration(context);
        }

        public override object VisitAssign(ShapPangParser.AssignContext context)
        {
            CurrentScenario.CurrentDerivation.Assignments.Add(context.ID().ToString());
            return base.VisitAssign(context);
        }

        public override object VisitExpressionReference(ShapPangParser.ExpressionReferenceContext context)
        {
                return base.VisitExpressionReference(context);          
        }

        /// <summary>
        /// Represents the current scenario within which this visitor is performing it's operations.
        /// </summary>
        public Scenario CurrentScenario { get; set; }
    }
}