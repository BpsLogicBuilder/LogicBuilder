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
    internal class EditConditionsFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditConditionsFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            IEditConditionsFormXml editXmlForm = disposableManager.GetEditConditionsFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<conditions>
	                            <function name=""Equals"" visibleText=""&lt;SkipCount&gt; Equals 3"">
		                            <genericArguments />
		                            <parameters>
			                            <literalParameter name=""value1"">
				                            <variable name=""SkipCount"" visibleText=""SkipCount"" />
			                            </literalParameter>
			                            <literalParameter name=""value2"">3</literalParameter>
		                            </parameters>
	                            </function>
                            </conditions>";
    }
}
