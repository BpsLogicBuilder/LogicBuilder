using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters
{
    internal interface IConfigureParametersStateImageSetter
    {
        void SetImage(XmlElement parameterElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application);
    }
}
