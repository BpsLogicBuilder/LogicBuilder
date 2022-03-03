using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters
{
    internal class MultipleChoiceParameterValidator : IMultipleChoiceParameterValidator
    {
        private readonly ITypeHelper _typeHelper;

        public MultipleChoiceParameterValidator(ITypeHelper typeHelper)
        {
            _typeHelper = typeHelper;
        }

        public bool ValidateMultipleChoiceParameter(MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length == 0)
                return true;

            ParameterInfo? multipleChoiceParam = parameters.FirstOrDefault(p => _typeHelper.IsValidConnectorList(p.ParameterType));

            return multipleChoiceParam == null || object.ReferenceEquals(multipleChoiceParam, parameters[^1]);
            //return !(multipleChoiceParam != null && !multipleChoiceParam.Equals(parameters[^1]));
            //throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.multipleChoiceParamNotLastFormat, methodInfo.Name));
        }
    }
}
