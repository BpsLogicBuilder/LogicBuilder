using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(IEnumHelper enumHelper, IExceptionHelper exceptionHelper, IStringHelper stringHelper, IXmlDocumentHelpers xmlDocumentHelpers, IReflectionHelper reflectionHelper, ITypeHelper typeHelper)
        {
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            StringHelper = stringHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
            ReflectionHelper = reflectionHelper;
            TypeHelper = typeHelper;

            MemberAttributeReader = new MemberAttributeReader(this);
            ParameterAttributeReader = new ParameterAttributeReader(this);
            ParametersManager = new ParametersManager(this);
            ConstructorManager = new ConstructorManager(this);
            ParametersXmlManager = new ParametersXmlParser(this);
        }

        public IEnumHelper EnumHelper { get; }

        public IExceptionHelper ExceptionHelper { get; }

        public IStringHelper StringHelper { get; }

        public IXmlDocumentHelpers XmlDocumentHelpers { get; }

        public IReflectionHelper ReflectionHelper { get; }

        public ITypeHelper TypeHelper { get; }

        public IMemberAttributeReader MemberAttributeReader { get; }

        public IParameterAttributeReader ParameterAttributeReader { get; }

        public IParametersManager ParametersManager { get; }

        public IConstructorManager ConstructorManager { get; }

        public IParametersXmlParser ParametersXmlManager { get; }
    }
}
