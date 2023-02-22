using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IUpdateParameterControlValues
    {
        void PrepopulateRequiredFields(
            IDictionary<string, ParameterControlSet> editControlsSet,
            IDictionary<string, XmlElement> parameterElementsDictionary,
            IDictionary<string, ParameterBase> parametersDictionary, 
            XmlDocument xmlDocument, 
            string parametersXPath, 
            ApplicationTypeInfo application);

        void SetDefaultsForLiterals(
            IDictionary<string, ParameterControlSet> editControlsSet,
            IDictionary<string, ParameterBase> parametersDictionary);

        void UpdateExistingFields(
            IList<XmlElement> parameterElementsList,
            IDictionary<string, ParameterControlSet> editControlsSet,
            IDictionary<string, ParameterBase> parametersDictionary,
            string? selectedParameter = null);
    }
}
