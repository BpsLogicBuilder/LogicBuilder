using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ContextProvider : IContextProvider
    {
        public ContextProvider(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IFunctionHelper functionHelper,
            IMainWindow mainWindow,
            IModuleNamesReader moduleNamesReader,
            IParameterHelper parameterHelper,
            IPathHelper pathHelper,
            IReflectionHelper reflectionHelper,
            IResultMessageBuilder resultMessageBuilder,
            IStringHelper stringHelper,
            ITypeHelper typeHelper,
            IVariableHelper variableHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            EnumHelper = enumHelper;
            ExceptionHelper = exceptionHelper;
            FileIOHelper = fileIOHelper;
            FunctionHelper = functionHelper;
            MainWindow = mainWindow;
            ModuleNamesReader = moduleNamesReader;
            ParameterHelper = parameterHelper;
            PathHelper = pathHelper;
            ReflectionHelper = reflectionHelper;
            ResultMessageBuilder = resultMessageBuilder;
            StringHelper = stringHelper;
            TypeHelper = typeHelper;
            VariableHelper = variableHelper;
            XmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IEnumHelper EnumHelper { get; }
        public IExceptionHelper ExceptionHelper { get; }
        public IFileIOHelper FileIOHelper { get; }
        public IFunctionHelper FunctionHelper { get; }
        public IMainWindow MainWindow { get; }
        public IModuleNamesReader ModuleNamesReader { get; }
        public IParameterHelper ParameterHelper { get; }
        public IPathHelper PathHelper { get; }
        public IReflectionHelper ReflectionHelper { get; }
        public IResultMessageBuilder ResultMessageBuilder { get; }
        public IStringHelper StringHelper { get; }
        public ITypeHelper TypeHelper { get; }
        public IVariableHelper VariableHelper { get; }
        public IXmlDocumentHelpers XmlDocumentHelpers { get; }
    }
}
