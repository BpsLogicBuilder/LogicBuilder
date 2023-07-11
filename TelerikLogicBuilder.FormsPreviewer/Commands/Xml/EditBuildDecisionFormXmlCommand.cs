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
    internal class EditBuildDecisionFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditBuildDecisionFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            using IEditBuildDecisionFormXml editXmlForm = disposableManager.GetEditBuildDecisionFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<decision name=""SkipCount"" visibleText=""&lt;SkipCount&gt; Greater Than 2 And &lt;SkipCount&gt; Less Than 7"">
	                                <and>
		                                <function name=""Greater Than"" visibleText=""&lt;SkipCount&gt; Greater Than 2"">
			                                <genericArguments />
			                                <parameters>
				                                <literalParameter name=""value1"">
					                                <variable name=""SkipCount"" visibleText=""SkipCount"" />
				                                </literalParameter>
				                                <literalParameter name=""value2"">2</literalParameter>
			                                </parameters>
		                                </function>
		                                <function name=""Less Than"" visibleText=""&lt;SkipCount&gt; Less Than 7"">
			                                <genericArguments />
			                                <parameters>
				                                <literalParameter name=""value1"">
					                                <variable name=""SkipCount"" visibleText=""SkipCount"" />
				                                </literalParameter>
				                                <literalParameter name=""value2"">7</literalParameter>
			                                </parameters>
		                                </function>
	                                </and>
                                </decision>";
    }
}
