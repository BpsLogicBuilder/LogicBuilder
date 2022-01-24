using System.IO;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ApplicationProperties
    {
        #region Constants
        internal readonly string ApplicationsStencil = Path.Combine("Stencils", "Applications.vss");
        internal readonly string FlowDiagramStencil = Path.Combine("Stencils", "Logic Builder.vss");
        internal readonly string Name = Strings.applicationNameLogicBuilder;
        #endregion Constants

        #region Variables
        private string _applicationPath;
        #endregion Variables

        #region Properties
        internal string ApplicationPath
        {
            get
            {
                if (_applicationPath == null)
                    _applicationPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                return _applicationPath;
            }
        }
        #endregion Properties
    }
}
