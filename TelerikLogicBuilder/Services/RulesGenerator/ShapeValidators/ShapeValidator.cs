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
        private readonly IActionShapeValidator actionShapeValidator;
        private readonly IExceptionHelper _exceptionHelper;

        public ShapeValidator(IActionShapeValidator actionShapeValidator, IExceptionHelper exceptionHelper)
        {
            this.actionShapeValidator = actionShapeValidator;
            _exceptionHelper = exceptionHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            switch (shape.Master.NameU)
            {
                case UniversalMasterName.ACTION:
                    actionShapeValidator.Validate(sourceFile, page, shape, validationErrors, application);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{1847D564-79A4-49C0-8B82-DD7A91B3EA44}");
            }
        }
    }
}
