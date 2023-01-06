using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal class IntellisenseVariableControlsValidator : IIntellisenseVariableControlsValidator
    {
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IIntellisenseVariableConfigurationControl intellisenseVariableConfigurationControl;

        public IntellisenseVariableControlsValidator(
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IIntellisenseVariableConfigurationControl intellisenseVariableConfigurationControl)
        {
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            this.intellisenseVariableConfigurationControl = intellisenseVariableConfigurationControl;
        }

        public void ValidateCastAs(VariableTreeNode treeNode)
        {
            string typeString = intellisenseVariableConfigurationControl.CmbCastVariableAs.Text.Trim();
            if (typeString.Length == 0)
                return;

            if (!_typeLoadHelper.TryGetSystemType(typeString, intellisenseVariableConfigurationControl.Application, out Type? castAsType))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, typeString));

            if (!_typeHelper.AssignableFrom(treeNode.UnCastMemberType, castAsType))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.typeNotAssignableFormat, castAsType.ToString(), treeNode.UnCastMemberType.ToString()));
        }
    }
}
