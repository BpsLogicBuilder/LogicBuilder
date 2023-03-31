using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands.Xml
{
    internal class EditDialogFunctionFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditDialogFunctionFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            IEditDialogFunctionFormXml editXmlForm = disposableManager.GetEditDialogFunctionFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<function name=""DisplayReadOnlyCollection"" visibleText=""DisplayReadOnlyCollection: ListFormSettingsParameters: setting"">
									<genericArguments />
									<parameters>
										<objectParameter name=""setting"">
											<constructor name=""ListFormSettingsParameters"" visibleText=""ListFormSettingsParameters: title=About;Type: modelType;loadingIndicatorText=Loading ...;itemTemplateName=TextDetailTemplate;GenericListOfItemBindingParameters: bindings;SelectorLambdaOperatorParameters: fieldsSelector;RequestDetailsParameters: requestDetails"">
												<genericArguments />
												<parameters>
													<literalParameter name=""title"">About</literalParameter>
													<objectParameter name=""modelType"">
														<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
															<genericArguments />
															<parameters>
																<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.LookUpsModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
															</parameters>
														</function>
													</objectParameter>
													<literalParameter name=""loadingIndicatorText"">Loading ...</literalParameter>
													<literalParameter name=""itemTemplateName"">TextDetailTemplate</literalParameter>
													<objectListParameter name=""bindings"">
														<objectList objectType=""Contoso.Forms.Parameters.Bindings.ItemBindingParameters"" listType=""GenericList"" visibleText=""bindings: Count(2)"">
															<object>
																<constructor name=""TextItemBindingParameters"" visibleText=""TextItemBindingParameters: name=Text;property=DateTimeValue;title=Enrollment Date;stringFormat={0:MM/dd/yyyy};TextFieldTemplateParameters: textTemplate"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""name"">Text</literalParameter>
																		<literalParameter name=""property"">DateTimeValue</literalParameter>
																		<literalParameter name=""title"">Enrollment Date</literalParameter>
																		<literalParameter name=""stringFormat"">{0:MM/dd/yyyy}</literalParameter>
																		<objectParameter name=""textTemplate"">
																			<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters"">
																				<genericArguments />
																				<parameters>
																					<literalParameter name=""templateName"">TextTemplate</literalParameter>
																				</parameters>
																			</constructor>
																		</objectParameter>
																	</parameters>
																</constructor>
															</object>
															<object>
																<constructor name=""TextItemBindingParameters"" visibleText=""TextItemBindingParameters: name=Detail;property=NumericValue;title=Count;stringFormat={0:f0};TextFieldTemplateParameters: textTemplate"">
																	<genericArguments />
																	<parameters>
																		<literalParameter name=""name"">Detail</literalParameter>
																		<literalParameter name=""property"">NumericValue</literalParameter>
																		<literalParameter name=""title"">Count</literalParameter>
																		<literalParameter name=""stringFormat"">{0:f0}</literalParameter>
																		<objectParameter name=""textTemplate"">
																			<constructor name=""TextFieldTemplateParameters"" visibleText=""TextFieldTemplateParameters"">
																				<genericArguments />
																				<parameters>
																					<literalParameter name=""templateName"">TextTemplate</literalParameter>
																				</parameters>
																			</constructor>
																		</objectParameter>
																	</parameters>
																</constructor>
															</object>
														</objectList>
													</objectListParameter>
													<objectParameter name=""fieldsSelector"">
														<constructor name=""SelectorLambdaOperatorParameters"" visibleText=""SelectorLambdaOperatorParameters: IExpressionParameter: selector;Type: sourceElementType;parameterName=$it;Type: bodyType"">
															<genericArguments />
															<parameters>
																<objectParameter name=""selector"">
																	<constructor name=""SelectOperatorParameters"" visibleText=""SelectOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;selectorParameterName=sel"">
																		<genericArguments />
																		<parameters>
																			<objectParameter name=""sourceOperand"">
																				<constructor name=""OrderByOperatorParameters"" visibleText=""OrderByOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;ListSortDirection: sortDirection;selectorParameterName=group"">
																					<genericArguments />
																					<parameters>
																						<objectParameter name=""sourceOperand"">
																							<constructor name=""GroupByOperatorParameters"" visibleText=""GroupByOperatorParameters: IExpressionParameter: sourceOperand;IExpressionParameter: selectorBody;selectorParameterName=item"">
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
																										<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=EnrollmentDate;IExpressionParameter: sourceOperand"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""memberFullName"">EnrollmentDate</literalParameter>
																												<objectParameter name=""sourceOperand"">
																													<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=item"">
																														<genericArguments />
																														<parameters>
																															<literalParameter name=""parameterName"">item</literalParameter>
																														</parameters>
																													</constructor>
																												</objectParameter>
																											</parameters>
																										</constructor>
																									</objectParameter>
																									<literalParameter name=""selectorParameterName"">item</literalParameter>
																								</parameters>
																							</constructor>
																						</objectParameter>
																						<objectParameter name=""selectorBody"">
																							<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Key;IExpressionParameter: sourceOperand"">
																								<genericArguments />
																								<parameters>
																									<literalParameter name=""memberFullName"">Key</literalParameter>
																									<objectParameter name=""sourceOperand"">
																										<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=group"">
																											<genericArguments />
																											<parameters>
																												<literalParameter name=""parameterName"">group</literalParameter>
																											</parameters>
																										</constructor>
																									</objectParameter>
																								</parameters>
																							</constructor>
																						</objectParameter>
																						<objectParameter name=""sortDirection"">
																							<variable name=""ListSortDirection_Descending"" visibleText=""ListSortDirection_Descending"" />
																						</objectParameter>
																						<literalParameter name=""selectorParameterName"">group</literalParameter>
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
																									<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=DateTimeValue;IExpressionParameter: selector"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""property"">DateTimeValue</literalParameter>
																											<objectParameter name=""selector"">
																												<constructor name=""MemberSelectorOperatorParameters"" visibleText=""MemberSelectorOperatorParameters: memberFullName=Key;IExpressionParameter: sourceOperand"">
																													<genericArguments />
																													<parameters>
																														<literalParameter name=""memberFullName"">Key</literalParameter>
																														<objectParameter name=""sourceOperand"">
																															<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=sel"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""parameterName"">sel</literalParameter>
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
																									<constructor name=""MemberBindingItem"" visibleText=""MemberBindingItem: property=NumericValue;IExpressionParameter: selector"">
																										<genericArguments />
																										<parameters>
																											<literalParameter name=""property"">NumericValue</literalParameter>
																											<objectParameter name=""selector"">
																												<constructor name=""ConvertOperatorParameters"" visibleText=""ConvertOperatorParameters: IExpressionParameter: sourceOperand;Type: type"">
																													<genericArguments />
																													<parameters>
																														<objectParameter name=""sourceOperand"">
																															<constructor name=""CountOperatorParameters"" visibleText=""CountOperatorParameters: IExpressionParameter: sourceOperand"">
																																<genericArguments />
																																<parameters>
																																	<objectParameter name=""sourceOperand"">
																																		<constructor name=""AsQueryableOperatorParameters"" visibleText=""AsQueryableOperatorParameters: IExpressionParameter: sourceOperand"">
																																			<genericArguments />
																																			<parameters>
																																				<objectParameter name=""sourceOperand"">
																																					<constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=sel"">
																																						<genericArguments />
																																						<parameters>
																																							<literalParameter name=""parameterName"">sel</literalParameter>
																																						</parameters>
																																					</constructor>
																																				</objectParameter>
																																			</parameters>
																																		</constructor>
																																	</objectParameter>
																																</parameters>
																															</constructor>
																														</objectParameter>
																														<objectParameter name=""type"">
																															<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Nullable`1[[System.Double, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"">
																																<genericArguments />
																																<parameters>
																																	<literalParameter name=""assemblyQualifiedTypeName"">System.Nullable`1[[System.Double, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e</literalParameter>
																																</parameters>
																															</function>
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
																			<literalParameter name=""selectorParameterName"">sel</literalParameter>
																		</parameters>
																	</constructor>
																</objectParameter>
																<objectParameter name=""sourceElementType"">
																	<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=System.Linq.IQueryable`1[[Contoso.Domain.Entities.StudentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""assemblyQualifiedTypeName"">System.Linq.IQueryable`1[[Contoso.Domain.Entities.StudentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Linq.Expressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</literalParameter>
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
																	<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.StudentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.StudentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
																		</parameters>
																	</function>
																</objectParameter>
																<objectParameter name=""dataType"">
																	<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Data.Entities.Student, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""assemblyQualifiedTypeName"">Contoso.Data.Entities.Student, Contoso.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
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
												</parameters>
											</constructor>
										</objectParameter>
									</parameters>
								</function>";
    }
}
