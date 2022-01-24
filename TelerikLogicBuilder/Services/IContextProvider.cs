namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IContextProvider
    {
        IEnumHelper EnumHelper { get; }
        IExceptionHelper ExceptionHelper { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
