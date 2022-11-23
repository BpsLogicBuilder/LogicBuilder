using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
    public class ObjectListParameterElementValidatorTest : IClassFixture<ObjectListParameterElementValidatorFixture>
    {
        private readonly ObjectListParameterElementValidatorFixture _fixture;

        public ObjectListParameterElementValidatorTest(ObjectListParameterElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanObjectListParameterElementValidator()
        {
            //arrange
            IObjectListParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ObjectListParameterElementValidatorWorksForValidData()
        {
            //arrange
            IObjectListParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListParameterElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<objectListParameter name=""response"">
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
                                                    </objectListParameter>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            ListOfObjectsParameter parameter = _fixture.ParameterFactory.GetListOfObjectsParameter
            (
                "list",
                false,
                "",
                "Contoso.Test.Business.Responses.TestResponseA",
                ListType.GenericList,
                ListParameterInputStyle.ListForm
            );

            //act
            xmlValidator.Validate
            (
                xmlElement,
                parameter,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ObjectListParameterElementValidatorThrowsForInvalidElementType()
        {
            //arrange
            IObjectListParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListParameterElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literalListParameter name=""Includes"">
                                                        <literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                        </literalList>
                                                      </literalListParameter>");
            List<string> errors = new();
            ListOfObjectsParameter parameter = _fixture.ParameterFactory.GetListOfObjectsParameter
            (
                "list",
                false,
                "",
                "Contoso.Test.Business.Responses.TestResponseA",
                ListType.GenericList,
                ListParameterInputStyle.ListForm
            );

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, parameter, applicationTypeInfo, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{B3303DFB-4BB3-4836-B900-ACA2666D5706}"),
                exception.Message
            );
        }

        [Fact]
        public void ObjectListParameterElementValidatorFailsIfObjectTypeCannotBeLoaded()
        {
            //arrange
            IObjectListParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectListParameterElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<objectListParameter name=""response"">
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
                                                    </objectListParameter>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            ListOfObjectsParameter parameter = _fixture.ParameterFactory.GetListOfObjectsParameter
            (
                "list",
                false,
                "",
                "Contoso.Test.Business.Responses.TypeNotFound",
                ListType.GenericList,
                ListParameterInputStyle.ListForm
            );

            //act
            xmlValidator.Validate
            (
                xmlElement,
                parameter,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, parameter.ObjectType),
                errors.First()
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

    public class ObjectListParameterElementValidatorFixture : IDisposable
    {
        public ObjectListParameterElementValidatorFixture()
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
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
