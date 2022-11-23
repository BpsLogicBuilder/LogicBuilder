using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectListVariableElementValidatorTest : IClassFixture<ObjectListVariableElementValidatorFixture>
    {
        private readonly ObjectListVariableElementValidatorFixture _fixture;

        public ObjectListVariableElementValidatorTest(ObjectListVariableElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateObjectListVariableElementValidator()
        {
            //arrange
            IObjectListVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ObjectListVariableElementValidatorWorksForValidData()
        {
            //arrange
            IObjectListVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<objectListVariable>
                                                        <objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""eee"">
                                                            <object>
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""stringProperty"">AA</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </object>
                                                            <object>
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""stringProperty"">BB</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </object>
                                                        </objectList>
                                                    </objectListVariable>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            ListOfObjectsVariable variable = (ListOfObjectsVariable)_fixture.ConfigurationService.VariableList.Variables["ObjectListVariable"];

            //act
            xmlValidator.Validate
            (
                xmlElement,
                variable,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.False(errors.Any());
        }

        [Fact]
        public void ObjectListVariableElementValidatorFailsIfObjectTypeCannotBeLoaded()
        {
            //arrange
            IObjectListVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<objectListVariable>
                                                        <objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""eee"">
                                                            <object>
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""stringProperty"">AA</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </object>
                                                            <object>
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""stringProperty"">BB</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </object>
                                                        </objectList>
                                                    </objectListVariable>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            ListOfObjectsVariable variable = _fixture.VariableFactory.GetListOfObjectsVariable
            (
                "ObjectListVariable",
                "ObjectListVariable",
                VariableCategory.StringKeyIndexer,
                "System.Collections.Generic.IList`1[[Contoso.Test.Business.Responses.TypeNotFound, Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                "",
                "flowManager.FlowDataCache.Items",
                "Field.Property.Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                "Contoso.Test.Business.Responses.TypeNotFound",
                ListType.GenericList,
                ListVariableInputStyle.ListForm
            );
            //act
            xmlValidator.Validate
            (
                xmlElement,
                variable,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, variable.ObjectType),
                errors.First()
            );
        }

        [Fact]
        public void ObjectListVariableElementValidatorThrowsForInvalidRootElement()
        {
            //arrange
            IObjectListVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<variable>
                                                        <objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""eee"">
                                                        </objectList>
                                                    </variable>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            ListOfObjectsVariable variable = (ListOfObjectsVariable)_fixture.ConfigurationService.VariableList.Variables["ObjectListVariable"];

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                xmlValidator.Validate
                (
                    xmlElement,
                    variable,
                    applicationTypeInfo,
                    errors
                )
            );

            //assert 
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{18D38FF4-CE3D-49A2-A8D7-19655D7E011F}"), 
                exception.Message
            );
        }

        private static XmlElement GetXmlElement(string xmlString)
            => GetXmlDocument(xmlString).DocumentElement!;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class ObjectListVariableElementValidatorFixture : IDisposable
    {
        public ObjectListVariableElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            TypeHelper = ServiceProvider.GetRequiredService<ITypeHelper>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
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
                    )
                },
                new HashSet<string>()
            );

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = ConstructorFactory.GetConstructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "stringProperty",
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
                        new List<string>(),
                        ""
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
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
                    ["ObjectListVariable"] = VariableFactory.GetListOfObjectsVariable
                    (
                        "ObjectListVariable",
                        "ObjectListVariable",
                        VariableCategory.StringKeyIndexer,
                        "System.Collections.Generic.IList`1[[Contoso.Test.Business.Responses.TestResponseA, Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        ListType.GenericList,
                        ListVariableInputStyle.ListForm
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );


            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

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
        }

        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
