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
            if (CurrentScenario.Process)
            {
                CurrentScenario.CurrentDerivation = CurrentScenario.CurrentElement.Derivations.Find(t => t.Name == context.ID().GetText());
                return base.VisitDerivationdeclaration(context);
            }
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
            if (CurrentScenario.Process)
                return base.VisitGivendeclaration(context);
            Given given = new Given(CurrentScenario.CurrentElement.ElementName + "." + context.ID().GetText(),0);
            CurrentScenario.Givens.Add(given);
            if (context.description != null)
                given.Description = context.description.Text.Substring(1, context.description.Text.Length -2);
            return base.VisitGivendeclaration(context);
        }

        public override object VisitAssign(ShapPangParser.AssignContext context)
        {
            if (!CurrentScenario.Process)
            {
                CurrentScenario.CurrentDerivation.Assignments.Add(context.ID().ToString());
                return base.VisitAssign(context);
            }
            else
            {
                if (context.ID().GetText() == CurrentScenario.CurrentDerivation.Name)
                {
                    CurrentScenario.CurrentDerivation.Value = (decimal)base.VisitAssign(context);
                    return CurrentScenario.CurrentDerivation.Value;
                }
                IValue val = CurrentScenario.ResolveReference(context.ID().GetText());
                return base.VisitAssign(context);
            }
        }

        public override object VisitExpressionMultiply(ShapPangParser.ExpressionMultiplyContext context)
        {            
            if (!CurrentScenario.Process)
                return base.VisitExpressionMultiply(context);
            CurrentScenario.CurrentlyBuildingExplanation += " the multiplication of";
            decimal left = (decimal)base.Visit(context.left);
            CurrentScenario.CurrentlyBuildingExplanation += " and";
            decimal right = (decimal)base.Visit(context.right);
            return left * right;
        }

        public override object VisitExpressionReference(ShapPangParser.ExpressionReferenceContext context)
        {
            if (!CurrentScenario.Process)
                return base.VisitExpressionReference(context);
            IValue val = CurrentScenario.ResolveReference(context.ID().GetText());
            string temp = val.GetType().ToString();
            switch (val.GetType().ToString())
            {
                case "ShapPang.Classes.Given":
                    Given giv = (Given)val;
                    if (giv.Description == null)
                        CurrentScenario.CurrentlyBuildingExplanation += " the given " + giv.Key + " (" + giv.Value.ToString() + ")";
                    else
                        CurrentScenario.CurrentlyBuildingExplanation += giv.Description;
                    break;
                case "ShapPang.Classes.Derivative":
                    Derivative div = (Derivative)val;
                    CurrentScenario.CurrentlyBuildingExplanation += div.Description + " ("+div.Value.ToString()+")";
                    break;
            }
            return val.Value;
        }

        /// <summary>
        /// Represents the current scenario within which this visitor is performing it's operations.
        /// </summary>
        public Scenario CurrentScenario { get; set; }
    }
}