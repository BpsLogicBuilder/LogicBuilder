using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConnectorElementValidator : IConnectorElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IExceptionHelper _exceptionHelper;

        public ConnectorElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _connectorDataParser = xmlElementValidator.ConnectorDataParser;
            _exceptionHelper = xmlElementValidator.ExceptionHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ILiteralElementValidator LiteralElementValidator => _xmlElementValidator.LiteralElementValidator;
        private IMetaObjectElementValidator MetaObjectElementValidator => _xmlElementValidator.MetaObjectElementValidator;

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
