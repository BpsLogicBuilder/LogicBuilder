using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers
{
    internal partial class ObjectParameterControlValidator : IObjectParameterControlValidator
    {
        private readonly IConfigureObjectParameterControl configureObjectParameterControl;

        private RadLabel LblCpName => configureObjectParameterControl.LblCpName;
        private RadTextBox TxtCpName => configureObjectParameterControl.TxtCpName;

        public ObjectParameterControlValidator(
            IConfigureObjectParameterControl configureObjectParameterControl)
        {
            this.configureObjectParameterControl = configureObjectParameterControl;
        }

        public void ValidateInputBoxes()
        {
            configureObjectParameterControl.ClearMessage();
            ValidateCpName();
        }

        private void ValidateCpName()
        {
            if (!XmlAttributeRegex().IsMatch(TxtCpName.Text))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, LblCpName.Text));
            }
        }

        [GeneratedRegex(RegularExpressions.XMLATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();
    }
}
