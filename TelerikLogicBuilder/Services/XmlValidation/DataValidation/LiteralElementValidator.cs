using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralElementValidator : ILiteralElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        //private readonly fields were injected into XmlElementValidator

        public LiteralElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private IConstructorElementValidator ConstructorElementValidator => _xmlElementValidator.ConstructorElementValidator;
        private IFunctionElementValidator FunctionElementValidator => _xmlElementValidator.FunctionElementValidator;
        private IVariableElementValidator VariableElementValidator => _xmlElementValidator.VariableElementValidator;

        public void Validate(XmlElement literalElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (literalElement.Name != XmlDataConstants.LITERALELEMENT
                && literalElement.Name != XmlDataConstants.LITERALPARAMETERELEMENT
                && literalElement.Name != XmlDataConstants.LITERALVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{BDEDB78F-98F9-4C7C-AE73-415D69FCF488}");

            if (assignedTo == typeof(string))
            {
                ValidateString(literalElement, assignedTo, application, validationErrors);
                return;
            }

            if (_typeHelper.IsNullable(assignedTo))
            {
                ValidateNullableLiteral(literalElement, assignedTo, application, validationErrors);
                return;
            }

            ValidateNonNullableLiteral(literalElement, assignedTo, application, validationErrors);
        }

        private void ValidateNullableLiteral(XmlElement literalElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (literalElement.ChildNodes.Count == 0)
                return;

            if (literalElement.ChildNodes.Count > 1)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidLiteralInputFormat2, assignedTo.ToString()));
                return;
            }

            DoStandardLiteralValidation
            (
                literalElement.ChildNodes[0]!,
                assignedTo,
                application,
                validationErrors
            );
        }

        private void ValidateNonNullableLiteral(XmlElement literalElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (literalElement.ChildNodes.Count == 0 || literalElement.ChildNodes.Count > 1)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidLiteralInputFormat2, assignedTo.ToString()));
                return;
            }

            DoStandardLiteralValidation
            (
                literalElement.ChildNodes[0]!, 
                assignedTo, 
                application, 
                validationErrors
            );
        }

        private void DoStandardLiteralValidation(XmlNode xmlNode, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            switch (xmlNode.NodeType)
            {
                case XmlNodeType.Element:
                    switch (xmlNode.Name)
                    {
                        case XmlDataConstants.CONSTRUCTORELEMENT:
                            ConstructorElementValidator.Validate((XmlElement)xmlNode, assignedTo, application, validationErrors);
                            break;
                        case XmlDataConstants.FUNCTIONELEMENT:
                            FunctionElementValidator.Validate((XmlElement)xmlNode, assignedTo, application, validationErrors);
                            break;
                        case XmlDataConstants.VARIABLEELEMENT:
                            VariableElementValidator.Validate((XmlElement)xmlNode, assignedTo, application, validationErrors);
                            break;
                        default:
                            throw _exceptionHelper.CriticalException("{FD9B516E-E07F-44B5-9FD3-79D4C2A080C0}");
                    }
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.Whitespace:
                    if (!_typeHelper.TryParse(xmlNode.Value!, assignedTo, out object? _))//not null for XmlNodeType.Text or XmlNodeType.Whitespace.
                        validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidLiteralInputFormat, xmlNode.Value, assignedTo.ToString()));
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{4DAEF381-0D1B-40D3-A5AA-76EAEA22BBE3}");
            }
        }

        private void ValidateString(XmlElement literalElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (literalElement.ChildNodes.Count == 0)
                return;

            if (literalElement.ChildNodes.Count > 1)
            {
                _xmlDocumentHelpers.GetChildElements(literalElement).ForEach(node =>
                {
                    DoValidation(node, typeof(object));//we call object.ToString() for each argument in string.Format(args).
                });

                return;
            }

            DoValidation(literalElement.ChildNodes[0]!, assignedTo);
            void DoValidation(XmlNode xmlNode, Type type)
            {
                switch (xmlNode.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlNode.Name)
                        {
                            case XmlDataConstants.CONSTRUCTORELEMENT:
                                ConstructorElementValidator.Validate((XmlElement)xmlNode, assignedTo, application, validationErrors);
                                break;
                            case XmlDataConstants.FUNCTIONELEMENT:
                                FunctionElementValidator.Validate((XmlElement)xmlNode, assignedTo, application, validationErrors);
                                break;
                            case XmlDataConstants.VARIABLEELEMENT:
                                VariableElementValidator.Validate((XmlElement)xmlNode, assignedTo, application, validationErrors);
                                break;
                            default:
                                throw _exceptionHelper.CriticalException("{DB7E438B-2F4A-4787-951E-F519483378AF}");
                        }
                        break;
                    case XmlNodeType.Text:
                    case XmlNodeType.Whitespace:
                        return;
                    default:
                        throw _exceptionHelper.CriticalException("{C53A1653-C415-4FF0-B3F6-66B0F98A1049}");
                }
            }
        }
    }
}
