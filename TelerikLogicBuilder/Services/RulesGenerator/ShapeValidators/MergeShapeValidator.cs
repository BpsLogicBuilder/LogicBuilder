using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class MergeShapeValidator : IMergeShapeValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IShapeHelper _shapeHelper;

        public MergeShapeValidator(
            IConfigurationService configurationService,
            IContextProvider contextProvider,
            IExceptionHelper exceptionHelper,
            IShapeHelper shapeHelper)
        {
            _configurationService = configurationService;
            _contextProvider = contextProvider;
            _exceptionHelper = exceptionHelper;
            _shapeHelper = shapeHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
        {
            new MergeShapeValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                _configurationService,
                _contextProvider,
                _exceptionHelper,
                _shapeHelper
            ).Validate();
        }
    }
}
