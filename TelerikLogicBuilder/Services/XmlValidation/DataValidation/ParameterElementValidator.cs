using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ParameterElementValidator : IParameterElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralParameterElementValidator _literalParameterElementValidator;
        private readonly IObjectParameterElementValidator _objectParameterElementValidator;
        private readonly ILiteralListParameterElementValidator _literalListParameterElementValidator;
        private readonly IObjectListParameterElementValidator _objectListParameterElementValidator;

        public ParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _literalParameterElementValidator = xmlElementValidator.LiteralParameterElementValidator;
            _objectParameterElementValidator = xmlElementValidator.ObjectParameterElementValidator;
            _literalListParameterElementValidator = xmlElementValidator.LiteralListParameterElementValidator;
            _objectListParameterElementValidator = xmlElementValidator.ObjectListParameterElementValidator;
        }

        public void Validate(ParameterBase parameter, XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            switch (parameter)
            {
                case LiteralParameter literalParameter:
                    _literalParameterElementValidator.Validate(literalParameter, parameterElement, application, validationErrors);
                    break;
                case ObjectParameter objectParameter:
                    _objectParameterElementValidator.Validate(objectParameter, parameterElement, application, validationErrors);
                    break;
                case ListOfLiteralsParameter listOfLiteralsParameter:
                    _literalListParameterElementValidator.Validate(listOfLiteralsParameter, parameterElement, application, validationErrors);
                    break;
                case ListOfObjectsParameter listOfObjectsParameter:
                    _objectListParameterElementValidator.Validate(listOfObjectsParameter, parameterElement, application, validationErrors);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{343B547B-7A8D-41F0-836A-DB4AFF897856}");
            }
        }
    }
}
