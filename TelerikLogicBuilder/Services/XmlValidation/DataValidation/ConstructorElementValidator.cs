using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConstructorElementValidator : IConstructorElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorGenericsConfigrationValidator _constructorGenericsConfigrationValidator;
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IGenericContructorHelper _genericContructorHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        //private readonly fields were injected into XmlElementValidator

        public ConstructorElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _constructorDataParser = xmlElementValidator.ConstructorDataParser;
            _constructorGenericsConfigrationValidator = xmlElementValidator.ConstructorGenericsConfigrationValidator;
            _constructorTypeHelper = xmlElementValidator.ConstructorTypeHelper;
            _genericContructorHelper = xmlElementValidator.GenericContructorHelper;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
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
        private IParameterElementValidator ParameterElementValidator => _xmlElementValidator.ParameterElementValidator;

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

                constructor = _genericContructorHelper.ConvertGenericTypes(constructor, constructorData.GenericArguments, application);
            }

            //get the new constructor type is a generic constructor
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

            ValidateParameters(constructor, constructorData, application, validationErrors);
        }

        private void ValidateParameters(Constructor constructor, ConstructorData constructorData, ApplicationTypeInfo application, List<string> validationErrors)
        {
            Dictionary<string, XmlElement> elements = constructorData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE));

            constructor.Parameters.ForEach(par =>
            {
                if (!elements.TryGetValue(par.Name, out XmlElement? pElement))
                {
                    if (!par.IsOptional)
                        validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.constrParameterNotOptionalFormat, par.Name, constructor.Name));
                    return;
                }

                switch (par.ParameterCategory)
                {
                    case ParameterCategory.Object:
                    case ParameterCategory.LiteralList:
                    case ParameterCategory.ObjectList:
                        if (!pElement.HasChildNodes)//this should never happen.  The UI will always add a child node.  If editing XML then the schema validator should fail.
                        {
                            validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidParameterElementFormat, par.Name, _enumHelper.GetVisibleEnumText(par.ParameterCategory)));
                            return;
                        }
                        break;
                    default:
                        break;
                }

                if (_enumHelper.GetParameterCategory(pElement.Name) != par.ParameterCategory)
                {
                    validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidParameterElementFormat, par.Name, _enumHelper.GetVisibleEnumText(par.ParameterCategory)));
                    return;
                }

                ParameterElementValidator.Validate(pElement, par, application, validationErrors);
            });
        }
    }
}
