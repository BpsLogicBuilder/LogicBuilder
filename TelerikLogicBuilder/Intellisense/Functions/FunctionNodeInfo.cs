using ABIS.LogicBuilder.FlowBuilder.Enums;
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
        private readonly ITypeHelper _typeHelper;
        private readonly IContextProvider _contextProvider;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly IParametersManager _parametersManager;
        private readonly IReturnTypeManager _returnTypeManager;

        public FunctionNodeInfo(
            MethodInfo mInfo,
            IContextProvider contextProvider,
            IMemberAttributeReader memberAttributeReader,
            IParametersManager parametersManager,
            IReturnTypeManager returnTypeManager)
        {
            MInfo = mInfo;
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
            _memberAttributeReader = memberAttributeReader;
            _parametersManager = parametersManager;
            _returnTypeManager = returnTypeManager;
        }

        #region Properties
        internal MethodInfo MInfo { get; }
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
        internal Function? GetFunction(string name, string memberName, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, ParametersLayout parametersLayout)
        {
            if (MInfo.DeclaringType == null)
                return null;

            return new
            (
                name,
                memberName,
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
                this.Sumnmary,
                _contextProvider
            );
        }
        #endregion Methods
    }
}
