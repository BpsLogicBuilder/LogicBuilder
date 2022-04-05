using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class DecisionsElementValidator : IDecisionsElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;

        public DecisionsElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _decisionsDataParser = xmlElementValidator.DecisionsDataParser;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        //so can't be sset as readonly fields in the constructor
        private IDecisionElementValidator DecisionElementValidator => _xmlElementValidator.DecisionElementValidator;

        public void Validate(XmlElement decisionsElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (decisionsElement.Name != XmlDataConstants.DECISIONSELEMENT)
                throw _exceptionHelper.CriticalException("{094ED028-A810-4EE8-AAA2-CD4CCE07E767}");

            _decisionsDataParser.Parse(decisionsElement).DecisionElements.ForEach
            (
                decisionElement => DecisionElementValidator.Validate(decisionElement, application, validationErrors)
            );
        }
    }
}
