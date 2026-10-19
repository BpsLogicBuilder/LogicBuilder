using System;
using System.IO;

namespace TelerikLogicBuilder.IntegrationTests.Constants
{
    public static class ProjectDirectory
    {
        public static string GetPath()
            => Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent!.FullName!;

        public static string GetSolutionPath()
            => Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent!.FullName!;
    }
}
