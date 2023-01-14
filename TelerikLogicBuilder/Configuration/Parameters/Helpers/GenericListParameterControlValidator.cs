using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers
{
    internal partial class GenericListParameterControlValidator : IGenericListParameterControlValidator
    {
        private readonly IConfigureGenericListParameterControl configureGenericListParameterControl;

        private RadDropDownList CmbListGpGenericArgumentName => configureGenericListParameterControl.CmbListGpGenericArgumentName;
        private RadLabel LblListGpGenericArgumentName => configureGenericListParameterControl.LblListGpGenericArgumentName;

        public GenericListParameterControlValidator(
            IConfigureGenericListParameterControl configureGenericListParameterControl)
        {
            this.configureGenericListParameterControl = configureGenericListParameterControl;
        }

        public void ValidateInputBoxes()
        {
            configureGenericListParameterControl.ClearMessage();
            ValidateGenericArgumentName
            (
                CmbListGpGenericArgumentName.SelectedItem?.Text ?? string.Empty,
                LblListGpGenericArgumentName
            );
        }

        private static void ValidateGenericArgumentName(string selectedValue, RadLabel fieldLabel)
        {
            if (!GenericArgumentNameRegEx().IsMatch(selectedValue))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.genericArgNameInvalidFormat, fieldLabel.Text, selectedValue));
            }
        }

        [GeneratedRegex(RegularExpressions.GENERICARGUMENTNAME)]
        private static partial Regex GenericArgumentNameRegEx();
    }
}
