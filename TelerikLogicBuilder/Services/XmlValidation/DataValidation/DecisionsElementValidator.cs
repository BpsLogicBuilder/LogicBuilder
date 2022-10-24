using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class DecisionsElementValidator : IDecisionsElementValidator
    {
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public DecisionsElementValidator(
            IDecisionsDataParser decisionsDataParser,
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _decisionsDataParser = decisionsDataParser;
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IDecisionElementValidator? _decisionElementValidator;
		private IDecisionElementValidator DecisionElementValidator => _decisionElementValidator ??= _xmlElementValidatorFactory.GetDecisionElementValidator();

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
