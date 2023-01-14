using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal partial class ConstructorControlsValidator : IConstructorControlsValidator
    {
        private readonly IConfigureConstructorControl configureConstructorControl;

        public ConstructorControlsValidator(
            IConfigureConstructorControl configureConstructorControl)
        {
            this.configureConstructorControl = configureConstructorControl;
        }

        public void ValidateInputBoxes()
        {
            this.configureConstructorControl.ClearMessage();
            ValidateConstructorName();
        }

        private void ValidateConstructorName()
        {
            if (!XmlAttributeRegex().IsMatch(configureConstructorControl.TxtConstructorName.Text))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidTxtNameTextFormat, configureConstructorControl.LblConstructorName.Text));
            }
        }

        [GeneratedRegex(RegularExpressions.XMLNAMEATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();
    }
}
