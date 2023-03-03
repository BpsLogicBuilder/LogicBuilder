using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal partial class EditObjectListControl : UserControl, IEditObjectListControl
    {
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingForm dataGraphEditingForm;
        private readonly Type assignedTo;
        private readonly ObjectListElementInfo objectListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;

        public EditObjectListControl(
            IObjectListDataParser objectListDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingForm dataGraphEditingForm,
            ObjectListElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex)
        {
            InitializeComponent();
            _objectListDataParser = objectListDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingForm = dataGraphEditingForm;
            this.objectListElementInfo = objectListElementInfo;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );
            this.assignedTo = assignedTo;
            this.selectedIndex = selectedIndex;
            var data = _objectListDataParser.Parse(xmlDocument.DocumentElement!);
            radTextBox1.Text = data.VisibleText;
        }

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public XmlElement XmlResult => throw new NotImplementedException();

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
