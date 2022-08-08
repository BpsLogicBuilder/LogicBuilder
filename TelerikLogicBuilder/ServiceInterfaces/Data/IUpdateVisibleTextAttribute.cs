using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IUpdateVisibleTextAttribute
    {
        void UpdateAssertFunctionVisibleText(XmlElement functionElement, ApplicationTypeInfo application);
        void UpdateConstructorVisibleText(XmlElement constructorElement, ApplicationTypeInfo application);
        void UpdateDecisionVisibleText(XmlElement decisionElement, ApplicationTypeInfo application);
        void UpdateFunctionVisibleText(XmlElement functionElement, ApplicationTypeInfo application);
        void UpdateLiteralListVisibleText(XmlElement literalListElement, ApplicationTypeInfo application, string? literalListParameterName);
        void UpdateObjectListVisibleText(XmlElement objectListElement, ApplicationTypeInfo application, string? objectListParameterName);
        void UpdateRetractFunctionVisibleText(XmlElement functionElement);
        void UpdateVariableVisibleText(XmlElement variableElement);
    }
}
