using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories
{
    internal class TypeAutoCompleteCommandFactory : ITypeAutoCompleteCommandFactory
    {
        private readonly Func<IApplicationForm, ITypeAutoCompleteTextControl, AddUpdateGenericArgumentsCommand> _getAddUpdateGenericArgumentsCommand;
        private readonly Func<ITypeAutoCompleteTextControl, CopySelectedTextCommand> _getCopySelectedTextCommand;
        private readonly Func<ITypeAutoCompleteTextControl, CutSelectedTextCommand> _getCutSelectedTextCommand;
        private readonly Func<ITypeAutoCompleteTextControl, PasteTextCommand> _getPasteTextCommand;
        private readonly Func<IApplicationForm, ITypeAutoCompleteTextControl, SetTextToAssemblyQualifiedNameCommand> _getSetTextToAssemblyQualifiedNameCommand;

        public TypeAutoCompleteCommandFactory(
            Func<IApplicationForm, ITypeAutoCompleteTextControl, AddUpdateGenericArgumentsCommand> getAddUpdateGenericArgumentsCommand,
            Func<ITypeAutoCompleteTextControl, CopySelectedTextCommand> getCopySelectedTextCommand,
            Func<ITypeAutoCompleteTextControl, CutSelectedTextCommand> getCutSelectedTextCommand,
            Func<ITypeAutoCompleteTextControl, PasteTextCommand> getPasteTextCommand,
            Func<IApplicationForm, ITypeAutoCompleteTextControl, SetTextToAssemblyQualifiedNameCommand> getSetTextToAssemblyQualifiedNameCommand)
        {
            _getAddUpdateGenericArgumentsCommand = getAddUpdateGenericArgumentsCommand;
            _getCopySelectedTextCommand = getCopySelectedTextCommand;
            _getCutSelectedTextCommand = getCutSelectedTextCommand;
            _getPasteTextCommand = getPasteTextCommand;
            _getSetTextToAssemblyQualifiedNameCommand = getSetTextToAssemblyQualifiedNameCommand;
        }

        public AddUpdateGenericArgumentsCommand GetAddUpdateGenericArgumentsCommand(IApplicationForm applicationForm, ITypeAutoCompleteTextControl textControl)
            => _getAddUpdateGenericArgumentsCommand(applicationForm, textControl);

        public CopySelectedTextCommand GetCopySelectedTextCommand(ITypeAutoCompleteTextControl textControl)
            => _getCopySelectedTextCommand(textControl);

        public CutSelectedTextCommand GetCutSelectedTextCommand(ITypeAutoCompleteTextControl textControl)
            => _getCutSelectedTextCommand(textControl);

        public PasteTextCommand GetPasteTextCommand(ITypeAutoCompleteTextControl textControl)
            => _getPasteTextCommand(textControl);

        public SetTextToAssemblyQualifiedNameCommand GetSetTextToAssemblyQualifiedNameCommand(IApplicationForm applicationForm, ITypeAutoCompleteTextControl textControl)
            => _getSetTextToAssemblyQualifiedNameCommand(applicationForm, textControl);
    }
}
