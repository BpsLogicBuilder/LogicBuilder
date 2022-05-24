using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class GetValidConfigurationFromData : IGetValidConfigurationFromData
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorGenericsConfigrationValidator _constructorGenericsConfigrationValidator;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IGenericConstructorHelper _genericConstructorHelper;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public GetValidConfigurationFromData(IConfigurationService configurationService, IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator, IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator, IGenericConstructorHelper genericConstructorHelper, IGenericFunctionHelper genericFunctionHelper, ITypeLoadHelper typeLoadHelper)
        {
            _configurationService = configurationService;
            _constructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            _functionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            _genericConstructorHelper = genericConstructorHelper;
            _genericFunctionHelper = genericFunctionHelper;
            _typeLoadHelper = typeLoadHelper;
        }

        public bool TryGetConstructor(ConstructorData constructorData, ApplicationTypeInfo application, out Constructor? constructor)
        {
            if (!_configurationService.ConstructorList.Constructors.TryGetValue(constructorData.Name, out constructor))
                return false;

            if (constructor.HasGenericArguments)
            {
                if (!_constructorGenericsConfigrationValidator.Validate(constructor, constructorData.GenericArguments, application, new List<string>()))
                    return false;

                constructor = _genericConstructorHelper.ConvertGenericTypes(constructor, constructorData.GenericArguments, application);
            }

            return _typeLoadHelper.TryGetSystemType(constructor.TypeName, application, out _);
        }

        public bool TryGetFunction(FunctionData functionData, ApplicationTypeInfo application, out Function? function)
        {
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out function))
                return false;

            if (function.HasGenericArguments)
            {
                if (!_functionGenericsConfigrationValidator.Validate(function, functionData.GenericArguments, application, new List<string>()))
                    return false;

                function = _genericFunctionHelper.ConvertGenericTypes(function, functionData.GenericArguments, application);
            }

            return _typeLoadHelper.TryGetSystemType(function.ReturnType, functionData.GenericArguments, application, out _);
        }

        public bool TryGetVariable(VariableData variableData, ApplicationTypeInfo application, out VariableBase? variable)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableData.Name, out variable))
                return false;

            return _typeLoadHelper.TryGetSystemType(variable, application, out _);
        }

        public bool TryGetVariable(DecisionData decisionData, ApplicationTypeInfo application, out VariableBase? variable)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(decisionData.Name, out variable))
                return false;

            return _typeLoadHelper.TryGetSystemType(variable, application, out _);
        }
    }
}
