using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(IConfigurationService configurationService, IEncryption encryption, IEnumHelper enumHelper, IExceptionHelper exceptionHelper, IFileIOHelper fileIOHelper, IMessageBoxOptionsHelper messageBoxOptionsHelper, IParameterHelper parameterHelper, IPathHelper pathHelper, IReflectionHelper reflectionHelper, IStringHelper stringHelper, ITypeHelper typeHelper, IVariableHelper variableHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            ConfigurationService = configurationService;
            Encryption = encryption;
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            FileIOHelper = fileIOHelper;
            MessageBoxOptionsHelper = messageBoxOptionsHelper;
            ParameterHelper = parameterHelper;
            PathHelper = pathHelper;
            ReflectionHelper = reflectionHelper;
            StringHelper = stringHelper;
            TypeHelper = typeHelper;
            VariableHelper = variableHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IConfigurationService ConfigurationService { get; }
        public IEncryption Encryption { get; }
        public IEnumHelper EnumHelper { get; }
        public IExceptionHelper ExceptionHelper { get; }
        public IFileIOHelper FileIOHelper { get; }
        public IMessageBoxOptionsHelper MessageBoxOptionsHelper { get; }
        public IParameterHelper ParameterHelper { get; }
        public IPathHelper PathHelper { get; }
        public IReflectionHelper ReflectionHelper { get; }
        public IStringHelper StringHelper { get; }
        public ITypeHelper TypeHelper { get; }
        public IVariableHelper VariableHelper { get; }
        public IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
