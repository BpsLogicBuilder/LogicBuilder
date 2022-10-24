using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConstructorElementValidator : IConstructorElementValidator
    {
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorGenericsConfigrationValidator _constructorGenericsConfigrationValidator;
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IGenericConstructorHelper _genericConstructorHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ConstructorElementValidator(
            IConstructorDataParser constructorDataParser,
            IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator,
            IConstructorTypeHelper constructorTypeHelper,
            IGenericConstructorHelper genericConstructorHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _constructorDataParser = constructorDataParser;
            _constructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            _constructorTypeHelper = constructorTypeHelper;
            _genericConstructorHelper = genericConstructorHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IParametersElementValidator? _parametersElementValidator;
		private IParametersElementValidator ParametersElementValidator => _parametersElementValidator ??= _xmlElementValidatorFactory.GetParametersElementValidator();

        public void Validate(XmlElement constructorElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            Validate
            (
                _constructorDataParser.Parse(constructorElement),
                _constructorTypeHelper.GetConstructors(assignedTo, application),
                assignedTo,
                application,
                validationErrors
            );
        }

        private void Validate(ConstructorData constructorData, IDictionary<string, Constructor> constructors, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (constructors.Count == 0)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredForObjectTypeFormat, assignedTo.ToString()));
                return;
            }

            if (!constructors.TryGetValue(constructorData.Name, out Constructor? constructor))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredForObjectTypeFormat2, constructorData.Name, assignedTo.ToString()));
                return;
            }

            //constructorType may be generic type definition
            if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, application, out Type? constructorType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForConstructorFormat, constructor.TypeName, constructor.Name));
                return;
            }

            //ensure constructor data is consistent with the constructor with regard to generics and
            //also ensure that the constructor type is a generic type definition if the data has generic arguments
            if (!_constructorGenericsConfigrationValidator.Validate(constructor, constructorData.GenericArguments, application, validationErrors))
            {
                return;
            }

            //now if applicable, convert the generic types to non-genric types types
            if (constructor.HasGenericArguments)
            {
                foreach (GenericConfigBase ga in constructorData.GenericArguments)
                {
                    if (!_typeLoadHelper.TryGetSystemType(ga, application, out Type? _))
                    {
                        validationErrors.Add
                        (
                            string.Format
                            (
                                Strings.cannotLoadTypeForGenericArgumentForConstructorFormat,
                                ga.GenericArgumentName,
                                constructorData.Name
                            )
                        );
                        return;
                    };
                }

                constructor = _genericConstructorHelper.ConvertGenericTypes(constructor, constructorData.GenericArguments, application);
            }

            //get the new constructor type if it is a generic constructor
            if (constructor.HasGenericArguments)
            {
                if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, application, out constructorType))
                {
                    validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForConstructorFormat, constructor.TypeName, constructor.Name));
                    return;
                }
            }

            //finally check that the constructor is assignable to the object it is being assigned to
            if (!_typeHelper.AssignableFrom(assignedTo, constructorType))
            {
                validationErrors.Add(string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.constructorNotAssignableFormat,
                    constructor.Name,
                    assignedTo.ToString()
                ));
                return;
            }

            ParametersElementValidator.Validate(constructor.Parameters, constructorData.ParameterElementsList, application, validationErrors);
        }
    }
}
