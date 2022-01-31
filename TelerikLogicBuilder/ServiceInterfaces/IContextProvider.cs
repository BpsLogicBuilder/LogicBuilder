using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IContextProvider
    {
        IEnumHelper EnumHelper { get; }
        IExceptionHelper ExceptionHelper { get; }
        IReflectionHelper ReflectionHelper { get; }
        IStringHelper StringHelper { get; }
        ITypeHelper TypeHelper { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
        IMemberAttributeReader MemberAttributeReader { get; }
        IParameterAttributeReader ParameterAttributeReader { get; }
        IParametersManager ParametersManager { get; }
        IParametersXmlParser ParametersXmlParser { get; }
        IConstructorManager ConstructorManager { get; }
        IConstructorXmlParser ConstructorXmlParser { get; }
    }
}
