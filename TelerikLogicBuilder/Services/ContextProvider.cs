using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(IEnumHelper enumHelper, IExceptionHelper exceptionHelper, IStringHelper stringHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            StringHelper = stringHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IEnumHelper EnumHelper { get; }

        public IExceptionHelper ExceptionHelper { get; }

        public IStringHelper StringHelper { get; }

        public IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
