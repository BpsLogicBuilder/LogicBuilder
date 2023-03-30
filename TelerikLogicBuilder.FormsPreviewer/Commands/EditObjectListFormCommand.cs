using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditObjectListFormCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterElementInfoHelper _objectListParameterElementInfoHelper;
		private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly RadForm1 radForm;

        public EditObjectListFormCommand(
            IConfigurationService configurationService,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterElementInfoHelper objectListParameterElementInfoHelper,
            ITypeLoadHelper typeLoadHelper,
            RadForm1 radForm)
        {
            _configurationService = configurationService;
            _objectListDataParser = objectListDataParser;
            _objectListParameterElementInfoHelper = objectListParameterElementInfoHelper;
			_typeLoadHelper = typeLoadHelper;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xml);

            var constructor = _configurationService.ConstructorList.Constructors["DataFormSettingsParameters"];
            ListOfObjectsParameter parameter = (ListOfObjectsParameter)constructor.Parameters.First(p => p.Name == "validationMessages");
			ObjectListData objectListData = _objectListDataParser.Parse(xmlDocument.DocumentElement!);
            _typeLoadHelper.TryGetSystemType(parameter, radForm.Application, out Type? type);
            IEditObjectListForm editObjectListForm = disposableManager.GetEditObjectListForm
            (
                type!,
                _objectListParameterElementInfoHelper.GetObjectListElementInfo(parameter),
                xmlDocument
            );
            editObjectListForm.ShowDialog(radForm);
            if (editObjectListForm.DialogResult != DialogResult.OK)
                return;
        }

        string xml = @"<objectList objectType=""Contoso.Forms.Parameters.Validation.ValidationMessageParameters"" listType=""GenericList"" visibleText=""validationMessages: Count(3)"">
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
