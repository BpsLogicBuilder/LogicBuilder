using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IExceptionHelper
    {
        Exception CriticalException(string guid);
        Exception NotImplimentedException(Type type);
    }
}
