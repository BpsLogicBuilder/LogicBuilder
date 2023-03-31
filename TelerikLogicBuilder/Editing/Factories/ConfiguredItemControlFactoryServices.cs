using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfiguredItemControlFactoryServices
    {
        internal static IServiceCollection AddSelectEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfiguredItemControlFactory, ConfiguredItemControlFactory>()
                .AddTransient<Func<ISelectConstructorForm, Type, ISelectConstructorControl>>
                (
                    provider =>
                    (selectConstructorForm, assignedToType) => new SelectConstructorControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ISelectConstructorViewControlFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        selectConstructorForm,
                        assignedToType
                    )
                )
                .AddTransient<Func<ISelectFragmentForm, ISelectFragmentControl>>
                (
                    provider =>
                    selectFragmentForm => new SelectFragmentControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ISelectFragmentViewControlFactory>(),
                        selectFragmentForm
                    )
                )
                .AddTransient<Func<ISelectFunctionForm, Type, ISelectFunctionControl>>
                (
                    provider =>
                    (selectFunctionForm, assignedToType) => new SelectFunctionControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ISelectFunctionViewControlFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        selectFunctionForm,
                        assignedToType
                    )
                ); 
        }
    }
}
