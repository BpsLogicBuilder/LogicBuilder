using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IUpdateFragments
    {
        void Update(XmlDocument xmlDocument);
    }
}
