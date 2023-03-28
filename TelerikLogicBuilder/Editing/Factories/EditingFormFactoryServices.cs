using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormFactoryServices
    {
        internal static IServiceCollection AddEditingFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<Type, XmlDocument, HashSet<string>, string, IEditConstructorForm>>
                (
                    provider =>
                    (assignedTo, constructorXmlDocument, constructorNames, selectedConstructor) => new EditConstructorForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditConstructorCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        assignedTo,
                        constructorXmlDocument,
                        constructorNames,
                        selectedConstructor
                    )
                )
                .AddTransient<IEditingFormFactory, EditingFormFactory>()
                .AddTransient<Func<Type, IEditVariableForm>>
                (
                    provider =>
                    assignedTo => new EditVariableForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditingControlFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                )
                .AddTransient<Func<Type, ISelectConstructorForm>>
                (
                    provider =>
                    assignedTo => new SelectConstructorForm
                    (
                        provider.GetRequiredService<IConfiguredItemControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                )
                .AddTransient<Func<IList<string>, string, ISelectFromDomainForm>>
                (
                    provider =>
                    (domain, comments) => new SelectFromDomainForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        domain, 
                        comments
                    )
                )
                .AddTransient<Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm>>
                (
                    provider =>
                    (assignedTo, functionDeictionary, treeFolders) => new SelectFunctionForm
                    (
                        provider.GetRequiredService<IConfiguredItemControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo,
                        functionDeictionary,
                        treeFolders
                    )
                );
        }
    }
}
