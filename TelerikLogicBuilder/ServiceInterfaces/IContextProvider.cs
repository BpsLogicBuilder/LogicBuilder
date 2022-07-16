using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
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
        IFunctionHelper FunctionHelper { get; }
        IMainWindow MainWindow { get; }
        IModuleNamesReader ModuleNamesReader { get; }
        IParameterHelper ParameterHelper { get; }
        IPathHelper PathHelper { get; }
        IReflectionHelper ReflectionHelper { get; }
        IResultMessageBuilder ResultMessageBuilder { get; }
        IStringHelper StringHelper { get; }
        ITypeHelper TypeHelper { get; }
        IVariableHelper VariableHelper { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
