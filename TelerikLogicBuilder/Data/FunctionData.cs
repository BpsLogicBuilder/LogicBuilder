using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class FunctionData
    {
        internal FunctionData(string name, string visibleText, List<GenericConfigBase> genericArguments, List<XmlElement> parameterElementsList, XmlElement functionElement, bool isNotFunction)
        {
            this.Name = name;
            this.GenericArguments = genericArguments;
            this.ParameterElementsList = parameterElementsList;
            this.FunctionElement = functionElement;
            this.VisibleText = visibleText;
            this.IsNotFunction = isNotFunction;
        }

        #region Properties
        internal bool IsNotFunction { get; }
        internal string Name { get; }
        internal string VisibleText { get; }
        internal List<GenericConfigBase> GenericArguments { get; }
        internal List<XmlElement> ParameterElementsList { get; }
        internal XmlElement FunctionElement { get; }
        #endregion Properties
    }
}
