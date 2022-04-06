using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectListElementValidator : IObjectListElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectListElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private IObjectElementValidator ObjectElementValidator => _xmlElementValidator.ObjectElementValidator;

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
