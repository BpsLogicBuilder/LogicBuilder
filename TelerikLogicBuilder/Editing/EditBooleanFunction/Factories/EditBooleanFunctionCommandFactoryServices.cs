using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditBooleanFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditBooleanFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditBooleanFunctionCommandFactory, EditBooleanFunctionCommandFactory>()
                .AddTransient<Func<IEditBooleanFunctionForm, EditBooleanFunctionFormXmlCommand>>
                (
                    provider =>
                    editFunctionForm => new EditBooleanFunctionFormXmlCommand
                    (
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editFunctionForm
                    )
                )
                .AddTransient<Func<IEditBooleanFunctionForm, SelectBooleanFunctionCommand>>
                (
                    provider =>
                    editFunctionForm => new SelectBooleanFunctionCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        editFunctionForm
                    )
                );
        }
    }
}
