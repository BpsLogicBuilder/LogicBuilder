using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ParameterElementValidator : IParameterElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ParameterElementValidator(
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ILiteralParameterElementValidator? _literalParameterElementValidator;
		private ILiteralParameterElementValidator LiteralParameterElementValidator => _literalParameterElementValidator ??= _xmlElementValidatorFactory.GetLiteralParameterElementValidator();
        private IObjectParameterElementValidator? _objectParameterElementValidator;
		private IObjectParameterElementValidator ObjectParameterElementValidator => _objectParameterElementValidator ??= _xmlElementValidatorFactory.GetObjectParameterElementValidator();
        private ILiteralListParameterElementValidator? _literalListParameterElementValidator;
		private ILiteralListParameterElementValidator LiteralListParameterElementValidator => _literalListParameterElementValidator ??= _xmlElementValidatorFactory.GetLiteralListParameterElementValidator();
        private IObjectListParameterElementValidator? _objectListParameterElementValidator;
		private IObjectListParameterElementValidator ObjectListParameterElementValidator => _objectListParameterElementValidator ??= _xmlElementValidatorFactory.GetObjectListParameterElementValidator();

        public void Validate(XmlElement parameterElement, ParameterBase parameter, ApplicationTypeInfo application, List<string> validationErrors)
        {
            switch (parameter)
            {
                case LiteralParameter literalParameter:
                    LiteralParameterElementValidator.Validate(parameterElement, literalParameter, application, validationErrors);
                    break;
                case ObjectParameter objectParameter:
                    ObjectParameterElementValidator.Validate(parameterElement, objectParameter, application, validationErrors);
                    break;
                case ListOfLiteralsParameter listOfLiteralsParameter:
                    LiteralListParameterElementValidator.Validate(parameterElement, listOfLiteralsParameter, application, validationErrors);
                    break;
                case ListOfObjectsParameter listOfObjectsParameter:
                    ObjectListParameterElementValidator.Validate(parameterElement, listOfObjectsParameter, application, validationErrors);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{343B547B-7A8D-41F0-836A-DB4AFF897856}");
            }
        }
    }
}
