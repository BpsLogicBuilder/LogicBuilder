using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class ShapeValidator : IShapeValidator
    {
        private readonly IActionShapeValidator _actionShapeValidator;
        private readonly IApplicationConnectorValidator _applicationConnectorValidator;
        private readonly IExceptionHelper _exceptionHelper;

        public ShapeValidator(IActionShapeValidator actionShapeValidator, IApplicationConnectorValidator applicationConnectorValidator, IExceptionHelper exceptionHelper)
        {
            _actionShapeValidator = actionShapeValidator;
            _applicationConnectorValidator = applicationConnectorValidator;
            _exceptionHelper = exceptionHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            switch (shape.Master.NameU)
            {
                case UniversalMasterName.ACTION:
                    _actionShapeValidator.Validate(sourceFile, page, shape, validationErrors, application);
                    break;
                case UniversalMasterName.APP01CONNECTOBJECT:
                case UniversalMasterName.APP02CONNECTOBJECT:
                case UniversalMasterName.APP03CONNECTOBJECT:
                case UniversalMasterName.APP04CONNECTOBJECT:
                case UniversalMasterName.APP05CONNECTOBJECT:
                case UniversalMasterName.APP06CONNECTOBJECT:
                case UniversalMasterName.APP07CONNECTOBJECT:
                case UniversalMasterName.APP08CONNECTOBJECT:
                case UniversalMasterName.APP09CONNECTOBJECT:
                case UniversalMasterName.APP10CONNECTOBJECT:
                case UniversalMasterName.OTHERSCONNECTOBJECT:
                    _applicationConnectorValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{1847D564-79A4-49C0-8B82-DD7A91B3EA44}");
            }
        }
    }
}
