using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditingControl
    {
        ApplicationTypeInfo Application { get; }
        DockStyle Dock { set; }
        XmlElement XmlResult { get; }
        bool IsValid { get; }
        Point Location { set; }
        void ClearMessage();
        void RequestDocumentUpdate();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateFields();
    }
}
