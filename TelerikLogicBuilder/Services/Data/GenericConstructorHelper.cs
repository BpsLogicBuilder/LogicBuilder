using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class GenericConstructorHelper : IGenericConstructorHelper
    {
        private readonly IConstructorFactory _constructorFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericParametersHelper _genericParametersHelper;
        private readonly IGenericsConfigrationValidator _genericsConfigrationValidator;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public GenericConstructorHelper(
            IConstructorFactory constructorFactory,
            IExceptionHelper exceptionHelper,
            IGenericParametersHelper genericParametersHelper,
            IGenericsConfigrationValidator genericsConfigrationValidator,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper)
        {
            _constructorFactory = constructorFactory;
            _exceptionHelper = exceptionHelper;
            _genericParametersHelper = genericParametersHelper;
            _genericsConfigrationValidator = genericsConfigrationValidator;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
        }

        public Constructor ConvertGenericTypes(Constructor constructor, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application)
        {
            if (!constructor.HasGenericArguments)
                return constructor;

            if (constructor.GenericArguments.Count != genericParameters.Count)
                throw _exceptionHelper.CriticalException("{FE6A7D95-BDE6-43D6-93BE-1018DF6F6C82}");

            return _constructorFactory.GetConstructor
            (
                constructor.Name,
                _typeHelper.ToId(MakeGenericType(constructor, genericParameters, application)),
                _genericParametersHelper.GetConvertedParameters(constructor.Parameters, genericParameters, application),
                constructor.GenericArguments,
                constructor.Summary
            );
        }

        public Type MakeGenericType(Constructor constructor, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application)
        {
            if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, application, out Type? constructorType))
                throw _exceptionHelper.CriticalException("{00C8F426-63F1-4C75-9861-160617E2CE1C}");

            if (!_genericsConfigrationValidator.GenericArgumentCountMatchesType(constructorType, genericParameters))
                throw _exceptionHelper.CriticalException("{4E9A10C7-3728-40C6-8658-3CD7FECA94CF}");

            IEnumerable<string> missingFromConfig = genericParameters.Select(a => a.GenericArgumentName).Except(constructor.GenericArguments);
            IEnumerable<string> missingFromData = constructor.GenericArguments.Except(genericParameters.Select(a => a.GenericArgumentName));

            if (missingFromConfig.Any())
                throw _exceptionHelper.CriticalException("{C2760460-DEEE-47DF-AE75-FAC480541435}");

            if (missingFromData.Any())
                throw _exceptionHelper.CriticalException("{BBC8B04E-B7A7-47D7-BF85-B9F8DBF36AFB}");

            Dictionary<string, GenericConfigBase> genericArgumentsTable = genericParameters.ToDictionary(ga => ga.GenericArgumentName);

            return constructorType.MakeGenericType
            (
                constructor
                .GenericArguments
                .Select
                (
                    ga =>
                    {
                        if (!_typeLoadHelper.TryGetSystemType(genericArgumentsTable[ga], application, out Type? argumentType))
                            throw _exceptionHelper.CriticalException("{7960EE3A-AB7E-4326-8724-88810EB1EBC2}");
                            //Generic configurations should be validated before converting a constructor

                        return argumentType;
                    }
                )
                .ToArray()
            );
        }
    }
}
