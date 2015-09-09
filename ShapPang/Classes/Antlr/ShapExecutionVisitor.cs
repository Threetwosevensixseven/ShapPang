using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapPang.Classes
{
    class ShapExecutionVisitor : ShapPangBaseVisitor<object>
    {
        public ShapExecutionVisitor(Derivative derivative, ParsingContext Context)
        {
            this.CurrentDerivation = derivative;
            this.ParsingContext = Context;
            CurrentScenario = derivative.Scenario;
        }

        public override object VisitDerivationdeclaration(ShapPangParser.DerivationdeclarationContext context)
        {            
            return base.VisitDerivationdeclaration(context);
        }

        public override object VisitGivendeclaration(ShapPangParser.GivendeclarationContext context)
        {
            return base.VisitGivendeclaration(context);
        }

        public override object VisitAssign(ShapPangParser.AssignContext context)
        {
            if (context.ID().GetText() == CurrentDerivation.Name)
            {
                CurrentDerivation.Value = (decimal)base.VisitAssign(context);
                return CurrentDerivation.Value;
            }
            IValue val = CurrentScenario.ResolveReference(context.ID().GetText(), ParsingContext.ElementScope.ElementName);
            return base.VisitAssign(context);
        }

        public override object VisitExpressionMultiply(ShapPangParser.ExpressionMultiplyContext context)
        {
            CurrentScenario.CurrentlyBuildingExplanation += " the multiplication of";
            decimal left = (decimal)base.Visit(context.left);
            CurrentScenario.CurrentlyBuildingExplanation += " and";
            decimal right = (decimal)base.Visit(context.right);
            return left * right;
        }

        public override object VisitExpressionAddMinus(ShapPangParser.ExpressionAddMinusContext context)
        {
            bool adding = false;
            if (context.@operator.Text == "+")
                adding = true;
            if (adding)
                CurrentScenario.CurrentlyBuildingExplanation += " the addition of";
            else
                CurrentScenario.CurrentlyBuildingExplanation += " the subtraction of";
            decimal right = (decimal)base.Visit(context.right);
            if (adding)
                CurrentScenario.CurrentlyBuildingExplanation += " to";
            else
                CurrentScenario.CurrentlyBuildingExplanation += " from";
            decimal left = (decimal)base.Visit(context.left);            
            if (adding)
                return left + right;
            else
                return left - right;
        }

        public override object VisitExpressionDivide(ShapPangParser.ExpressionDivideContext context)
        {
            CurrentScenario.CurrentlyBuildingExplanation += " the division of";
            decimal left = (decimal)base.Visit(context.left);
            CurrentScenario.CurrentlyBuildingExplanation += " by";
            decimal right = (decimal)base.Visit(context.right);
            return left / right;
        }

        public override object VisitExpressionReference(ShapPangParser.ExpressionReferenceContext context)
        {
            IValue val = CurrentScenario.ResolveReference(context.ID().GetText(), ParsingContext.ElementScope.ElementName);
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
                    if (div.Calculated == false)
                        div.CalculateDerivative();
                    CurrentScenario.CurrentlyBuildingExplanation += div.Description + " (" + div.Value.ToString() + ")";
                    break;
            }
            return val.Value;
        }

        /// <summary>
        /// Represents the current scenario within which this visitor is performing it's operations.
        /// </summary>
        public Scenario CurrentScenario { get; set; }

        public Derivative CurrentDerivation { get; set; }

        public ParsingContext ParsingContext { get; set; }
    }
}