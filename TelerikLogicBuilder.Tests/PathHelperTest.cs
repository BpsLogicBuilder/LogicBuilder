using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class PathHelperTest
    {
        public PathHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void TwoPartCombinePathsReturnsExpected()
        {
            //arrange
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var combinedPath = pathHelper.CombinePaths(@"MyPath\ConsoleApp1", "ConsoleApp1.sln");

            //assert
            Assert.Equal(@"MyPath\ConsoleApp1\ConsoleApp1.sln", combinedPath);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CombinePathsWorksWithoutSeparators()
        {
            //arrange
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var combinedPath = pathHelper.CombinePaths(@"C:", @"MyPath\ConsoleApp1", "ConsoleApp1.sln");

            //assert
            Assert.Equal(@"C:\MyPath\ConsoleApp1\ConsoleApp1.sln", combinedPath);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void ThreePartCombinePathsWorksWithSeparators()
        {
            //arrange
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var combinedPath = pathHelper.CombinePaths(@"C:\", @"\MyPath\ConsoleApp1\", @"\ConsoleApp1.sln");

            //assert
            Assert.Equal(@"C:\MyPath\ConsoleApp1\ConsoleApp1.sln", combinedPath);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void FourPartsWorksWithoutSeparators()
        {
            //arrange
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var combinedPath = pathHelper.CombinePaths(@"C:", @"MyPath", "ConsoleApp1", "ConsoleApp1.sln");

            //assert
            Assert.Equal(@"C:\MyPath\ConsoleApp1\ConsoleApp1.sln", combinedPath);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void FourPartsWorksWithSeparators()
        {
            //arrange
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var combinedPath = pathHelper.CombinePaths(@"C:\", @"\MyPath\", @"\ConsoleApp1\", @"\ConsoleApp1.sln");

            //assert
            Assert.Equal(@"C:\MyPath\ConsoleApp1\ConsoleApp1.sln", combinedPath);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void ExtensionReturnsTheExtension()
        {
            //arrange
            var fullPath = @"C:\MyPath\ConsoleApp1\ConsoleApp1.sln";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var folderName = pathHelper.GetExtension(fullPath);

            //assert
            Assert.Equal(".sln", folderName);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void FileNameReturnsTheFileName()
        {
            //arrange
            var fullPath = @"C:\MyPath\ConsoleApp1\ConsoleApp1.sln";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var folderName = pathHelper.GetFileName(fullPath);

            //assert
            Assert.Equal("ConsoleApp1.sln", folderName);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void FileNameNoExtentionReturnsTheFileNameNoExtention()
        {
            //arrange
            var fullPath = @"C:\MyPath\ConsoleApp1\ConsoleApp1.sln";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var folderName = pathHelper.GetFileNameNoExtention(fullPath);

            //assert
            Assert.Equal("ConsoleApp1", folderName);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void FolderNameReturnsOnlyTheFolderName()
        {
            //arrange
            var fullPath = @"C:\MyPath\ConsoleApp1\";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var folderName = pathHelper.GetFolderName(fullPath);

            //assert
            Assert.Equal("ConsoleApp1", folderName);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void ModuleNameReturnsTheModuleName()
        {
            //arrange
            var fullPath = @"C:\MyPath\ConsoleApp1\Save Student.sln";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var folderName = pathHelper.GetModuleName(fullPath);

            //assert
            Assert.Equal("save_student", folderName);
        }

        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IPathHelper, PathHelper>()
                .BuildServiceProvider();
        }
    }
}