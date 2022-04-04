using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConditionsElementValidator : IConditionsElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;

        public ConditionsElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _conditionsDataParser = xmlElementValidator.ConditionsDataParser;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
        //These fields may be null in the constructor i.e. when new FunctionElementValidator((XmlElementValidator)this) runs
        //therefore they must be properties.
        private IFunctionElementValidator FunctionElementValidator => _xmlElementValidator.FunctionElementValidator;

        public void Validate(XmlElement condtionsElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (condtionsElement.Name != XmlDataConstants.CONDITIONSELEMENT)
                throw _exceptionHelper.CriticalException("{4C53E295-8C5E-4FD4-9340-BF2BC9BC8F11}");

            _conditionsDataParser.Parse(condtionsElement).FunctionElements.ForEach
            (
                functionElement => FunctionElementValidator.Validate
                (
                    functionElement, 
                    typeof(bool), 
                    application, 
                    validationErrors
                )
            );
        }
    }
}
