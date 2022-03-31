using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConstructorElementValidator : IConstructorElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorGenericsConfigrationValidator _constructorGenericsConfigrationValidator;
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IGenericConstructorHelper _genericConstructorHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        //private readonly fields were injected into XmlElementValidator

        public ConstructorElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _constructorDataParser = xmlElementValidator.ConstructorDataParser;
            _constructorGenericsConfigrationValidator = xmlElementValidator.ConstructorGenericsConfigrationValidator;
            _constructorTypeHelper = xmlElementValidator.ConstructorTypeHelper;
            _genericConstructorHelper = xmlElementValidator.GenericConstructorHelper;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
        //e.g. if new ConstructorElementValidator((XmlElementValidator)this) is called in the
        //XmlElementValidator constructor and _parameterElementValidator is assigned in this constructor,
        //then _parameterElementValidator could be null.
        //using properties e.g.
        // private IParameterElementValidator? _parameterElementValidator;
        // public IParameterElementValidator ParameterElementValidator
        //     => _parameterElementValidator ??= new ParameterElementValidator(this);
        // resulted in multiple .dll access errors in the tests.
        private IParametersElementValidator ParametersElementValidator => _xmlElementValidator.ParametersElementValidator;

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
