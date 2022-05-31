using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct RuleFunctionConstants
    {
        internal const string RESOURCESHELPERCLASS = "LogicBuilder.RulesDirector.ResourcesHelper";
        internal const string GETRESOURCEMETHODNAME = "GetResource";//T GetResource(string shortValue, DirectorBase director)

        internal const string DIRECTORPROPERTYNAME = "Director";
        internal const string COLLECTIONCLASSNAME = "System.Collections.ObjectModel.Collection";
        internal const string LISTCLASSNAME = "System.Collections.Generic.List";
        internal const string COLLECTIONELEMENTSCLASSNAME = "System.Object";

        internal const string SETMODULENAMEFUNCTIONNAME = "SetModuleName";

        internal const string VIRTUALFUNCTIONFORMATSTRING = "FormatString";
        internal const string VIRTUALFUNCTIONFLOWCOMPLETE = "FlowComplete";
        internal const string VIRTUALFUNCTIONTERMINATE = "Terminate";
        internal const string VIRTUALFUNCTIONWAIT = "Wait";
        internal const string VIRTUALFUNCTIONSETBUSINESSBACKUPDATA = "SetCurrentBusinessBackupData";
    }
}
