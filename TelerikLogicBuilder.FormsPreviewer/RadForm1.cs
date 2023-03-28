using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Services.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.FormsPreviewer.Commands;

namespace TelerikLogicBuilder.FormsPreviewer
{
    internal partial class RadForm1 : Telerik.WinControls.UI.RadForm, IApplicationForm
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
            //C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso\Contoso.lbproj
            //C:\.github\BlaiseD\LogicBuilder.Samples\Xamarin\Contoso\FlowProjects\Contoso.XPlatform\Contoso.XPlatform.lbproj
            //C:\TelerikLogicBuilder\FlowProjects\Contoso.Test\Contoso.Test.lbproj
            //_configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\TelerikLogicBuilder\FlowProjects\Contoso.Test\Contoso.Test.lbproj");
            //_configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso\Contoso.lbproj");
            _configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\.github\BlaiseD\LogicBuilder.Samples\Xamarin\Contoso\FlowProjects\Contoso.XPlatform\Contoso.XPlatform.lbproj");
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();
            _themeManager = serviceProvider.GetRequiredService<IThemeManager>();
            serviceProvider.GetRequiredService<IMainWindow>().Instance = new MockMdiParent();
            _loadContextSponsor.LoadAssembiesIfNeeded();
            _applicationTypeInfoManager = serviceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            _application = _applicationTypeInfoManager.GetApplicationTypeInfo(_configurationService.GetSelectedApplication().Name);
            Initialize();
            Settings.Default.colorTheme = "Dark";
            Settings.Default.Save();
        }

        event EventHandler<ApplicationChangedEventArgs>? IApplicationForm.ApplicationChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        #region Fields
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly IVariableListInitializer _variableListInitializer;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly ILoadContextSponsor _loadContextSponsor;
        private readonly IThemeManager _themeManager;

        private ApplicationTypeInfo _application;
        public ApplicationTypeInfo Application => _application;

        //public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;
        #endregion Fields

        private void Initialize()
        {
            AddButtonClickCommand(btnEditVariableForm, new EditVariableFormCommand(this));
            AddButtonClickCommand(btnSelectConstructorForm, new SelectConstructorFormCommand(this));
            AddButtonClickCommand
            (
                btnSelectBoolFunction,
                new SelectFunctionFormCommand
                (
                    this,
                    _configurationService.FunctionList.BooleanFunctions,
                    new TreeFolder[] { _configurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder, _configurationService.FunctionList.BooleanFunctionsTreeFolder }
                )
            );
            AddButtonClickCommand
            (
                btnSelectDialogFunction,
                new SelectFunctionFormCommand
                (
                    this,
                    _configurationService.FunctionList.DialogFunctions,
                    new TreeFolder[] { _configurationService.FunctionList.BuiltInDialogFunctionsTreeFolder, _configurationService.FunctionList.DialogFunctionsTreeFolder }
                )
            );
            AddButtonClickCommand
            (
                btnSelectTableFunction,
                new SelectFunctionFormCommand
                (
                    this,
                    _configurationService.FunctionList.TableFunctions,
                    new TreeFolder[] { _configurationService.FunctionList.BuiltInTableFunctionsTreeFolder, _configurationService.FunctionList.TableFunctionsTreeFolder }
                )
            );
            AddButtonClickCommand
            (
                btnSelectValueFunction,
                new SelectFunctionFormCommand
                (
                    this,
                    _configurationService.FunctionList.ValueFunctions,
                    new TreeFolder[] { _configurationService.FunctionList.BuiltInValueFunctionsTreeFolder, _configurationService.FunctionList.ValueFunctionsTreeFolder }
                )
            );
            AddButtonClickCommand
            (
                btnSelectVoidFunction,
                new SelectFunctionFormCommand
                (
                    this,
                    _configurationService.FunctionList.VoidFunctions,
                    new TreeFolder[] { _configurationService.FunctionList.BuiltInVoidFunctionsTreeFolder, _configurationService.FunctionList.VoidFunctionsTreeFolder }
                )
            );
            AddButtonClickCommand
            (
                btnEditConstructorForm,
                new EditConstructorFormCommand
                (
                    serviceProvider.GetRequiredService<IConstructorTypeHelper>(),
                    serviceProvider.GetRequiredService<ITypeLoadHelper>(),
                    this
                )
            );
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void RadButtonReloadConfiguration_Click(object sender, EventArgs e)
        {
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();
        }

        private void RadButtonSetFontSize09_Click(object sender, EventArgs e)
        {
            _themeManager.SetFontSize(9);
        }

        private void RadButtonSetFontSize10_Click(object sender, EventArgs e)
        {
            _themeManager.SetFontSize(10);
        }

        private void RadButtonSetFontSize11_Click(object sender, EventArgs e)
        {
            _themeManager.SetFontSize(11);
        }

        private void RadButtonSetFontSize12_Click(object sender, EventArgs e)
        {
            _themeManager.SetFontSize(12);
        }

        private void radButtonSetFontSize13_Click(object sender, EventArgs e)
        {
            _themeManager.SetFontSize(13);
        }

        private void RadButtonSetFontSize14_Click(object sender, EventArgs e)
        {
            _themeManager.SetFontSize(14);
        }

        public void ClearMessage()
        {
        }

        public void SetErrorMessage(string message)
        {
        }

        public void SetMessage(string message, string title = "")
        {
        }
    }
}
