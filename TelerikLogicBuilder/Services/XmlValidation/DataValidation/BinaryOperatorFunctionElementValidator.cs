using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class BinaryOperatorFunctionElementValidator : IBinaryOperatorFunctionElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IAnyParametersHelper _anyParametersHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParameterHelper _parameterHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public BinaryOperatorFunctionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _anyParametersHelper = xmlElementValidator.AnyParametersHelper;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _parameterHelper = xmlElementValidator.ContextProvider.ParameterHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
        //These fields may be null in the constructor i.e. when new FunctionElementValidator((XmlElementValidator)this)
        //therefore they must be properties.
        private IParametersElementValidator ParametersElementValidator => _xmlElementValidator.ParametersElementValidator;
        private IConstructorElementValidator ConstructorElementValidator => _xmlElementValidator.ConstructorElementValidator;
        private IFunctionElementValidator FunctionElementValidator => _xmlElementValidator.FunctionElementValidator;
        private IVariableElementValidator VariableElementValidator => _xmlElementValidator.VariableElementValidator;

        public void Validate(Function function, IList<XmlElement> parameterElementsList, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if(function.FunctionCategory != FunctionCategories.BinaryOperator)
                throw _exceptionHelper.CriticalException("{C97A6D3B-C877-439A-AE7C-3520EDF071E0}");

            if (!IsValidCodeBinaryOperator(function.MemberName))
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        Strings.binaryOperatorCodeNameInvalidFormat,
                        function.MemberName,
                        function.Name,
                        string.Join
                        (
                            Strings.itemsCommaSeparator,
                            _enumHelper.ConvertEnumListToStringList
                            (
                                new CodeBinaryOperatorType[] { CodeBinaryOperatorType.Assign }
                            ).Select(i => string.Concat("\"", i, "\""))
                        )
                    )
                );

                return;
            }

            if (function.Parameters.Count != 2)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.parameterCountMustBeTwoFormat, function.Name));
                return;
            }

            if (parameterElementsList.Count != 2)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.parameterCountMustBeTwoFormat, function.Name));
                return;
            }

            if (_parameterHelper.IsParameterAny(function.Parameters[0]) ^ _parameterHelper.IsParameterAny(function.Parameters[1]))
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.invalidAnyParameterConfigurationForBinaryOperatorFormat,
                        function.Name,
                        function.Parameters[0].Name,
                        function.Parameters[1].Name
                    )
                );
                return;
            }

            if (_parameterHelper.IsParameterAny(function.Parameters[0]))
            {
                ValidateFunctionDataForParameterTypeAny
                (
                    parameterElementsList[0],
                    parameterElementsList[1]
                );
            }
            else
            {
                ParametersElementValidator.Validate(function.Parameters, parameterElementsList, application, validationErrors);
            }

            void ValidateFunctionDataForParameterTypeAny(XmlElement firstXmlParameter, XmlElement secondXmlParameter)
            {
                ValidateChildElementsForAnyParameter(firstXmlParameter, application, validationErrors);
                if (validationErrors.Any())
                    return;

                ValidateChildElementsForAnyParameter(secondXmlParameter, application, validationErrors);
                if (validationErrors.Any())
                    return;

                ValidateParameterTypeAny
                (
                    _anyParametersHelper.GetTypes(firstXmlParameter, secondXmlParameter, application)
                );
            }

            void ValidateParameterTypeAny(AnyParameterPair types)
            {
                ValidateTypes(types.ParameterOneType, types.ParameterTwoType);
                void ValidateTypes(Type? p1, Type? p2)
                {
                    if (p1 == null || p2 == null)
                    {
                        validationErrors.Add(Strings.neitherOperandCanBeEmptyBinaryOperationAny);
                        return;
                    }

                    if (!_typeHelper.IsLiteralType(p1))
                    {
                        validationErrors.Add(string.Format(Strings.parameterNotLiteralFormat, function.Parameters[0].Name, p1.ToString()));
                        return;
                    }

                    if (!_typeHelper.IsLiteralType(p2))
                    {
                        validationErrors.Add(string.Format(Strings.parameterNotLiteralFormat, function.Parameters[1].Name, p2.ToString()));
                        return;
                    }

                    if (!_typeHelper.AreCompatibleForOperation(p1, p2, _enumHelper.ParseEnumText<CodeBinaryOperatorType>(function.MemberName)))
                    {
                        validationErrors.Add(string.Format(Strings.parametersNotCompatibleForBinaryOperation, p1.ToString(), p2.ToString(), function.MemberName));
                    }
                }
            }
        }

        private void ValidateChildElementsForAnyParameter(XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (parameterElement.ChildNodes.Count == 0)
                return;

            if (parameterElement.ChildNodes.Count > 1)
            {
                _xmlDocumentHelpers.GetChildElements(parameterElement).ForEach(node =>
                {
                    DoValidation(node, typeof(object));//we call object.ToString() for each argument in string.Format(args).
                });

                return;
            }

            DoValidation(parameterElement.ChildNodes[0]!, typeof(object));//typeof(object) as assignedTo because we don't know the type for the Any parameter.
            void DoValidation(XmlNode xmlNode, Type assignedTo)
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
                                throw _exceptionHelper.CriticalException("{2CC0FFDD-C0D8-44BC-ABD7-9C2CA95B744C}");
                        }
                        break;
                    case XmlNodeType.Text:
                    case XmlNodeType.Whitespace:
                        return;
                    default:
                        throw _exceptionHelper.CriticalException("{14522A05-0C79-4803-8BF7-BF8770B6E90D}");
                }
            }
        }

        private bool IsValidCodeBinaryOperator(string item)
        {
            if (!Enum.IsDefined(typeof(CodeBinaryOperatorType), item))
                return false;

            return _enumHelper.ParseEnumText<CodeBinaryOperatorType>(item) != CodeBinaryOperatorType.Assign;
        }
    }
}
