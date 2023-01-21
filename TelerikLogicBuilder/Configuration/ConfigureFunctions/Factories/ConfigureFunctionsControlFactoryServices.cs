using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunctionsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFunctionsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureFunctionsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFunctionsForm, IConfigureFunctionControl>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionControl
                    (
                        provider.GetRequiredService<IConfigureFunctionControlCommandFactory>(),
                        provider.GetRequiredService<IConfigureFunctionsStateImageSetter>(),
                        provider.GetRequiredService<IFunctionControlValidatorFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<IConfigureFunctionsControlFactory, ConfigureFunctionsControlFactory>()
                .AddTransient<Func<IConfigureFunctionsForm, IConfigureFunctionsFolderControl>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsFolderControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionsForm
                    )
                );
        }
    }
}
