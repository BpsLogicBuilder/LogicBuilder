using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal interface IEditXmlFormFactory : IDisposable
    {
        IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo);
    }
}
