using System;
using System.IO;

namespace Contoso.Test.Flow.Test
{
    public static class ProjectDirectory
    {
        public static string GetPath() 
            => Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent!.FullName!;
    }
}
