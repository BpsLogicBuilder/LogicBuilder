using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class FunctionNodeBuilder : IFunctionNodeBuilder
    {
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly IIntellisenseTreeNodeFactory _intellisenseTreeNodeFactory;
        private readonly IParametersManager _parametersManager;
        private readonly IParametersMatcher _parametersMatcher;
        private readonly ITypeHelper _typeHelper;

        public FunctionNodeBuilder(
            IIntellisenseHelper intellisenseHelper,
            IIntellisenseTreeNodeFactory intellisenseTreeNodeFactory,
            IParametersManager parametersManager,
            IParametersMatcher parametersMatcher,
            ITypeHelper typeHelper)
        {
            _intellisenseHelper = intellisenseHelper;
            _intellisenseTreeNodeFactory = intellisenseTreeNodeFactory;
            _parametersManager = parametersManager;
            _parametersMatcher = parametersMatcher;
            _typeHelper = typeHelper;
        }

        public BaseTreeNode? Build(FunctionCategories functionCategory, VariableTreeNode? parentNode, Type parentType, string methodName, IList<ParameterBase> configuredParameters, BindingFlagCategory bindingFlagCategory)
        {
            switch (functionCategory)
            {
                case FunctionCategories.DialogForm:
                case FunctionCategories.Standard:
                    MethodInfo? mInfo = parentType.GetMethods(_intellisenseHelper.GetBindingFlags(bindingFlagCategory)).FirstOrDefault
                    (
                        m =>
                        {
                            if (m.Name != methodName)
                                return false;

                            ICollection<ParameterNodeInfoBase> parsFromMInfo = _parametersManager.GetParameterNodeInfos
                            (
                                m.GetParameters()
                                    .Where(p => !_typeHelper.IsValidConnectorList(p.ParameterType))
                            );

                            return _parametersMatcher.MatchParameters(parsFromMInfo.ToArray(), configuredParameters);
                        }
                    );

                    return mInfo != null
                        ? _intellisenseTreeNodeFactory.GetFunctionTreeNode(mInfo, parentNode)
                        : null;
                default:
                    return null;
            }
        }
    }
}
