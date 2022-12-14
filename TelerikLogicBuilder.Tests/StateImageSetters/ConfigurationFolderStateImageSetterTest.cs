using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.StateImageSetters
{
    public class ConfigurationFolderStateImageSetterTest
    {
        public ConfigurationFolderStateImageSetterTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateConfigurationFolderStateImageSetter()
        {
            //arrange
            IConfigurationFolderStateImageSetter service = serviceProvider.GetRequiredService<IConfigurationFolderStateImageSetter>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void FolderSetToCheckedFromChildFolder()
        {
            //arrange
            IConfigurationFolderStateImageSetter service = serviceProvider.GetRequiredService<IConfigurationFolderStateImageSetter>();
            ICompareImages compareImages = serviceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode parentNode = new
            (
                "Variables",
                new RadTreeNode[]
                {
                    new StateImageRadTreeNode
                    (
                        "StringItem"
                    )
                    {
                        StateImage = ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark
                    },
                    new StateImageRadTreeNode
                    (
                        "Decimaltem"
                    )
                    {
                        StateImage = ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark
                    }
                }
            );

            //act
            service.SetImage(parentNode);

            //assert
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
        }

        [Fact]
        public void FolderSetToErrorFromChildFolder()
        {
            //arrange
            IConfigurationFolderStateImageSetter service = serviceProvider.GetRequiredService<IConfigurationFolderStateImageSetter>();
            ICompareImages compareImages = serviceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode parentNode = new
            (
                "Variables",
                new RadTreeNode[]
                {
                    new StateImageRadTreeNode
                    (
                        "StringItem"
                    )
                    {
                        StateImage = ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error
                    },
                    new StateImageRadTreeNode
                    (
                        "Decimaltem"
                    )
                    {
                        StateImage = ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark
                    }
                }
            );

            //act
            service.SetImage(parentNode);

            //assert
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
        }
    }
}
