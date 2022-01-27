using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ReflectionHelper : IReflectionHelper
    {
        private readonly ITypeHelper _typeHelper;

        public ReflectionHelper(ITypeHelper typeHelper)
        {
            _typeHelper = typeHelper;
        }

        public int ComplexParameterCount(ParameterInfo[] parameters)
        {
            int count = parameters.Aggregate(0, (cnt, next) =>
            {
                if (!_typeHelper.IsLiteralType(next.ParameterType))
                    cnt++;
                return cnt;
            });

            return count;
        }
    }
}
