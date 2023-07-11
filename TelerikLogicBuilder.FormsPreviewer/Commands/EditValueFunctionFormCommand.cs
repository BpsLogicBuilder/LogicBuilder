using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditValueFunctionFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;

        public EditValueFunctionFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);

            using IEditValueFunctionForm editValueFunctionForm = disposableManager.GetEditValueFunctionForm
            (
                typeof(System.Type),
                xmlDococument
            );

            editValueFunctionForm.ShowDialog(radForm);
            if (editValueFunctionForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<function name=""Get Type"" visibleText=""Get Type: assemblyQualifiedTypeName=Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"">
	                                <genericArguments />
	                                <parameters>
		                                <literalParameter name=""assemblyQualifiedTypeName"">Contoso.Domain.Entities.CourseModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</literalParameter>
	                                </parameters>
                                </function>";

    }
}
