using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IUpdateVariables
    {
        void Update(XmlDocument xmlDocument);
    }
}
