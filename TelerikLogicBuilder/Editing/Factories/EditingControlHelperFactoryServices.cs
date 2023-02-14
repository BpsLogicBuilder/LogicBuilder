using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingControlHelperFactoryServices
    {
        internal static IServiceCollection AddEditingControlHelperFactories(this IServiceCollection services)
        {
            return services

                .AddTransient<IEditingControlHelperFactory, EditingControlHelperFactory>()
                .AddTransient<Func<IEditingControl, IEditingForm, ILoadParameterControlsDictionary>>
                (
                    provider =>
                    (editingControl, edifForm) => new LoadParameterControlsDictionary
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        editingControl, 
                        edifForm
                    )
                );
        }
    }
}
