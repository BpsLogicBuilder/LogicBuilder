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
    internal class EditLiteralListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditLiteralListFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            using IEditLiteralListFormXml editXmlForm = disposableManager.GetEditLiteralListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument),
                typeof(object)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<literalList literalType=""String"" listType=""GenericList"" visibleText=""keyFields: Count(1)"">
	                                <literal>CourseID</literal>
                                </literalList>";
    }
}
