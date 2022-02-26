using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IFunctionTreeFolderBuilder
    {
        TreeFolder GetVoidFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetBooleanFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetValueFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetDialogFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetTableFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetBuiltInVoidFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetBuiltInBooleanFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetBuiltInValueFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetBuiltInDialogFunctionsTreeFolder(XmlDocument xmlDocument);
        TreeFolder GetBuiltInTableFunctionsTreeFolder(XmlDocument xmlDocument);
    }
}
