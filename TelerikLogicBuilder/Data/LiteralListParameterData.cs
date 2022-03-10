using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListParameterData
    {
        public LiteralListParameterData(string parameterName, XmlElement childElement, XmlElement literalListParameterElement, IEnumHelper enumHelper)
        {
            ParameterName = parameterName;
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            LiteralListParameterElement = literalListParameterElement;
        }

        internal string ParameterName { get; }
        internal XmlElement ChildElement { get; }
        internal ObjectCategory ChildElementCategory { get; }
        internal XmlElement LiteralListParameterElement { get; }
    }
}
