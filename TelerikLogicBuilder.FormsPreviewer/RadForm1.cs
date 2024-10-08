﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.FormsPreviewer.Commands;
using TelerikLogicBuilder.FormsPreviewer.Commands.Xml;

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

        event EventHandler<ApplicationChangedEventArgs>? IApplicationHostControl.ApplicationChanged
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

        private readonly ApplicationTypeInfo _application;
        public ApplicationTypeInfo Application => _application;

        //public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;
        #endregion Fields

        private void Initialize()
        {
            AddButtonClickCommand(btnEditVariableForm, new EditVariableFormCommand(this));
            AddButtonClickCommand(btnSelectConstructorForm, new SelectConstructorFormCommand(this));
            AddButtonClickCommand(btnSelectFragmentForm, new SelectFragmentFormCommand(this));
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
            AddButtonClickCommand
            (
                btnEditBooleanFunctionForm,
                new EditBooleanFunctionFormCommand
                (
                    this
                )
            );
            AddButtonClickCommand(btnEditDecisionForm, new EditDecisionFormCommand(this));
            AddButtonClickCommand
            (
                btnEditDialogFunctionForm,
                new EditDialogFunctionFormCommand
                (
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditValueFunctionForm,
                new EditValueFunctionFormCommand
                (
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditLiteralListForm,
                new EditParameterLiteralListFormCommand
                (
                    _configurationService,
                    serviceProvider.GetRequiredService<ILiteralListDataParser>(),
                    serviceProvider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditObjectListForm,
                new EditParameterObjectListFormCommand
                (
                    _configurationService,
                    serviceProvider.GetRequiredService<IObjectListDataParser>(),
                    serviceProvider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                    serviceProvider.GetRequiredService<ITypeLoadHelper>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditBooleanFunctionFormXml,
                new EditBooleanFunctionFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditBuildDecisionFormXml,
                new EditBuildDecisionFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditConditionsFormXml,
                new EditConditionsFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditConstructorFormXml,
                new EditConstructorFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditDecisionsFormXml,
                new EditDecisionsFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditDialogFunctionFormXml,
                new EditDialogFunctionFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditFunctionsFormXml,
                new EditFunctionsFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditLiteralListFormXml,
                new EditLiteralListFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditObjectListFormXml,
                new EditObjectListFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditTableFunctionsFormXml,
                new EditTableFunctionsFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    this
                )
            );
            AddButtonClickCommand
            (
                btnEditValueFunctionFormXml,
                new EditValueFunctionFormXmlCommand
                (
                    serviceProvider.GetRequiredService<IXmlDocumentHelpers>(),
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

        private void RadButtonSetFontSize13_Click(object sender, EventArgs e)
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
