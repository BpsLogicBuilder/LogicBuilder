using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectEditingControlFactoryServices
    {
        internal static IServiceCollection AddSelectEditingControlFactories(this IServiceCollection services)
        {
            return services
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
                .AddTransient<ISelectEditingControlFactory, SelectEditingControlFactory>()
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
                )
                .AddTransient<Func<IEditingForm, Type, IEditVariableControl>>
                (
                    provider =>
                    (editingForm, assignedToType) => new EditVariableControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IEditVariableViewControlFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        editingForm, 
                        assignedToType
                    )
                ); 
        }
    }
}
