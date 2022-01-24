using ABIS.LogicBuilder.FlowBuilder.Enums;
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
    public class EnumHelperTest
    {
        public EnumHelperTest()
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
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var visibleText = enumHelper.GetVisibleEnumText(ParameterType.String);

            //assert
            Assert.Equal("String", visibleText);
        }

        [Fact]
        public void GetVisibleEnumTextReturnsCorrectStringForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var visibleText = enumHelper.GetVisibleEnumText(ParameterType.String);

            //assert
            Assert.Equal("StringFR", visibleText);
        }

        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IEnumHelper, EnumHelper>()
                .BuildServiceProvider();
        }
    }
}
