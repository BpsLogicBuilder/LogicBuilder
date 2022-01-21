using System;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IExceptionHelper
    {
        Exception CriticalException(string guid);
        Exception NotImplimentedException(Type type);
    }
}
