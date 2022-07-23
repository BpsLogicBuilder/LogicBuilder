using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.UserControls.Helpers
{
    public class RadDropDownListHelperTest
    {
        public RadDropDownListHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateRadDropDownListHelper()
        {
            //arrange
            IRadDropDownListHelper helper = serviceProvider.GetRequiredService<IRadDropDownListHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void CanLoadBooleansToRadDropDownList()
        {
            //arrange
            IRadDropDownListHelper helper = serviceProvider.GetRequiredService<IRadDropDownListHelper>();
            RadDropDownList radDropDownList = new();

            //act
            helper.LoadBooleans(radDropDownList);

            //assert
            Assert.True((bool)radDropDownList.Items[0].Value);
            Assert.False((bool)radDropDownList.Items[1].Value);
            Assert.Equal(true.ToString(CultureInfo.CurrentCulture).ToLowerInvariant(), radDropDownList.Items[0].Text);
            Assert.Equal(false.ToString(CultureInfo.CurrentCulture).ToLowerInvariant(), radDropDownList.Items[1].Text);
        }

        [Fact]
        public void CanLoadEnumValuesToRadDropDownList()
        {
            //arrange
            IRadDropDownListHelper helper = serviceProvider.GetRequiredService<IRadDropDownListHelper>();
            RadDropDownList radDropDownList = new();

            //act
            helper.LoadComboItems<RuleChainingBehavior>(radDropDownList);

            //assert
            Assert.Equal(RuleChainingBehavior.None, radDropDownList.Items[0].Value);
            Assert.Equal(RuleChainingBehavior.UpdateOnly, radDropDownList.Items[1].Value);
            Assert.Equal(RuleChainingBehavior.Full, radDropDownList.Items[2].Value);
            Assert.Equal("None", radDropDownList.Items[0].Text);
            Assert.Equal("Update Only", radDropDownList.Items[1].Text);
            Assert.Equal("Full", radDropDownList.Items[2].Text);
        }

        [Fact]
        public void CanLoadEnumValuesToRadDropDownListusingSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            IRadDropDownListHelper helper = serviceProvider.GetRequiredService<IRadDropDownListHelper>();
            RadDropDownList radDropDownList = new();

            //act
            helper.LoadComboItems<RuleChainingBehavior>(radDropDownList);

            //assert
            Assert.Equal(RuleChainingBehavior.None, radDropDownList.Items[0].Value);
            Assert.Equal(RuleChainingBehavior.UpdateOnly, radDropDownList.Items[1].Value);
            Assert.Equal(RuleChainingBehavior.Full, radDropDownList.Items[2].Value);
            Assert.Equal("NoneFR", radDropDownList.Items[0].Text);
            Assert.Equal("Update OnlyFR", radDropDownList.Items[1].Text);
            Assert.Equal("FullFR", radDropDownList.Items[2].Text);
        }

        [Fact]
        public void CanLoadTextValuesToRadDropDownList()
        {
            //arrange
            IRadDropDownListHelper helper = serviceProvider.GetRequiredService<IRadDropDownListHelper>();
            RadDropDownList radDropDownList = new();

            //act
            helper.LoadTextItems(radDropDownList, new string[] { "First", "Second", "Third" });

            //assert
            Assert.Equal("First", radDropDownList.Items[0].Text);
            Assert.Equal("Second", radDropDownList.Items[1].Text);
            Assert.Equal("Third", radDropDownList.Items[2].Text);
        }
    }
}
