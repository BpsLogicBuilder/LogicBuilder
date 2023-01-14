using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers
{
    internal partial class ObjectListParameterControlValidator : IObjectListParameterControlValidator
    {
        private readonly IConfigureObjectListParameterControl configureObjectListParameterControl;
        private RadLabel LblListCpName => configureObjectListParameterControl.LblListCpName;
        private RadTextBox TxtListCpName => configureObjectListParameterControl.TxtListCpName;

        public ObjectListParameterControlValidator(
            IConfigureObjectListParameterControl configureObjectListParameterControl)
        {
            this.configureObjectListParameterControl = configureObjectListParameterControl;
        }

        public void ValidateInputBoxes()
        {
            configureObjectListParameterControl.ClearMessage();
            ValidateListCpName();
        }

        private void ValidateListCpName()
        {
            if (!XmlAttributeRegex().IsMatch(TxtListCpName.Text))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, LblListCpName.Text));
            }
        }

        [GeneratedRegex(RegularExpressions.XMLATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();
    }
}
