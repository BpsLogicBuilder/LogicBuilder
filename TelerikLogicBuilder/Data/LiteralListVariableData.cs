using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListVariableData
    {
        public LiteralListVariableData(XmlElement childElement, XmlElement literalListVariableElement, IEnumHelper enumHelper)
        {
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            LiteralListVariableElement = literalListVariableElement;
        }

        internal XmlElement ChildElement { get; }
        internal ObjectCategory ChildElementCategory { get; }
        internal XmlElement LiteralListVariableElement { get; }
    }
}
