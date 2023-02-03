using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.FormsPreviewer.Commands;

namespace TelerikLogicBuilder.FormsPreviewer
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        public RadForm1()
        {
            InitializeComponent();
            ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider;
            _configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            _loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            _constructorListInitializer = serviceProvider.GetRequiredService<IConstructorListInitializer>();
            _fragmentListInitializer = serviceProvider.GetRequiredService<IFragmentListInitializer>();
            _functionListInitializer = serviceProvider.GetRequiredService<IFunctionListInitializer>();
            _variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            _loadContextSponsor = serviceProvider.GetRequiredService<ILoadContextSponsor>();
            _configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\TelerikLogicBuilder\FlowProjects\Contoso.Test\Contoso.Test.lbproj");
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();
            serviceProvider.GetRequiredService<IMainWindow>().Instance = new MockMdiParent();
            _loadContextSponsor.LoadAssembiesIfNeeded();
            Initialize();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly IVariableListInitializer _variableListInitializer;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly ILoadContextSponsor _loadContextSponsor;
        #endregion Fields

        private void Initialize()
        {
            AddButtonClickCommand(btnSelectVariableForm, new SelectVariableFormCommand(this));
            AddButtonClickCommand(btnSelectConstructorForm, new SelectConstructorFormCommand(this));
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }
    }
}
