using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ConstructorXmlParserTest
    {
        public ConstructorXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetConstructorFromXml()
        {
            //arrange
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
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
            Constructor result = contextProvider.ConstructorXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("OperatorGroup", result.Name);
            Assert.Equal(2, result.Parameters.Count);
            Assert.True(result.Parameters[0] is LiteralParameter);
            Assert.True(result.Parameters[1] is ListOfObjectsParameter);
        }

        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IEnumHelper, EnumHelper>()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IMemberAttributeReader, MemberAttributeReader>()
                .AddSingleton<IParameterAttributeReader, ParameterAttributeReader>()
                .AddSingleton<IStringHelper, StringHelper>()
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
                .AddSingleton<IReflectionHelper, ReflectionHelper>()
                .AddSingleton<ITypeHelper, TypeHelper>()
                .AddSingleton<IContextProvider, ContextProvider>()
                .AddSingleton<IChildConstructorFinder, ChildConstructorFinder>()
                .BuildServiceProvider();
        }

        private static XmlElement GetXmlElement(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument.DocumentElement;
        }
    }
}
