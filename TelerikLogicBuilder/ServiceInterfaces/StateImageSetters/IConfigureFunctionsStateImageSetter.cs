using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters
{
    internal interface IConfigureFunctionsStateImageSetter
    {
        void SetImage(XmlElement functionElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application);
    }
}
