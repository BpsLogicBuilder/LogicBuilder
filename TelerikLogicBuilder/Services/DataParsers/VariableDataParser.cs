using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class VariableDataParser : IVariableDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;

        public VariableDataParser(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public VariableData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.VARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{31965208-3E68-41D2-B6DB-1926FBEBAE56}");

            return new VariableData
            (
                xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, 
                xmlElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
                xmlElement
            );
        }
    }
}
