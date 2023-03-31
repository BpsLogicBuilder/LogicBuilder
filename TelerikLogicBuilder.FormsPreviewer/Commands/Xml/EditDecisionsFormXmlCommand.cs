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
    internal class EditDecisionsFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditDecisionsFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            IEditDecisionsFormXml editXmlForm = disposableManager.GetEditDecisionsFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<decisions>
	                                <decision name=""SkipCount"" visibleText=""&lt;SkipCount&gt; Equals 1 And &lt;SearchText&gt; Equals Green"">
		                                <and>
			                                <function name=""Equals"" visibleText=""&lt;SkipCount&gt; Equals 1"">
				                                <genericArguments />
				                                <parameters>
					                                <literalParameter name=""value1"">
						                                <variable name=""SkipCount"" visibleText=""SkipCount"" />
					                                </literalParameter>
					                                <literalParameter name=""value2"">1</literalParameter>
				                                </parameters>
			                                </function>
			                                <function name=""Equals"" visibleText=""&lt;SearchText&gt; Equals Green"">
				                                <genericArguments />
				                                <parameters>
					                                <literalParameter name=""value1"">
						                                <variable name=""SearchText"" visibleText=""SearchText"" />
					                                </literalParameter>
					                                <literalParameter name=""value2"">Green</literalParameter>
				                                </parameters>
			                                </function>
		                                </and>
	                                </decision>
                                </decisions>";
    }
}
