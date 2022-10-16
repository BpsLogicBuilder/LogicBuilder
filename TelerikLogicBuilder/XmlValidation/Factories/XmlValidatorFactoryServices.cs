using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories
{
    internal static class XmlValidatorFactoryServices
    {
        internal static IServiceCollection AddXmlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<SchemaName, IXmlValidator>>
                (
                    provider =>
                    xmlSchema =>
                    {
                        switch (xmlSchema)
                        {
                            case SchemaName.ConnectorDataSchema:
                                return provider.GetRequiredService<IConnectorDataXmlValidator>();
                            case SchemaName.ConstructorSchema:
                                return provider.GetRequiredService<IConstructorsXmlValidator>();
                            case SchemaName.FunctionsSchema:
                                return provider.GetRequiredService<IFunctionsXmlValidator>();
                            case SchemaName.VariablesSchema:
                                return provider.GetRequiredService<IVariablesXmlValidator>();
                            default:
                                break;
                        }

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
                )
                .AddTransient<IXmlValidatorFactory, XmlValidatorFactory>();
        }
    }
}
