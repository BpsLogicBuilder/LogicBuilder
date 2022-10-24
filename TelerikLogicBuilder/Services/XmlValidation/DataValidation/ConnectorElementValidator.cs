using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConnectorElementValidator : IConnectorElementValidator
    {
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ConnectorElementValidator(
            IConnectorDataParser connectorDataParser,
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _connectorDataParser = connectorDataParser;
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ILiteralElementValidator? _literalElementValidator;
		private ILiteralElementValidator LiteralElementValidator => _literalElementValidator ??= _xmlElementValidatorFactory.GetLiteralElementValidator();
        private IMetaObjectElementValidator? _metaObjectElementValidator;
		private IMetaObjectElementValidator MetaObjectElementValidator => _metaObjectElementValidator ??= _xmlElementValidatorFactory.GetMetaObjectElementValidator();

        public void Validate(XmlElement connectorElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (connectorElement.Name != XmlDataConstants.CONNECTORELEMENT)
                throw _exceptionHelper.CriticalException("{2D8C7C90-9111-4BAB-BF1A-07AE914EA291}");

            ConnectorData connectorData = _connectorDataParser.Parse(connectorElement);
            LiteralElementValidator.Validate(connectorData.TextXmlNode, typeof(string), application, validationErrors);

            if (connectorData.ConnectorCategory == ConnectorCategory.Dialog)
            {
                if (connectorData.MetaObjectDataXmlNode == null)
                    throw _exceptionHelper.CriticalException("{F0CE5C2F-7B0C-4119-A49B-EC49726106E5}");

                MetaObjectElementValidator.Validate(connectorData.MetaObjectDataXmlNode, application, validationErrors); //
            }
        }
    }
}
