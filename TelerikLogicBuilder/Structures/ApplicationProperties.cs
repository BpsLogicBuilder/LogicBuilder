using System.IO;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ApplicationProperties
    {
        #region Constants
        internal static readonly string ApplicationsStencil = Path.Combine("Stencils", "Applications.vss");
        internal static readonly string FlowDiagramStencil = Path.Combine("Stencils", "Logic Builder.vss");
        internal static readonly string Name = Strings.applicationNameLogicBuilder;
        #endregion Constants

        #region Variables
        private static string? _applicationPath;
        #endregion Variables

        #region Properties
        internal static string ApplicationPath
        {
            get
            {
                if (_applicationPath == null)
                    _applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

                return _applicationPath;
            }
        }
        #endregion Properties
    }
}
