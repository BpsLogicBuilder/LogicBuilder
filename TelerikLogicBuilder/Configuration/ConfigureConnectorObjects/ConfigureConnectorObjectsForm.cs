using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects
{
    internal partial class ConfigureConnectorObjectsForm : Telerik.WinControls.UI.RadForm, IConfigureConnectorObjectsForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
        private readonly IConfigureConnectorObjectsControl _configureConnectorObjectsControl;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IProjectPropertiesXmlParser _projectPropertiesXmlParser;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IUpdateProjectProperties _updateProjectProperties;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private ApplicationTypeInfo _application;
        private readonly bool openedAsReadOnly;

        public ConfigureConnectorObjectsForm(
            IConfigurationControlFactory configurationControlFactory,
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            ILoadProjectProperties loadProjectProperties,
            IProjectPropertiesXmlParser projectPropertiesXmlParser,
            IServiceFactory serviceFactory,
            IUpdateProjectProperties updateProjectProperties,
            IXmlDocumentHelpers xmlDocumentHelpers,
            bool openedAsReadOnly)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _configurationService = configurationService;
            _configureConnectorObjectsControl = configurationControlFactory.GetConfigureConnectorObjectsControl(this);
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _loadProjectProperties = loadProjectProperties;
            _projectPropertiesXmlParser = projectPropertiesXmlParser;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );
            _updateProjectProperties = updateProjectProperties;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.openedAsReadOnly = openedAsReadOnly;
            Initialize();
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{742CA5F9-3CE2-4F2E-9EFA-007C996788F2}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage()
        {
            _dialogFormMessageControl.ClearMessage();
        }

        public void SetErrorMessage(string message)
        {
            _dialogFormMessageControl.SetErrorMessage(message);
        }

        public void SetMessage(string message, string title = "")
        {
            _dialogFormMessageControl.SetMessage(message, title);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFill);
            CollapsePanelBorder(radPanelMessages);

            InitializeControls();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            this.FormClosing += ConfigureConnectorObjectsForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 470);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            LoadXmlDocument();
            LoadListBox();
        }

        private void InitializeControls()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeConfigureConnectorObjectsControl();
        }

        private void InitializeApplicationDropDownList()
        {
            ((ISupportInitialize)this.radGroupBoxApplication).BeginInit();
            this.radGroupBoxApplication.SuspendLayout();

            _applicationDropDownList.Dock = DockStyle.Fill;
            _applicationDropDownList.Location = new Point(0, 0);
            this.radGroupBoxApplication.Controls.Add((Control)_applicationDropDownList);

            ((ISupportInitialize)this.radGroupBoxApplication).EndInit();
            this.radGroupBoxApplication.ResumeLayout(true);
        }

        private void InitializeConfigureConnectorObjectsControl()
        {
            ((ISupportInitialize)this.radPanelFill).BeginInit();
            this.radPanelFill.SuspendLayout();

            _configureConnectorObjectsControl.Dock = DockStyle.Fill;
            _configureConnectorObjectsControl.Location = new Point(0, 0);
            this.radPanelFill.Controls.Add((Control)_configureConnectorObjectsControl);

            ((ISupportInitialize)this.radPanelFill).EndInit();
            this.radPanelFill.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }

        private void LoadListBox()
        {
            ProjectProperties projectProperties = _projectPropertiesXmlParser.GeProjectProperties
            (
                _treeViewXmlDocumentHelper.XmlTreeDocument.DocumentElement!,
                _configurationService.ProjectProperties.ProjectName,
                _configurationService.ProjectProperties.ProjectPath
            );

            _configureConnectorObjectsControl.SetObjectTypes(projectProperties.ConnectorObjectTypes.ToArray());
        }

        private void LoadXmlDocument()
        {
            ProjectProperties projectProperties = _loadProjectProperties.Load(_configurationService.ProjectProperties.ProjectFileFullName);
            _treeViewXmlDocumentHelper.LoadXmlDocument(projectProperties.ToXml);
        }

        private void UpdateXmlDocument()
        {
            _xmlDocumentHelpers
                .SelectSingleElement(_treeViewXmlDocumentHelper.XmlTreeDocument, $"{XmlDataConstants.PROJECTPROPERTIESELEMENT}/{XmlDataConstants.CONNECTOROBJECTTYPESELEMENT}")
                .InnerXml = BuildConnectorObjectTypesXml();

            _treeViewXmlDocumentHelper.ValidateXmlDocument();

            string BuildConnectorObjectTypesXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                {
                    foreach (string typeName in _configureConnectorObjectsControl.GetObjectTypes())
                        xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, typeName);
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                }
                return stringBuilder.ToString();
            }
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void ConfigureConnectorObjectsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (!this.openedAsReadOnly && this.DialogResult == DialogResult.OK)
                {
                    UpdateXmlDocument();

                    ProjectProperties projectProperties = _projectPropertiesXmlParser.GeProjectProperties
                    (
                        _treeViewXmlDocumentHelper.XmlTreeDocument.DocumentElement!,
                        _configurationService.ProjectProperties.ProjectName,
                        _configurationService.ProjectProperties.ProjectPath
                    );

                    _updateProjectProperties.Update
                    (
                        projectProperties.ProjectFileFullName,
                        projectProperties.ApplicationList,
                        projectProperties.ConnectorObjectTypes
                    );
                }
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }
        #endregion Event Handlers
    }
}
