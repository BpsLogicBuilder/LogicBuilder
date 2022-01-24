namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(IEnumHelper enumHelper, IExceptionHelper exceptionHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IEnumHelper EnumHelper { get; }

        public IExceptionHelper ExceptionHelper { get; }

        public IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
