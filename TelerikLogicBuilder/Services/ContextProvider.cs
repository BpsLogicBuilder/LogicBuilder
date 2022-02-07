using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense;

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

            MemberAttributeReader = new MemberAttributeReader(this);
            ParameterAttributeReader = new ParameterAttributeReader(this);
            ParametersManager = new ParametersManager(this);
            ConstructorManager = new ConstructorManager(this);
            ParametersXmlParser = new ParametersXmlParser(this);
            ConstructorXmlParser = new ConstructorXmlParser(this);
            WebApiDeploymentXmlParser = new WebApiDeploymentXmlParser(this);
            ApplicationXmlParser = new ApplicationXmlParser(this);
            CreateProjectProperties = new CreateProjectProperties(this);
            LoadProjectProperties = new LoadProjectProperties(this);
            UpdateProjectProperties = new UpdateProjectProperties(this);
        }

        public IEnumHelper EnumHelper { get; }

        public IExceptionHelper ExceptionHelper { get; }

        public IStringHelper StringHelper { get; }

        public IXmlDocumentHelpers XmlDocumentHelpers { get; }

        public IReflectionHelper ReflectionHelper { get; }

        public ITypeHelper TypeHelper { get; }

        public IEncryption Encryption { get; }

        public IXmlValidator XmlValidator { get; }

        public IPathHelper PathHelper { get; }

        public IFileIOHelper FileIOHelper { get; }

        public IMessageBoxOptionsHelper MessageBoxOptionsHelper { get; }

        public IMemberAttributeReader MemberAttributeReader { get; }

        public IParameterAttributeReader ParameterAttributeReader { get; }

        public IParametersManager ParametersManager { get; }

        public IConstructorManager ConstructorManager { get; }

        public IParametersXmlParser ParametersXmlParser { get; }

        public IConstructorXmlParser ConstructorXmlParser { get; }

        public IWebApiDeploymentXmlParser WebApiDeploymentXmlParser { get; }

        public IApplicationXmlParser ApplicationXmlParser { get; }

        public ICreateProjectProperties CreateProjectProperties { get; }

        public ILoadProjectProperties LoadProjectProperties { get; }

        public IUpdateProjectProperties UpdateProjectProperties { get; }
    }
}
