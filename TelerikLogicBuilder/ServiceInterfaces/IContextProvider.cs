using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IContextProvider
    {
        IConfigurationService ConfigurationService { get; }
        IEncryption Encryption { get; }
        IEnumHelper EnumHelper { get; }
        IExceptionHelper ExceptionHelper { get; }
        IFileIOHelper FileIOHelper { get; }
        IMessageBoxOptionsHelper MessageBoxOptionsHelper { get; }
        IParameterHelper ParameterHelper { get; }
        IPathHelper PathHelper { get; }
        IReflectionHelper ReflectionHelper { get; }
        IStringHelper StringHelper { get; }
        ITypeHelper TypeHelper { get; }
        IVariableHelper VariableHelper { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
