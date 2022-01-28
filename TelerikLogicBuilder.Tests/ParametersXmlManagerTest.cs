using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ParametersXmlManagerTest
    {
        public ParametersXmlManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.IntegrationTest)]
        public void GetParametersFromXml()
        {
            //arrange
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(@"<literalParameter name=""Refresh"">
					<literalType>Boolean</literalType>
					<control>SingleLineTextBox</control>
					<optional>false</optional>
					<useForEquality>true</useForEquality>
					<useForHashCode>true</useForHashCode>
					<useForToString>true</useForToString>
					<propertySource />
					<propertySourceParameter />
					<defaultValue>true</defaultValue>
					<domain>
						<item>true</item>
						<item>false</item>
					</domain>
					<comments></comments>
				</literalParameter>");

            //act
            ParameterBase result = contextProvider.ParametersXmlManager.BuildParameter(xmlDocument.DocumentElement);

            //assert
            Assert.NotNull(result);
            Assert.True(result is LiteralParameter);
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
    }
}
