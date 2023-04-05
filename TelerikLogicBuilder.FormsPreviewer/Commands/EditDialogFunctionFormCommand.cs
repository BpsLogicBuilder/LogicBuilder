using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditDialogFunctionFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;

        public EditDialogFunctionFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xml);

            IEditDialogFunctionForm editDialogFunctionForm = disposableManager.GetEditDialogFunctionForm
            (
                xmlDocument
            );

            editDialogFunctionForm.ShowDialog(radForm);
            if (editDialogFunctionForm.DialogResult != DialogResult.OK)
                return;

            string result = editDialogFunctionForm.ShapeXml;
        }

        readonly string xml = @"<functions>
									<function name=""DisplayEditForm"" visibleText=""DisplayEditForm: DataFormSettingsParameters: setting"">
										<genericArguments />
										<parameters>
											<objectParameter name=""setting"">
												<constructor name=""DataFormSettingsParameters"" visibleText=""DataFormSettingsParameters: title=Add Department;GenericListOfValidationMessageParameters: validationMessages;GenericListOfFormItemSettingsParameters: fieldSettings;FormType: formType;Type: modelType;FormRequestDetailsParameters: requestDetails"">
													<genericArguments />
													<parameters>
														<literalParameter name=""title"">Add Department</literalParameter>
														<objectListParameter name=""validationMessages"">
															<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(4)"">
																<object>
																	<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=Budget;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">Budget</literalParameter>
																			<objectListParameter name=""rules"">
																				<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(2)"">
																					<object>
																						<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=MustBePositiveNumberRule;message=Budget must be a positive number."">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""className"">MustBePositiveNumberRule</literalParameter>
																								<literalParameter name=""message"">Budget must be a positive number.</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																					<object>
																						<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Budget is required."">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""className"">RequiredRule</literalParameter>
																								<literalParameter name=""message"">Budget is required.</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																				</objectList>
																			</objectListParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=InstructorID;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">InstructorID</literalParameter>
																			<objectListParameter name=""rules"">
																				<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																					<object>
																						<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Administrator is required."">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""className"">RequiredRule</literalParameter>
																								<literalParameter name=""message"">Administrator is required.</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																				</objectList>
																			</objectListParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=Name;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">Name</literalParameter>
																			<objectListParameter name=""rules"">
																				<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																					<object>
																						<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Name is required."">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""className"">RequiredRule</literalParameter>
																								<literalParameter name=""message"">Name is required.</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																				</objectList>
																			</objectListParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=StartDate;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">StartDate</literalParameter>
																			<objectListParameter name=""rules"">
																				<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																					<object>
																						<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Start Date is required."">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""className"">RequiredRule</literalParameter>
																								<literalParameter name=""message"">Start Date is required.</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																				</objectList>
																			</objectListParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<objectListParameter name=""fieldSettings"">
															<objectList objectType=""Contoso.Forms.Parameters.DataForm.FormItemSettingsParameters"" listType=""GenericList"" visibleText=""fieldSettings: Count(5)"">
																<object>
																	<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=Budget;title=Budget;placeholder=Budget (required);stringFormat={0:F2};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">Budget</literalParameter>
																			<literalParameter name=""title"">Budget</literalParameter>
																			<literalParameter name=""placeholder"">Budget (required)</literalParameter>
																			<literalParameter name=""stringFormat"">{0:F2}</literalParameter>
																			<objectParameter name=""type"">
																				<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Decimal"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""assemblyQualifiedTypeName"">System.Decimal</literalParameter>
																					</parameters>
																				</function>
																			</objectParameter>
																			<objectParameter name=""validationSetting"">
																				<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																					<genericArguments />
																					<parameters>
																						<objectParameter name=""defaultValue"">
																							<function name=""Cast"" visibleText=""Cast: "">
																								<genericArguments>
																									<literalParameter genericArgumentName=""From"">
																										<literalType>Decimal</literalType>
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
																						<objectListParameter name=""validators"">
																							<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorDefinitionParameters"" listType=""GenericList"" visibleText=""validators: Count(2)"">
																								<object>
																									<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RequiredRule;functionName=Check"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""className"">RequiredRule</literalParameter>
																											<literalParameter name=""functionName"">Check</literalParameter>
																										</parameters>
																									</constructor>
																								</object>
																								<object>
																									<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=MustBePositiveNumberRule;functionName=Check"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""className"">MustBePositiveNumberRule</literalParameter>
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
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=InstructorID;title=Administrator;placeholder=Administrator (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;DropDownTemplateParameters: dropDownTemplate;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">InstructorID</literalParameter>
																			<literalParameter name=""title"">Administrator</literalParameter>
																			<literalParameter name=""placeholder"">Administrator (required)</literalParameter>
																			<literalParameter name=""stringFormat"">{0}</literalParameter>
																			<objectParameter name=""type"">
																				<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Int32"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""assemblyQualifiedTypeName"">System.Int32</literalParameter>
																					</parameters>
																				</function>
																			</objectParameter>
																			<objectParameter name=""validationSetting"">
																				<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																					<genericArguments />
																					<parameters>
																						<objectParameter name=""defaultValue"">
																							<function name=""Cast"" visibleText=""Cast: "">
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
																			<objectParameter name=""dropDownTemplate"">
																				<constructor name=""DropDownTemplateParameters"" visibleText=""DropDownTemplateParameters: templateName=PickerTemplate;titleText=Select Administrator:;textField=FullName;valueField=ID;loadingIndicatorText=Loading ...;SelectorLambdaOperatorParameters: textAndValueSelector;RequestDetailsParameters: requestDetails;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""templateName"">PickerTemplate</literalParameter>
																						<literalParameter name=""titleText"">Select Administrator:</literalParameter>
																						<literalParameter name=""textField"">FullName</literalParameter>
																						<literalParameter name=""valueField"">ID</literalParameter>
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
																																<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=FullName;IExpressionParameter: sourceOperand"">
																																	<genericArguments />
																																	<parameters>
																																		<literalParameter name=""memberFullName"">FullName</literalParameter>
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
																																<objectList objectType=""Contoso.Parameters.Expressions.MemberBindingItem"" listType=""GenericList"" visibleText=""memberBindings: Count(4)"">
																																	<object>
																																		<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=ID;IExpressionParameter: selector;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
																																			<genericArguments />
																																			<parameters>
																																				<literalParameter name=""property"">ID</literalParameter>
																																				<objectParameter name=""selector"">
																																					<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=ID;IExpressionParameter: sourceOperand"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""memberFullName"">ID</literalParameter>
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
																																				<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
																																			</parameters>
																																		</constructor>
																																	</object>
																																	<object>
																																		<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=FirstName;IExpressionParameter: selector;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
																																			<genericArguments />
																																			<parameters>
																																				<literalParameter name=""property"">FirstName</literalParameter>
																																				<objectParameter name=""selector"">
																																					<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=FirstName;IExpressionParameter: sourceOperand"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""memberFullName"">FirstName</literalParameter>
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
																																				<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
																																			</parameters>
																																		</constructor>
																																	</object>
																																	<object>
																																		<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=LastName;IExpressionParameter: selector;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
																																			<genericArguments />
																																			<parameters>
																																				<literalParameter name=""property"">LastName</literalParameter>
																																				<objectParameter name=""selector"">
																																					<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=LastName;IExpressionParameter: sourceOperand"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""memberFullName"">LastName</literalParameter>
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
																																				<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
																																			</parameters>
																																		</constructor>
																																	</object>
																																	<object>
																																		<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=FullName;IExpressionParameter: selector;fieldTypeSource=Contoso.Domain.Entities.InstructorModel"">
																																			<genericArguments />
																																			<parameters>
																																				<literalParameter name=""property"">FullName</literalParameter>
																																				<objectParameter name=""selector"">
																																					<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=FullName;IExpressionParameter: sourceOperand"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""memberFullName"">FullName</literalParameter>
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
																																				<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
																																			</parameters>
																																		</constructor>
																																	</object>
																																</objectList>
																															</objectListParameter>
																															<objectParameter name=""newType"">
																																<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																																	<genericArguments />
																																	<parameters>
																																		<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
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
																										<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																											</parameters>
																										</function>
																									</objectParameter>
																									<literalParameter name=""parameterName"">$it</literalParameter>
																									<objectParameter name=""bodyType"">
																										<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
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
																									<objectParameter name=""modelReturnType"">
																										<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.InstructorModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																											</parameters>
																										</function>
																									</objectParameter>
																									<objectParameter name=""dataReturnType"">
																										<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Data.Entities.Instructor, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Data.Entities.Instructor, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																											</parameters>
																										</function>
																									</objectParameter>
																									<literalParameter name=""dataSourceUrl"">api/List/GetList</literalParameter>
																								</parameters>
																							</constructor>
																						</objectParameter>
																						<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.InstructorModel</literalParameter>
																					</parameters>
																				</constructor>
																			</objectParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=Name;title=Name;placeholder=Name (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">Name</literalParameter>
																			<literalParameter name=""title"">Name</literalParameter>
																			<literalParameter name=""placeholder"">Name (required)</literalParameter>
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
																							<function name=""Cast"" visibleText=""Cast: "">
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
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=StartDate;title=Start Date;placeholder=Start Date(required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""field"">StartDate</literalParameter>
																			<literalParameter name=""title"">Start Date</literalParameter>
																			<literalParameter name=""placeholder"">Start Date(required)</literalParameter>
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
																							<function name=""Cast"" visibleText=""Cast: "">
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
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""FormGroupArraySettingsParameters"" visibleText=""FormGroupArraySettingsParameters: field=Courses;GenericListOfString: keyFields;title=Courses;placeholder=(Courses);Type: modelType;Type: type;validFormControlText=(Courses);invalidFormControlText=(Invalid Courses);FormsCollectionDisplayTemplateParameters: formsCollectionDisplayTemplate;FormGroupTemplateParameters: formGroupTemplate;GenericListOfFormItemSettingsParameters: fieldSettings;GenericListOfValidationMessageParameters: validationMessages;fieldTypeSource=Contoso.Domain.Entities.DepartmentModel;listElementTypeSource=Contoso.Domain.Entities.CourseModel"">
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
																			<objectParameter name=""modelType"">
																				<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																					</parameters>
																				</function>
																			</objectParameter>
																			<objectParameter name=""type"">
																				<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Collections.Generic.ICollection`1[[Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""assemblyQualifiedTypeName"">System.Collections.Generic.ICollection`1[[Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e</literalParameter>
																					</parameters>
																				</function>
																			</objectParameter>
																			<literalParameter name=""validFormControlText"">(Courses)</literalParameter>
																			<literalParameter name=""invalidFormControlText"">(Invalid Courses)</literalParameter>
																			<objectParameter name=""formsCollectionDisplayTemplate"">
																				<constructor name=""FormsCollectionDisplayTemplateParameters"" visibleText=""FormsCollectionDisplayTemplateParameters: templateName=HeaderTextDetailTemplate;GenericListOfItemBindingParameters: bindings"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""templateName"">HeaderTextDetailTemplate</literalParameter>
																						<objectListParameter name=""bindings"">
																							<objectList objectType=""Contoso.Forms.Parameters.Bindings.ItemBindingParameters"" listType=""GenericList"" visibleText=""bindings: Count(3)"">
																								<object>
																									<constructor name=""TextItemBindingParameters"" visibleText=""TextItemBindingParameters: name=Header;property=CourseID;title=ID;stringFormat={0};TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""name"">Header</literalParameter>
																											<literalParameter name=""property"">CourseID</literalParameter>
																											<literalParameter name=""title"">ID</literalParameter>
																											<literalParameter name=""stringFormat"">{0}</literalParameter>
																											<objectParameter name=""textTemplate"">
																												<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=TextTemplate"">
																													<genericArguments />
																													<parameters>
																														<literalParameter name=""templateName"">TextTemplate</literalParameter>
																													</parameters>
																												</constructor>
																											</objectParameter>
																											<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																										</parameters>
																									</constructor>
																								</object>
																								<object>
																									<constructor name=""TextItemBindingParameters"" visibleText=""TextItemBindingParameters: name=Text;property=Title;title=Title;stringFormat={0};TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""name"">Text</literalParameter>
																											<literalParameter name=""property"">Title</literalParameter>
																											<literalParameter name=""title"">Title</literalParameter>
																											<literalParameter name=""stringFormat"">{0}</literalParameter>
																											<objectParameter name=""textTemplate"">
																												<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=TextTemplate"">
																													<genericArguments />
																													<parameters>
																														<literalParameter name=""templateName"">TextTemplate</literalParameter>
																													</parameters>
																												</constructor>
																											</objectParameter>
																											<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																										</parameters>
																									</constructor>
																								</object>
																								<object>
																									<constructor name=""TextItemBindingParameters"" visibleText=""TextItemBindingParameters: name=Detail;property=Credits;title=Credits;stringFormat={0};TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""name"">Detail</literalParameter>
																											<literalParameter name=""property"">Credits</literalParameter>
																											<literalParameter name=""title"">Credits</literalParameter>
																											<literalParameter name=""stringFormat"">{0}</literalParameter>
																											<objectParameter name=""textTemplate"">
																												<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters: templateName=TextTemplate"">
																													<genericArguments />
																													<parameters>
																														<literalParameter name=""templateName"">TextTemplate</literalParameter>
																													</parameters>
																												</constructor>
																											</objectParameter>
																											<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																										</parameters>
																									</constructor>
																								</object>
																							</objectList>
																						</objectListParameter>
																					</parameters>
																				</constructor>
																			</objectParameter>
																			<objectParameter name=""formGroupTemplate"">
																				<constructor name=""FormGroupTemplateParameters"" visibleText=""FormGroupTemplateParameters: templateName=FormGroupArrayTemplate"">
																					<genericArguments />
																					<parameters>
																						<literalParameter name=""templateName"">FormGroupArrayTemplate</literalParameter>
																					</parameters>
																				</constructor>
																			</objectParameter>
																			<objectListParameter name=""fieldSettings"">
																				<objectList objectType=""Contoso.Forms.Parameters.DataForm.FormItemSettingsParameters"" listType=""GenericList"" visibleText=""fieldSettings: Count(3)"">
																					<object>
																						<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=CourseID;title=ID;placeholder=Course (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">CourseID</literalParameter>
																								<literalParameter name=""title"">ID</literalParameter>
																								<literalParameter name=""placeholder"">Course (required)</literalParameter>
																								<literalParameter name=""stringFormat"">{0}</literalParameter>
																								<objectParameter name=""type"">
																									<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Int32"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""assemblyQualifiedTypeName"">System.Int32</literalParameter>
																										</parameters>
																									</function>
																								</objectParameter>
																								<objectParameter name=""validationSetting"">
																									<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																										<genericArguments />
																										<parameters>
																											<objectParameter name=""defaultValue"">
																												<function name=""Cast"" visibleText=""Cast: "">
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
																											<objectListParameter name=""validators"">
																												<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorDefinitionParameters"" listType=""GenericList"" visibleText=""validators: Count(2)"">
																													<object>
																														<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RequiredRule;functionName=Check"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""className"">RequiredRule</literalParameter>
																																<literalParameter name=""functionName"">Check</literalParameter>
																															</parameters>
																														</constructor>
																													</object>
																													<object>
																														<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=MustBeIntegerRule;functionName=Check"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""className"">MustBeIntegerRule</literalParameter>
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
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																					<object>
																						<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=Credits;title=Credits;placeholder=Credits (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;DropDownTemplateParameters: dropDownTemplate;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">Credits</literalParameter>
																								<literalParameter name=""title"">Credits</literalParameter>
																								<literalParameter name=""placeholder"">Credits (required)</literalParameter>
																								<literalParameter name=""stringFormat"">{0}</literalParameter>
																								<objectParameter name=""type"">
																									<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Int32"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""assemblyQualifiedTypeName"">System.Int32</literalParameter>
																										</parameters>
																									</function>
																								</objectParameter>
																								<objectParameter name=""validationSetting"">
																									<constructor name=""FieldValidationSettingsParameters"" visibleText=""FieldValidationSettingsParameters: Object: defaultValue"">
																										<genericArguments />
																										<parameters>
																											<objectParameter name=""defaultValue"">
																												<function name=""Cast"" visibleText=""Cast: "">
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
																											<objectListParameter name=""validators"">
																												<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorDefinitionParameters"" listType=""GenericList"" visibleText=""validators: Count(2)"">
																													<object>
																														<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RequiredRule;functionName=Check"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""className"">RequiredRule</literalParameter>
																																<literalParameter name=""functionName"">Check</literalParameter>
																															</parameters>
																														</constructor>
																													</object>
																													<object>
																														<constructor name=""ValidatorDefinitionParameters"" visibleText=""ValidatorDefinitionParameters: className=RangeRule;functionName=Check"">
																															<genericArguments />
																															<parameters>
																																<literalParameter name=""className"">RangeRule</literalParameter>
																																<literalParameter name=""functionName"">Check</literalParameter>
																																<objectListParameter name=""arguments"">
																																	<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidatorArgumentParameters"" listType=""GenericList"" visibleText=""arguments: Count(2)"">
																																		<object>
																																			<constructor name=""ValidatorArgumentParameters"" visibleText=""ValidatorArgumentParameters: name=min;Object: value;Type: type"">
																																				<genericArguments />
																																				<parameters>
																																					<literalParameter name=""name"">min</literalParameter>
																																					<objectParameter name=""value"">
																																						<function name=""Cast"" visibleText=""Cast: "">
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
																																					<objectParameter name=""type"">
																																						<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Int32"">
																																							<genericArguments />
																																							<parameters>
																																								<literalParameter name=""assemblyQualifiedTypeName"">System.Int32</literalParameter>
																																							</parameters>
																																						</function>
																																					</objectParameter>
																																				</parameters>
																																			</constructor>
																																		</object>
																																		<object>
																																			<constructor name=""ValidatorArgumentParameters"" visibleText=""ValidatorArgumentParameters: name=max;Object: value;Type: type"">
																																				<genericArguments />
																																				<parameters>
																																					<literalParameter name=""name"">max</literalParameter>
																																					<objectParameter name=""value"">
																																						<function name=""Cast"" visibleText=""Cast: "">
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
																																					<objectParameter name=""type"">
																																						<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Int32"">
																																							<genericArguments />
																																							<parameters>
																																								<literalParameter name=""assemblyQualifiedTypeName"">System.Int32</literalParameter>
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
																									<constructor name=""DropDownTemplateParameters"" visibleText=""DropDownTemplateParameters: templateName=PickerTemplate;titleText=Select credits:;textField=Text;valueField=NumericValue;loadingIndicatorText=Loading ...;SelectorLambdaOperatorParameters: textAndValueSelector;RequestDetailsParameters: requestDetails;fieldTypeSource=Contoso.Domain.Entities.LookUpsModel"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""templateName"">PickerTemplate</literalParameter>
																											<literalParameter name=""titleText"">Select credits:</literalParameter>
																											<literalParameter name=""textField"">Text</literalParameter>
																											<literalParameter name=""valueField"">NumericValue</literalParameter>
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
																																		<constructor name=""OrderByOperatorParameters"" visibleText=""OrderByOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;ListSortDirection: sortDirection;selectorParameterName=l"">
																																			<genericArguments />
																																			<parameters>
																																				<objectParameter name=""sourceOperand"">
																																					<constructor name=""WhereOperatorParameters"" visibleText=""WhereOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: filterBody;filterParameterName=l"">
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
																																							<objectParameter name=""filterBody"">
																																								<constructor name=""EqualsBinaryOperatorParameters"" visibleText=""EqualsBinaryOperatorParameters: IExpressionParameter: left;IExpressionParameter: right"">
																																									<genericArguments />
																																									<parameters>
																																										<objectParameter name=""left"">
																																											<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=ListName;IExpressionParameter: sourceOperand"">
																																												<genericArguments />
																																												<parameters>
																																													<literalParameter name=""memberFullName"">ListName</literalParameter>
																																													<objectParameter name=""sourceOperand"">
																																														<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=l"">
																																															<genericArguments />
																																															<parameters>
																																																<literalParameter name=""parameterName"">l</literalParameter>
																																															</parameters>
																																														</constructor>
																																													</objectParameter>
																																												</parameters>
																																											</constructor>
																																										</objectParameter>
																																										<objectParameter name=""right"">
																																											<constructor name=""ConstantOperatorParameters"" visibleText=""ConstantOperatorParameters: Object: constantValue;Type: type"">
																																												<genericArguments />
																																												<parameters>
																																													<objectParameter name=""constantValue"">
																																														<function name=""Cast"" visibleText=""Cast: "">
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
																																																<literalParameter name=""From"">Credits</literalParameter>
																																															</parameters>
																																														</function>
																																													</objectParameter>
																																													<objectParameter name=""type"">
																																														<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.String"">
																																															<genericArguments />
																																															<parameters>
																																																<literalParameter name=""assemblyQualifiedTypeName"">System.String</literalParameter>
																																															</parameters>
																																														</function>
																																													</objectParameter>
																																												</parameters>
																																											</constructor>
																																										</objectParameter>
																																									</parameters>
																																								</constructor>
																																							</objectParameter>
																																							<literalParameter name=""filterParameterName"">l</literalParameter>
																																						</parameters>
																																					</constructor>
																																				</objectParameter>
																																				<objectParameter name=""selectorBody"">
																																					<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=NumericValue;IExpressionParameter: sourceOperand"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""memberFullName"">NumericValue</literalParameter>
																																							<objectParameter name=""sourceOperand"">
																																								<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=l"">
																																									<genericArguments />
																																									<parameters>
																																										<literalParameter name=""parameterName"">l</literalParameter>
																																									</parameters>
																																								</constructor>
																																							</objectParameter>
																																						</parameters>
																																					</constructor>
																																				</objectParameter>
																																				<objectParameter name=""sortDirection"">
																																					<variable name=""ListSortDirection_Descending"" visibleText=""ListSortDirection_Descending"" />
																																				</objectParameter>
																																				<literalParameter name=""selectorParameterName"">l</literalParameter>
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
																																							<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=NumericValue;IExpressionParameter: selector;fieldTypeSource=Contoso.Domain.Entities.LookUpsModel"">
																																								<genericArguments />
																																								<parameters>
																																									<literalParameter name=""property"">NumericValue</literalParameter>
																																									<objectParameter name=""selector"">
																																										<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=NumericValue;IExpressionParameter: sourceOperand"">
																																											<genericArguments />
																																											<parameters>
																																												<literalParameter name=""memberFullName"">NumericValue</literalParameter>
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
																																									<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																																								</parameters>
																																							</constructor>
																																						</object>
																																						<object>
																																							<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=Text;IExpressionParameter: selector;fieldTypeSource=Contoso.Domain.Entities.LookUpsModel"">
																																								<genericArguments />
																																								<parameters>
																																									<literalParameter name=""property"">Text</literalParameter>
																																									<objectParameter name=""selector"">
																																										<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Text;IExpressionParameter: sourceOperand"">
																																											<genericArguments />
																																											<parameters>
																																												<literalParameter name=""memberFullName"">Text</literalParameter>
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
																																									<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																																								</parameters>
																																							</constructor>
																																						</object>
																																					</objectList>
																																				</objectListParameter>
																																				<objectParameter name=""newType"">
																																					<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
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
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																																</parameters>
																															</function>
																														</objectParameter>
																														<literalParameter name=""parameterName"">$it</literalParameter>
																														<objectParameter name=""bodyType"">
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
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
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																																</parameters>
																															</function>
																														</objectParameter>
																														<objectParameter name=""dataType"">
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Data.Entities.LookUps, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Data.Entities.LookUps, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																																</parameters>
																															</function>
																														</objectParameter>
																														<objectParameter name=""modelReturnType"">
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																																</parameters>
																															</function>
																														</objectParameter>
																														<objectParameter name=""dataReturnType"">
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Data.Entities.LookUps, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Data.Entities.LookUps, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
																																</parameters>
																															</function>
																														</objectParameter>
																														<literalParameter name=""dataSourceUrl"">api/List/GetList</literalParameter>
																													</parameters>
																												</constructor>
																											</objectParameter>
																											<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.LookUpsModel</literalParameter>
																										</parameters>
																									</constructor>
																								</objectParameter>
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																					<object>
																						<constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=Title;title=Title;placeholder=Title (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;TextFieldTemplateParameters: textTemplate;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">Title</literalParameter>
																								<literalParameter name=""title"">Title</literalParameter>
																								<literalParameter name=""placeholder"">Title (required)</literalParameter>
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
																												<function name=""Cast"" visibleText=""Cast: "">
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
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																				</objectList>
																			</objectListParameter>
																			<objectListParameter name=""validationMessages"">
																				<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(4)"">
																					<object>
																						<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=CourseID;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">CourseID</literalParameter>
																								<objectListParameter name=""rules"">
																									<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(2)"">
																										<object>
																											<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=CourseID is required."">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""className"">RequiredRule</literalParameter>
																													<literalParameter name=""message"">CourseID is required.</literalParameter>
																												</parameters>
																											</constructor>
																										</object>
																										<object>
																											<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=MustBeIntegerRule;message=CourseID must be an integer."">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""className"">MustBeIntegerRule</literalParameter>
																													<literalParameter name=""message"">CourseID must be an integer.</literalParameter>
																												</parameters>
																											</constructor>
																										</object>
																									</objectList>
																								</objectListParameter>
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																					<object>
																						<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=Credits;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">Credits</literalParameter>
																								<objectListParameter name=""rules"">
																									<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(2)"">
																										<object>
																											<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Credits is required."">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""className"">RequiredRule</literalParameter>
																													<literalParameter name=""message"">Credits is required.</literalParameter>
																												</parameters>
																											</constructor>
																										</object>
																										<object>
																											<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RangeRule;message=Credits must be between 0 and 5 inclusive."">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""className"">RangeRule</literalParameter>
																													<literalParameter name=""message"">Credits must be between 0 and 5 inclusive.</literalParameter>
																												</parameters>
																											</constructor>
																										</object>
																									</objectList>
																								</objectListParameter>
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																					<object>
																						<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=DepartmentID;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">DepartmentID</literalParameter>
																								<objectListParameter name=""rules"">
																									<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																										<object>
																											<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Department is required."">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""className"">RequiredRule</literalParameter>
																													<literalParameter name=""message"">Department is required.</literalParameter>
																												</parameters>
																											</constructor>
																										</object>
																									</objectList>
																								</objectListParameter>
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																					<object>
																						<constructor name=""ValidationMessageParameters"" visibleText=""ValidationMessageParameters: field=Title;fieldTypeSource=Contoso.Domain.Entities.CourseModel"">
																							<genericArguments />
																							<parameters>
																								<literalParameter name=""field"">Title</literalParameter>
																								<objectListParameter name=""rules"">
																									<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationRuleParameters"" listType=""GenericList"" visibleText=""rules: Count(1)"">
																										<object>
																											<constructor name=""ValidationRuleParameters"" visibleText=""ValidationRuleParameters: className=RequiredRule;message=Title is required."">
																												<genericArguments />
																												<parameters>
																													<literalParameter name=""className"">RequiredRule</literalParameter>
																													<literalParameter name=""message"">Title is required.</literalParameter>
																												</parameters>
																											</constructor>
																										</object>
																									</objectList>
																								</objectListParameter>
																								<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																							</parameters>
																						</constructor>
																					</object>
																				</objectList>
																			</objectListParameter>
																			<literalParameter name=""fieldTypeSource"">Contoso.Domain.Entities.DepartmentModel</literalParameter>
																			<literalParameter name=""listElementTypeSource"">Contoso.Domain.Entities.CourseModel</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
														<objectParameter name=""formType"">
															<variable name=""FormType_Add"" visibleText=""FormType_Add"" />
														</objectParameter>
														<objectParameter name=""modelType"">
															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																</parameters>
															</function>
														</objectParameter>
														<objectParameter name=""requestDetails"">
															<constructor name=""FormRequestDetailsParameters"" visibleText=""FormRequestDetailsParameters: getUrl=api/Entity/GetEntity;addUrl=api/Department/Save;updateUrl=api/Department/Save;deleteUrl=api/Department/Delete;Type: modelType;Type: dataType"">
																<genericArguments />
																<parameters>
																	<literalParameter name=""getUrl"">api/Entity/GetEntity</literalParameter>
																	<literalParameter name=""addUrl"">api/Department/Save</literalParameter>
																	<literalParameter name=""updateUrl"">api/Department/Save</literalParameter>
																	<literalParameter name=""deleteUrl"">api/Department/Delete</literalParameter>
																	<objectParameter name=""modelType"">
																		<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																			<genericArguments />
																			<parameters>
																				<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																			</parameters>
																		</function>
																	</objectParameter>
																	<objectParameter name=""dataType"">
																		<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Data.Entities.Department, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																			<genericArguments />
																			<parameters>
																				<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Data.Entities.Department, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																			</parameters>
																		</function>
																	</objectParameter>
																</parameters>
															</constructor>
														</objectParameter>
													</parameters>
												</constructor>
											</objectParameter>
										</parameters>
									</function>
                                </functions>";
    }
}
