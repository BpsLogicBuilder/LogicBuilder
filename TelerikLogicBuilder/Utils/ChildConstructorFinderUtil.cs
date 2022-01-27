using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Utils
{
    internal class ChildConstructorFinderUtil
    {
        private readonly IConstructorManager _constructorManager;
        private readonly IParametersManager _parametersManager;
        private readonly IReflectionHelper _reflectionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly Dictionary<Type, string> constructorNameMaps;
        private readonly Dictionary<string, Constructor> existingConstructors;

        public ChildConstructorFinderUtil(Dictionary<string, Constructor> existingConstructors, IConstructorManager constructorManager, IParametersManager parametersManager, IReflectionHelper reflectionHelper, ITypeHelper typeHelper, IStringHelper stringHelper)
        {
            _constructorManager = constructorManager;
            _parametersManager = parametersManager;
            _reflectionHelper = reflectionHelper;
            _typeHelper = typeHelper;
            _stringHelper = stringHelper;
            this.constructorNameMaps = new Dictionary<Type, string>();
            this.existingConstructors = existingConstructors;
        }

        internal void AddChildConstructors(IEnumerable<ParameterInfo> parameters)
        {
            foreach(ParameterNodeInfoBase parameterNodeInfo in _parametersManager.GetParameterNodeInfos(parameters))
            {
                if (parameterNodeInfo is ObjectParameterNodeInfo objectParameterNodeInfo)
                {
                    FindConstructor(objectParameterNodeInfo.PInfo.ParameterType);
                }
                else if (parameterNodeInfo is ListOfObjectsParameterNodeInfo listOfObjectsParameterNodeInfo)
                {
                    FindConstructor(_typeHelper.GetUndelyingTypeForValidList(listOfObjectsParameterNodeInfo.PInfo.ParameterType));
                }
            }
        }

        private void FindConstructor(Type type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsEnum)
            {//just return the type's fullName.
            }
            else if (this.constructorNameMaps != null && this.constructorNameMaps.ContainsKey(type))
            {//This check ends recursion when a type is self referencing.  We are creating a constructor for a 
                //parameter but the constructor is currently being created.
            }
            else if (this.existingConstructors.Any(kvp => kvp.Value.TypeName == _typeHelper.ToId(type)))
            {
            }
            else
            {
                ConstructorInfo cInfo = type.GetConstructors().OrderByDescending
                (
                    c => c.GetParameters().Length
                )
                .ThenByDescending
                (
                    c => _reflectionHelper.ComplexParameterCount(c.GetParameters())
                ).FirstOrDefault();

                if (cInfo != null)
                    this.AddConstructor(cInfo);
            }
        }

        private void AddConstructor(ConstructorInfo cInfo)
        {
            string name = _stringHelper.EnsureUniqueName
            (
                _typeHelper.GetTypeDescription(cInfo.DeclaringType),
                new HashSet<string>(existingConstructors.Keys)
            );

            if (!this.constructorNameMaps.ContainsKey(cInfo.DeclaringType))
                this.constructorNameMaps.Add(cInfo.DeclaringType, name);

            AddChildConstructors(cInfo.GetParameters());

            Constructor constructor = this._constructorManager.CreateConstructor(name, cInfo);
            if (constructor != null)
                this.existingConstructors.Add(constructor.Name, constructor);
        }
    }
}
