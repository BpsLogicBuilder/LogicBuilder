using ABIS.LogicBuilder.FlowBuilder.AttributeReaders;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IContextProvider
    {
        IEnumHelper EnumHelper { get; }
        IExceptionHelper ExceptionHelper { get; }
        IStringHelper StringHelper { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
