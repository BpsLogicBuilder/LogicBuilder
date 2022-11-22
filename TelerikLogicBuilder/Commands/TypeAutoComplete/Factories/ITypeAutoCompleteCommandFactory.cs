using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories
{
    internal interface ITypeAutoCompleteCommandFactory
    {
        AddUpdateGenericArgumentsCommand GetAddUpdateGenericArgumentsCommand(IApplicationForm applicationForm,
            ITypeAutoCompleteTextControl textControl);
        CopySelectedTextCommand GetCopySelectedTextCommand(ITypeAutoCompleteTextControl textControl);
        CutSelectedTextCommand GetCutSelectedTextCommand(ITypeAutoCompleteTextControl textControl);
        PasteTextCommand GetPasteTextCommand(ITypeAutoCompleteTextControl textControl);
        SetTextToAssemblyQualifiedNameCommand GetSetTextToAssemblyQualifiedNameCommand(IApplicationForm applicationForm,
            ITypeAutoCompleteTextControl textControl);
    }
}
