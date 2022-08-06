using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal class FunctionsFormFieldSetHelper : IFunctionsFormFieldSetHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionHelper _functionHelper;
        private readonly ITreeViewService _treeViewService;

        public FunctionsFormFieldSetHelper(
            IConfigurationService configurationService,
            IFunctionHelper functionHelper,
            ITreeViewService treeViewService)
        {
            _configurationService = configurationService;
            _functionHelper = functionHelper;
            _treeViewService = treeViewService;
        }

        public FunctionsFormFieldSet GetFunctionsFormFieldSet(Function function)
        {
            if (function.FunctionCategory == FunctionCategories.BinaryOperator
                && function.Parameters.Count == 2
                && function.ReturnType.ReturnTypeCategory == ReturnTypeCategory.Literal
                && _functionHelper.IsBoolean(function))
                return FunctionsFormFieldSet.BinaryLayout;
            else if (function.FunctionCategory == FunctionCategories.Assert)
                return FunctionsFormFieldSet.SetValue;
            else if (function.FunctionCategory == FunctionCategories.Retract)
                return FunctionsFormFieldSet.SetValueToNull;
            else if (function.ParametersLayout == ParametersLayout.Binary)
                return FunctionsFormFieldSet.BinaryLayout;
            else
                return FunctionsFormFieldSet.Standard;
        }

        public FunctionsFormFieldSet GetFunctionsFormFieldSet(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsMethodNode(treeNode))
                return FunctionsFormFieldSet.Folder;

            return GetFunctionsFormFieldSet(_configurationService.FunctionList.Functions[treeNode.Text]);
        }
    }
}
