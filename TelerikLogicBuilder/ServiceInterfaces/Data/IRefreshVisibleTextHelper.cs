using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IRefreshVisibleTextHelper
    {
        XmlDocument RefreshConstructorVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlDocument RefreshDecisionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlDocument RefreshFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlDocument RefreshLiteralListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlDocument RefreshObjectListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlDocument RefreshSetValueFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application);
        XmlDocument RefreshSetValueToNullFunctionVisibleTexts(XmlDocument xmlDocument);
        XmlDocument RefreshVariableVisibleTexts(XmlDocument xmlDocument);
    }
}
