using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ExceptionHelper : IExceptionHelper
    {
        public Exception CriticalException(string guid) => new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, guid));

        public Exception NotImplimentedException(Type type) => new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Strings.notImplementedMessageFormat, type));
    }
}
