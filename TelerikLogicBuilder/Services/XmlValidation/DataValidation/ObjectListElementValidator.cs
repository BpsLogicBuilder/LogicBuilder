using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectListElementValidator : IObjectListElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ObjectListElementValidator(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IObjectElementValidator? _objectElementValidator;
		private IObjectElementValidator ObjectElementValidator => _objectElementValidator ??= _xmlElementValidatorFactory.GetObjectElementValidator();

        public void Validate(XmlElement objectListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{0615AE34-1A8D-49B0-82BB-A40F8C8332A6}");

            string objectElementTypeString = objectListElement.GetAttribute(XmlDataConstants.OBJECTTYPEATTRIBUTE);
            if (!_typeLoadHelper.TryGetSystemType(objectElementTypeString, application, out Type? elementType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, objectElementTypeString));
                return;
            }

            ListType listType = _enumHelper.ParseEnumText<ListType>
            (
                objectListElement.GetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE)
            );

            Type listSystemType = _enumHelper.GetSystemType(listType, elementType);
            if (!_typeHelper.AssignableFrom(assignedTo, listSystemType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeNotAssignableFormat, listSystemType.ToString(), assignedTo.ToString()));
                return;
            }

            _xmlDocumentHelpers.GetChildElements(objectListElement).ForEach
            (
                element => ObjectElementValidator.Validate(element, elementType, application, validationErrors)
            );
        }
    }
}
