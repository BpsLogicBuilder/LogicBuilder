using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureVariablesFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureVariablesRootNode;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories
{
    internal class ConfigureVariablesControlFactory : IConfigureVariablesControlFactory
    {
        private readonly Func<IConfigureVariablesForm, IConfigureLiteralListVariableControl> _getConfigureLiteralListVariableControl;
        private readonly Func<IConfigureVariablesForm, IConfigureLiteralVariableControl> _getConfigureLiteralVariableControl;
        private readonly Func<IConfigureVariablesForm, IConfigureObjectListVariableControl> _getConfigureObjectListVariableControl;
        private readonly Func<IConfigureVariablesForm, IConfigureObjectVariableControl> _getConfigureObjectVariableControl;
        private readonly Func<IConfigureVariablesForm, IConfigureVariablesFolderControl> _getConfigureVariablesFolderControl;

        public ConfigureVariablesControlFactory(
            Func<IConfigureVariablesForm, IConfigureLiteralListVariableControl> getConfigureLiteralListVariableControl,
            Func<IConfigureVariablesForm, IConfigureLiteralVariableControl> getConfigureLiteralVariableControl,
            Func<IConfigureVariablesForm, IConfigureObjectListVariableControl> getConfigureObjectListVariableControl,
            Func<IConfigureVariablesForm, IConfigureObjectVariableControl> getConfigureObjectVariableControl,
            Func<IConfigureVariablesForm, IConfigureVariablesFolderControl> getConfigureVariablesFolderControl)
        {
            _getConfigureLiteralListVariableControl = getConfigureLiteralListVariableControl;
            _getConfigureLiteralVariableControl = getConfigureLiteralVariableControl;
            _getConfigureObjectListVariableControl = getConfigureObjectListVariableControl;
            _getConfigureObjectVariableControl = getConfigureObjectVariableControl;
            _getConfigureVariablesFolderControl = getConfigureVariablesFolderControl;
        }

        public IConfigureLiteralListVariableControl GetConfigureLiteralListVariableControl(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureLiteralListVariableControl(configureVariablesForm);

        public IConfigureLiteralVariableControl GetConfigureLiteralVariableControl(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureLiteralVariableControl(configureVariablesForm);

        public IConfigureObjectListVariableControl GetConfigureObjectListVariableControl(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureObjectListVariableControl(configureVariablesForm);

        public IConfigureObjectVariableControl GetConfigureObjectVariableControl(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureObjectVariableControl(configureVariablesForm);

        public IConfigureVariablesFolderControl GetConfigureVariablesFolderControl(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesFolderControl(configureVariablesForm);

        public IConfigureVariablesRootNodeControl GetConfigureVariablesRootNodeControl()
            => new ConfigureVariablesRootNodeControl();
    }
}
