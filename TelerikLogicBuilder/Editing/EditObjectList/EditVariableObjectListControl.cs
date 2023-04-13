using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ObjectListItemEditor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using MediaFoundation;
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
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal partial class EditVariableObjectListControl : UserControl, IEditVariableObjectListControl
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;
        private readonly Type assignedTo;
        private readonly ObjectListVariableElementInfo objectListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;

        public EditVariableObjectListControl(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            ObjectListVariableElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex)
        {
            InitializeComponent();
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.objectListElementInfo = objectListElementInfo;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );
            this.assignedTo = assignedTo;
            this.selectedIndex = selectedIndex;
        }

        public IApplicationControl ApplicationControl => throw new NotImplementedException();

        public ListVariableInputStyle ListControl => throw new NotImplementedException();

        public Type ObjectType => throw new NotImplementedException();

        public IRadListBoxManager<IObjectListBoxItem> RadListBoxManager => throw new NotImplementedException();

        public IObjectListItemValueControl ValueControl => throw new NotImplementedException();

        public bool DenySpecialCharacters => throw new NotImplementedException();

        public bool DisplayNotCheckBox => throw new NotImplementedException();

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public XmlElement XmlResult => throw new NotImplementedException();

        public string VisibleText => throw new NotImplementedException();

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
