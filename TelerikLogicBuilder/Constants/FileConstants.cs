using System.Globalization;
using System.IO;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct FileConstants
    {
        internal const string TABLESTRING = ":tb";
        internal static readonly string DIRECTORYSEPARATOR = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
    }
}
