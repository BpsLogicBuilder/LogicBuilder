using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        [Theory]
        [InlineData(ThemeCollections.Dark, 25, ThemeCollections.ControlDefault)]
        [InlineData(ThemeCollections.Dark, 9, ThemeCollections.Office2019Dark)]
        [InlineData(ThemeCollections.Dark, 10, ThemeCollections.Office2019Dark10)]
        [InlineData(ThemeCollections.Dark, 11, ThemeCollections.Office2019Dark11)]
        [InlineData(ThemeCollections.Dark, 12, ThemeCollections.Office2019Dark12)]
        [InlineData(ThemeCollections.Dark, 13, ThemeCollections.Office2019Dark13)]
        [InlineData(ThemeCollections.Dark, 14, ThemeCollections.Office2019Dark14)]
        [InlineData(ThemeCollections.Gray, 25, ThemeCollections.ControlDefault)]
        [InlineData(ThemeCollections.Gray, 9, ThemeCollections.Office2019Gray)]
        [InlineData(ThemeCollections.Gray, 10, ThemeCollections.Office2019Gray10)]
        [InlineData(ThemeCollections.Gray, 11, ThemeCollections.Office2019Gray11)]
        [InlineData(ThemeCollections.Gray, 12, ThemeCollections.Office2019Gray12)]
        [InlineData(ThemeCollections.Gray, 13, ThemeCollections.Office2019Gray13)]
        [InlineData(ThemeCollections.Gray, 14, ThemeCollections.Office2019Gray14)]
        [InlineData(ThemeCollections.Light, 25, ThemeCollections.ControlDefault)]
        [InlineData(ThemeCollections.Light, 9, ThemeCollections.Office2019Light)]
        [InlineData(ThemeCollections.Light, 10, ThemeCollections.Office2019Light10)]
        [InlineData(ThemeCollections.Light, 11, ThemeCollections.Office2019Light11)]
        [InlineData(ThemeCollections.Light, 12, ThemeCollections.Office2019Light12)]
        [InlineData(ThemeCollections.Light, 13, ThemeCollections.Office2019Light13)]
        [InlineData(ThemeCollections.Light, 14, ThemeCollections.Office2019Light14)]
        public void CanSetColorTheme(string colorTheme, int fontSetting, string expectedTheme)
        {
            //arrange
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();
            Settings.Default.fontSize = fontSetting;
            Settings.Default.Save();

            //act
            manager.SetColorTheme(colorTheme);

            //assert
            Assert.Equal(expectedTheme, ThemeResolutionService.ApplicationThemeName);
        }

        [Theory]
        [InlineData("Blue", 9, ThemeCollections.ControlDefault)]
        [InlineData(ThemeCollections.Dark, 9, ThemeCollections.Office2019Dark)]
        [InlineData(ThemeCollections.Dark, 10, ThemeCollections.Office2019Dark10)]
        [InlineData(ThemeCollections.Dark, 11, ThemeCollections.Office2019Dark11)]
        [InlineData(ThemeCollections.Dark, 12, ThemeCollections.Office2019Dark12)]
        [InlineData(ThemeCollections.Dark, 13, ThemeCollections.Office2019Dark13)]
        [InlineData(ThemeCollections.Dark, 14, ThemeCollections.Office2019Dark14)]
        [InlineData("Yellow", 9, ThemeCollections.ControlDefault)]
        [InlineData(ThemeCollections.Gray, 9, ThemeCollections.Office2019Gray)]
        [InlineData(ThemeCollections.Gray, 10, ThemeCollections.Office2019Gray10)]
        [InlineData(ThemeCollections.Gray, 11, ThemeCollections.Office2019Gray11)]
        [InlineData(ThemeCollections.Gray, 12, ThemeCollections.Office2019Gray12)]
        [InlineData(ThemeCollections.Gray, 13, ThemeCollections.Office2019Gray13)]
        [InlineData(ThemeCollections.Gray, 14, ThemeCollections.Office2019Gray14)]
        [InlineData("Red", 9, ThemeCollections.ControlDefault)]
        [InlineData(ThemeCollections.Light, 9, ThemeCollections.Office2019Light)]
        [InlineData(ThemeCollections.Light, 10, ThemeCollections.Office2019Light10)]
        [InlineData(ThemeCollections.Light, 11, ThemeCollections.Office2019Light11)]
        [InlineData(ThemeCollections.Light, 12, ThemeCollections.Office2019Light12)]
        [InlineData(ThemeCollections.Light, 13, ThemeCollections.Office2019Light13)]
        [InlineData(ThemeCollections.Light, 14, ThemeCollections.Office2019Light14)]
        public void CanSetFontSize(string colorThemeSetting, int fontSize, string expectedTheme)
        {
            //arrange
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();
            Settings.Default.colorTheme = colorThemeSetting;
            Settings.Default.Save();

            //act
            manager.SetFontSize(fontSize);

            //assert
            Assert.Equal(expectedTheme, ThemeResolutionService.ApplicationThemeName);
        }

        [Fact]
        public void CanCheckmenuItemsForCurrentSettings()
        {
            //arrange
            string colorThemeSetting = ThemeCollections.Light;
            int fontSizeSetting = 9;
            IThemeManager manager = serviceProvider.GetRequiredService<IThemeManager>();
            Settings.Default.colorTheme = colorThemeSetting;
            Settings.Default.fontSize = fontSizeSetting;
            Settings.Default.Save();
            RadMenuItem radMenuItemColorTheme = new();
            radMenuItemColorTheme.Items.AddRange
            (
                new RadItem[] 
                { 
                    new RadMenuItem() { Tag = ThemeCollections.Light},
                    new RadMenuItem() { Tag = ThemeCollections.Dark}
                }
            );
            RadMenuItem radMenuItemFontSize = new();
            radMenuItemFontSize.Items.AddRange
            (
                new RadItem[]
                {
                    new RadMenuItem() { Tag = "10"},
                    new RadMenuItem() { Tag = "9"}
                }
            );
            manager.SetFontSize(fontSizeSetting);

            //act
            manager.CheckMenuItemsForCurrentSettings(radMenuItemColorTheme.Items, radMenuItemFontSize.Items);

            //assert
            Assert.True(((RadMenuItem)radMenuItemColorTheme.Items[0]).IsChecked);
            Assert.False(((RadMenuItem)radMenuItemColorTheme.Items[1]).IsChecked);
            Assert.False(((RadMenuItem)radMenuItemFontSize.Items[0]).IsChecked);
            Assert.True(((RadMenuItem)radMenuItemFontSize.Items[1]).IsChecked);
        }
    }
}
