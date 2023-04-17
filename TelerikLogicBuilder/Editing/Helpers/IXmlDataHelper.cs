using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IXmlDataHelper
    {
        string BuildAssertFunctionXml(string name, string visibleText, string variableName, string variableValueInnerXml);
        string BuildConstructorXml(string name, string visibleText, string genericArgumentsXml, string parametersXml);
        string BuildDecisionsXml(string innerXml);
        string BuildDefaultConstructorXml(ClosedConstructor closedConstructor);
        string BuildEmptyAssertFunctionXml(string name);
        string BuildEmptyConstructorXml(string name, string visibleText);
        string BuildEmptyFunctionXml(string name, string visibleText);
        string BuildEmptyRetractFunctionXml(string name);
        string BuildFunctionsXml(string innerXml);
        string BuildFunctionXml(string name, string visibleText, string genericArgumentsXml, string parametersXml);
        string BuildGenericArgumentsXml(IList<GenericConfigBase> genericArgs);
        string BuildLiteralListXml(LiteralListElementType literalType, ListType listType, string visibleText, string innerXml);
        string BuildLiteralXml(string innerXml);
        string BuildNotXml(string innerXml);
        string BuildObjectListXml(string objectType, ListType listType, string visibleText, string innerXml);
        string BuildObjectXml(string innerXml);
        string BuildParameterXml(ParameterBase parameter, string innerXml);
        string BuildRetractFunctionXml(string name, string visibleText, string variableName);
        string BuildVariableValueXml(VariableBase variable, string innerXml);
        string BuildVariableXml(string innerXml);
        string GetElementName(ParameterCategory category);
    }
}
