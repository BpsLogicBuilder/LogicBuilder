using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IContextProvider
    {
        IEncryption Encryption { get; }
        IEnumHelper EnumHelper { get; }
        IExceptionHelper ExceptionHelper { get; }
        IFileIOHelper FileIOHelper { get; }
        IMessageBoxOptionsHelper MessageBoxOptionsHelper { get; }
        IPathHelper PathHelper { get; }
        IReflectionHelper ReflectionHelper { get; }
        IStringHelper StringHelper { get; }
        ITypeHelper TypeHelper { get; }
        IVariableHelper VariableHelper { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
