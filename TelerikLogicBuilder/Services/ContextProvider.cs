using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(IEnumHelper enumHelper, IExceptionHelper exceptionHelper, IStringHelper stringHelper, IXmlDocumentHelpers xmlDocumentHelpers, IReflectionHelper reflectionHelper, ITypeHelper typeHelper, IEncryption encryption, IPathHelper pathHelper, IFileIOHelper fileIOHelper, IMessageBoxOptionsHelper messageBoxOptionsHelper, IVariableHelper variableHelper)
        {
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            StringHelper = stringHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
            ReflectionHelper = reflectionHelper;
            TypeHelper = typeHelper;
            Encryption = encryption;
            PathHelper = pathHelper;
            FileIOHelper = fileIOHelper;
            MessageBoxOptionsHelper = messageBoxOptionsHelper;
            VariableHelper = variableHelper;
        }

        public IEncryption Encryption { get; }
        public IEnumHelper EnumHelper { get; }
        public IExceptionHelper ExceptionHelper { get; }
        public IFileIOHelper FileIOHelper { get; }
        public IMessageBoxOptionsHelper MessageBoxOptionsHelper { get; }
        public IPathHelper PathHelper { get; }
        public IReflectionHelper ReflectionHelper { get; }
        public IStringHelper StringHelper { get; }
        public ITypeHelper TypeHelper { get; }
        public IVariableHelper VariableHelper { get; }
        public IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
