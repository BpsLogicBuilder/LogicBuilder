using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditValueFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditValueFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditValueFunctionCommandFactory, EditValueFunctionCommandFactory>()
                .AddTransient<Func<IEditValueFunctionForm, EditValueFunctionFormXmlCommand>>
                (
                    provider =>
                    editFunctionForm => new EditValueFunctionFormXmlCommand
                    (
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editFunctionForm
                    )
                )
                .AddTransient<Func<IEditValueFunctionForm, SelectValueFunctionCommand>>
                (
                    provider =>
                    editFunctionForm => new SelectValueFunctionCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        editFunctionForm
                    )
                );
        }
    }
}
