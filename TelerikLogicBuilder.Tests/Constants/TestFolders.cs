using System;
using System.IO;

namespace TelerikLogicBuilder.Tests.Constants
{
    internal struct TestFolders
    {
        internal static readonly string TestAssembliesFolder = @$"{ProjectDirectory.GetSolutionPath()}";
        internal static readonly string FlowProjectPath = @$"{ProjectDirectory.GetSolutionPath()}\FlowProjects\Contoso.Test";
        internal static readonly string LogicBuilderTests = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), TestFolder);
        private const string TestFolder = "LogicBuilderTests";
    }
}
