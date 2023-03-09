using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IXmlDataHelper
    {
        string BuildConstructorXml(string name, string visibleText, string genericArgumentsXml, string parametersXml);
        string BuildDefaultConstructorXml(ClosedConstructor closedConstructor);
        string BuildFunctionXml(string name, string visibleText, string genericArgumentsXml, string parametersXml);
        string BuildGenericArgumentsXml(IList<GenericConfigBase> genericArgs);
        string BuildLiteralListXml(LiteralParameterType literalType, ListType listType, string visibleText, string innerXml);
        string BuildLiteralXml(string innerXml);
        string BuildNotXml(string innerXml);
        string BuildObjectListXml(string objectType, ListType listType, string visibleText, string innerXml);
        string BuildObjectXml(string innerXml);
        string BuildParameterXml(ParameterBase parameter, string innerXml);
        string GetElementName(ParameterCategory category);
    }
}
