using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IParametersElementValidator
    {
        void Validate(IList<ParameterBase> parameters, IList<XmlElement> parameterElementsList, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
