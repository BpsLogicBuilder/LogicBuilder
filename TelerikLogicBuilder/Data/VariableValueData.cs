using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class VariableValueData
    {
        public VariableValueData(XmlElement childElement, XmlElement variableValueElement, IEnumHelper enumHelper)
        {
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetVariableTypeCategory(ChildElement.Name);
            VariableValueElement = variableValueElement;
        }

        /// <summary>
        /// Child element of  variableValue <variableValue></variableValue> (literalVariable, objectVariable, literalListVariable, objectListVariable)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (literalVariable, objectVariable, literalListVariable, objectListVariable)
        /// </summary>
        internal VariableTypeCategory ChildElementCategory { get; }

        /// <summary>
        /// <variableValue></variableValue> element
        /// </summary>
        internal XmlElement VariableValueElement { get; }
    }
}
