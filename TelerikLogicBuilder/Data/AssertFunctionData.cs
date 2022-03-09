using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class AssertFunctionData
    {
        internal AssertFunctionData(string name, string visibleText, XmlElement variableElement, XmlElement variableValueElement, XmlElement assertFunctionElement)
        {
            this.Name = name;
            this.VisibleText = visibleText;
            this.VariableElement = variableElement;
            this.VariableValueElement = variableValueElement;
            this.AssertFunctionElement = assertFunctionElement;
        }

        #region Properties
        internal string Name { get; }
        internal string VisibleText { get; }
        internal XmlElement VariableElement { get; }
        internal XmlElement VariableValueElement { get; }
        internal XmlElement AssertFunctionElement { get; }
        #endregion Properties
    }
}
