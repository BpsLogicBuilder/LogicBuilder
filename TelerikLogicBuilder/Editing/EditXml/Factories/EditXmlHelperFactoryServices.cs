using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditXmlHelperFactoryServices
    {
        internal static IServiceCollection AddEditXmlHelperFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditXmlHelperFactory, EditXmlHelperFactory>()
                .AddTransient<Func<IEditFormXml, IValidateXmlTextHelper>>
                (
                    provider =>
                    editXmlForm => new ValidateXmlTextHelper
                    (
                        editXmlForm
                    )
                );
        }
    }
}
