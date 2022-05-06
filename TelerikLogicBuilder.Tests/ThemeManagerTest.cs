using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ThemeManagerTest
    {
        public ThemeManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateThemeManager()
        {
            //arrange
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();

            //assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void CanSetNewTheme()
        {
            //arrange
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();

            //act
            manager.SetTheme(ThemeCollections.Office2010Blue);

            //assert
            Assert.Equal(ThemeCollections.Office2010Blue, Settings.Default.themeName);
            Assert.Equal(ThemeCollections.Office2010Blue, ThemeResolutionService.ApplicationThemeName);
        }

        [Fact]
        public void CanCheckMenuItemToMatchCurrentTheme()
        {
            //arrange
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();
            RadMenuItem radMenuItemParent = new();
            RadMenuItem office2010BlueItem = new() { Tag = ThemeCollections.Office2010Blue };
            RadMenuItem office2010BlackItem = new() { Tag = ThemeCollections.Office2010Black };
            RadMenuItem otherItem = new() {  };
            radMenuItemParent.Items.AddRange(new RadItem[] { office2010BlueItem, office2010BlackItem, otherItem });

            //act
            manager.SetTheme(ThemeCollections.Office2010Blue);
            manager.CheckMenuItemForCurrentTheme(radMenuItemParent.Items);

            //assert
            Assert.True(office2010BlueItem.IsChecked);
            Assert.False(office2010BlackItem.IsChecked);
            Assert.False(otherItem.IsChecked);
        }

        [Fact]
        public void SetThemeThrowsForInvalidThemName()
        {
            //arrange
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => manager.SetTheme("xyz"));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{22FDC293-A727-4845-AF68-A3275B239D4E}"),
                exception.Message
            );
        }
    }
}
