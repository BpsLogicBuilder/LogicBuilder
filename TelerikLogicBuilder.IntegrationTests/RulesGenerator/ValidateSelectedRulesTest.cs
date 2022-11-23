using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator
{
    public class ValidateSelectedRulesTest : IClassFixture<ValidateSelectedRulesFixture>
    {
        private readonly ValidateSelectedRulesFixture _fixture;

        public ValidateSelectedRulesTest(ValidateSelectedRulesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateValidateSelectedRules()
        {
            //arrange
            IValidateSelectedRules validator = _fixture.ServiceProvider.GetRequiredService<IValidateSelectedRules>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public async Task RulesValidateSelectedRulesSucceeds()
        {
            //arrange
            IValidateSelectedRules validator = _fixture.ServiceProvider.GetRequiredService<IValidateSelectedRules>();
            var application = _fixture.ConfigurationService.GetSelectedApplication();
            var cancellationToken = new CancellationTokenSource();
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });

            //act
            IList<ResultMessage> results = await _fixture.LoadContextSponsor.RunAsync
            (
                () => validator.Validate
                (
                    new string[] { GetFullSourceFilePath("savecourse") },
                    application,
                    progress,
                    cancellationToken
                ),
                progress
            );

            Assert.Single(results);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.ruleSetIsValidFormat,
                    "savecourse.module"
                ), 
                results[0].Message
            );
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
          => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"RuleSets\{nameof(ValidateSelectedRulesTest)}\{fileNameNoExtension}.module");
    }

    public class ValidateSelectedRulesFixture : IDisposable
    {
        public ValidateSelectedRulesFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            TypeHelper = ServiceProvider.GetRequiredService<ITypeHelper>();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            VariableFactory = ServiceProvider.GetRequiredService<IVariableFactory>();
            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ProjectPropertiesItemFactory.GetApplication
                    (
                        "App02",
                        "App02",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["ClearErrorMessages"] = FunctionFactory.GetFunction
                    (
                        "ClearErrorMessages",
                        "Clear",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.Response.ErrorMessages",
                        "Field.Property.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["Set Variable"] = FunctionFactory.GetFunction
                    (
                        "Set Variable",
                        "assert",
                        FunctionCategories.Assert,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["WriteToLog"] = FunctionFactory.GetFunction
                    (
                        "WriteToLog",
                        "WriteToLog",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "message",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>()
                             )
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.VariableList = new VariableList
            (
                new Dictionary<string, VariableBase>
                {
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );


            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

            VisioApplication = new InvisibleApp();
            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        LiteralVariable GetLiteralVariable(string name, LiteralVariableType literalVariableType)
            => VariableFactory.GetLiteralVariable
            (
                name,
                name,
                VariableCategory.StringKeyIndexer,
                TypeHelper.ToId(EnumHelper.GetSystemType(literalVariableType)),
                "",
                "flowManager.FlowDataCache.Items",
                "Field.Property.Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                literalVariableType,
                LiteralVariableInputStyle.SingleLineTextBox,
                "",
                "",
                new List<string>()
            );

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
            foreach (Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }

        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IVariableFactory VariableFactory;
    }
}
