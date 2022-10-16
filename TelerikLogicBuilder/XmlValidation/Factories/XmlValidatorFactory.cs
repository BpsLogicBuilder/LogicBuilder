using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories
{
    internal class XmlValidatorFactory : IXmlValidatorFactory
    {
        private readonly Func<SchemaName, IXmlValidator> _getXmlValidator;

        public XmlValidatorFactory(Func<SchemaName, IXmlValidator> getXmlValidator)
        {
            _getXmlValidator = getXmlValidator;
        }

        public IXmlValidator GetXmlValidator(SchemaName xmlSchema)
            => _getXmlValidator(xmlSchema);
    }
}
