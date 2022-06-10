using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class ShapeValidator : IShapeValidator
    {
        private readonly IActionShapeValidator _actionShapeValidator;
        private readonly IApplicationConnectorValidator _applicationConnectorValidator;
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IBeginShapeValidator _beginShapeValidator;
        private readonly ICommentShapeValidator _commentShapeValidator;
        private readonly IConditionShapeValidator _conditionShapeValidator;
        private readonly IDecisionShapeValidator _decisionShapeValidator;
        private readonly IDialogShapeValidator _dialogShapeValidator;
        private readonly IEndShapeValidator _endShapeValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IJumpShapeValidator _jumpShapeValidator;
        private readonly IMergeShapeValidator _mergeShapeValidator;
        private readonly IModuleShapeValidator _moduleShapeValidator;
        private readonly IRegularConnectorValidator _regularConnectorValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IWaitConditionShapeValidator _waitConditionShapeValidator;
        private readonly IWaitDecisionShapeValidator _waitDecisionShapeValidator;

        public ShapeValidator(
            IActionShapeValidator actionShapeValidator,
            IApplicationConnectorValidator applicationConnectorValidator,
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IBeginShapeValidator beginShapeValidator,
            ICommentShapeValidator commentShapeValidator,
            IConditionShapeValidator conditionShapeValidator,
            IDecisionShapeValidator decisionShapeValidator,
            IDialogShapeValidator dialogShapeValidator,
            IEndShapeValidator endShapeValidator,
            IExceptionHelper exceptionHelper,
            IJumpShapeValidator jumpShapeValidator,
            IMergeShapeValidator mergeShapeValidator,
            IModuleShapeValidator moduleShapeValidator,
            IRegularConnectorValidator regularConnectorValidator,
            IShapeHelper shapeHelper,
            IWaitConditionShapeValidator waitConditionShapeValidator,
            IWaitDecisionShapeValidator waitDecisionShapeValidator)
        {
            _actionShapeValidator = actionShapeValidator;
            _applicationConnectorValidator = applicationConnectorValidator;
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _beginShapeValidator = beginShapeValidator;
            _commentShapeValidator = commentShapeValidator;
            _conditionShapeValidator = conditionShapeValidator;
            _decisionShapeValidator = decisionShapeValidator;
            _dialogShapeValidator = dialogShapeValidator;
            _endShapeValidator = endShapeValidator;
            _exceptionHelper = exceptionHelper;
            _jumpShapeValidator = jumpShapeValidator;
            _mergeShapeValidator = mergeShapeValidator;
            _moduleShapeValidator = moduleShapeValidator;
            _regularConnectorValidator = regularConnectorValidator;
            _shapeHelper = shapeHelper;
            _waitConditionShapeValidator = waitConditionShapeValidator;
            _waitDecisionShapeValidator = waitDecisionShapeValidator;
        }

        public void ValidateShape(string sourceFile, Page page, ShapeBag shapeBag, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            Shape shape = shapeBag.Shape;
            switch (shape.Master.NameU)
            {
                case UniversalMasterName.ACTION:
                    foreach (Shape connector in _shapeHelper.GetOutgoingBlankConnectors(shape))
                    {//need the appropriate ApplicationTypeInfo for application specific connectors
                        _actionShapeValidator.Validate(sourceFile, page, shape, validationErrors, GetApplicationTypeInfo(connector));
                    }
                    break;
                case UniversalMasterName.BEGINFLOW:
                case UniversalMasterName.MODULEBEGIN:
                    _beginShapeValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                case UniversalMasterName.COMMENT:
                    _commentShapeValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                case UniversalMasterName.CONDITIONOBJECT:
                    _conditionShapeValidator.Validate(sourceFile, page, shape, validationErrors, application);
                    break;
                case UniversalMasterName.DECISIONOBJECT:
                    _decisionShapeValidator.Validate(sourceFile, page, shape, validationErrors, application);
                    break;
                case UniversalMasterName.DIALOG:
                    _dialogShapeValidator.Validate(sourceFile, page, shape, validationErrors, application);
                    break;
                case UniversalMasterName.ENDFLOW:
                case UniversalMasterName.MODULEEND:
                case UniversalMasterName.TERMINATE:
                    _endShapeValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                case UniversalMasterName.JUMPOBJECT:
                    _jumpShapeValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                case UniversalMasterName.MERGEOBJECT:
                    _mergeShapeValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                case UniversalMasterName.MODULE:
                    _moduleShapeValidator.Validate(sourceFile, page, shape, validationErrors);
                    break;
                case UniversalMasterName.WAITCONDITIONOBJECT:
                    foreach (Shape connector in _shapeHelper.GetOutgoingBlankConnectors(shape))
                    {//need the appropriate ApplicationTypeInfo for application specific connectors
                        _waitConditionShapeValidator.Validate(sourceFile, page, shape, validationErrors, GetApplicationTypeInfo(connector));
                    }
                    break;
                case UniversalMasterName.WAITDECISIONOBJECT:
                    foreach (Shape connector in _shapeHelper.GetOutgoingBlankConnectors(shape))
                    {//need the appropriate ApplicationTypeInfo for application specific connectors
                        _waitDecisionShapeValidator.Validate(sourceFile, page, shape, validationErrors, GetApplicationTypeInfo(connector));
                    }
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{1847D564-79A4-49C0-8B82-DD7A91B3EA44}");
            }

            ApplicationTypeInfo GetApplicationTypeInfo(Shape connector)
            {
                if (connector.Master.NameU == UniversalMasterName.CONNECTOBJECT)
                    return application;

                return _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _shapeHelper.GetApplicationList(connector, shapeBag).OrderBy(n => n).First()
                );
            }
        }

        public void ValidateConnector(string sourceFile, Page page, Shape connector, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            switch (connector.Master.NameU)
            {
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
                    _applicationConnectorValidator.Validate(sourceFile, page, connector, validationErrors);
                    break;
                case UniversalMasterName.CONNECTOBJECT:
                    _regularConnectorValidator.Validate(sourceFile, page, connector, validationErrors, application);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{1010EF7E-2B8C-4053-8577-CA45333A22FB}");
            }
        }
    }
}
