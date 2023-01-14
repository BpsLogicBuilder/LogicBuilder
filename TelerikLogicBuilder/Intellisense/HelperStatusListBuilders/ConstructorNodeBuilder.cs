using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class ConstructorNodeBuilder : IConstructorNodeBuilder
    {
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly IParametersManager _parametersManager;
        private readonly IParametersMatcher _parametersMatcher;

        public ConstructorNodeBuilder(
            IIntellisenseHelper intellisenseHelper,
            IParametersManager parametersManager,
            IParametersMatcher parametersMatcher)
        {
            _intellisenseHelper = intellisenseHelper;
            _parametersManager = parametersManager;
            _parametersMatcher = parametersMatcher;
        }

        public ConstructorTreeNode? Build(Type declaringType, IList<ParameterBase> configuredParameters)
        {
            ConstructorInfo? cInfo = declaringType.GetConstructors
            (
                _intellisenseHelper.GetConstructorBindingFlags()
            )
            .FirstOrDefault
            (
                constructor => _parametersMatcher.MatchParameters
                (
                    _parametersManager.GetParameterNodeInfos(constructor.GetParameters()).ToArray(),
                    configuredParameters
                )
            );

            if (cInfo != null)
            {
                return new ConstructorTreeNode(cInfo);
            }

            return null;
        }
    }
}
