using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class EnumHelper : IEnumHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public EnumHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public string GetVisibleEnumText<T>(T enumType)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{6BE2E8CC-C074-42AF-87C4-26AD076A8805}");

            return Strings.ResourceManager.GetString(string.Concat(MiscellaneousConstants.ENUMDESCRIPTION, Enum.GetName(typeof(T), enumType)));
        }
    }
}
