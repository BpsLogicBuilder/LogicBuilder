using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.CodeDom;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IAnyParametersHelper
    {
        AnyParameterPair GetTypes(XmlElement firstXmlParameter, XmlElement secondXmlParameter, CodeBinaryOperatorType codeBinaryOperatorType, ApplicationTypeInfo application);
    }
}
