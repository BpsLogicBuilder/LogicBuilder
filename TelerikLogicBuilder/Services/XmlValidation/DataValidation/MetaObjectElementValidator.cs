using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class MetaObjectElementValidator : IMetaObjectElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMetaObjectDataParser _metaObjectDataParser;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public MetaObjectElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _metaObjectDataParser = xmlElementValidator.MetaObjectDataParser;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ICallElementValidator CallElementValidator => _xmlElementValidator.CallElementValidator;

        public void Validate(XmlElement metaObjectElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (metaObjectElement.Name != XmlDataConstants.METAOBJECTELEMENT)
                throw _exceptionHelper.CriticalException("{D0A340D8-6C59-490C-B193-D1566AFDE541}");

            MetaObjectData metaObjectData = _metaObjectDataParser.Parse(metaObjectElement);
            if(!_typeLoadHelper.TryGetSystemType(metaObjectData.ObjectType, application, out Type? assignedTo))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, metaObjectData.ObjectType));
                return;
            }

            CallElementValidator.Validate(metaObjectData.ChildElement, assignedTo, application, validationErrors);
        }
    }
}
