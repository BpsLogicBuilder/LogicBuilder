using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal partial class EditVariableLiteralListControl : UserControl, IEditVariableLiteralListControl
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;
        private readonly Type assignedTo;
        private readonly LiteralListVariableElementInfo literalListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;

        public EditVariableLiteralListControl(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            LiteralListVariableElementInfo literalListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex)
        {
            InitializeComponent();
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.literalListElementInfo = literalListElementInfo;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );
            this.assignedTo = assignedTo;
            this.selectedIndex = selectedIndex;
        }

        public IApplicationControl ApplicationControl => throw new NotImplementedException();

        public ListVariableInputStyle ListControl => throw new NotImplementedException();

        public Type LiteralType => throw new NotImplementedException();

        public IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager => throw new NotImplementedException();

        public ILiteralListItemValueControl ValueControl => throw new NotImplementedException();

        public bool DenySpecialCharacters => throw new NotImplementedException();

        public bool DisplayNotCheckBox => throw new NotImplementedException();

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public XmlElement XmlResult => throw new NotImplementedException();

        public bool IsValid => true;

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void RequestDocumentUpdate()
        {
            throw new NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public void ValidateFields()
        {
        }
    }
}
