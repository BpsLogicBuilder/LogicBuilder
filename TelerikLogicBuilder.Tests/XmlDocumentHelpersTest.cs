using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class XmlDocumentHelpersTest
    {
        public XmlDocumentHelpersTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetVisibleEnumTextReturnsCorrectStringForDefaultCulture()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var writer = helper.CreateUnformattedXmlWriter(new StringBuilder());

            //assert
            Assert.False(writer.Settings.Indent);
            Assert.True(writer.Settings.OmitXmlDeclaration);
        }

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
