using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories
{
    internal class XmlValidatorFactory : IXmlValidatorFactory
    {
        public IXmlValidator GetXmlValidator(SchemaName xmlSchema)
        {
            switch (xmlSchema)
            {
                case SchemaName.ConnectorDataSchema:
                    return ActivatorUtilities.CreateInstance<ConnectorDataXmlValidator>(Program.ServiceProvider);
                case SchemaName.ConstructorSchema:
                    return ActivatorUtilities.CreateInstance<ConstructorsXmlValidator>(Program.ServiceProvider);
                case SchemaName.FunctionsSchema:
                    return ActivatorUtilities.CreateInstance<FunctionsXmlValidator>(Program.ServiceProvider);
                case SchemaName.VariablesSchema:
                    return ActivatorUtilities.CreateInstance<VariablesXmlValidator>(Program.ServiceProvider);
                default:
                    Dictionary<SchemaName, XmlSchema> schemas = new()
                    {
                        [SchemaName.ProjectPropertiesSchema] = Schemas.ProjectPropertiesSchema,
                        [SchemaName.ShapeDataSchema] = Schemas.ShapeDataSchema,
                        [SchemaName.DecisionsDataSchema] = Schemas.DecisionsDataSchema,
                        [SchemaName.ConditionsDataSchema] = Schemas.ConditionsDataSchema,
                        [SchemaName.FunctionDataSchema] = Schemas.FunctionDataSchema,
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
    }
}
