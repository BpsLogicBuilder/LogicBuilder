using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class PathHelperTest
    {
        public PathHelperTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
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
        public void ExtensionReturnsTheExtension()
        {
            //arrange
            var fullPath = @"C:\MyPath\ConsoleApp1\ConsoleApp1.SLN";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var extension = pathHelper.GetExtension(fullPath);

            //assert
            Assert.Equal(".sln", extension);
        }

        [Fact]
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
        public void GetFilePathReturnsTheExpectedResult()
        {
            //arrange
            var fullPath = $@"{TestFolders.TestAssembliesFolder}\FlowProjects\Contoso.Test\Contoso.Test.lbproj";
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            var folderPath = pathHelper.GetFilePath(fullPath);

            //assert
            Assert.Equal($@"{TestFolders.TestAssembliesFolder}\FlowProjects\Contoso.Test", folderPath, true);
        }

        [Fact]
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
    }
}