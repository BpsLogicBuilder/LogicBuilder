using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditBooleanFunctionFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;

        public EditBooleanFunctionFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);

            IEditBooleanFunctionForm editBooleanFunctionForm = disposableManager.GetEditBooleanFunctionForm
            (
                xmlDococument
            );

            editBooleanFunctionForm.ShowDialog(radForm);
            if (editBooleanFunctionForm.DialogResult != DialogResult.OK)
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
