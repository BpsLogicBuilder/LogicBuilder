using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration;
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
