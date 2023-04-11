using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditLiteralListFormCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterElementInfoHelper _literalListParameterElementInfoHelper;
        private readonly RadForm1 radForm;

        public EditLiteralListFormCommand(
            IConfigurationService configurationService,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterElementInfoHelper literalListParameterElementInfoHelper,
            RadForm1 radForm)
        {
            _configurationService = configurationService;
            _literalListDataParser = literalListDataParser;
            _literalListParameterElementInfoHelper = literalListParameterElementInfoHelper;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xml);
            //MultiSelectFormControlSettingsParameters
            var constructor = _configurationService.ConstructorList.Constructors["MultiSelectFormControlSettingsParameters"];
            ListOfLiteralsParameter parameter = (ListOfLiteralsParameter)constructor.Parameters.First(p => p.Name == "keyFields");
            //LiteralListData literalListData = _literalListDataParser.Parse(xmlDocument.DocumentElement!);
            IEditParameterLiteralListForm editLiteralListForm = disposableManager.GetEditLiteralListForm
            (
                typeof(List<string>),
                _literalListParameterElementInfoHelper.GetLiteralListElementInfo(parameter, ""),
                xmlDocument
            );
            editLiteralListForm.ShowDialog(radForm);
            if (editLiteralListForm.DialogResult != DialogResult.OK)
                return;
        }

        string xml = @"<literalList literalType=""String"" listType=""GenericList"" visibleText=""keyFields: Count(1)"">
	                    <literal>CourseID</literal>
                    </literalList>";
    }
}
