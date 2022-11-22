using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths
{
    internal class AssemblyPath : IListBoxManageable
    {
        public AssemblyPath(
            IPathHelper pathHelper,
            string path)
        {
            Path = pathHelper.RemoveTrailingPathSeparator(path.Trim());
        }

        internal string Path;

        public IList<string> Errors
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                    return new List<string> { Strings.listItemIsEmpty };

                if (!Regex.IsMatch(Path, RegularExpressions.FILEPATH))
                    return new List<string> { string.Format(CultureInfo.CurrentCulture, Strings.invalidFilePathMessageFormat, Path) };

                return new List<string>();
            }
        }

        #region Methods
        public override string ToString()
            => Path ?? string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is not AssemblyPath other) return false;
            return Path.ToLowerInvariant().Equals(other.Path.ToLowerInvariant());
        }

        public override int GetHashCode()
            => Path.ToLowerInvariant().GetHashCode();
        #endregion Methods
    }
}
