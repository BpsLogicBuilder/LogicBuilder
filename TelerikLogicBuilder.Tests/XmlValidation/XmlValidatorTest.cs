using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlValidation
{
    public class XmlValidatorTest
    {
        public XmlValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateXmlValidator()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void CanValidateInvalidConstructorData()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidator>();

            //act
            var result = xmlValidator.Validate(SchemaName.ParametersDataSchema, @"<constructor name=""TestResponseC"" visibleText=""TestResponseC"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <objectParameter name=""objectProperty""></objectParameter>
                                                        </parameters>
                                                    </constructor>");

            //assert
            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
        }

        [Fact]
        public void CanValidateValidConstructorData()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidator>();

            //act
            var result = xmlValidator.Validate(SchemaName.ParametersDataSchema, @"<constructor name=""TestResponseC"" visibleText=""TestResponseC"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <objectParameter name=""objectProperty"">
                                                                <variable name=""sameVariable"" visibleText =""visibleText"" />
                                                            </objectParameter>
                                                        </parameters>
                                                    </constructor>");

            //assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }
    }
}
