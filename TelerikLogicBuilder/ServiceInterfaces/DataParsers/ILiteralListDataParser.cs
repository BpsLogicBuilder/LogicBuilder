using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface ILiteralListDataParser
    {
        LiteralListData Parse(XmlElement xmlElement);

        /// <summary>
        /// Parsing for unique elements prevents duplicates when pasting XML (adding items one at a time in the literal list form already prevents duplication). 
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="listInfo"></param>
        /// <param name="applicationControl"></param>
        /// <returns></returns>
        LiteralListData Parse(XmlElement xmlElement, LiteralListParameterElementInfo listInfo, IApplicationControl applicationControl
            );

        /// <summary>
        /// Parsing for unique elements prevents duplicates when pasting XML (adding items one at a time in the literal list form already prevents duplication). 
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="listInfo"></param>
        /// <param name="applicationControl"></param>
        /// <returns></returns>
        LiteralListData Parse(XmlElement xmlElement, LiteralListVariableElementInfo listInfo, IApplicationControl applicationControl);
    }
}
