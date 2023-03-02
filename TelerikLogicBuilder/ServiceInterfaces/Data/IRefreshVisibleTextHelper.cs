using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IRefreshVisibleTextHelper
    {
        XmlElement RefreshAllVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshConstructorVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlElement RefreshConstructorVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshDecisionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlElement RefreshDecisionVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlElement RefreshFunctionVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshLiteralListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlElement RefreshLiteralListVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshObjectListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlElement RefreshObjectListVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshSetValueFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlElement RefreshSetValueFunctionVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application);
        XmlDocument RefreshSetValueToNullFunctionVisibleTexts(XmlDocument xmlDocument);
        XmlElement RefreshSetValueToNullFunctionVisibleTexts(XmlElement xmlElement);
        XmlDocument RefreshVariableVisibleTexts(XmlDocument xmlDocument);
        XmlElement RefreshVariableVisibleTexts(XmlElement xmlElement);
    }
}
