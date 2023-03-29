using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConstructorCommandFactoryServices
    {
        internal static IServiceCollection AddEditConstructorCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditConstructorCommandFactory, EditConstructorCommandFactory>()
                .AddTransient<Func<IEditConstructorForm, EditConstructorFormXmlCommand>>
                (
                    provider =>
                    editConstructorForm => new EditConstructorFormXmlCommand
                    (
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editConstructorForm
                    )
                )
                .AddTransient<Func<IEditConstructorForm, SelectConstructorCommand>>
                (
                    provider =>
                    editConstructorForm => new SelectConstructorCommand
                    (
                        editConstructorForm
                    )
                );
        }
    }
}
