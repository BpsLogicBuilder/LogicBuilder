﻿using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditVoidFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditVoidFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditVoidFunctionCommandFactory, EditVoidFunctionCommandFactory>()
                .AddTransient<Func<IEditVoidFunctionControl, SelectVoidFunctionCommand>>
                (
                    provider =>
                    editVoidFunctionControl => new SelectVoidFunctionCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        editVoidFunctionControl
                    )
                );
        }
    }
}
