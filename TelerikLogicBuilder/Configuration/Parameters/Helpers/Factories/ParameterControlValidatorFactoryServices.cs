using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ParameterControlValidatorFactoryServices
    {
        internal static IServiceCollection AddParameterControlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureGenericListParameterControl, IGenericListParameterControlValidator>>
                (
                    provider =>
                    parameterControl => new GenericListParameterControlValidator
                    (
                        parameterControl
                    )
                )
                .AddTransient<Func<IConfigureGenericParameterControl, IGenericParameterControlValidator>>
                (
                    provider =>
                    parameterControl => new GenericParameterControlValidator
                    (
                        parameterControl
                    )
                )
                .AddTransient<Func<IConfigureLiteralListParameterControl, ILiteralListParameterControlValidator>>
                (
                    provider =>
                    parameterControl => new LiteralListParameterControlValidator
                    (
                        provider.GetRequiredService<IParametersXmlParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        parameterControl
                    )
                )
                .AddTransient<Func<IConfigureLiteralParameterControl, ILiteralParameterControlValidator>>
                (
                    provider =>
                    parameterControl => new LiteralParameterControlValidator
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IParametersXmlParser>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        parameterControl
                    )
                )
                .AddTransient<Func<IConfigureObjectListParameterControl, IObjectListParameterControlValidator>>
                (
                    provider =>
                    parameterControl => new ObjectListParameterControlValidator
                    (
                        parameterControl
                    )
                )
                .AddTransient<Func<IConfigureObjectParameterControl, IObjectParameterControlValidator>>
                (
                    provider =>
                    parameterControl => new ObjectParameterControlValidator
                    (
                        parameterControl
                    )
                )
                .AddTransient<IParameterControlValidatorFactory, ParameterControlValidatorFactory>();
        }
    }
}
