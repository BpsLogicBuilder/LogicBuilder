using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories
{
    internal class TypeAutoCompleteCommandFactory : ITypeAutoCompleteCommandFactory
    {
        public AddUpdateGenericArgumentsCommand GetAddUpdateGenericArgumentsCommand(IApplicationHostControl applicationHostControl, ITypeAutoCompleteTextControl textControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                applicationHostControl,
                textControl
            );

        public CopySelectedTextCommand GetCopySelectedTextCommand(ITypeAutoCompleteTextControl textControl)
            => new
            (
                textControl
            );

        public CutSelectedTextCommand GetCutSelectedTextCommand(ITypeAutoCompleteTextControl textControl)
            => new
            (
                textControl
            );

        public PasteTextCommand GetPasteTextCommand(ITypeAutoCompleteTextControl textControl)
            => new
            (
                textControl
            );

        public SetTextToAssemblyQualifiedNameCommand GetSetTextToAssemblyQualifiedNameCommand(IApplicationControl applicationControl, ITypeAutoCompleteTextControl textControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                applicationControl,
                textControl
            );
    }
}
