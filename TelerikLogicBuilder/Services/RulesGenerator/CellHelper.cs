using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class CellHelper : ICellHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public CellHelper(ICellXmlHelper cellXmlHelper, IConfigurationService configurationService, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _cellXmlHelper = cellXmlHelper;
            _configurationService = configurationService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public int CountDialogFunctions(object fieldValue)
        {
            int returnValue = 0;
            string functionsXml = _cellXmlHelper.GetXmlString((string)fieldValue, SchemaName.FunctionsDataSchema);
            if (functionsXml.Length == 0)
                return returnValue;

            return _xmlDocumentHelpers.SelectElements
            (
                _xmlDocumentHelpers.ToXmlDocument(functionsXml),
                $"//{XmlDataConstants.FUNCTIONELEMENT}"
            )
            .Select
            (
                element => element.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value
            )
            .Aggregate
            (
                0,
                (count, functionName) =>
                {
                    if (_configurationService.FunctionList.Functions.TryGetValue(functionName, out Function? function)
                            && function.FunctionCategory == FunctionCategories.DialogForm)
                        count++;

                    return count;
                }
            );
        }
    }
}
