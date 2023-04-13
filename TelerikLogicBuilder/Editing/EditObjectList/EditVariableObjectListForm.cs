using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal partial class EditVariableObjectListForm : Telerik.WinControls.UI.RadForm, IEditVariableObjectListForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDataGraphEditingFormEventsHelper _dataGraphEditingFormEventsHelper;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditObjectListCommandFactory _editObjectListCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private ApplicationTypeInfo _application;
        private readonly Type assignedTo;
        private readonly ObjectListVariableElementInfo objectListInfo;

        public EditVariableObjectListForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IEditingFormHelperFactory editingFormHelperFactory,
            IEditObjectListCommandFactory editObjectListCommandFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            Type assignedTo,
            ObjectListVariableElementInfo objectListInfo,
            XmlDocument objectListXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editObjectListCommandFactory = editObjectListCommandFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.ParametersDataSchema);
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.assignedTo = assignedTo;
            this.objectListInfo = objectListInfo;

            _treeViewXmlDocumentHelper.LoadXmlDocument(objectListXmlDocument.OuterXml);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            //Initialize();
        }

        public bool DenySpecialCharacters => throw new NotImplementedException();

        public bool DisplayNotCheckBox => throw new NotImplementedException();

        public RadPanel RadPanelFields => throw new NotImplementedException();

        public RadTreeView TreeView => throw new NotImplementedException();

        public string VisibleText => throw new NotImplementedException();

        public XmlDocument XmlDocument => throw new NotImplementedException();

        public XmlElement XmlResult => throw new NotImplementedException();

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public Type AssignedTo => throw new NotImplementedException();

        public IDictionary<string, string> ExpandedNodes => throw new NotImplementedException();

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage()
        {
            ApplicationChanged?.Invoke(null, new ApplicationChangedEventArgs(Application));
            throw new NotImplementedException();
        }

        public void DisableControlsDuringEdit(bool disable)
        {
            throw new NotImplementedException();
        }

        public void RebuildTreeView()
        {
            throw new NotImplementedException();
        }

        public void ReloadXmlDocument(string xmlString)
        {
            throw new NotImplementedException();
        }

        public void RequestDocumentUpdate(IEditingControl editingControl)
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

        public void ValidateXmlDocument()
        {
            throw new NotImplementedException();
        }
    }
}
