using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml
{
    internal interface IEditFormXml : IApplicationForm
    {
        RadButton BtnOk { get; }
        XmlElement FormattedXmlElement { get; }
        RichTextBox RichTextBox { get; }
        SchemaName Schema { get; }
        XmlElement UnFormattedXmlElement { get; }
        string XmlResult { get; }
        void ValidateElement();
        void ValidateSchema();
    }
}
