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
        /// <summary>
        /// Name attribute of <assertFunction></assertFunction> element
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// VisibleText attribute of <assertFunction></assertFunction> element
        /// </summary>
        internal string VisibleText { get; }

        /// <summary>
        /// <variable></variable> element
        /// </summary>
        internal XmlElement VariableElement { get; }

        /// <summary>
        /// <variableValue></variableValue> element
        /// </summary>
        internal XmlElement VariableValueElement { get; }

        /// <summary>
        /// <assertFunction></assertFunction> element
        /// </summary>
        internal XmlElement AssertFunctionElement { get; }
        #endregion Properties
    }
}
