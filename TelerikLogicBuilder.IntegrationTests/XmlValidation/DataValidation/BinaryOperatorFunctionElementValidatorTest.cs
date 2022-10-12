﻿using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class BinaryOperatorFunctionElementValidatorTest : IClassFixture<BinaryOperatorFunctionElementValidatorFixture>
    {
        private readonly BinaryOperatorFunctionElementValidatorFixture _fixture;

        public BinaryOperatorFunctionElementValidatorTest(BinaryOperatorFunctionElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void CanCreateBinaryOperatorFunctionElementValidator()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ValidateSucceedsForBothParametersAny()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateSucceedsForBothParametersNotAny()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddStrings"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateThrowsForWrongFunctionCategory()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddWrongFunctionCategory"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();


            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(function, parameterElementsList, application, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{C97A6D3B-C877-439A-AE7C-3520EDF071E0}"),
                exception.Message
            );
        }

        [Fact]
        public void ValidateFailsForInvalidCodeBinaryOperatorName()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["UndefinedCodeBinaryOperatorType"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.binaryOperatorCodeNameInvalidFormat,
                    function.MemberName,
                    function.Name,
                    string.Join
                    (
                        Strings.itemsCommaSeparator,
                        _fixture.ServiceProvider.GetRequiredService<IEnumHelper>().ConvertEnumListToStringList
                        (
                            new CodeBinaryOperatorType[] { CodeBinaryOperatorType.Assign }
                        ).Select(i => string.Concat("\"", i, "\""))
                    )
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsForAssign()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["Assign"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.binaryOperatorCodeNameInvalidFormat,
                    function.MemberName,
                    function.Name,
                    string.Join
                    (
                        Strings.itemsCommaSeparator,
                        _fixture.ServiceProvider.GetRequiredService<IEnumHelper>().ConvertEnumListToStringList
                        (
                            new CodeBinaryOperatorType[] { CodeBinaryOperatorType.Assign }
                        ).Select(i => string.Concat("\"", i, "\""))
                    )
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsForConfiguredParameterCountNotEqualToTwo()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddOneParameter"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.parameterCountMustBeTwoFormat,
                    function.Name
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsForDataParameterCountNotEqualToTwo()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.parameterCountMustBeTwoFormat,
                    function.Name
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsForConfiguredParameterCountEqualToTwoAndOnlyOneParameterIsAny()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAnyOneParameterAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.invalidAnyParameterConfigurationForBinaryOperatorFormat,
                    function.Name,
                    "value1",
                    "value2"
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateSThrowsForInvalidChildElement()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1""><invalidElement /></literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2"">AAA</literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //assert
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(function, parameterElementsList, application, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{2CC0FFDD-C0D8-44BC-ABD7-9C2CA95B744C}"),
                exception.Message
            );
        }

        [Fact]
        public void ValidateFailsIfOneParameterTypeResolvesToNull()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1""><variable name=""System_Object"" visibleText=""visibleText"" /></literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""value2""></literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                Strings.neitherOperandCanBeEmptyBinaryOperationAny,
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsIfTheFirstParameterIsNotALiteralType()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement(@"<literalParameter name=""value1""><variable name=""System_Object"" visibleText=""visibleText"" /></literalParameter>") ,
                GetXmlElement($@"<literalParameter name=""value2""><variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Byte)}Item"" visibleText=""visibleText"" /></literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.parameterNotLiteralFormat,
                    "value1",
                    typeof(object).ToString()
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsIfTheSecondParameterIsNotALiteralType()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement($@"<literalParameter name=""value1""><variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Byte)}Item"" visibleText=""visibleText"" /></literalParameter>") ,
                GetXmlElement($@"<literalParameter name=""value2""><variable name=""System_Object"" visibleText=""visibleText"" /></literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.parameterNotLiteralFormat,
                    "value2",
                    typeof(object).ToString()
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsIfParametersAreNotCompatibleForTheOperation()
        {
            //arrange
            IBinaryOperatorFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IBinaryOperatorFunctionElementValidator>();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["AddAny"];
            IList<XmlElement> parameterElementsList = new XmlElement[]
            {
                GetXmlElement($@"<literalParameter name=""value1""><variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Byte)}Item"" visibleText=""visibleText"" /></literalParameter>") ,
                GetXmlElement($@"<literalParameter name=""value2""><variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.DateOnly)}Item"" visibleText=""visibleText"" /></literalParameter>")
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(function, parameterElementsList, application, errors);

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format
                (
                    Strings.parametersNotCompatibleForBinaryOperation,
                    typeof(byte).ToString(),
                    typeof(DateOnly).ToString(),
                    Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Add)
                ),
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

    public class BinaryOperatorFunctionElementValidatorFixture : IDisposable
    {
        public BinaryOperatorFunctionElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
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
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["UndefinedCodeBinaryOperatorType"] = new Function
                    (
                        "UndefinedCodeBinaryOperatorType",
                        "UndefinedCodeBinaryOperatorType",
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Byte),
                        "",
                        ContextProvider
                    ),
                    ["Assign"] = new Function
                    (
                        "Assign",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Assign)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Byte),
                        "",
                        ContextProvider
                    ),
                    ["AddOneParameter"] = new Function
                    (
                        "AddOneParameter",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Add)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Byte),
                        "",
                        ContextProvider
                    ),
                    ["AddStrings"] = new Function
                    (
                        "AddStrings",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Add)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.String),
                        "",
                        ContextProvider
                    ),
                    ["AddAny"] = new Function
                    (
                        "AddAny",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Add)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.SByte),
                        "",
                        ContextProvider
                    ),
                    ["AddAnyOneParameterAny"] = new Function
                    (
                        "AddAnyOneParameterAny",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Add)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.SByte),
                        "",
                        ContextProvider
                    ),
                    ["AddWrongFunctionCategory"] = new Function
                    (
                        "AddAny",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.Add)!,
                        FunctionCategories.Standard,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.SByte),
                        "",
                        ContextProvider
                    ),
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
                    ["System_Object"] = new ObjectVariable
                    (
                        "System_Object",
                        "System_Object",
                        VariableCategory.StringKeyIndexer,
                        "System.Object",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "System.Object",
                        ContextProvider
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
            => new
            (
                name,
                name,
                VariableCategory.StringKeyIndexer,
                ContextProvider.TypeHelper.ToId(ContextProvider.EnumHelper.GetSystemType(literalVariableType)),
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
                new List<string>(),
                ContextProvider
            );

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
