using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class ChildConstructorFinderUtility
    {
        private readonly IConstructorManager _constructorManager;
        private readonly IParametersManager _parametersManager;
        private readonly IReflectionHelper _reflectionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly Dictionary<Type, string> constructorNameMaps;
        private readonly Dictionary<string, Constructor> existingConstructors;

        public ChildConstructorFinderUtility(Dictionary<string, Constructor> existingConstructors, 
            IConstructorManager constructorManager,
            IParametersManager parametersManager,
            IReflectionHelper reflectionHelper,
            ITypeHelper typeHelper,
            IStringHelper stringHelper,
            IMemberAttributeReader memberAttributeReader)
        {
            _reflectionHelper = reflectionHelper;
            _typeHelper = typeHelper;
            _stringHelper = stringHelper;
            _constructorManager = constructorManager;
            _parametersManager = parametersManager;
            _memberAttributeReader = memberAttributeReader;
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
                ConstructorInfo? cInfo = type.GetConstructors().OrderByDescending
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
                GetName(_memberAttributeReader.GetAlsoKnownAs(cInfo)),
                new HashSet<string>(existingConstructors.Keys)
            );

            if (cInfo.DeclaringType == null)
                return;

            if (!this.constructorNameMaps.ContainsKey(cInfo.DeclaringType))
                this.constructorNameMaps.Add(cInfo.DeclaringType, name);

            AddChildConstructors(cInfo.GetParameters());

            Constructor? constructor = this._constructorManager.CreateConstructor(name, cInfo);
            if (constructor != null)
                this.existingConstructors.Add(constructor.Name, constructor);

            string GetName(string alsoKnownAs)
                => string.IsNullOrEmpty(alsoKnownAs) 
                    ? _typeHelper.GetTypeDescription(cInfo.DeclaringType!)//returns on line 92 if cInfo.DeclaringType == null
                    : alsoKnownAs;
        }
    }
}
