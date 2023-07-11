using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditDecisionFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;

        public EditDecisionFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xml);

            using IEditDecisionForm editDialogFunctionForm = disposableManager.GetEditDecisionForm
            (
                xmlDocument
            );

            editDialogFunctionForm.ShowDialog(radForm);
            if (editDialogFunctionForm.DialogResult != DialogResult.OK)
                return;

            string result = editDialogFunctionForm.DecisionXml;
        }

        readonly string xml = @"<decision name=""SkipCount"" visibleText=""&lt;SkipCount&gt; Equals 4 And &lt;SearchText&gt; Equals ed"">
	                                <and>
		                                <function name=""Equals"" visibleText=""&lt;SkipCount&gt; Equals 4"">
			                                <genericArguments />
			                                <parameters>
				                                <literalParameter name=""value1"">
					                                <variable name=""SkipCount"" visibleText=""SkipCount"" />
				                                </literalParameter>
				                                <literalParameter name=""value2"">4</literalParameter>
			                                </parameters>
		                                </function>
		                                <function name=""Equals"" visibleText=""&lt;SearchText&gt; Equals ed"">
			                                <genericArguments />
			                                <parameters>
				                                <literalParameter name=""value1"">
					                                <variable name=""SearchText"" visibleText=""SearchText"" />
				                                </literalParameter>
				                                <literalParameter name=""value2"">ed</literalParameter>
			                                </parameters>
		                                </function>
	                                </and>
                                </decision>";
    }
}
