using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers
{
    internal partial class GenericParameterControlValidator : IGenericParameterControlValidator
    {
        private readonly IConfigureGenericParameterControl configureGenericParameterControl;

        private RadDropDownList CmbGpGenericArgumentName => configureGenericParameterControl.CmbGpGenericArgumentName;
        private RadLabel LblGpGenericArgumentName => configureGenericParameterControl.LblGpGenericArgumentName;

        public GenericParameterControlValidator(
            IConfigureGenericParameterControl configureGenericParameterControl)
        {
            this.configureGenericParameterControl = configureGenericParameterControl;
        }

        public void ValidateInputBoxes()
        {
            configureGenericParameterControl.ClearMessage();
            ValidateGenericArgumentName
            (
                CmbGpGenericArgumentName.SelectedItem?.Text ?? string.Empty,
                LblGpGenericArgumentName
            );
        }

        private static void ValidateGenericArgumentName(string selectedValue, RadLabel fieldLabel)
        {
            if (!GenericArgumentNameRegex().IsMatch(selectedValue))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.genericArgNameInvalidFormat, fieldLabel.Text, selectedValue));
            }
        }

        [GeneratedRegex(RegularExpressions.GENERICARGUMENTNAME)]
        private static partial Regex GenericArgumentNameRegex();
    }
}
