using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class FunctionNodeInfo
    {
        private readonly IFunctionFactory _functionFactory;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly IParametersManager _parametersManager;
        private readonly IReturnTypeManager _returnTypeManager;
        private readonly ITypeHelper _typeHelper;

        public FunctionNodeInfo(
            IFunctionFactory functionFactory,
            IMemberAttributeReader memberAttributeReader,
            IParametersManager parametersManager,
            IReturnTypeManager returnTypeManager,
            ITypeHelper typeHelper,
            MethodInfo mInfo)
        {
            MInfo = mInfo;
            _functionFactory = functionFactory;
            _typeHelper = typeHelper;
            _memberAttributeReader = memberAttributeReader;
            _parametersManager = parametersManager;
            _returnTypeManager = returnTypeManager;
        }

        #region Properties
        internal MethodInfo MInfo { get; }
        internal string AlsoKnownAs => _memberAttributeReader.GetAlsoKnownAs(MInfo);
        internal string MemberName => MInfo.Name;
        internal string Name 
            => string.IsNullOrEmpty(AlsoKnownAs)
                ? MInfo.ReflectedType != null
                    ? $"{MInfo.ReflectedType.Name}{MiscellaneousConstants.UNDERSCORE}{MInfo.Name}"
                    : MInfo.Name
                : AlsoKnownAs;

        internal string Sumnmary => _memberAttributeReader.GetSummary(MInfo);
        internal FunctionCategories FunctionCategory
            => HasValidMultipleChoiceParameter
                    ? FunctionCategories.DialogForm
                    : _memberAttributeReader.GetFunctionCategory(MInfo);

        private bool HasValidMultipleChoiceParameter
        {
            get
            {
                var parameters = this.MInfo.GetParameters();
                if (parameters.Length == 0)
                    return false;

                return _typeHelper.IsValidConnectorList(parameters[^1].ParameterType);
            }
        }
        #endregion Properties

        #region Methods
        internal Function? GetFunction(string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, ParametersLayout parametersLayout)
        {
            if (MInfo.DeclaringType == null)
                return null;

            return _functionFactory.GetFunction
            (
                Name,
                MemberName,
                FunctionCategory,
                typeName,
                referenceName,
                referenceDefinition,
                castReferenceAs,
                referenceCategory,
                parametersLayout,
                _parametersManager
                    .GetParameterNodeInfos
                    (
                        MInfo.GetParameters()
                            .Where(p => !_typeHelper.IsValidConnectorList(p.ParameterType))
                            .ToArray()
                    )
                    .Select(info => info.Parameter)
                    .ToList(),
                new List<string>(MInfo.DeclaringType.GetGenericArguments().Select(a => a.Name)),
                _returnTypeManager.GetReturnTypeInfo(MInfo).GetReturnType(),
                this.Sumnmary
            );
        }
        #endregion Methods
    }
}
