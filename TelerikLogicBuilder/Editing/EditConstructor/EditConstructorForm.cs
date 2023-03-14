using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor
{
	internal partial class EditConstructorForm : Telerik.WinControls.UI.RadForm, IEditConstructorForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
		private readonly IDataGraphEditingFormEventsHelper _dataGraphEditingFormEventsHelper;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditingControlFactory _editingControlFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
		private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;

        private ApplicationTypeInfo _application;
        private readonly Type assignedTo;

        public EditConstructorForm(
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IEditingControlFactory editingControlFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IServiceFactory serviceFactory,
            Type assignedTo)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editingControlFactory = editingControlFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
			_treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.ParametersDataSchema);
            this.assignedTo = assignedTo;
            
			_treeViewXmlDocumentHelper.LoadXmlDocument(button2Xml);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            Initialize();
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{0C223B16-511C-4019-A272-7AB8CEC6E297}");

        public bool DenySpecialCharacters => false;

        public bool DisplayNotCheckBox => false;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

		public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
        {
        }

        public void RequestDocumentUpdate(IEditingControl editingControl) => _dataGraphEditingFormEventsHelper.RequestDocumentUpdate(editingControl);

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument() => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.Enabled = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            //radButton1.Click += RadButton1_Click;
            //radButton2.Click += RadButton2_Click;
            //radButton3.Click += RadButton3_Click;
            splitPanelLeft.Click += SplitPanelLeft_Click;

            _formInitializer.SetToEditSize(this);

            _dataGraphEditingFormEventsHelper.Setup();
			LoadTreeview();
        }

        private void LoadTreeview()
        {
            _parametersDataTreeBuilder.CreateConstructorTreeProfile(TreeView, XmlDocument, assignedTo);
            if (TreeView.SelectedNode == null)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        private void SplitPanelLeft_Click(object? sender, EventArgs e)
        {
            radPanelFields.Controls.Clear();
        }

        private void RadButton1_Click(object? sender, EventArgs e)
        {
            //var constructorName = "DropDownItemBindingParameters";
            //var constructorName = "CommandButtonParameters";
            //var constructorName = "DirectiveDescriptionParameters";
            //var constructorName = "FormGroupArraySettingsParameters";
            //var constructorName = "MultiSelectFormControlSettingsParameters";
            var constructorName = "GenericResponse`2[A,B]";
            var constructor = _configurationService.ConstructorList.Constructors[constructorName];
            Navigate((Control)_editingControlFactory.GetEditConstructorControl(this, constructor, assignedTo, GetXmlDocument(BuildConstructorXml(constructorName)), "/constructor"));
            //Navigate((Control)_editingControlFactory.GetEditConstructorControl(this, constructor, assignedTo, GetXmlDocument(button1Xml), "/constructor"));
        }

        private void RadButton2_Click(object? sender, EventArgs e)
        {
            //var constructorName = "DateTime_yy_mm_dd";
            //var constructorName = "ColumnSettingsParameters";
            //var constructorName = "RequestDetailsParameters";
            //var constructorName = "DetailFieldSettingParameters";
            // var constructorName = "EditFormSettingsParameters";//
            //var constructorName = "EditFormSettingsParameters";
            var constructorName = "CommandButtonParameters";
            var constructor = _configurationService.ConstructorList.Constructors[constructorName];
            Navigate((Control)_editingControlFactory.GetEditConstructorControl(this, constructor, assignedTo, GetXmlDocument(BuildConstructorXml(constructorName)), "/constructor"));
            //Navigate((Control)_editingControlFactory.GetEditConstructorControl(this, constructor, assignedTo, GetXmlDocument(button2Xml), "/constructor"));
        }

        private void RadButton3_Click(object? sender, EventArgs e)
        {
			var functionName = "AddItem";
            var function = _configurationService.FunctionList.Functions[functionName];
            Navigate((Control)_editingControlFactory.GetEditStandardFunctionControl(this, function, assignedTo, GetXmlDocument(BuildFunctionXml(functionName)), "/function"));
        }

        private string BuildConstructorXml(string name)
		{
			_ = button1Xml + button2Xml;
            //ThemeResolutionService.Fon

            return new XmlDataHelper(_exceptionHelper, new Services.XmlDocumentHelpers(_exceptionHelper))
				.BuildConstructorXml(name, name, "<genericArguments />", "<parameters />");

        }

        private string BuildFunctionXml(string name)
        {
            _ = button1Xml + button2Xml;
            //ThemeResolutionService.Fon

            return new XmlDataHelper(_exceptionHelper, new Services.XmlDocumentHelpers(_exceptionHelper))
                .BuildFunctionXml(name, name, "<genericArguments />", "<parameters />");

        }

        private static System.Xml.XmlDocument GetXmlDocument(string xmlString)
        {
            System.Xml.XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }

        string button2Xml = @"<constructor name=""DataFormSettingsParameters"" visibleText=""DataFormSettingsParameters: title=Add Instructor;GenericListOfValidationMessageParameters: validationMessages;GenericListOfFormItemSettingsParameters: fieldSettings;FormType: formType;Type: modelType;FormRequestDetailsParameters: requestDetails"">
								<genericArguments />
								<parameters>
									<literalParameter name=""title"">Add Instructor</literalParameter>
									<objectListParameter name=""validationMessages"">
										<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(3)"">
											<object>
												<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=FirstName;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">FirstName</literalParameter>
														<objectListParameter name=""rules"">
															<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																<object>
																	<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=First Name is required."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""className"">RequiredRule</literalParameter>
																			<literalParameter name=""message"">First Name is required.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=LastName;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">LastName</literalParameter>
														<objectListParameter name=""rules"">
															<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																<object>
																	<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Last Name is required."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""className"">RequiredRule</literalParameter>
																			<literalParameter name=""message"">Last Name is required.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=HireDate;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">HireDate</literalParameter>
														<objectListParameter name=""rules"">
															<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																<object>
																	<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Hire Date is required."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""className"">RequiredRule</literalParameter>
																			<literalParameter name=""message"">Hire Date is required.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
										</objectList>
									</objectListParameter>
									<objectListParameter name=""fieldSettings"">
										<objectList objectType=""Contoso.Forms.Parameters.DataForm.FormItemSettingsParameters"" listType=""GenericList"" visibleText=""fieldSettings: Count(6)"">
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=FirstName;title=First Name;placeholder=First Name (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">FirstName</literalParameter>
														<literalParameter name=""title"">First Name</literalParameter>
														<literalParameter name=""placeholder"">First Name (required)</literalParameter>
														<literalParameter name=""stringFormat"">{0}</literalParameter>
														<objectParameter name=""type"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.String"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">System.String</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""validationSetting"">
															<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																<genericArguments />
																<parameters>
																	<objectParameter name=""defaultValue"">
																		<function name=""Cast"" visibleText=""Cast: From="">
																			<genericArguments>
																				<literalParameter genericArgumentName=""From"">
																					<literalType>String</literalType>
																					<control>SingleLineTextBox</control>
																					<useForEquality>true</useForEquality>
																					<useForHashCode>false</useForHashCode>
																					<useForToString>true</useForToString>
																					<propertySource />
																					<propertySourceParameter />
																					<defaultValue />
																					<domain />
																				</literalParameter>
																				<objectParameter genericArgumentName=""To"">
																					<objectType>System.Object</objectType>
																					<useForEquality>false</useForEquality>
																					<useForHashCode>false</useForHashCode>
																					<useForToString>true</useForToString>
																				</objectParameter>
																			</genericArguments>
																			<parameters>
																				<literalParameter name=""From"" />
																			</parameters>
																		</function>
																	</objectParameter>
																	<objectListParameter name=""validators"">
																		<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorDefinitionParameters"" listType=""GenericList"" visibleText=""validators: Count(1)"">
																			<object>
																				<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RequiredRule;functionName=Check"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">RequiredRule</literalParameter>
																						<literalParameter name=""functionName"">Check</literalParameter>
																					</parameters>
																				</constructor>
																			</object>
																		</objectList>
																	</objectListParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectParameter name=""textTemplate"">
															<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=TextTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">TextTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=LastName;title=Last Name;placeholder=Last Name (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">LastName</literalParameter>
														<literalParameter name=""title"">Last Name</literalParameter>
														<literalParameter name=""placeholder"">Last Name (required)</literalParameter>
														<literalParameter name=""stringFormat"">{0}</literalParameter>
														<objectParameter name=""type"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.String"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">System.String</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""validationSetting"">
															<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																<genericArguments />
																<parameters>
																	<objectParameter name=""defaultValue"">
																		<function name=""Cast"" visibleText=""Cast: From="">
																			<genericArguments>
																				<literalParameter genericArgumentName=""From"">
																					<literalType>String</literalType>
																					<control>SingleLineTextBox</control>
																					<useForEquality>true</useForEquality>
																					<useForHashCode>false</useForHashCode>
																					<useForToString>true</useForToString>
																					<propertySource />
																					<propertySourceParameter />
																					<defaultValue />
																					<domain />
																				</literalParameter>
																				<objectParameter genericArgumentName=""To"">
																					<objectType>System.Object</objectType>
																					<useForEquality>false</useForEquality>
																					<useForHashCode>false</useForHashCode>
																					<useForToString>true</useForToString>
																				</objectParameter>
																			</genericArguments>
																			<parameters>
																				<literalParameter name=""From"" />
																			</parameters>
																		</function>
																	</objectParameter>
																	<objectListParameter name=""validators"">
																		<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorDefinitionParameters"" listType=""GenericList"" visibleText=""validators: Count(1)"">
																			<object>
																				<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RequiredRule;functionName=Check"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">RequiredRule</literalParameter>
																						<literalParameter name=""functionName"">Check</literalParameter>
																					</parameters>
																				</constructor>
																			</object>
																		</objectList>
																	</objectListParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectParameter name=""textTemplate"">
															<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=TextTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">TextTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=HireDate;title=Hire Date;placeholder=Hire Date (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">HireDate</literalParameter>
														<literalParameter name=""title"">Hire Date</literalParameter>
														<literalParameter name=""placeholder"">Hire Date (required)</literalParameter>
														<literalParameter name=""stringFormat"">{0}</literalParameter>
														<objectParameter name=""type"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.DateTime"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">System.DateTime</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""validationSetting"">
															<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																<genericArguments />
																<parameters>
																	<objectParameter name=""defaultValue"">
																		<function name=""Cast"" visibleText=""Cast: From=DateTime_yy_mm_dd: year=1900;month=1;day=1"">
																			<genericArguments>
																				<literalParameter genericArgumentName=""From"">
																					<literalType>DateTime</literalType>
																					<control>SingleLineTextBox</control>
																					<useForEquality>true</useForEquality>
																					<useForHashCode>false</useForHashCode>
																					<useForToString>true</useForToString>
																					<propertySource />
																					<propertySourceParameter />
																					<defaultValue />
																					<domain />
																				</literalParameter>
																				<objectParameter genericArgumentName=""To"">
																					<objectType>System.Object</objectType>
																					<useForEquality>false</useForEquality>
																					<useForHashCode>false</useForHashCode>
																					<useForToString>true</useForToString>
																				</objectParameter>
																			</genericArguments>
																			<parameters>
																				<literalParameter name=""From"">
																					<constructor name=""DateTime_yy_mm_dd"" visibleText=""DateTime_yy_mm_dd: year=1900;month=1;day=1"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""year"">1900</literalParameter>
																							<literalParameter name=""month"">1</literalParameter>
																							<literalParameter name=""day"">1</literalParameter>
																						</parameters>
																					</constructor>
																				</literalParameter>
																			</parameters>
																		</function>
																	</objectParameter>
																	<objectListParameter name=""validators"">
																		<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorDefinitionParameters"" listType=""GenericList"" visibleText=""validators: Count(1)"">
																			<object>
																				<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RequiredRule;functionName=Check"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">RequiredRule</literalParameter>
																						<literalParameter name=""functionName"">Check</literalParameter>
																					</parameters>
																				</constructor>
																			</object>
																		</objectList>
																	</objectListParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectParameter name=""textTemplate"">
															<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=DateTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">DateTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormGroupSettingsParameters"" visibleText=""FormGroupSettingsParameters: field=OfficeAssignment;title=Title;validFormControlText=(OfficeAssignment);invalidFormControlText=(Invalid OfficeAssignment);placeholder=Empty String;Type: modelType;FormGroupTemplateParameters: formGroupTemplate;GenericListOfFormItemSettingsParameters: fieldSettings;GenericListOfValidationMessageParameters: validationMessages;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">OfficeAssignment</literalParameter>
														<literalParameter name=""title"">Title</literalParameter>
														<literalParameter name=""validFormControlText"">(OfficeAssignment)</literalParameter>
														<literalParameter name=""invalidFormControlText"">(Invalid OfficeAssignment)</literalParameter>
														<literalParameter name=""placeholder"" />
														<objectParameter name=""modelType"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.OfficeAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.OfficeAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""formGroupTemplate"">
															<constructor name=""FormGroupTemplateParameters"" visibleText=""FormGroupTemplateParameters: templateName=PopupFormGroupTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">PopupFormGroupTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectListParameter name=""fieldSettings"">
															<objectList objectType=""Contoso.Forms.Parameters.DataForm.FormItemSettingsParameters"" listType=""GenericList"" visibleText=""fieldSettings: Count(1)"">
																<object>
																	<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=Location;title=Location;placeholder=Location;stringFormat={0};Type: type;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.OfficeAssignmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">Location</literalParameter>
																			<literalParameter name=""title"">Location</literalParameter>
																			<literalParameter name=""placeholder"">Location</literalParameter>
																			<literalParameter name=""stringFormat"">{0}</literalParameter>
																			<objectParameter name=""type"">
																				<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.String"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""assemblyQualifiedTypeName"">System.String</literalParameter>
																					</parameters>
																				</function>
																			</objectParameter>
																			<objectParameter name=""textTemplate"">
																				<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=TextTemplate"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""templateName"">TextTemplate</literalParameter>
																					</parameters>
																				</constructor>
																			</objectParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.OfficeAssignmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<objectListParameter name=""validationMessages"">
															<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(0)"" />
														</objectListParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""MultiSelectFormControlSettingsParameters"" visibleText=""MultiSelectFormControlSettingsParameters: field=Courses;GenericListOfString: keyFields;title=Courses;placeholder=(Courses);stringFormat={0};Type: type;MultiSelectTemplateParameters: multiSelectTemplate;FieldValidationSettingsParameters: validationSetting;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">Courses</literalParameter>
														<literalListParameter name=""keyFields"">
															<literalList literalType=""String"" listType=""GenericList"" visibleText=""keyFields: Count(1)"">
																<literal>CourseID</literal>
															</literalList>
														</literalListParameter>
														<literalParameter name=""title"">Courses</literalParameter>
														<literalParameter name=""placeholder"">(Courses)</literalParameter>
														<literalParameter name=""stringFormat"">{0}</literalParameter>
														<objectParameter name=""type"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Collections.Generic.ICollection`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">System.Collections.Generic.ICollection`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""multiSelectTemplate"">
															<constructor name=""MultiSelectTemplateParameters"" visibleText=""MultiSelectTemplateParameters: templateName=MultiSelectTemplate;placeholderText=(Courses);textField=CourseTitle;valueField=CourseID;Type: modelType;loadingIndicatorText=Loading ...;SelectorLambdaOperatorParameters: textAndValueSelector;RequestDetailsParameters: requestDetails;fieldTypeSource=Contoso.Domain.Entities.CourseAssignmentModel"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">MultiSelectTemplate</literalParameter>
																	<literalParameter name=""placeholderText"">(Courses)</literalParameter>
																	<literalParameter name=""textField"">CourseTitle</literalParameter>
																	<literalParameter name=""valueField"">CourseID</literalParameter>
																	<objectParameter name=""modelType"">
																		<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																			<genericArguments />
																			<parameters>
																				<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																			</parameters>
																		</function>
																	</objectParameter>
																	<literalParameter name=""loadingIndicatorText"">Loading ...</literalParameter>
																	<objectParameter name=""textAndValueSelector"">
																		<constructor name=""SelectorLambdaOperatorParameters"" visibleText=""SelectorLambdaOperatorParameters: IExpressionParameter: selector;Type: sourceElementType;parameterName=$it;Type: bodyType"">
																			<genericArguments />
																			<parameters>
																				<objectParameter name=""selector"">
																					<constructor name=""SelectOperatorParameters"" visibleText=""SelectOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;selectorParameterName=s"">
																						<genericArguments />
																						<parameters>
																							<objectParameter name=""sourceOperand"">
																								<constructor name=""OrderByOperatorParameters"" visibleText=""OrderByOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;ListSortDirection: sortDirection;selectorParameterName=d"">
																									<genericArguments />
																									<parameters>
																										<objectParameter name=""sourceOperand"">
																											<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""parameterName"">$it</literalParameter>
																												</parameters>
																											</constructor>
																										</objectParameter>
																										<objectParameter name=""selectorBody"">
																											<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Title;IExpressionParameter: sourceOperand"">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""memberFullName"">Title</literalParameter>
																													<objectParameter name=""sourceOperand"">
																														<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=d"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""parameterName"">d</literalParameter>
																															</parameters>
																														</constructor>
																													</objectParameter>
																												</parameters>
																											</constructor>
																										</objectParameter>
																										<objectParameter name=""sortDirection"">
																											<variable name=""ListSortDirection_Ascending"" visibleText=""ListSortDirection_Ascending"" />
																										</objectParameter>
																										<literalParameter name=""selectorParameterName"">d</literalParameter>
																									</parameters>
																								</constructor>
																							</objectParameter>
																							<objectParameter name=""selectorBody"">
																								<constructor name=""MemberInitOperatorParameters"" visibleText=""MemberInitOperatorParameters: Type: newType"">
																									<genericArguments />
																									<parameters>
																										<objectListParameter name=""memberBindings"">
																											<objectList objectType=""Contoso.Parameters.Expressions.MemberBindingItem"" listType=""GenericList"" visibleText=""memberBindings: Count(2)"">
																												<object>
																													<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=CourseID;IExpressionParameter: selector"">
																														<genericArguments />
																														<parameters>
																															<literalParameter name=""property"">CourseID</literalParameter>
																															<objectParameter name=""selector"">
																																<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=CourseID;IExpressionParameter: sourceOperand"">
																																	<genericArguments />
																																	<parameters>
																																		<literalParameter name=""memberFullName"">CourseID</literalParameter>
																																		<objectParameter name=""sourceOperand"">
																																			<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=s"">
																																				<genericArguments />
																																				<parameters>
																																					<literalParameter name=""parameterName"">s</literalParameter>
																																				</parameters>
																																			</constructor>
																																		</objectParameter>
																																	</parameters>
																																</constructor>
																															</objectParameter>
																														</parameters>
																													</constructor>
																												</object>
																												<object>
																													<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=CourseTitle;IExpressionParameter: selector"">
																														<genericArguments />
																														<parameters>
																															<literalParameter name=""property"">CourseTitle</literalParameter>
																															<objectParameter name=""selector"">
																																<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Title;IExpressionParameter: sourceOperand"">
																																	<genericArguments />
																																	<parameters>
																																		<literalParameter name=""memberFullName"">Title</literalParameter>
																																		<objectParameter name=""sourceOperand"">
																																			<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=s"">
																																				<genericArguments />
																																				<parameters>
																																					<literalParameter name=""parameterName"">s</literalParameter>
																																				</parameters>
																																			</constructor>
																																		</objectParameter>
																																	</parameters>
																																</constructor>
																															</objectParameter>
																														</parameters>
																													</constructor>
																												</object>
																											</objectList>
																										</objectListParameter>
																										<objectParameter name=""newType"">
																											<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																												</parameters>
																											</function>
																										</objectParameter>
																									</parameters>
																								</constructor>
																							</objectParameter>
																							<literalParameter name=""selectorParameterName"">s</literalParameter>
																						</parameters>
																					</constructor>
																				</objectParameter>
																				<objectParameter name=""sourceElementType"">
																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																						</parameters>
																					</function>
																				</objectParameter>
																				<literalParameter name=""parameterName"">$it</literalParameter>
																				<objectParameter name=""bodyType"">
																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																						</parameters>
																					</function>
																				</objectParameter>
																			</parameters>
																		</constructor>
																	</objectParameter>
																	<objectParameter name=""requestDetails"">
																		<constructor name=""RequestDetailsParameters"" visibleText=""RequestDetailsParameters: Type: modelType;Type: dataType;Type: modelReturnType;Type: dataReturnType;dataSourceUrl=api/List/GetList"">
																			<genericArguments />
																			<parameters>
																				<objectParameter name=""modelType"">
																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																						</parameters>
																					</function>
																				</objectParameter>
																				<objectParameter name=""dataType"">
																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Data.Entities.Course, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Data.Entities.Course, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																						</parameters>
																					</function>
																				</objectParameter>
																				<objectParameter name=""modelReturnType"">
																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																						</parameters>
																					</function>
																				</objectParameter>
																				<objectParameter name=""dataReturnType"">
																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Data.Entities.CourseAssignment, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Data.Entities.CourseAssignment, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																						</parameters>
																					</function>
																				</objectParameter>
																				<literalParameter name=""dataSourceUrl"">api/List/GetList</literalParameter>
																			</parameters>
																		</constructor>
																	</objectParameter>
																	<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseAssignmentModel</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectParameter name=""validationSetting"">
															<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																<genericArguments />
																<parameters>
																	<objectParameter name=""defaultValue"">
																		<function name=""Add"" visibleText=""1 Add 1"">
																			<genericArguments />
																			<parameters>
																				<literalParameter name=""value1"">1</literalParameter>
																				<literalParameter name=""value2"">1</literalParameter>
																			</parameters>
																		</function>
																	</objectParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=ID;title=Title;placeholder=(Title) required;stringFormat={0};Type: type;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">ID</literalParameter>
														<literalParameter name=""title"">Title</literalParameter>
														<literalParameter name=""placeholder"">(Title) required</literalParameter>
														<literalParameter name=""stringFormat"">{0}</literalParameter>
														<objectParameter name=""type"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Int32"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">System.Int32</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""textTemplate"">
															<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=HiddenTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">HiddenTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
													</parameters>
												</constructor>
											</object>
										</objectList>
									</objectListParameter>
									<objectParameter name=""formType"">
										<variable name=""FormType_Add"" visibleText=""FormType_Add"" />
									</objectParameter>
									<objectParameter name=""modelType"">
										<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
											<genericArguments />
											<parameters>
												<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
											</parameters>
										</function>
									</objectParameter>
									<objectParameter name=""requestDetails"">
										<constructor name=""FormRequestDetailsParameters"" visibleText=""FormRequestDetailsParameters: getUrl=api/Entity/GetEntity;addUrl=api/Instructor/Save;updateUrl=api/Instructor/Save;deleteUrl=api/Instructor/Delete;Type: modelType;Type: dataType"">
											<genericArguments />
											<parameters>
												<literalParameter name=""getUrl"">api/Entity/GetEntity</literalParameter>
												<literalParameter name=""addUrl"">api/Instructor/Save</literalParameter>
												<literalParameter name=""updateUrl"">api/Instructor/Save</literalParameter>
												<literalParameter name=""deleteUrl"">api/Instructor/Delete</literalParameter>
												<objectParameter name=""modelType"">
													<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
														<genericArguments />
														<parameters>
															<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
														</parameters>
													</function>
												</objectParameter>
												<objectParameter name=""dataType"">
													<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Data.Entities.Instructor, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
														<genericArguments />
														<parameters>
															<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Data.Entities.Instructor, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
														</parameters>
													</function>
												</objectParameter>
											</parameters>
										</constructor>
									</objectParameter>
								</parameters>
							</constructor>";

        string button1Xml = @"<constructor name=""MultiSelectFormControlSettingsParameters"" visibleText=""MultiSelectFormControlSettingsParameters: field=Courses;GenericListOfString: keyFields;title=Courses;placeholder=(Courses);stringFormat={0};Type: type;MultiSelectTemplateParameters: multiSelectTemplate;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
								<genericArguments />
								<parameters>
									<literalParameter name=""field"">Courses</literalParameter>
									<literalListParameter name=""keyFields"">
										<literalList literalType=""String"" listType=""GenericList"" visibleText=""keyFields: Count(1)"">
											<literal>CourseID</literal>
										</literalList>
									</literalListParameter>
									<literalParameter name=""title"">Courses</literalParameter>
									<literalParameter name=""placeholder"">(Courses)</literalParameter>
									<literalParameter name=""stringFormat"">{0}</literalParameter>
									<objectParameter name=""type"">
										<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Collections.Generic.ICollection`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"">
											<genericArguments />
											<parameters>
												<literalParameter name=""assemblyQualifiedTypeName"">System.Collections.Generic.ICollection`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e</literalParameter>
											</parameters>
										</function>
									</objectParameter>
									<objectParameter name=""multiSelectTemplate"">
										<constructor name=""MultiSelectTemplateParameters"" visibleText=""MultiSelectTemplateParameters: templateName=MultiSelectTemplate;placeholderText=(Courses);textField=CourseTitle;valueField=CourseID;Type: modelType;loadingIndicatorText=Loading ...;SelectorLambdaOperatorParameters: textAndValueSelector;RequestDetailsParameters: requestDetails;fieldTypeSource=Contoso.Domain.Entities.CourseAssignmentModel"">
											<genericArguments />
											<parameters>
												<literalParameter name=""templateName"">MultiSelectTemplate</literalParameter>
												<literalParameter name=""placeholderText"">(Courses)</literalParameter>
												<literalParameter name=""textField"">CourseTitle</literalParameter>
												<literalParameter name=""valueField"">CourseID</literalParameter>
												<objectParameter name=""modelType"">
													<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
														<genericArguments />
														<parameters>
															<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
														</parameters>
													</function>
												</objectParameter>
												<literalParameter name=""loadingIndicatorText"">Loading ...</literalParameter>
												<objectParameter name=""textAndValueSelector"">
													<constructor name=""SelectorLambdaOperatorParameters"" visibleText=""SelectorLambdaOperatorParameters: IExpressionParameter: selector;Type: sourceElementType;parameterName=$it;Type: bodyType"">
														<genericArguments />
														<parameters>
															<objectParameter name=""selector"">
																<constructor name=""SelectOperatorParameters"" visibleText=""SelectOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;selectorParameterName=s"">
																	<genericArguments />
																	<parameters>
																		<objectParameter name=""sourceOperand"">
																			<constructor name=""OrderByOperatorParameters"" visibleText=""OrderByOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;ListSortDirection: sortDirection;selectorParameterName=d"">
																				<genericArguments />
																				<parameters>
																					<objectParameter name=""sourceOperand"">
																						<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""parameterName"">$it</literalParameter>
																							</parameters>
																						</constructor>
																					</objectParameter>
																					<objectParameter name=""selectorBody"">
																						<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Title;IExpressionParameter: sourceOperand"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""memberFullName"">Title</literalParameter>
																								<objectParameter name=""sourceOperand"">
																									<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=d"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""parameterName"">d</literalParameter>
																										</parameters>
																									</constructor>
																								</objectParameter>
																							</parameters>
																						</constructor>
																					</objectParameter>
																					<objectParameter name=""sortDirection"">
																						<variable name=""ListSortDirection_Ascending"" visibleText=""ListSortDirection_Ascending"" />
																					</objectParameter>
																					<literalParameter name=""selectorParameterName"">d</literalParameter>
																				</parameters>
																			</constructor>
																		</objectParameter>
																		<objectParameter name=""selectorBody"">
																			<constructor name=""MemberInitOperatorParameters"" visibleText=""MemberInitOperatorParameters: Type: newType"">
																				<genericArguments />
																				<parameters>
																					<objectListParameter name=""memberBindings"">
																						<objectList objectType=""Contoso.Parameters.Expressions.MemberBindingItem"" listType=""GenericList"" visibleText=""memberBindings: Count(2)"">
																							<object>
																								<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=CourseID;IExpressionParameter: selector"">
																									<genericArguments />
																									<parameters>
																										<literalParameter name=""property"">CourseID</literalParameter>
																										<objectParameter name=""selector"">
																											<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=CourseID;IExpressionParameter: sourceOperand"">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""memberFullName"">CourseID</literalParameter>
																													<objectParameter name=""sourceOperand"">
																														<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=s"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""parameterName"">s</literalParameter>
																															</parameters>
																														</constructor>
																													</objectParameter>
																												</parameters>
																											</constructor>
																										</objectParameter>
																									</parameters>
																								</constructor>
																							</object>
																							<object>
																								<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=CourseTitle;IExpressionParameter: selector"">
																									<genericArguments />
																									<parameters>
																										<literalParameter name=""property"">CourseTitle</literalParameter>
																										<objectParameter name=""selector"">
																											<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Title;IExpressionParameter: sourceOperand"">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""memberFullName"">Title</literalParameter>
																													<objectParameter name=""sourceOperand"">
																														<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=s"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""parameterName"">s</literalParameter>
																															</parameters>
																														</constructor>
																													</objectParameter>
																												</parameters>
																											</constructor>
																										</objectParameter>
																									</parameters>
																								</constructor>
																							</object>
																						</objectList>
																					</objectListParameter>
																					<objectParameter name=""newType"">
																						<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																							</parameters>
																						</function>
																					</objectParameter>
																				</parameters>
																			</constructor>
																		</objectParameter>
																		<literalParameter name=""selectorParameterName"">s</literalParameter>
																	</parameters>
																</constructor>
															</objectParameter>
															<objectParameter name=""sourceElementType"">
																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																	</parameters>
																</function>
															</objectParameter>
															<literalParameter name=""parameterName"">$it</literalParameter>
															<objectParameter name=""bodyType"">
																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																	</parameters>
																</function>
															</objectParameter>
														</parameters>
													</constructor>
												</objectParameter>
												<objectParameter name=""requestDetails"">
													<constructor name=""RequestDetailsParameters"" visibleText=""RequestDetailsParameters: Type: modelType;Type: dataType;Type: modelReturnType;Type: dataReturnType;dataSourceUrl=api/List/GetList"">
														<genericArguments />
														<parameters>
															<objectParameter name=""modelType"">
																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																	</parameters>
																</function>
															</objectParameter>
															<objectParameter name=""dataType"">
																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Data.Entities.Course, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Data.Entities.Course, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																	</parameters>
																</function>
															</objectParameter>
															<objectParameter name=""modelReturnType"">
																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.CourseAssignmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																	</parameters>
																</function>
															</objectParameter>
															<objectParameter name=""dataReturnType"">
																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Data.Entities.CourseAssignment, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Data.Entities.CourseAssignment, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																	</parameters>
																</function>
															</objectParameter>
															<literalParameter name=""dataSourceUrl"">api/List/GetList</literalParameter>
														</parameters>
													</constructor>
												</objectParameter>
												<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseAssignmentModel</literalParameter>
											</parameters>
										</constructor>
									</objectParameter>
									<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
								</parameters>
							</constructor>";

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

        private void Navigate(Control newControl)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            ((ISupportInitialize)radPanelFields).BeginInit();
            radPanelFields.SuspendLayout();

            ClearFieldControls();
            newControl.Dock = DockStyle.Fill;
            newControl.Location = new Point(0, 0);
            radPanelFields.Controls.Add(newControl);

            ((ISupportInitialize)radPanelFields).EndInit();
            radPanelFields.ResumeLayout(false);
            radPanelFields.PerformLayout();

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearFieldControls()
            {
                foreach (Control control in radPanelFields.Controls)
                {
                    control.Visible = false;
					if (!control.IsDisposed)
                        control.Dispose();
                }

                radPanelFields.Controls.Clear();
            }
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
