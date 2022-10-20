using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface ICodeExpressionBuilder
    {
        /// <summary>
        /// Result of multiple binary operations where operatorType == CodeBinaryOperatorType.BooleanAnd or operatorType == CodeBinaryOperatorType.BooleanOr
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        CodeExpression AggregateConditions(IEnumerable<IfCondition> conditions, CodeBinaryOperatorType operatorType);

        /// <summary>
        /// Builds a Code Assign Statement given the value data and the variable
        /// </summary>
        /// <param name="valueData"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        CodeStatement BuildAssignmentStatement(VariableValueData valueData, VariableBase variable);

        /// <summary>
        /// Builds a Code Assign Statement to set a variable to null.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        CodeStatement BuildAssignToNullStatement(VariableBase variable);

        /// <summary>
        /// Returns a Code Binary Operator Expression for property == propertyValue test
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        CodeExpression BuildDirectorPropertyCondition(string property, CodeExpression propertyValue);

        /// <summary>
        /// Returns a Code Binary Operator Expression for the Driver condition given shape index and page index
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        CodeExpression BuildDriverCondition(int shapeIndex, int pageIndex);

        /// <summary>
        /// Builds a Method Invoke Expression given function data
        /// </summary>
        /// <param name="functionData"></param>
        /// <param name="connectorDataList"></param>
        /// <returns></returns>
        /// <exception cref="CriticalLogicBuilderException"></exception>
        CodeExpression BuildFunction(FunctionData functionData, IList<ConnectorData>? connectorDataList);

        /// <summary>
        /// returns If Condition given decision data
        /// </summary>
        /// <param name="decisionData"></param>
        /// <returns></returns>
        IfCondition BuildIfCondition(DecisionData decisionData);

        /// <summary>
        /// Returns If Condition given function data
        /// </summary>
        /// <param name="functionData"></param>
        /// <returns></returns>
        IfCondition BuildIfCondition(FunctionData functionData);

        /// <summary>
        /// Returns a Code Binary Operator Expression for the Selection condition given connector XML
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        CodeExpression BuildSelectCondition(XmlNode connectorXmlNode);

        /// <summary>
        /// Returns a Code Binary Operator Expression for the Selection condition given a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        CodeExpression BuildSelectCondition(string text);
    }
}
