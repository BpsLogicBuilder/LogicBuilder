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
            
			_treeViewXmlDocumentHelper.LoadXmlDocument(button1Xml);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            Initialize();
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{0C223B16-511C-4019-A272-7AB8CEC6E297}");

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

		public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
        {
        }

        public void RequestDocumentUpdate() => _dataGraphEditingFormEventsHelper.RequestDocumentUpdate();

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

        string button2Xml = @"<constructor name=""EditFormSettingsParameters"" visibleText=""EditFormSettingsParameters: title=Course;displayField=title;RequestDetailsParameters: requestDetails;GenericListOfValidationMessageParameters: validationMessages;GenericListOfFormItemSettingParameters: fieldSettings;FilterGroupParameters: filterGroup;modelType=Contoso.Domain.Entities.CourseModel"">
								<genericArguments />
								<parameters>
									<literalParameter name=""title"">Course</literalParameter>
									<literalParameter name=""displayField"">title</literalParameter>
									<objectParameter name=""requestDetails"">
										<constructor name=""RequestDetailsParameters"" visibleText=""RequestDetailsParameters: modelType=Contoso.Domain.Entities.CourseModel;dataType=Contoso.Data.Entities.Course;dataSourceUrl=/api/Generic/GetData;getUrl=/api/Generic/GetSingle;addUrl=/api/Generic/Add;updateUrl=/api/Generic/Update;deleteUrl=/api/Generic/Delete"">
											<genericArguments />
											<parameters>
												<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
												<literalParameter name=""dataType"">Contoso.Data.Entities.Course</literalParameter>
												<literalParameter name=""dataSourceUrl"">/api/Generic/GetData</literalParameter>
												<literalParameter name=""getUrl"">/api/Generic/GetSingle</literalParameter>
												<literalParameter name=""addUrl"">/api/Generic/Add</literalParameter>
												<literalParameter name=""updateUrl"">/api/Generic/Update</literalParameter>
												<literalParameter name=""deleteUrl"">/api/Generic/Delete</literalParameter>
											</parameters>
										</constructor>
									</objectParameter>
									<objectListParameter name=""validationMessages"">
										<objectList objectType=""Contoso.Forms.Parameters.Common.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(3)"">
											<object>
												<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=credits;GenericListOfValidationMethodParameters: methods;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">credits</literalParameter>
														<objectListParameter name=""methods"">
															<objectList objectType=""Contoso.Forms.Parameters.Common.ValidationMethodParameters"" listType=""GenericList"" visibleText=""methods: Count(4)"">
																<object>
																	<constructor name=""ValidationMethodParameters"" visibleText=""ValidationMethodParameters: method=required;message=Credits field is required."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""method"">required</literalParameter>
																			<literalParameter name=""message"">Credits field is required.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""ValidationMethodParameters"" visibleText=""ValidationMethodParameters: method=mustBeANumber;message=Credits must be between 0 and 5 inclusive."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""method"">mustBeANumber</literalParameter>
																			<literalParameter name=""message"">Credits must be between 0 and 5 inclusive.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""ValidationMethodParameters"" visibleText=""ValidationMethodParameters: method=max;message=Credits must be between 0 and 5 inclusive."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""method"">max</literalParameter>
																			<literalParameter name=""message"">Credits must be between 0 and 5 inclusive.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""ValidationMethodParameters"" visibleText=""ValidationMethodParameters: method=min;message=Credits must be between 0 and 5 inclusive."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""method"">min</literalParameter>
																			<literalParameter name=""message"">Credits must be between 0 and 5 inclusive.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=departmentID;GenericListOfValidationMethodParameters: methods;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">departmentID</literalParameter>
														<objectListParameter name=""methods"">
															<objectList objectType=""Contoso.Forms.Parameters.Common.ValidationMethodParameters"" listType=""GenericList"" visibleText=""methods: Count(1)"">
																<object>
																	<constructor name=""ValidationMethodParameters"" visibleText=""ValidationMethodParameters: method=required;message=Department is required."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""method"">required</literalParameter>
																			<literalParameter name=""message"">Department is required.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=title;GenericListOfValidationMethodParameters: methods;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">title</literalParameter>
														<objectListParameter name=""methods"">
															<objectList objectType=""Contoso.Forms.Parameters.Common.ValidationMethodParameters"" listType=""GenericList"" visibleText=""methods: Count(1)"">
																<object>
																	<constructor name=""ValidationMethodParameters"" visibleText=""ValidationMethodParameters: method=required;message=Title is required."">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""method"">required</literalParameter>
																			<literalParameter name=""message"">Title is required.</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
										</objectList>
									</objectListParameter>
									<objectListParameter name=""fieldSettings"">
										<objectList objectType=""Contoso.Forms.Parameters.Common.FormItemSettingParameters"" listType=""GenericList"" visibleText=""fieldSettings: Count(4)"">
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=courseID;domElementId=courseIDid;title=Number;placeHolder=Number (required);type=numeric;TextFieldTemplateParameters: textTemplate;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">courseID</literalParameter>
														<literalParameter name=""domElementId"">courseIDid</literalParameter>
														<literalParameter name=""title"">Number</literalParameter>
														<literalParameter name=""placeHolder"">Number (required)</literalParameter>
														<literalParameter name=""type"">numeric</literalParameter>
														<objectParameter name=""textTemplate"">
															<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=labelTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">labelTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=credits;domElementId=creditsid;title=Credits;placeHolder=Credits (required);type=numeric;FormValidationSettingParameters: validationSetting;DropDownTemplateParameters: dropDownTemplate;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">credits</literalParameter>
														<literalParameter name=""domElementId"">creditsid</literalParameter>
														<literalParameter name=""title"">Credits</literalParameter>
														<literalParameter name=""placeHolder"">Credits (required)</literalParameter>
														<literalParameter name=""type"">numeric</literalParameter>
														<objectParameter name=""validationSetting"">
															<constructor name=""FormValidationSettingParameters"" visibleText=""FormValidationSettingParameters: GenericListOfValidatorDescriptionParameters: validators"">
																<genericArguments />
																<parameters>
																	<objectListParameter name=""validators"">
																		<objectList objectType=""Contoso.Forms.Parameters.Common.ValidatorDescriptionParameters"" listType=""GenericList"" visibleText=""validators: Count(4)"">
																			<object>
																				<constructor name=""ValidatorDescriptionParameters"" visibleText=""ValidatorDescriptionParameters: className=Validators;functionName=required"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">Validators</literalParameter>
																						<literalParameter name=""functionName"">required</literalParameter>
																					</parameters>
																				</constructor>
																			</object>
																			<object>
																				<constructor name=""ValidatorDescriptionParameters"" visibleText=""ValidatorDescriptionParameters: className=NumberValidators;functionName=mustBeANumber"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">NumberValidators</literalParameter>
																						<literalParameter name=""functionName"">mustBeANumber</literalParameter>
																					</parameters>
																				</constructor>
																			</object>
																			<object>
																				<constructor name=""ValidatorDescriptionParameters"" visibleText=""ValidatorDescriptionParameters: className=Validators;functionName=max;GenericListOfValidatorArgumentParameters: arguments"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">Validators</literalParameter>
																						<literalParameter name=""functionName"">max</literalParameter>
																						<objectListParameter name=""arguments"">
																							<objectList objectType=""Contoso.Forms.Parameters.Common.ValidatorArgumentParameters"" listType=""GenericList"" visibleText=""arguments: Count(1)"">
																								<object>
																									<constructor name=""ValidatorArgumentParameters"" visibleText=""ValidatorArgumentParameters: name=max;Object: value"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""name"">max</literalParameter>
																											<objectParameter name=""value"">
																												<function name=""Cast"" visibleText=""Cast: From=5"">
																													<genericArguments>
																														<literalParameter genericArgumentName=""From"">
																															<literalType>Integer</literalType>
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
																														<literalParameter name=""From"">5</literalParameter>
																													</parameters>
																												</function>
																											</objectParameter>
																										</parameters>
																									</constructor>
																								</object>
																							</objectList>
																						</objectListParameter>
																					</parameters>
																				</constructor>
																			</object>
																			<object>
																				<constructor name=""ValidatorDescriptionParameters"" visibleText=""ValidatorDescriptionParameters: className=Validators;functionName=min;GenericListOfValidatorArgumentParameters: arguments"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">Validators</literalParameter>
																						<literalParameter name=""functionName"">min</literalParameter>
																						<objectListParameter name=""arguments"">
																							<objectList objectType=""Contoso.Forms.Parameters.Common.ValidatorArgumentParameters"" listType=""GenericList"" visibleText=""arguments: Count(1)"">
																								<object>
																									<constructor name=""ValidatorArgumentParameters"" visibleText=""ValidatorArgumentParameters: name=max;Object: value"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""name"">max</literalParameter>
																											<objectParameter name=""value"">
																												<function name=""Cast"" visibleText=""Cast: From=0"">
																													<genericArguments>
																														<literalParameter genericArgumentName=""From"">
																															<literalType>Integer</literalType>
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
																														<literalParameter name=""From"">0</literalParameter>
																													</parameters>
																												</function>
																											</objectParameter>
																										</parameters>
																									</constructor>
																								</object>
																							</objectList>
																						</objectListParameter>
																					</parameters>
																				</constructor>
																			</object>
																		</objectList>
																	</objectListParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectParameter name=""dropDownTemplate"">
															<constructor name=""DropDownTemplateParameters"" visibleText=""DropDownTemplateParameters: templateName=dropDownTemplate;placeHolderText=Select One ...;textField=text;valueField=numericValue;RequestDetailsParameters: requestDetails;DataRequestStateParameters: state;modelType=Contoso.Domain.Entities.LookUpsModel"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">dropDownTemplate</literalParameter>
																	<literalParameter name=""placeHolderText"">Select One ...</literalParameter>
																	<literalParameter name=""textField"">text</literalParameter>
																	<literalParameter name=""valueField"">numericValue</literalParameter>
																	<objectParameter name=""requestDetails"">
																		<constructor name=""RequestDetailsParameters"" visibleText=""RequestDetailsParameters: modelType=Contoso.Domain.Entities.LookUpsModel;dataType=Contoso.Data.Entities.LookUps;dataSourceUrl=/api/Generic/GetFilterData;getUrl=/api/Generic/GetSingle;addUrl=/api/Generic/Add;updateUrl=/api/Generic/Update;deleteUrl=/api/Generic/Delete;GenericListOfSelectParameters: selects"">
																			<genericArguments />
																			<parameters>
																				<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																				<literalParameter name=""dataType"">Contoso.Data.Entities.LookUps</literalParameter>
																				<literalParameter name=""dataSourceUrl"">/api/Generic/GetFilterData</literalParameter>
																				<literalParameter name=""getUrl"">/api/Generic/GetSingle</literalParameter>
																				<literalParameter name=""addUrl"">/api/Generic/Add</literalParameter>
																				<literalParameter name=""updateUrl"">/api/Generic/Update</literalParameter>
																				<literalParameter name=""deleteUrl"">/api/Generic/Delete</literalParameter>
																				<objectListParameter name=""selects"">
																					<objectList objectType=""Contoso.Forms.Parameters.Common.SelectParameters"" listType=""GenericList"" visibleText=""selects: Count(3)"">
																						<object>
																							<constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=numericValue;sourceMember=numericValue;modelType=Contoso.Domain.Entities.LookUpsModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""fieldName"">numericValue</literalParameter>
																									<literalParameter name=""sourceMember"">numericValue</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																						<object>
																							<constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=text;sourceMember=text;modelType=Contoso.Domain.Entities.LookUpsModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""fieldName"">text</literalParameter>
																									<literalParameter name=""sourceMember"">text</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																						<object>
																							<constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=listName;sourceMember=listName;modelType=Contoso.Domain.Entities.LookUpsModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""fieldName"">listName</literalParameter>
																									<literalParameter name=""sourceMember"">listName</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																					</objectList>
																				</objectListParameter>
																			</parameters>
																		</constructor>
																	</objectParameter>
																	<objectParameter name=""state"">
																		<constructor name=""DataRequestStateParameters"" visibleText=""DataRequestStateParameters: GenericListOfSortParameters: sort;FilterGroupParameters: filterGroup"">
																			<genericArguments />
																			<parameters>
																				<objectListParameter name=""sort"">
																					<objectList objectType=""Contoso.Forms.Parameters.Common.SortParameters"" listType=""GenericList"" visibleText=""sort: Count(1)"">
																						<object>
																							<constructor name=""SortParameters"" visibleText=""SortParameters: field=numericValue;dir=desc;modelType=Contoso.Domain.Entities.LookUpsModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""field"">numericValue</literalParameter>
																									<literalParameter name=""dir"">desc</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																					</objectList>
																				</objectListParameter>
																				<objectParameter name=""filterGroup"">
																					<constructor name=""FilterGroupParameters"" visibleText=""FilterGroupParameters: logic=and;GenericListOfFilterDefinitionParameters: filters"">
																						<genericArguments />
																						<parameters>
																							<literalParameter name=""logic"">and</literalParameter>
																							<objectListParameter name=""filters"">
																								<objectList objectType=""Contoso.Forms.Parameters.Common.FilterDefinitionParameters"" listType=""GenericList"" visibleText=""filters: Count(1)"">
																									<object>
																										<constructor name=""FilterDefinitionParameters"" visibleText=""FilterDefinitionParameters: field=listName;oper=eq;Object: value;modelType=Contoso.Domain.Entities.LookUpsModel"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""field"">listName</literalParameter>
																												<literalParameter name=""oper"">eq</literalParameter>
																												<objectParameter name=""value"">
																													<function name=""Cast"" visibleText=""Cast: From=credits"">
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
																															<literalParameter name=""From"">credits</literalParameter>
																														</parameters>
																													</function>
																												</objectParameter>
																												<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																											</parameters>
																										</constructor>
																									</object>
																								</objectList>
																							</objectListParameter>
																						</parameters>
																					</constructor>
																				</objectParameter>
																			</parameters>
																		</constructor>
																	</objectParameter>
																	<literalParameter name=""modelType"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=departmentID;domElementId=departmentIDid;title=Department;placeHolder=Department (required);type=text;FormValidationSettingParameters: validationSetting;DropDownTemplateParameters: dropDownTemplate;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">departmentID</literalParameter>
														<literalParameter name=""domElementId"">departmentIDid</literalParameter>
														<literalParameter name=""title"">Department</literalParameter>
														<literalParameter name=""placeHolder"">Department (required)</literalParameter>
														<literalParameter name=""type"">text</literalParameter>
														<objectParameter name=""validationSetting"">
															<constructor name=""FormValidationSettingParameters"" visibleText=""FormValidationSettingParameters: GenericListOfValidatorDescriptionParameters: validators"">
																<genericArguments />
																<parameters>
																	<objectListParameter name=""validators"">
																		<objectList objectType=""Contoso.Forms.Parameters.Common.ValidatorDescriptionParameters"" listType=""GenericList"" visibleText=""validators: Count(1)"">
																			<object>
																				<constructor name=""ValidatorDescriptionParameters"" visibleText=""ValidatorDescriptionParameters: className=Validators;functionName=required"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""className"">Validators</literalParameter>
																						<literalParameter name=""functionName"">required</literalParameter>
																					</parameters>
																				</constructor>
																			</object>
																		</objectList>
																	</objectListParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<objectParameter name=""dropDownTemplate"">
															<constructor name=""DropDownTemplateParameters"" visibleText=""DropDownTemplateParameters: templateName=dropDownTemplate;placeHolderText=Select One ...;textField=name;valueField=departmentID;RequestDetailsParameters: requestDetails;DataRequestStateParameters: state;modelType=Contoso.Domain.Entities.DepartmentModel"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">dropDownTemplate</literalParameter>
																	<literalParameter name=""placeHolderText"">Select One ...</literalParameter>
																	<literalParameter name=""textField"">name</literalParameter>
																	<literalParameter name=""valueField"">departmentID</literalParameter>
																	<objectParameter name=""requestDetails"">
																		<constructor name=""RequestDetailsParameters"" visibleText=""RequestDetailsParameters: modelType=Contoso.Domain.Entities.DepartmentModel;dataType=Contoso.Data.Entities.Department;dataSourceUrl=/api/Generic/GetFilterData;getUrl=/api/Generic/GetSingle;addUrl=/api/Generic/Add;updateUrl=/api/Generic/Update;deleteUrl=/api/Generic/Delete;GenericListOfSelectParameters: selects;distinct=true"">
																			<genericArguments />
																			<parameters>
																				<literalParameter name=""modelType"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																				<literalParameter name=""dataType"">Contoso.Data.Entities.Department</literalParameter>
																				<literalParameter name=""dataSourceUrl"">/api/Generic/GetFilterData</literalParameter>
																				<literalParameter name=""getUrl"">/api/Generic/GetSingle</literalParameter>
																				<literalParameter name=""addUrl"">/api/Generic/Add</literalParameter>
																				<literalParameter name=""updateUrl"">/api/Generic/Update</literalParameter>
																				<literalParameter name=""deleteUrl"">/api/Generic/Delete</literalParameter>
																				<objectListParameter name=""selects"">
																					<objectList objectType=""Contoso.Forms.Parameters.Common.SelectParameters"" listType=""GenericList"" visibleText=""selects: Count(2)"">
																						<object>
																							<constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=departmentID;sourceMember=departmentID;modelType=Contoso.Domain.Entities.DepartmentModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""fieldName"">departmentID</literalParameter>
																									<literalParameter name=""sourceMember"">departmentID</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																						<object>
																							<constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=name;sourceMember=name;modelType=Contoso.Domain.Entities.DepartmentModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""fieldName"">name</literalParameter>
																									<literalParameter name=""sourceMember"">name</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																					</objectList>
																				</objectListParameter>
																				<literalParameter name=""distinct"">true</literalParameter>
																			</parameters>
																		</constructor>
																	</objectParameter>
																	<objectParameter name=""state"">
																		<constructor name=""DataRequestStateParameters"" visibleText=""DataRequestStateParameters: GenericListOfSortParameters: sort"">
																			<genericArguments />
																			<parameters>
																				<objectListParameter name=""sort"">
																					<objectList objectType=""Contoso.Forms.Parameters.Common.SortParameters"" listType=""GenericList"" visibleText=""sort: Count(1)"">
																						<object>
																							<constructor name=""SortParameters"" visibleText=""SortParameters: field=name;dir=asc;modelType=Contoso.Domain.Entities.DepartmentModel"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""field"">name</literalParameter>
																									<literalParameter name=""dir"">asc</literalParameter>
																									<literalParameter name=""modelType"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																								</parameters>
																							</constructor>
																						</object>
																					</objectList>
																				</objectListParameter>
																			</parameters>
																		</constructor>
																	</objectParameter>
																	<literalParameter name=""modelType"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
											<object>
												<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=title;domElementId=titleid;title=Title;placeHolder=Title (required);type=text;TextFieldTemplateParameters: textTemplate;modelType=Contoso.Domain.Entities.CourseModel"">
													<genericArguments />
													<parameters>
														<literalParameter name=""field"">title</literalParameter>
														<literalParameter name=""domElementId"">titleid</literalParameter>
														<literalParameter name=""title"">Title</literalParameter>
														<literalParameter name=""placeHolder"">Title (required)</literalParameter>
														<literalParameter name=""type"">text</literalParameter>
														<objectParameter name=""textTemplate"">
															<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=textTemplate"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""templateName"">textTemplate</literalParameter>
																</parameters>
															</constructor>
														</objectParameter>
														<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
													</parameters>
												</constructor>
											</object>
										</objectList>
									</objectListParameter>
									<objectParameter name=""filterGroup"">
										<variable name=""GridItemFilter"" visibleText=""GridItemFilter"" />
									</objectParameter>
									<literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
								</parameters>
							</constructor>";

        string button1Xml = @"<constructor name=""MultiSelectFormControlSettingsParameters"" visibleText=""MultiSelectFormControlSettingsParameters: GenericListOfString: keyFields;field=courses;domElementId=coursesId;title=Courses;placeHolder=Select Courses ...;type=text;MultiSelectTemplateParameters: multiSelectTemplate;modelType=Contoso.Domain.Entities.InstructorModel"">
	                            <genericArguments />
	                            <parameters>
		                            <literalListParameter name=""keyFields"">
			                            <literalList literalType=""String"" listType=""GenericList"" visibleText=""keyFields: Count(1)"">
				                            <literal>courseID</literal>
			                            </literalList>
		                            </literalListParameter>
		                            <literalParameter name=""field"">courses</literalParameter>
		                            <literalParameter name=""domElementId"">coursesId</literalParameter>
		                            <literalParameter name=""title"">Courses</literalParameter>
		                            <literalParameter name=""placeHolder"">Select Courses ...</literalParameter>
		                            <literalParameter name=""type"">text</literalParameter>
		                            <objectParameter name=""multiSelectTemplate"">
			                            <constructor name=""MultiSelectTemplateParameters"" visibleText=""MultiSelectTemplateParameters: templateName=multiSelectTemplate;placeHolderText=Select One ...;textField=courseTitle;valueField=courseID;RequestDetailsParameters: requestDetails;DataRequestStateParameters: state;modelType=Contoso.Domain.Entities.CourseAssignmentModel"">
				                            <genericArguments />
				                            <parameters>
					                            <literalParameter name=""templateName"">multiSelectTemplate</literalParameter>
					                            <literalParameter name=""placeHolderText"">Select One ...</literalParameter>
					                            <literalParameter name=""textField"">courseTitle</literalParameter>
					                            <literalParameter name=""valueField"">courseID</literalParameter>
					                            <objectParameter name=""requestDetails"">
						                            <constructor name=""RequestDetailsParameters"" visibleText=""RequestDetailsParameters: modelType=Contoso.Domain.Entities.CourseModel;dataType=Contoso.Data.Entities.Course;dataSourceUrl=/api/Generic/GetFilterData;getUrl=/api/Generic/GetSingle;addUrl=/api/Generic/Add;updateUrl=/api/Generic/Update;deleteUrl=/api/Generic/Delete;GenericListOfSelectParameters: selects"">
							                            <genericArguments />
							                            <parameters>
								                            <literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
								                            <literalParameter name=""dataType"">Contoso.Data.Entities.Course</literalParameter>
								                            <literalParameter name=""dataSourceUrl"">/api/Generic/GetFilterData</literalParameter>
								                            <literalParameter name=""getUrl"">/api/Generic/GetSingle</literalParameter>
								                            <literalParameter name=""addUrl"">/api/Generic/Add</literalParameter>
								                            <literalParameter name=""updateUrl"">/api/Generic/Update</literalParameter>
								                            <literalParameter name=""deleteUrl"">/api/Generic/Delete</literalParameter>
								                            <objectListParameter name=""selects"">
									                            <objectList objectType=""Contoso.Forms.Parameters.Common.SelectParameters"" listType=""GenericList"" visibleText=""selects: Count(2)"">
										                            <object>
											                            <constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=courseID;sourceMember=courseID;modelType=Contoso.Domain.Entities.CourseModel"">
												                            <genericArguments />
												                            <parameters>
													                            <literalParameter name=""fieldName"">courseID</literalParameter>
													                            <literalParameter name=""sourceMember"">courseID</literalParameter>
													                            <literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
												                            </parameters>
											                            </constructor>
										                            </object>
										                            <object>
											                            <constructor name=""SelectParameters"" visibleText=""SelectParameters: fieldName=courseTitle;sourceMember=title;modelType=Contoso.Domain.Entities.CourseModel"">
												                            <genericArguments />
												                            <parameters>
													                            <literalParameter name=""fieldName"">courseTitle</literalParameter>
													                            <literalParameter name=""sourceMember"">title</literalParameter>
													                            <literalParameter name=""modelType"">Contoso.Domain.Entities.CourseModel</literalParameter>
												                            </parameters>
											                            </constructor>
										                            </object>
									                            </objectList>
								                            </objectListParameter>
							                            </parameters>
						                            </constructor>
					                            </objectParameter>
					                            <objectParameter name=""state"">
						                            <constructor name=""DataRequestStateParameters"" visibleText=""DataRequestStateParameters"">
							                            <genericArguments />
							                            <parameters />
						                            </constructor>
					                            </objectParameter>
					                            <literalParameter name=""modelType"">Contoso.Domain.Entities.CourseAssignmentModel</literalParameter>
				                            </parameters>
			                            </constructor>
		                            </objectParameter>
		                            <literalParameter name=""modelType"">Contoso.Domain.Entities.InstructorModel</literalParameter>
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
                    control.Visible = false;

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
