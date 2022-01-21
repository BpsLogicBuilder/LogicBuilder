using System.IO;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal static class ApplicationProperties
    {
        #region Constants
        internal static readonly string ApplicationsStencil = Path.Combine("Stencils", "Applications.vss");
        internal static readonly string FlowDiagramStencil = Path.Combine("Stencils", "Logic Builder.vss");
        internal static readonly string Name = Strings.applicationNameLogicBuilder;
        #endregion Constants

        #region Variables
        private static string applicationPath;
        #endregion Variables

        #region Properties
        internal static string ApplicationPath
        {
            get
            {
                if (applicationPath == null)
                    applicationPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                return applicationPath;
            }
        }
        #endregion Properties
    }
}
