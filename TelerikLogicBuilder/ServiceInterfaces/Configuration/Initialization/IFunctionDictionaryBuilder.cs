using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IFunctionDictionaryBuilder
    {
        IDictionary<string, Function> GetBooleanFunctionDictionary(XmlDocument xmlDocument);
        IDictionary<string, Function> GetDialogFunctionDictionary(XmlDocument xmlDocument);
        IDictionary<string, Function> GetDictionary(XmlDocument xmlDocument);
        IDictionary<string, Function> GetTableFunctionDictionary(XmlDocument xmlDocument);
        IDictionary<string, Function> GetValueFunctionDictionary(XmlDocument xmlDocument);
        IDictionary<string, Function> GetVoidFunctionDictionary(XmlDocument xmlDocument);
    }
}
