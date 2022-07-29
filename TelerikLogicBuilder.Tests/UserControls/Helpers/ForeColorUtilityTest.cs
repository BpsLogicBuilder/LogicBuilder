using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Xunit;

namespace TelerikLogicBuilder.Tests.UserControls.Helpers
{
    public class ForeColorUtilityTest
    {
        public ForeColorUtilityTest()
        {
            ABIS.LogicBuilder.FlowBuilder.Program.LoadThemes();
        }

        public static List<object[]> OkForeColors_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { ThemeCollections.ControlDefault, Color.FromArgb(255, 21, 66, 139) },
                    new object[] { ThemeCollections.Office2007Black, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2007Silver, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2010Black, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2010Blue, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2010Silver, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2013Dark, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2013Light, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2019Dark, Color.FromArgb(255, 241, 241, 241) },
                    new object[] { ThemeCollections.Office2019Gray, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2019Light, Color.FromArgb(255, 0, 0, 0) }
                };
            }
        }

        public static List<object[]> ErrorForeColors_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { ThemeCollections.ControlDefault, Color.FromArgb(255, 0, 0, 0) },
                    new object[] { ThemeCollections.Office2007Black, Color.FromArgb(255, 0, 50, 208) },
                    new object[] { ThemeCollections.Office2007Silver, Color.FromArgb(255, 0, 50, 208) },
                    new object[] { ThemeCollections.Office2010Black, Color.FromArgb(255, 255, 255, 255) },
                    new object[] { ThemeCollections.Office2010Blue, Color.FromArgb(255, 255, 0, 0) },/*RedBorder*/
                    new object[] { ThemeCollections.Office2010Silver, Color.FromArgb(255, 255, 0, 0) },/*RedBorder*/
                    //new object[] { ThemeCollections.Office2010Blue, Color.FromArgb(255, 216, 80, 0) },/*ForeColorOrange*/
                    //new object[] { ThemeCollections.Office2010Silver, Color.FromArgb(255, 214, 121, 3) },/*ForeColor(214;121;3)*/
                    new object[] { ThemeCollections.Office2013Dark, Color.FromArgb(255, 0, 114, 198) },
                    new object[] { ThemeCollections.Office2013Light, Color.FromArgb(255, 0, 114, 198) },
                    new object[] { ThemeCollections.Office2019Dark, Color.FromArgb(255, 47, 150, 237) },
                    new object[] { ThemeCollections.Office2019Gray, Color.FromArgb(255, 16, 110, 190) },
                    new object[] { ThemeCollections.Office2019Light, Color.FromArgb(255, 16, 110, 190) }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ErrorForeColors_Data))]
        public void GetErrorForeColorReturnsExpectedColors(string themeName, Color expectedColor)
        {
            //act
            Color result = ForeColorUtility.GetErrorForeColor(themeName);

            //assert
            Assert.Equal(expectedColor.A, result.A);
            Assert.Equal(expectedColor.R, result.R);
            Assert.Equal(expectedColor.G, result.G);
            Assert.Equal(expectedColor.B, result.B);
        }

        [Fact]
        public void GetErrorForeColorThrowsForInvalidThemeName()
        {
            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => ForeColorUtility.GetErrorForeColor("xyz"));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{E49A3619-879C-4DCF-9575-E77F97F31E0E}"),
                exception.Message
            );
        }

        [Theory]
        [MemberData(nameof(OkForeColors_Data))]
        public void GetOkForeColorReturnsExpectedColors(string themeName, Color expectedColor)
        {
            Color result = ForeColorUtility.GetOkForeColor(themeName);

            //assert
            Assert.Equal(expectedColor.A, result.A);
            Assert.Equal(expectedColor.R, result.R);
            Assert.Equal(expectedColor.G, result.G);
            Assert.Equal(expectedColor.B, result.B);
        }

        [Fact]
        public void GetOkForeColorThrowsForInvalidThemeName()
        {
            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => ForeColorUtility.GetOkForeColor("xyz"));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{53746E6D-A654-4818-A81D-303295E6346C}"),
                exception.Message
            );
        }
    }
}
