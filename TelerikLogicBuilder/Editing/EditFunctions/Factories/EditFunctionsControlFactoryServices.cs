using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditFunctionsControlFactoryServices
    {
        internal static IServiceCollection AddEditFunctionsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditFunctionsControlFactory, EditFunctionsControlFactory>()
                .AddTransient<Func<IEditFunctionsForm, IEditVoidFunctionControl>>
                (
                    provider =>
                    editFunctionsForm => new EditVoidFunctionControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditVoidFunctionCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editFunctionsForm
                    )
                );
        }
    }
}
