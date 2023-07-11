using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseFactoryServices
    {
        internal static IServiceCollection AddIntellisenseFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsHelperForm, IIntellisenseConstructorsFormManager>>
                (
                    provider =>
                    configureConstructorsHelperForm => new IntellisenseConstructorsFormManager
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configureConstructorsHelperForm
                    )
                )
                .AddTransient<IIntellisenseFactory, IntellisenseFactory>()
                .AddSingleton<IIntellisenseFormFactory, IntellisenseFormFactory>()
                .AddTransient<Func<IConfiguredItemHelperForm, IIntellisenseFunctionsFormManager>>
                (
                    provider =>
                    configuredItemHelperForm => new IntellisenseFunctionsFormManager
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                )
                .AddTransient<Func<IIncludesHelperForm, IIntellisenseIncludesFormManager>>
                (
                    provider =>
                    includesHelperForm => new IntellisenseIncludesFormManager
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        includesHelperForm
                    )
                )
                .AddTransient<Func<IConfiguredItemHelperForm, IIntellisenseVariablesFormManager>>
                (
                    provider =>
                    configuredItemHelperForm => new IntellisenseVariablesFormManager
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configuredItemHelperForm
                    )
                );
        }
    }
}
