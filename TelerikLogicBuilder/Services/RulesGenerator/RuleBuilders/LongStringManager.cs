using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class LongStringManager : ILongStringManager
    {
        #region Properties
        private static readonly IDictionary<RuntimeType, string> StrongNames = new Dictionary<RuntimeType, string>
        {
            { RuntimeType.NetCore, AssemblyStrongNames.NETCORE },
            { RuntimeType.NetFramework, AssemblyStrongNames.NETFRAMEWORK },
            { RuntimeType.Xamarin, AssemblyStrongNames.XAMARIN },
            { RuntimeType.NetNative, AssemblyStrongNames.NETNATIVE }
        };

        private static readonly IDictionary<RuntimeType, string> CodeDomStrongNames = new Dictionary<RuntimeType, string>
        {
            { RuntimeType.NetCore, AssemblyStrongNames.CODEDOM_NETCORE },
            { RuntimeType.NetFramework, AssemblyStrongNames.CODEDOM_NETFRAMEWORK },
            { RuntimeType.Xamarin, AssemblyStrongNames.CODEDOM_XAMARIN },
            { RuntimeType.NetNative, AssemblyStrongNames.CODEDOM_NETNATIVE}
        };

        private static readonly IDictionary<RuntimeType, string> LinqStrongNames = new Dictionary<RuntimeType, string>
        {
            { RuntimeType.NetCore, AssemblyStrongNames.LINQ_NETCORE },
            { RuntimeType.NetFramework, AssemblyStrongNames.LINQ_NETFRAMEWORK },
            { RuntimeType.Xamarin, AssemblyStrongNames.LINQ_XAMARIN },
            { RuntimeType.NetNative, AssemblyStrongNames.LINQ_NETNATIVE}
        };
        #endregion Properties

        public string GetLongStringForBinary(string longString, RuntimeType platForm)
        {
            longString = UpdateStrongNameByPlatForm(longString, platForm);

            if (longString.Contains("\\r\\n"))
                longString = longString.Replace("\\r\\n", "\n");
            if (longString.Contains("\\n"))
                longString = longString.Replace("\\n", "\n");
            if (longString.Contains(Environment.NewLine))
                longString = longString.Replace(Environment.NewLine, "\n");
            if (longString.Contains("\n"))
                longString = longString.Replace("\n", Environment.NewLine);
            if (longString.Contains("&#123;"))
                longString = longString.Replace("&#123;", "{");
            if (longString.Contains("&#125;"))
                longString = longString.Replace("&#125;", "}");
            if (longString.Contains("&#92;"))
                longString = longString.Replace("&#92;", "\\");

            return longString;
        }

        public string GetLongStringForText(string longString, RuntimeType platForm)
        {
            longString = GetLongStringForBinary(longString, platForm);
            if (longString.Contains(Environment.NewLine))
                longString = longString.Replace(Environment.NewLine, "\\r\\n");

            return longString;
        }

        public string UpdateStrongNameByPlatForm(string ruleSetOrResourceString, RuntimeType platForm)
        {
            //Update all platforms including Core (keep the serialized core version at 4.0.0.0).
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.NETCORE_MATCH, StrongNames[platForm]);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.CODEDOM_NETCORE_MATCH, CodeDomStrongNames[platForm]);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.LINQ_NETCORE_MATCH, LinqStrongNames[platForm]);

            return ruleSetOrResourceString;
        }

        public string UpdateStrongNameToNetCore(string ruleSetOrResourceString)
        {
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.NETFRAMEWORK_MATCH, AssemblyStrongNames.NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.XAMARIN_MATCH, AssemblyStrongNames.NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.NETNATIVE_MATCH, AssemblyStrongNames.NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.CODEDOM_NETFRAMEWORK_MATCH, AssemblyStrongNames.CODEDOM_NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.CODEDOM_XAMARIN_MATCH, AssemblyStrongNames.CODEDOM_NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.CODEDOM_NETNATIVE_MATCH, AssemblyStrongNames.CODEDOM_NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.LINQ_NETFRAMEWORK_MATCH, AssemblyStrongNames.LINQ_NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.LINQ_XAMARIN_MATCH, AssemblyStrongNames.LINQ_NETCORE);
            ruleSetOrResourceString = Regex.Replace(ruleSetOrResourceString, AssemblyStrongNames.LINQ_NETNATIVE_MATCH, AssemblyStrongNames.LINQ_NETCORE);

            return ruleSetOrResourceString;
        }
    }
}
