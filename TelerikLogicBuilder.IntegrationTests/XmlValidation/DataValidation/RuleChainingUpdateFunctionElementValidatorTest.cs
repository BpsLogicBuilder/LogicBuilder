using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class RuleChainingUpdateFunctionElementValidatorTest
    {
        public RuleChainingUpdateFunctionElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateRuleChainingUpdateFunctionElementValidator()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ValidateThrowsForWrongFunctionCategory()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            IList<XmlElement> parameterElementsList = new XmlElement[] { GetXmlElement(@"<literalParameter name=""Variable"">AAA</literalParameter>") };
            Function function = new
            (
                "ChainingUpdate",
                "Update",
                FunctionCategories.Standard,
                "",
                "",
                "",
                "",
                ReferenceCategories.None,
                ParametersLayout.Sequential,
                new List<ParameterBase>
                {
                    parameterFactory.GetLiteralParameter
                    (
                        "Variable",
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
                        new List<string>()
                    )
                },
                new List<string>(),
                returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                "",
                contextProvider
            );
            List<string> errors = new();


            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(function, parameterElementsList, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{71221267-4D2F-4E9F-809F-B46734714729}"),
                exception.Message
            );
        }

        [Fact]
        public void ValidateThrowsForConfiguredParameterCountNotEqualToOne()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            IList<XmlElement> parameterElementsList = new XmlElement[] { GetXmlElement(@"<literalParameter name=""Variable"">AAA</literalParameter>") };
            Function function = new
            (
                "ChainingUpdate",
                "Update",
                FunctionCategories.RuleChainingUpdate,
                "",
                "",
                "",
                "",
                ReferenceCategories.None,
                ParametersLayout.Sequential,
                new List<ParameterBase>
                {
                    parameterFactory.GetLiteralParameter
                    (
                        "Variable",
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
                        new List<string>()
                    ),
                    parameterFactory.GetLiteralParameter
                    (
                        "stringProperty2",
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
                        new List<string>()
                    )
                },
                new List<string>(),
                returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                "",
                contextProvider
            );
            List<string> errors = new();


            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(function, parameterElementsList, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{A44A1D22-84AC-4382-8E9A-3A5A7CA75444}"),
                exception.Message
            );
        }

        [Fact]
        public void ValidateThrowsForDataParameterCountNotEqualToOne()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            IList<XmlElement> parameterElementsList = new XmlElement[] 
            { 
                GetXmlElement(@"<literalParameter name=""Variable"">AAA</literalParameter>") ,
                GetXmlElement(@"<literalParameter name=""literalProperty"">AAA</literalParameter>")
            };
            Function function = new
            (
                "ChainingUpdate",
                "Update",
                FunctionCategories.RuleChainingUpdate,
                "",
                "",
                "",
                "",
                ReferenceCategories.None,
                ParametersLayout.Sequential,
                new List<ParameterBase>
                {
                    parameterFactory.GetLiteralParameter
                    (
                        "Variable",
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
                        new List<string>()
                    )
                },
                new List<string>(),
                returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                "",
                contextProvider
            );
            List<string> errors = new();


            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(function, parameterElementsList, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{EFC886E0-860C-4573-B98A-B680C1168170}"),
                exception.Message
            );
        }

        [Fact]
        public void ValidateFailsForSingleDataParameterWithNoChildNodes()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            IList<XmlElement> parameterElementsList = new XmlElement[] { GetXmlElement(@"<literalParameter name=""Variable""></literalParameter>") };
            Function function = new
            (
                "ChainingUpdate",
                "Update",
                FunctionCategories.RuleChainingUpdate,
                "",
                "",
                "",
                "",
                ReferenceCategories.None,
                ParametersLayout.Sequential,
                new List<ParameterBase>
                {
                    parameterFactory.GetLiteralParameter
                    (
                        "Variable",
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
                        new List<string>()
                    )
                },
                new List<string>(),
                returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                "",
                contextProvider
            );
            List<string> errors = new();


            //act
            xmlValidator.Validate(function, parameterElementsList, errors);
            Assert.Equal
            (
                Strings.chainingUpdateValidationError,
                errors.First()
            );
        }

        [Fact]
        public void ValidateFailsForSingleDataParameterWithMultipleChildNodes()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            IList<XmlElement> parameterElementsList = new XmlElement[] { GetXmlElement(@"<literalParameter name=""Variable"">ZZZ<variable name=""IntegerItem"" visibleText=""visibleText"" /></literalParameter>") };
            Function function = new
            (
                "ChainingUpdate",
                "Update",
                FunctionCategories.RuleChainingUpdate,
                "",
                "",
                "",
                "",
                ReferenceCategories.None,
                ParametersLayout.Sequential,
                new List<ParameterBase>
                {
                    parameterFactory.GetLiteralParameter
                    (
                        "Variable",
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
                        new List<string>()
                    )
                },
                new List<string>(),
                returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                "",
                contextProvider
            );
            List<string> errors = new();


            //act
            xmlValidator.Validate(function, parameterElementsList, errors);
            Assert.Equal
            (
                Strings.chainingUpdateValidationError,
                errors.First()
            );
        }

        [Fact]
        public void ValidateSucceedsForSingleDataParameterWithSingleTextChildNode()
        {
            //arrange
            IRuleChainingUpdateFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            IList<XmlElement> parameterElementsList = new XmlElement[] { GetXmlElement(@"<literalParameter name=""Variable"">ZZZ</literalParameter>") };
            Function function = new
            (
                "ChainingUpdate",
                "Update",
                FunctionCategories.RuleChainingUpdate,
                "",
                "",
                "",
                "",
                ReferenceCategories.None,
                ParametersLayout.Sequential,
                new List<ParameterBase>
                {
                    parameterFactory.GetLiteralParameter
                    (
                        "Variable",
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
                        new List<string>()
                    )
                },
                new List<string>(),
                returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                "",
                contextProvider
            );
            List<string> errors = new();


            //act
            xmlValidator.Validate(function, parameterElementsList, errors);
            Assert.Empty(errors);
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
}
