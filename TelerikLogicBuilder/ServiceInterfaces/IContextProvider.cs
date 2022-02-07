using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IContextProvider
    {
        IEnumHelper EnumHelper { get; }
        IExceptionHelper ExceptionHelper { get; }
        IReflectionHelper ReflectionHelper { get; }
        IStringHelper StringHelper { get; }
        ITypeHelper TypeHelper { get; }
        IEncryption Encryption { get; }
        IXmlDocumentHelpers XmlDocumentHelpers { get; }
        IXmlValidator XmlValidator { get; }
        IPathHelper PathHelper { get; }
        IFileIOHelper FileIOHelper { get; }
        IMessageBoxOptionsHelper MessageBoxOptionsHelper { get; }
        IMemberAttributeReader MemberAttributeReader { get; }
        IParameterAttributeReader ParameterAttributeReader { get; }
        IParametersManager ParametersManager { get; }
        IParametersXmlParser ParametersXmlParser { get; }
        IConstructorManager ConstructorManager { get; }
        IConstructorXmlParser ConstructorXmlParser { get; }
        IWebApiDeploymentXmlParser WebApiDeploymentXmlParser { get; }
        IApplicationXmlParser ApplicationXmlParser { get; }
        ICreateProjectProperties CreateProjectProperties { get; }
        ILoadProjectProperties LoadProjectProperties { get; }
        IUpdateProjectProperties UpdateProjectProperties { get; }
    }
}
