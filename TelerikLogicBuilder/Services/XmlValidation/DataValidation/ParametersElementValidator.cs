using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ParametersElementValidator : IParametersElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ParametersElementValidator(
            IEnumHelper enumHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IParameterElementValidator? _parameterElementValidator;
		private IParameterElementValidator ParameterElementValidator => _parameterElementValidator ??= _xmlElementValidatorFactory.GetParameterElementValidator();

        public void Validate(IList<ParameterBase> parameters, IList<XmlElement> parameterElementsList, ApplicationTypeInfo application, List<string> validationErrors)
        {
            Dictionary<string, XmlElement> elements = parameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE));

            foreach (ParameterBase parameter in parameters)
                ValidateParameter(parameter);

            void ValidateParameter(ParameterBase parameter)
            {
                if (!elements.TryGetValue(parameter.Name, out XmlElement? pElement))
                {
                    if (!parameter.IsOptional)
                        validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.parameterNotOptionalFormat, parameter.Name));
                    return;
                }

                switch (parameter.ParameterCategory)
                {
                    case ParameterCategory.Object:
                    case ParameterCategory.LiteralList:
                    case ParameterCategory.ObjectList:
                        if (!pElement.HasChildNodes)//this should never happen.  The UI will always add a child node.  If editing XML then the schema validator should fail.
                        {
                            validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidParameterElementFormat, parameter.Name, _enumHelper.GetVisibleEnumText(parameter.ParameterCategory)));
                            return;
                        }
                        break;
                    default:
                        break;
                }

                if (_enumHelper.GetParameterCategory(pElement.Name) != parameter.ParameterCategory)
                {
                    validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidParameterElementFormat, parameter.Name, _enumHelper.GetVisibleEnumText(parameter.ParameterCategory)));
                    return;
                }

                ParameterElementValidator.Validate(pElement, parameter, application, validationErrors);
            }
        }
    }
}
