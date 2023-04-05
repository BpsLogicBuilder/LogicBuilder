using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDialogFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditDialogFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditDialogFunctionCommandFactory, EditDialogFunctionCommandFactory>()
                .AddTransient<Func<IEditDialogFunctionForm, EditDialogFunctionFormXmlCommand>>
                (
                    provider =>
                    editFunctionForm => new EditDialogFunctionFormXmlCommand
                    (
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editFunctionForm
                    )
                )
                .AddTransient<Func<IEditDialogFunctionForm, SelectDialogFunctionCommand>>
                (
                    provider =>
                    editFunctionForm => new SelectDialogFunctionCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        editFunctionForm
                    )
                );
        }
    }
}
