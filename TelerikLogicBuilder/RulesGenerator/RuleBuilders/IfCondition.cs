using System.CodeDom;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class IfCondition
    {
        internal IfCondition(CodeExpression condition, bool not)
        {
            this.condition = condition;
            this.not = not;
        }

        internal IfCondition(CodeExpression condition)
        {
            this.condition = condition;
            this.not = false;
        }

        #region Variables
        private readonly CodeExpression condition;
        private readonly bool not;
        #endregion Variables

        #region Properties
        /// <summary>
        /// Resultant Condition i.e. whether condition is true or false
        /// </summary>
        internal CodeExpression ResultantCondition
            => not
            ? new CodeBinaryOperatorExpression
            {
                Left = condition,
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(false)
            }
            : condition;
        #endregion Properties
    }
}
