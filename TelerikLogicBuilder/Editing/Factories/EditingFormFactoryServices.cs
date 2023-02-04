﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormFactoryServices
    {
        internal static IServiceCollection AddEditingFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditingFormFactory, EditingFormFactory>()
                .AddTransient<Func<Type, ISelectConstructorForm>>
                (
                    provider =>
                    assignedTo => new SelectConstructorForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<ISelectEditingControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                )
                .AddTransient<Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm>>
                (
                    provider =>
                    (assignedTo, functionDeictionary, treeFolders) => new SelectFunctionForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<ISelectEditingControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo,
                        functionDeictionary,
                        treeFolders
                    )
                )
                .AddTransient<Func<Type, ISelectVariableForm>>
                (
                    provider =>
                    assignedTo => new SelectVariableForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<ISelectEditingControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                );
        }
    }
}
