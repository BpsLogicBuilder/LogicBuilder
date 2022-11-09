using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
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
                if (string.IsNullOrEmpty(this.Path))
                    return new List<string> { Strings.listItemIsEmpty };

                if (!Regex.IsMatch(this.Path, RegularExpressions.FILEPATH))
                    return new List<string> { string.Format(CultureInfo.CurrentCulture, Strings.invalidFilePathMessageFormat, this.Path) };

                return new List<string>();
            }
        }

        #region Methods
        public override string ToString() 
            => Path ?? string.Empty;

        public override bool Equals(object? obj)
        {
            AssemblyPath? other = obj as AssemblyPath;
            if (other == null) return false;
            return this.Path.ToLowerInvariant().Equals(other.Path.ToLowerInvariant());
        }

        public override int GetHashCode() 
            => this.Path.ToLowerInvariant().GetHashCode();
        #endregion Methods
    }
}
