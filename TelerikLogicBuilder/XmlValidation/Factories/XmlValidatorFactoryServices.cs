using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlValidatorFactoryServices
    {
        internal static IServiceCollection AddXmlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<Func<IAssertFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IAssertFunctionElementValidator>()
                )
                .AddSingleton<Func<IBinaryOperatorFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IBinaryOperatorFunctionElementValidator>()
                )
                .AddSingleton<Func<ICallElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ICallElementValidator>()
                )
                .AddSingleton<Func<IConditionsElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IConditionsElementValidator>()
                )
                .AddSingleton<Func<IConnectorElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IConnectorElementValidator>()
                )
                .AddSingleton<Func<IConstructorElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IConstructorElementValidator>()
                )
                .AddSingleton<Func<IDecisionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IDecisionElementValidator>()
                )
                .AddSingleton<Func<IDecisionsElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IDecisionsElementValidator>()
                )
                .AddSingleton<Func<IFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IFunctionElementValidator>()
                )
                .AddSingleton<Func<IFunctionsElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IFunctionsElementValidator>()
                )
                .AddSingleton<Func<ILiteralElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralElementValidator>()
                )
                .AddSingleton<Func<ILiteralListElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralListElementValidator>()
                )
                .AddSingleton<Func<ILiteralListParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralListParameterElementValidator>()
                )
                .AddSingleton<Func<ILiteralListVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralListVariableElementValidator>()
                )
                .AddSingleton<Func<ILiteralParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralParameterElementValidator>()
                )
                .AddSingleton<Func<ILiteralVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralVariableElementValidator>()
                )
                .AddSingleton<Func<IMetaObjectElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IMetaObjectElementValidator>()
                )
                .AddSingleton<Func<IObjectElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectElementValidator>()
                )
                .AddSingleton<Func<IObjectListElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectListElementValidator>()
                )
                .AddSingleton<Func<IObjectListParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectListParameterElementValidator>()
                )
                .AddSingleton<Func<IObjectListVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectListVariableElementValidator>()
                )
                .AddSingleton<Func<IObjectParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectParameterElementValidator>()
                )
                .AddSingleton<Func<IObjectVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectVariableElementValidator>()
                )
                .AddSingleton<Func<IParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IParameterElementValidator>()
                )
                .AddSingleton<Func<IParametersElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IParametersElementValidator>()
                )
                .AddSingleton<Func<IRetractFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IRetractFunctionElementValidator>()
                )
                .AddSingleton<Func<IRuleChainingUpdateFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>()
                )
                .AddSingleton<Func<IVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IVariableElementValidator>()
                )
                .AddSingleton<IXmlElementValidatorFactory, XmlElementValidatorFactory>()
                .AddTransient<Func<SchemaName, IXmlValidator>>
                (
                    provider =>
                    xmlSchema =>
                    {
                        switch (xmlSchema)
                        {
                            case SchemaName.ConnectorDataSchema:

                                return ActivatorUtilities.CreateInstance<ConnectorDataXmlValidator>(provider);
                            case SchemaName.ConstructorSchema:
                                return ActivatorUtilities.CreateInstance<ConstructorsXmlValidator>(provider);
                            case SchemaName.FunctionsSchema:
                                return ActivatorUtilities.CreateInstance<FunctionsXmlValidator>(provider);
                            case SchemaName.VariablesSchema:
                                return ActivatorUtilities.CreateInstance<VariablesXmlValidator>(provider);
                            default:
                                Dictionary<SchemaName, XmlSchema> schemas = new()
                                {
                                    [SchemaName.ProjectPropertiesSchema] = Schemas.ProjectPropertiesSchema,
                                    [SchemaName.ShapeDataSchema] = Schemas.ShapeDataSchema,
                                    [SchemaName.DecisionsDataSchema] = Schemas.DecisionsDataSchema,
                                    [SchemaName.ConditionsDataSchema] = Schemas.ConditionsDataSchema,
                                    [SchemaName.FunctionsDataSchema] = Schemas.FunctionsDataSchema,
                                    [SchemaName.TableSchema] = Schemas.TableSchema,
                                    [SchemaName.ParametersDataSchema] = Schemas.ParametersDataSchema,
                                    [SchemaName.FragmentsSchema] = Schemas.FragmentsSchema
                                };

                                if (!schemas.TryGetValue(xmlSchema, out XmlSchema? schema))
                                    throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{327FD292-4774-460B-AD6F-F0F2BAA22604}"));

                                return new XmlValidator(schema);
                        }
                    }
                )
                .AddTransient<IXmlValidatorFactory, XmlValidatorFactory>();
        }
    }
}
