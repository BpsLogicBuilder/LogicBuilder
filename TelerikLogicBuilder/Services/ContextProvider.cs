using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(IEnumHelper enumHelper, IExceptionHelper exceptionHelper, IStringHelper stringHelper, IXmlDocumentHelpers xmlDocumentHelpers, IReflectionHelper reflectionHelper, ITypeHelper typeHelper, IEncryption encryption, IXmlValidator xmlValidator, IPathHelper pathHelper, IFileIOHelper fileIOHelper, IMessageBoxOptionsHelper messageBoxOptionsHelper)
        {
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            StringHelper = stringHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
            ReflectionHelper = reflectionHelper;
            TypeHelper = typeHelper;
            Encryption = encryption;
            XmlValidator = xmlValidator;
            PathHelper = pathHelper;
            FileIOHelper = fileIOHelper;
            MessageBoxOptionsHelper = messageBoxOptionsHelper;
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
        public IXmlDocumentHelpers XmlDocumentHelpers { get; }
        public IXmlValidator XmlValidator { get; }
    }
}
