using ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete;
using ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class TypeAutoCompleteCommandFactoryServices
    {
        internal static IServiceCollection AddTypeAutoCompleteCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IApplicationForm, ITypeAutoCompleteTextControl, AddUpdateGenericArgumentsCommand>>
                (
                    provider =>
                    (applicationForm, textControl) => new AddUpdateGenericArgumentsCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        applicationForm, 
                        textControl
                    )
                )
                .AddTransient<Func<ITypeAutoCompleteTextControl, CopySelectedTextCommand>>
                (
                    provider =>
                    (textControl) => new CopySelectedTextCommand
                    (
                        textControl
                    )
                )
                .AddTransient<Func<ITypeAutoCompleteTextControl, CutSelectedTextCommand>>
                (
                    provider =>
                    (textControl) => new CutSelectedTextCommand
                    (
                        textControl
                    )
                )
                .AddTransient<Func<ITypeAutoCompleteTextControl, PasteTextCommand>>
                (
                    provider =>
                    (textControl) => new PasteTextCommand
                    (
                        textControl
                    )
                )
                .AddTransient<Func<IApplicationForm, ITypeAutoCompleteTextControl, SetTextToAssemblyQualifiedNameCommand>>
                (
                    provider =>
                    (applicationForm, textControl) => new SetTextToAssemblyQualifiedNameCommand
                    (
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        applicationForm,
                        textControl
                    )
                )
                .AddTransient<ITypeAutoCompleteCommandFactory, TypeAutoCompleteCommandFactory>();
        }
    }
}
