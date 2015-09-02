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
            //VisitChildren(context);
            return base.VisitCompileUnit(context);
        }

        public override object VisitElement(ShapPangParser.ElementContext context)
        {
            CurrentElement = new Element(context.elementname.Text);
            CurrentScenario.Elements.Add(CurrentElement);                       
            return base.VisitElement(context);            
        }

        public override object VisitDerivationdeclaration(ShapPangParser.DerivationdeclarationContext context)
        {
            CurrentDerivation = new Derivative(context.ID().GetText());
            CurrentElement.Derivations.Add(CurrentDerivation);
            return base.VisitDerivationdeclaration(context);
        }

        public override object VisitGivendeclaration(ShapPangParser.GivendeclarationContext context)
        {        
            CurrentScenario.Givens.Add(CurrentElement.ElementName+"."+context.ID().GetText(), "");
            return base.VisitGivendeclaration(context);
        }

        public override object VisitAssign(ShapPangParser.AssignContext context)
        {
            CurrentDerivation.Assignments.Add(context.ID().ToString());
            object pendingReturn = base.VisitAssign(context);
            if (!CurrentDerivation.Assignments.Contains(CurrentDerivation.Name))
            {
                throw new Exception("A derivative must contain an assignment to itself in order to set it's return value");
            }
            return pendingReturn;
        }

        /// <summary>
        /// Represents the current scenario within which this visitor is performing it's operations.
        /// </summary>
        public Scenario CurrentScenario { get; set; }

        public Element CurrentElement { get; set; }

        public Derivative CurrentDerivation { get; set; }
    }
}