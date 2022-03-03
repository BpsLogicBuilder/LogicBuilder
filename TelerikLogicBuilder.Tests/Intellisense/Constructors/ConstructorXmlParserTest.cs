using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Constructors
{
    public class ConstructorXmlParserTest
    {
        public ConstructorXmlParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void GetConstructorFromXml()
        {
            //arrange
            IConstructorXmlParser constructorXmlParser = serviceProvider.GetRequiredService<IConstructorXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""OperatorGroup"">
			<typeName>LogicBuilder.Forms.Parameters.Grid.OperatorGroup</typeName>
			<parameters>
				<literalParameter name=""typeName"">
					<literalType>String</literalType>
					<control>SingleLineTextBox</control>
					<optional>false</optional>
					<useForEquality>true</useForEquality>
					<useForHashCode>true</useForHashCode>
					<useForToString>true</useForToString>
					<propertySource />
					<propertySourceParameter />
					<defaultValue>string</defaultValue>
					<domain>
						<item>string</item>
						<item>number</item>
					</domain>
					<comments></comments>
				</literalParameter>
				<objectListParameter name=""Operators"">
                    <objectType>Operator</objectType>
					<listType>GenericList</listType>
					<control>HashSetForm</control>
					<optional>false</optional>
					<comments></comments>
				</objectListParameter>
			</parameters>
			<genericArguments />
			<summary></summary>
		</constructor>");

            //act
            Constructor result = constructorXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("OperatorGroup", result.Name);
            Assert.Equal(2, result.Parameters.Count);
            Assert.True(result.Parameters[0] is LiteralParameter);
            Assert.True(result.Parameters[1] is ListOfObjectsParameter);
        }

        private static XmlElement GetXmlElement(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument.DocumentElement!;
        }
    }
}
