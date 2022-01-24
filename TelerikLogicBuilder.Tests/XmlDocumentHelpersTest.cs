using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            serviceProvider = new ServiceCollection()
                .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
                .BuildServiceProvider();
        }
    }
}
