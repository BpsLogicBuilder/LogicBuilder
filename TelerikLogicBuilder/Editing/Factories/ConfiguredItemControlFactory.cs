using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class ConfiguredItemControlFactory : IConfiguredItemControlFactory
    {
        public ISelectConstructorControl GetSelectConstructorControl(ISelectConstructorForm selectConstructorForm, Type assignedTo)
            => new SelectConstructorControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ISelectConstructorViewControlFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                selectConstructorForm,
                assignedTo
            );

        public ISelectFragmentControl GetSelectFragmentControl(ISelectFragmentForm selectFragmentForm)
            => new SelectFragmentControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ISelectFragmentViewControlFactory>(),
                selectFragmentForm
            );

        public ISelectFunctionControl GetSelectFunctionControl(ISelectFunctionForm selectFunctionForm, Type assignedTo)
            => new SelectFunctionControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ISelectFunctionViewControlFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                selectFunctionForm,
                assignedTo
            );
    }
}
