using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct FileExtensions
    {
        internal const string CONFIGFILEEXTENSION = ".data";
        internal const string PROJECTFILEEXTENSION = ".lbproj";
        internal const string VISIOFILEEXTENSION = ".vsd";
        internal const string VSDXFILEEXTENSION = ".vsdx";
        internal const string TABLEFILEEXTENSION = ".tbl";
        internal const string RULESFILEEXTENSION = ".module";
        internal const string RESOURCEFILEEXTENSION = ".resources";
        internal const string RESOURCETEXTFILEEXTENSION = ".txt";

        internal static readonly HashSet<string> DocumentExtensions = new
        (
            new string[] 
            { 
                VISIOFILEEXTENSION, 
                VSDXFILEEXTENSION, 
                TABLEFILEEXTENSION 
            }
        );

        internal static readonly HashSet<string> RulesFolderFileExtensions = new
        (
            new string[]
            {
                RULESFILEEXTENSION,
                RESOURCEFILEEXTENSION,
                RESOURCETEXTFILEEXTENSION
            }
        );
    }
}
