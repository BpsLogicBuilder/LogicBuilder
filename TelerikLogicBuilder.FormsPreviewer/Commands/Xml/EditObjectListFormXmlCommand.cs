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
    internal class EditObjectListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditObjectListFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            using IEditObjectListFormXml editXmlForm = disposableManager.GetEditObjectListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument),
                typeof(object)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(3)"">
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
								</objectList>";
    }
}
