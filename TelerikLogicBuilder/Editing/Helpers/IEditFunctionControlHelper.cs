using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IEditFunctionControlHelper
    {
        XmlElement GetXmlResult(IDictionary<string, ParameterControlSet> editControlsSet, bool notChecked);
        void UpdateParameterControls(TableLayoutPanel tableLayoutPanel, IDictionary<string, ParameterControlSet> editControlsSet);
        bool ValidateGenericArgs();
        bool ValidateParameters();
    }
}
