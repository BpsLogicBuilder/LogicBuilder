using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class FunctionsElementValidator : IFunctionsElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;

        public FunctionsElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _functionsDataParser = xmlElementValidator.FunctionsDataParser;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        //so can't be sset as readonly fields in the constructor
        private IAssertFunctionElementValidator AssertFunctionElementValidator => _xmlElementValidator.AssertFunctionElementValidator;
        private IFunctionElementValidator FunctionElementValidator => _xmlElementValidator.FunctionElementValidator;
        private IRetractFunctionElementValidator RetractFunctionElementValidator => _xmlElementValidator.RetractFunctionElementValidator;

        public void Validate(XmlElement functionsElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (functionsElement.Name != XmlDataConstants.FUNCTIONSELEMENT)
                throw _exceptionHelper.CriticalException("{1FD9B3FC-D16D-4B20-8051-5171B3250FF1}");

            _functionsDataParser.Parse(functionsElement).FunctionElements.ForEach
            (
                functionElement =>
                {
                    switch (functionElement.Name)
                    {
                        //case XmlDataConstants.NOTELEMENT: Not only applies to boolean nested functions - not as a child of <functions />
                        case XmlDataConstants.FUNCTIONELEMENT:
                            FunctionElementValidator.Validate
                            (
                                functionElement,
                                typeof(object),//The functions element is used for the action and dialog shapes, typically void functions i.e. we don't care about type validation
                                application,
                                validationErrors
                            );
                            break;
                        case XmlDataConstants.ASSERTFUNCTIONELEMENT:
                            AssertFunctionElementValidator.Validate(functionElement, application, validationErrors);
                            break;
                        case XmlDataConstants.RETRACTFUNCTIONELEMENT:
                            RetractFunctionElementValidator.Validate(functionElement, application, validationErrors);
                            break;
                        default:
                            throw _exceptionHelper.CriticalException("{CE439CE9-F152-440E-AA09-A78F4BE63443}");
                    }
                    
                });
        }
    }
}
