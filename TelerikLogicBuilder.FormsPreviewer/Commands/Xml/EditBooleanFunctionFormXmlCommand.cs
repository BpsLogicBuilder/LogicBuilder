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
    internal class EditBooleanFunctionFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditBooleanFunctionFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            using IEditBooleanFunctionFormXml editXmlForm = disposableManager.GetEditBooleanFunctionFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument),
                typeof(bool)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<function name=""Equals"" visibleText=""&lt;SearchText&gt; Equals Green"">
	                                <genericArguments />
	                                <parameters>
		                                <literalParameter name=""value1"">
			                                <variable name=""SearchText"" visibleText=""SearchText"" />
		                                </literalParameter>
		                                <literalParameter name=""value2"">Green</literalParameter>
	                                </parameters>
                                </function>";
    }
}
