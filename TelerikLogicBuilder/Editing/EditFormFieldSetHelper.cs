using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal class EditFormFieldSetHelper : IEditFormFieldSetHelper
    {
        private readonly IFunctionHelper _functionHelper;

        public EditFormFieldSetHelper(IFunctionHelper functionHelper)
        {
            _functionHelper = functionHelper;
        }

        public EditFormFieldSet GetFieldSetForFunction(Function function)
        {
            if (function.FunctionCategory == FunctionCategories.BinaryOperator
                && function.Parameters.Count == 2
                && _functionHelper.IsBoolean(function))
                return EditFormFieldSet.BinaryFunction;

            return function.FunctionCategory switch
            {
                FunctionCategories.Assert => EditFormFieldSet.SetValueFunction,
                FunctionCategories.Retract => EditFormFieldSet.SetValueToNullFunction,
                _ => function.ParametersLayout == ParametersLayout.Binary
                                            ? EditFormFieldSet.BinaryFunction
                                            : EditFormFieldSet.StandardFunction,
            };
        }
    }
}
