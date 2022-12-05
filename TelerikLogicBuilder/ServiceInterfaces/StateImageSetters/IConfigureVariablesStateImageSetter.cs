using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters
{
    internal interface IConfigureVariablesStateImageSetter
    {
        void SetImage(XmlElement variableElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application);
    }
}
