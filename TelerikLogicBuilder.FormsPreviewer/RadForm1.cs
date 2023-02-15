﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
            //C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso\Contoso.lbproj
            //C:\.github\BlaiseD\LogicBuilder.Samples\Xamarin\Contoso\FlowProjects\Contoso.XPlatform\Contoso.XPlatform.lbproj
            _configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso\Contoso.lbproj");
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
                    this
                )
            );
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }
    }
}