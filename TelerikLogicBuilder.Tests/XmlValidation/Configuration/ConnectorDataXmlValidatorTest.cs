﻿using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlValidation.Configuration
{
    public class ConnectorDataXmlValidatorTest
    {
        public ConnectorDataXmlValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ValidateValidDialogXmlWorks()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidatorFactory>().GetXmlValidator(SchemaName.ConnectorDataSchema);

            //act
            var result = xmlValidator.Validate(@"<connector name=""1"" connectorCategory=""1"">
                                                  <text>
                                                    FFF
                                                    <function name=""get message"" visibleText=""visibleText"">
                                                      <genericArguments />
                                                      <parameters>
                                                        <literalParameter name=""value"">
                                                          <function name=""table"" visibleText=""visibleText"">
                                                            <genericArguments />
                                                            <parameters>
                                                              <literalParameter name=""value"">tmq</literalParameter>
                                                              <literalParameter name=""key"">
                                                                <variable name=""tmqkey"" visibleText=""visibleText"" />
                                                              </literalParameter>
                                                              <literalParameter name=""field"">MSGID</literalParameter>
                                                            </parameters>
                                                          </function>
                                                        </literalParameter>
                                                      </parameters>
                                                    </function>
                                                  </text>
                                                  <metaObject objectType=""Fully.Qualified.Type.Name"">
                                                    <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                                      <genericArguments />
                                                      <parameters>
                                                        <literalParameter name=""val1"">
                                                          <variable name=""ZBU"" visibleText=""visibleText"" />
                                                        </literalParameter>
                                                        <literalParameter name=""val2"">CS</literalParameter>
                                                      </parameters>
                                                    </constructor>
                                                  </metaObject>
                                                </connector>");

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidateValidDecisionXmlWorks()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidatorFactory>().GetXmlValidator(SchemaName.ConnectorDataSchema);

            //act
            var result = xmlValidator.Validate(@"<connector name=""1"" connectorCategory=""0"">
                                                  <text>
                                                    FFF
                                                    <function name=""get message"" visibleText=""visibleText"">
                                                      <genericArguments />
                                                      <parameters>
                                                        <literalParameter name=""value"">
                                                          <function name=""table"" visibleText=""visibleText"">
                                                            <genericArguments />
                                                            <parameters>
                                                              <literalParameter name=""value"">tmq</literalParameter>
                                                              <literalParameter name=""key"">
                                                                <variable name=""tmqkey"" visibleText=""visibleText"" />
                                                              </literalParameter>
                                                              <literalParameter name=""field"">MSGID</literalParameter>
                                                            </parameters>
                                                          </function>
                                                        </literalParameter>
                                                      </parameters>
                                                    </function>
                                                  </text>
                                                </connector>");

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidateReturnsFailureResponseForInvalidStructure()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidatorFactory>().GetXmlValidator(SchemaName.ConnectorDataSchema);

            //act
            var result = xmlValidator.Validate(@"<folder name=""variables"">
                                                    <undefinedVariable>
		                                            </undefinedVariable>
                                                </folder>");
            Assert.False(result.Success);
        }

        [Fact]
        public void ValidateThrowsXmlExceptionForInvalidXml()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidatorFactory>().GetXmlValidator(SchemaName.ConnectorDataSchema);

            //act
            Assert.Throws<XmlException>(() => xmlValidator.Validate(@"<folder1 name=""variables"">
                                                                    </folder>"));
        }

        [Fact]
        public void XmlWithInvalidCategoryReturnsFailureResponse()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidatorFactory>().GetXmlValidator(SchemaName.ConnectorDataSchema);

            //act
            var result = xmlValidator.Validate(@"<connector name=""1"" connectorCategory=""2"">
                                                  <text>
                                                    FFF
                                                  </text>
                                                </connector>");

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.connectorCategoryUndefinedFormat,
                    "2"
                ),
                result.Errors.First()
            );
        }

        [Fact]
        public void XmlWithDialodCategoryAndWithoutMetaObjectElementReturnsFailureResponse()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidatorFactory>().GetXmlValidator(SchemaName.ConnectorDataSchema);

            //act
            var result = xmlValidator.Validate(@"<connector name=""1"" connectorCategory=""1"">
                                                  <text>
                                                    FFF
                                                  </text>
                                                </connector>");

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.metaObjectRequiredForDialogConnectorsFormat,
                    XmlDataConstants.METAOBJECTELEMENT
                ),
                result.Errors.First()
            );
        }
    }
}
